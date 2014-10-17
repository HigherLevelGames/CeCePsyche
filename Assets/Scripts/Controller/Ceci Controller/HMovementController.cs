using UnityEngine;
using System.Collections;

public class HMovementController : MonoBehaviour
{
	// hspeed for CeciAnimControl.cs
	public int hSpeed
	{
		get
		{
			if(Mathf.Abs(newX-prevX) < 0.05f)
			{
				return 0;
			}
			else
			{
				return (int)Mathf.Sign(newX-prevX);
			}
		}
	}

	public float MaxSpeed = 10.0f;
	public bool isFacingRight = true; // default, public for Anger Ability to reference
	private Quaternion reverseRotation = new Quaternion(0.0f,180.0f,0.0f,0.0f);
	private float prevX = 0.0f;
	private float newX = 0.0f;

	void Start() {}
	
	void FixedUpdate()
	{
		Movement();

		// temp vars for storing camera rotation/position
		// since we made the camera a child of Ceci
		Quaternion tempRot = Camera.main.transform.rotation;
		Vector3 tempPos = Camera.main.transform.position;

		FaceDirection();

		// give temp vars back to camera
		Camera.main.transform.rotation = tempRot;
		Camera.main.transform.position = tempPos;
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(checker.pt1, 0.1f);
		Gizmos.color = Color.green;
		Gizmos.DrawSphere(checker.pt2, 0.1f);
		Gizmos.color = Color.blue;
		Gizmos.DrawSphere(checker.pt3, 0.1f);
	}

	GroundDetect checker = new GroundDetect();
	void Movement()
	{
		prevX = this.transform.position.x;

		// velocity = speed + direction
		newX = prevX + RebindableInput.GetAxis("Horizontal") * MaxSpeed * Time.deltaTime;
		if(RebindableInput.GetAxis("Horizontal") != 0) // check needed in case standing still
		{
			isFacingRight = RebindableInput.GetAxis("Horizontal") > 0;
		}

		// consideration for slopes
		float newY = checker.checkForward(this.transform, newX);
		if(RebindableInput.GetAxis("Vertical") != 0.0f || RebindableInput.GetKeyDown("Jump") )
		{
			newY = this.transform.position.y;
		}

		// actually move Ceci
		this.transform.position = new Vector2(newX, newY);
	}

	void FaceDirection()
	{
		// face left or right by changing the y rotation value
		if(isFacingRight)
		{
			this.transform.rotation = Quaternion.identity;
		}
		else
		{
			this.transform.rotation = reverseRotation;
		}
	}
}
