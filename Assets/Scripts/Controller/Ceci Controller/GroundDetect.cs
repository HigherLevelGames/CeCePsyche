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

	EdgeCollider2D ground
	{
		get
		{
			if(hitCenter)
			{
				return (hitCenter.collider as EdgeCollider2D);
			}
			else if(hitRight)
			{
				return (hitRight.collider as EdgeCollider2D);
			}
			else if(hitLeft)
			{
				return (hitLeft.collider as EdgeCollider2D);
			}
			else
			{
				return null;
			}
		}
	}
	
	// generic ground check
	public bool check(Transform player, Vector2 newPos)
	{
		//get box collider
		BoxCollider2D col = player.collider2D as BoxCollider2D;

		// middle
		//Vector2 myPos = player.position.ToVector2() + col.center * player.localScale.x;
		int modifier = (player.rotation.y == 0.0f) ? 1 : -1;
		Vector2 myPos = newPos + new Vector2(col.center.x*modifier, col.center.y) * player.localScale.x;
		Vector2 groundPos = myPos - Vector2.up * col.size.y * (0.5f+lineLength) * player.localScale.x;
		//Vector2 groundPos = pos - Vector2.up * col.size.y * (0.5f+lineLength) * player.localScale.x;
		/*if(velocity.y < 0)
		{
			groundPos += velocity;
		}*/
		//myPos -= Vector2.up * col.size.y * (0.5f-lineLength) * player.localScale.x;
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

	public float checkForward(Transform player, Vector2 newPos, bool isRight)
	{
		if(check(player, newPos))
		{
			// find specific point (and edges) of collision (if any)
			Vector2 colPoint = Vector2.zero;
			EdgeCollider2D edges = new EdgeCollider2D();
			BoxCollider2D col = (player.collider2D) as BoxCollider2D;
			if(isRight)
			{
				colPoint = hitLeft.point;
				edges = hitLeft.collider as EdgeCollider2D;
			}
			else
			{
				colPoint = hitRight.point;
				edges = hitRight.collider as EdgeCollider2D;
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
						float newY = getY(temp.ToVector2(), temp2.ToVector2(), newPos.x);
						newY += col.size.y * (0.5f+2.0f*lineLength) * player.localScale.y;
						return newY;
					}
				}
			}
		}

		return player.position.y;
	}

	#region Helper Functions
	public bool isInBound(Vector3 pt1, Vector3 pt2, Vector2 check)
	{
		Vector3 mid = getMidpoint(pt1, pt2);
		Bounds box = new Bounds(mid, new Vector3(Mathf.Abs(pt2.x-pt1.x), Mathf.Abs(pt2.y-pt1.y), 1.0f));
		return box.Contains(check.ToVector3());
	}

	public bool isColinear(Vector3 pt1, Vector3 pt2, Vector2 check)
	{
		Vector3 diff1 = (check.ToVector3() - pt1);
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
	#endregion
}
