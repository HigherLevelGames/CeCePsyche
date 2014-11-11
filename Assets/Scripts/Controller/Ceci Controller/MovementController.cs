using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class MovementController : MonoBehaviour
{
	#region Speed Variables
	public float MaxHSpeed = 10.0f;
	public float ClimbSpeed = 10.0f;
	public float JumpSpeed = 10.0f;
	public float VarJumpTime = 0.5f;//half a second, how long player can hold down jump key
	public float MaxFallSpeed = -50.0f;
	#endregion

	public bool isFacingRight = true; // default, public for Anger Ability to reference
	private Quaternion reverseRotation = new Quaternion(0.0f, 180.0f, 0.0f, 0.0f);

	#region Remote Controls
	public bool Left, Right;
	public bool UpPress, UpHold, UpRelease, PrevUp;
	public bool Up, Down; // ladder
	#endregion

	#region 2D Platformer physics
	private Vector2 prevPos = Vector2.zero;
	private Vector2 newPos = Vector2.zero;
	public Vector2 velocity = Vector2.zero;
	private GroundDetect checker = new GroundDetect();
	#endregion

	#region climbing variables
	public bool lockVertical = false;
	public bool lockHorizontal = false;
	public bool isStartClimb = false;
	public bool isClimbing = false;
	#endregion
	
	#region Jump Variables
	// Basic Jump Variables
	private enum JumpState
	{
		Grounded,
		Jumping, // moving upwards
		Falling, // moving downwards
	}
	JumpState CurJumpState = JumpState.Grounded;
	
	// Variable Jump Variables
	private bool CanVarJump = true;
	private float VarJumpElapsedTime = 0.0f;
	#endregion

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
			if(isGrounded || Mathf.Abs(newPos.y-prevPos.y) < 0.05f) // if(isGrounded || velocity.y == 0.0f)
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

	void Start()
	{
		// don't use rigidbody2D's weird 2D physics
		this.rigidbody2D.gravityScale = 0.0f;
		this.rigidbody2D.isKinematic = true;
		CurJumpState = JumpState.Falling;
	}
	
	void FixedUpdate()
	{
		if(isStartClimb && (Up || Down))
		{
			isClimbing = true;
		}

		if(lockVertical)
		{
			velocity.y = 0.0f;
		}
		if(lockHorizontal)
		{
			velocity.x = 0.0f;
		}

		HMovement();
		if(isClimbing)
		{
			Climb ();
		}
		else // Normal Movement
		{
			GroundCheck();
			JumpControl();
			Fall();
			FaceDirection();
		}

		Move ();
        PrevUp = UpPress;
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

	void Move()
	{
		// Actually Move Ceci
		newPos = prevPos + velocity * Time.deltaTime;

		// Slope Movement
		if((Right || Left) && !UpPress && CurJumpState == JumpState.Grounded)
		{
			newPos.y = checker.checkForward(this.transform, newPos, isFacingRight);
		}

		this.transform.position = newPos;
	}

	void Climb()
	{
		prevPos = newPos = this.transform.position;

		// Horizontal Movement
		if(Right)
		{
			velocity.x = MaxHSpeed;
		}
		else if(Left)
		{
			velocity.x = -MaxHSpeed;
		}
		else
		{
			velocity.x = 0.0f;
		}

		// Vertical Movement
		if(Up)
		{
			velocity.y = ClimbSpeed;
		}
		else if(Down)
		{
			velocity.y = -ClimbSpeed;
		}
	}

	void HMovement()
	{
		prevPos = newPos = this.transform.position;

		// Horizontal Movement
		if(Right)
		{
			velocity.x = MaxHSpeed;
		}
		else if(Left)
		{
			velocity.x = -MaxHSpeed;
		}
		else
		{
			velocity.x = 0.0f;
		}
	}

	void Fall()
	{
		// Falling
		if(CurJumpState == JumpState.Grounded)
		{
			velocity.y = 0;
		}
		else
		{
			velocity.y += 4.0f * Physics.gravity.y * Time.deltaTime;
			velocity.y = Mathf.Max(velocity.y, MaxFallSpeed);
		}
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
			velocity.y = JumpSpeed;
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
				velocity.y = JumpSpeed;
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

	#region Ladder Trigger Zone
	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.tag == "Ladder")
		{
			lockVertical = true;
			isStartClimb = true;
		}
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		lockVertical = true;
		lockHorizontal = true;
	}
	void OnCollisionExit2D(Collision2D col)
	{
		lockVertical = false;
		lockHorizontal = false;
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
		if(col.gameObject.tag == "Ladder")
		{
			lockVertical = true;
			isClimbing = true;
		}
	}
	#endregion*/
}
