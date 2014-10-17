using UnityEngine;
using System.Collections;
using Common;

public class GroundDetect
{
	float lineLength = 0.05f;
	public Vector3 pt1 = Vector3.zero;
	public Vector3 pt2 = Vector3.zero;
	public Vector3 pt3 = Vector3.zero;

	RaycastHit2D hitCenter;
	RaycastHit2D hitRight;
	RaycastHit2D hitLeft;
	bool grounded
	{
		get
		{
			return hitCenter || hitRight || hitLeft;
		}
	}
	
	// generic ground check
	public bool check(Transform player)
	{
		//get box collider
		BoxCollider2D col = player.collider2D as BoxCollider2D;

		// middle
		//Vector2 myPos = Utility.toVector2(player.position) + col.center * player.localScale.x;
		int modifier = (player.rotation.y == 0.0f) ? 1 : -1;
		Vector2 myPos = Utility.toVector2(player.position) + new Vector2(col.center.x*modifier, col.center.y) * player.localScale.x;
		Vector2 groundPos = myPos - Vector2.up * col.size.y * (0.5f+lineLength) * player.localScale.x;
		myPos -= Vector2.up * col.size.y * (0.5f-lineLength) * player.localScale.x;
		hitCenter = Physics2D.Linecast(myPos, groundPos, 1 << LayerMask.NameToLayer("Ground"));
		//add trigger check because Ceci will stop no matter what the collider is here

		Debug.DrawLine(myPos, groundPos, Color.magenta);

		Vector2 width = Vector2.right * col.size.x * 0.5f * player.localScale.x;

		// right
		Vector2 temp = myPos + width;
		Vector2 temp2 = groundPos + width;
		hitRight = Physics2D.Linecast(temp, temp2, 1<<LayerMask.NameToLayer("Ground"));
		Debug.DrawLine(temp, temp2, Color.magenta);

		// left
		temp = myPos - width;
		temp2 = groundPos - width;
		hitLeft = Physics2D.Linecast(temp, temp2, 1<<LayerMask.NameToLayer("Ground"));
		Debug.DrawLine(temp, temp2, Color.magenta);

		return grounded;
	}

	public float checkForward(Transform player, float newX)
	{
		if(check(player))
		{
			// find specific point (and edges) of collision (if any)
			Vector2 colPoint = Vector2.zero;
			EdgeCollider2D edges = new EdgeCollider2D();
			if(hitLeft)
			{
				colPoint = hitLeft.point;
				edges = hitLeft.collider as EdgeCollider2D;
			}
			if(hitRight)
			{
				colPoint = hitRight.point;
				edges = hitRight.collider as EdgeCollider2D;
			}
			if(hitCenter)
			{
				colPoint = hitCenter.point;
				edges = hitCenter.collider as EdgeCollider2D;
			}

			if(edges != null)
			{
				// check all edges
				for(int i = 0; i < edges.pointCount-1; i++)
				{
					// transform edge points from local space to world space
					Vector3 temp = edges.gameObject.transform.TransformPoint(edges.points[i]);
					Vector3 temp2 = edges.gameObject.transform.TransformPoint(edges.points[i+1]);

					// figure out if the point 
					if(isColinear(temp,temp2,colPoint) && isInBound(temp, temp2, colPoint))
					{
						// Colors! (see HMovementController.cs OnDrawGizmo())
						pt1 = temp;
						pt2 = temp2;
						pt3 = colPoint;

						// find newY where newX is located
						float newY = getY(Utility.toVector2(temp), Utility.toVector2(temp2), newX);
						newY += (player.collider2D as BoxCollider2D).size.y * (0.5f-lineLength) * player.localScale.x;
						return newY;
					}
				}
			}
		}

		return player.position.y;
	}

	public bool isInBound(Vector3 pt1, Vector3 pt2, Vector2 check)
	{
		Vector3 mid = getMidpoint(pt1, pt2);
		Bounds box = new Bounds(mid, new Vector3(Mathf.Abs(pt2.x-pt1.x), Mathf.Abs(pt2.y-pt1.y), 1.0f));
		return box.Contains(Utility.toVector3(check));
	}

	public bool isColinear(Vector3 pt1, Vector3 pt2, Vector2 check)
	{
		Vector3 diff1 = (Utility.toVector3(check) - pt1);
		Vector3 diff2 = (pt1 - pt2);
		if(Vector3.Cross(diff1, diff2).sqrMagnitude < 0.01f)
		{
			return true;
		}
		return false;
	}

	public Vector3 getMidpoint(Vector2 pt1, Vector2 pt2)
	{
		return (pt1+pt2)*0.5f;
	}

	public float getY(Vector2 pt1, Vector2 pt2, float newX)
	{
		return getSlope(pt1, pt2) * (newX-pt1.x) + pt1.y;
	}
	
	public float getSlope(Vector2 pt1, Vector2 pt2)
	{
		Vector2 temp = pt2-pt1;
		return temp.y/temp.x;
	}
}
