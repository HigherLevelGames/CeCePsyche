using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class MovementController : MonoBehaviour
{
	public float MaxSpeed = 10.0f;
	public bool Left, Right;
	public bool UpPress, UpHold, UpRelease;
	private bool prevUp;

	public bool isFacingRight = true; // default, public for Anger Ability to reference
	private Quaternion reverseRotation = new Quaternion(0.0f, 180.0f, 0.0f, 0.0f);

	// 2D Platformer physics
	private Vector2 prevPos = Vector2.zero;
	private Vector2 newPos = Vector2.zero;
	public Vector2 velocity = Vector2.zero;
	private GroundDetect checker = new GroundDetect();

	#region Properties for CeciAnimControl.cs
	// hspeed for CeciAnimControl.cs
	public int hSpeed
	{
		get
		{
			if(Mathf.Abs(newPos.x-prevPos.x) < 0.05f)
			{
				return 0;
			}
			else
			{
				return (int)Mathf.Sign(newPos.x-prevPos.x);
			}
		}
	}

	// vspeed for CeciAnimControl.cs
	public int vSpeed
	{
		get
		{
			if(Mathf.Abs(newPos.y-prevPos.y) < 0.05f) // if(isGrounded || velocity.y == 0.0f)
			{
				return 0;
			}
			else
			{
				return (int)Mathf.Sign(newPos.y-prevPos.y); // velocity.y
			}
		}
	}
	
	// grounded for CeciAnimControl.cs
	public bool isGrounded
	{
		get
		{
			return CurJumpState == JumpState.Grounded;
		}
	}
	#endregion

	#region climbing variables
	public bool isStartClimb = false;
	public bool isClimbing = false;
	public float ClimbSpeed = 10.0f;
	public bool lockVertical = false;
	#endregion
	
	#region Jump Variables
	// Basic Jump Variables
	public float JumpSpeed = 10.0f;
	private enum JumpState
	{
		Grounded,
		Jumping, // moving upwards
		Falling, // moving downwards
	}
	JumpState CurJumpState = JumpState.Grounded;
	
	// Variable Jump Variables
	public float VarJumpTime = 0.5f;//half a second, how long player can hold down jump key
	private bool CanVarJump = true;
	private float VarJumpElapsedTime = 0.0f;
	#endregion

	void Start()
	{
		// don't use rigidbody2D's weird 2D physics
		this.rigidbody2D.gravityScale = 0.0f;
		this.rigidbody2D.isKinematic = true;
		CurJumpState = JumpState.Falling;
	}
	
	void FixedUpdate()
	{
		CheckInput();
		//if(!lockVertical)
		//{
			JumpControl();
			//Movement();
		//}
		// Vertical above, Horizontal below
		Movement();
		GroundCheck();

		// temp vars for storing camera rotation/position
		// since we made the camera a child of Ceci
		//Quaternion tempRot = Camera.main.transform.rotation;
		//Vector3 tempPos = Camera.main.transform.position;
		
		FaceDirection();
		
		// give temp vars back to camera
		//Camera.main.transform.rotation = tempRot;
		//Camera.main.transform.position = tempPos;
	}

	// JNN: will delete sometime in the future after debugging's done and through
	void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(checker.pt1, 0.1f);
		Gizmos.color = Color.green;
		Gizmos.DrawSphere(checker.pt2, 0.1f);
		Gizmos.color = Color.blue;
		Gizmos.DrawSphere(checker.pt3, 0.1f);
	}

	void CheckInput()
	{
		// Left and Right Stuff
		int rawH = RebindableInput.GetAxis("Horizontal");
		Right = rawH > 0;
		Left = rawH < 0;
		if(Right || Left) // check needed in case standing still
		{
			isFacingRight = Right;
		}

		// Jumping Stuff
		int rawV = RebindableInput.GetAxis("Vertical");
		bool pressJump = RebindableInput.GetKeyDown("Jump");
		UpPress = rawV > 0 || pressJump;
		UpHold = UpPress && (prevUp == UpPress);
		UpRelease = !UpPress && prevUp;
		prevUp = UpPress;
	}

	void Movement()
	{
		prevPos = newPos = this.transform.position;

		// Horizontal Movement
		if(Right)
		{
			newPos.x = prevPos.x + 1 * MaxSpeed * Time.deltaTime;
		}
		else if(Left)
		{
			newPos.x = prevPos.x - 1 * MaxSpeed * Time.deltaTime;
		}
		
		// Slope Movement
		if((Right || Left) && !UpPress)
		{
			newPos.y = checker.checkForward(this.transform, newPos, isFacingRight);
		}


		// Falling
		if(CurJumpState == JumpState.Grounded)
		{
			velocity.y = 0;
		}
		else
		{
			newPos.y += velocity.y * Time.deltaTime;
			velocity.y += 4.0f * Physics.gravity.y * Time.deltaTime;
		}

		Vector3 dir = newPos.ToVector3() - this.transform.position;
		Debug.DrawLine (this.transform.position, this.transform.position+dir);

		// actually move Ceci
		this.transform.position = new Vector2(newPos.x, newPos.y);
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
	
	void GroundCheck()
	{
		if(checker.check(this.transform, this.transform.position))
		{
			CurJumpState = JumpState.Grounded;
		}
		else
		{
			CurJumpState = JumpState.Falling;
		}
	}
	
	// JumpControl() changes CurJumpState accordingly to either Jumping or Falling
	void JumpControl()
	{
		// pressed jump once
		if((UpPress && !UpHold) && CurJumpState == JumpState.Grounded)
		{
			CurJumpState = JumpState.Jumping;
			velocity = JumpSpeed * Vector2.up;
			VarJumpElapsedTime = 0.0f;
			CanVarJump = true;
		}
		
		// press and hold jump button
		if(UpHold && CanVarJump)
		{
			VarJumpElapsedTime += Time.deltaTime;
			if(VarJumpElapsedTime < VarJumpTime)
			{
				CurJumpState = JumpState.Jumping;
				velocity = JumpSpeed*Vector2.up;
			}
			else // time for variable jump is up
			{
				CurJumpState = JumpState.Falling;
				CanVarJump = false;
			}
		}
		
		// released Jump Button
		if(UpRelease)
		{
			CurJumpState = JumpState.Falling;
			CanVarJump = false;
		}
	}

	/*
	#region Ladder Trigger Zone
	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.tag == "Ladder")
		{
			lockVertical = true;
			isStartClimb = true;
		}
	}
	
	void OnTriggerExit2D(Collider2D col)
	{
		if(col.gameObject.tag == "Ladder")
		{
			lockVertical = false;
			isStartClimb = false;
			isClimbing = false;
		}
	}
	
	void OnTriggerStay2D(Collider2D col)
	{
		// Vertical Movement
		if(col.gameObject.tag == "Ladder")
		{
			lockVertical = true;
			isClimbing = true;
			VVelocity.y = RebindableInput.GetAxis("Vertical") * ClimbSpeed * Time.deltaTime;
			this.transform.position += VVelocity;
		}
	}
	#endregion*/
}
