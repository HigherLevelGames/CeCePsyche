using UnityEngine;
using System.Collections;
using Common;

[RequireComponent(typeof(BoxCollider2D))]
public class VMovementController : MonoBehaviour
{
	#region Properties for CeciAnimControl.cs
	// vspeed for CeciAnimControl.cs
	public int vSpeed
	{
		get
		{
			if(isGrounded || VVelocity == Vector3.zero)//0.0f)
			{
				return 0;
			}
			else
			{
				return (int)Mathf.Sign(VVelocity.y);
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

	public bool isStartClimb = false;
	public bool isClimbing = false;
	float ClimbSpeed = 10.0f;
	int prevVValue = 0; // for Input.GetAxis()

	// Jump Variables
	public float JumpSpeed = 10.0f;
	public Vector3 VVelocity = Vector3.zero;
	public enum JumpState
	{
		Grounded,
		Jumping, // moving upwards
		Falling, // moving downwards
	}
	JumpState CurJumpState = JumpState.Grounded;
	
	// Variable Jump Variables
	public float VarJumpTime = 0.5f;//half a second, for when the player holds down jump key
	private bool CanVarJump = true;
	private float VarJumpElapsedTime = 0.0f;

	public bool lockVertical = false;

	void Start()
	{
		// don't use rigidbody2D's weird 2D physics
		this.rigidbody2D.gravityScale = 0.0f;
		this.rigidbody2D.isKinematic = false;
		CurJumpState = JumpState.Falling;
	}
	
	void FixedUpdate()
	{
		GroundCheck();
		if(!lockVertical)
		{
			JumpControl();
			Movement();
		}
	}

	GroundDetect checker = new GroundDetect();
	void GroundCheck()
	{
		if(checker.check(this.transform))
		{
			CurJumpState = JumpState.Grounded;
		}
		else
		{
			CurJumpState = JumpState.Falling;
		}
	}
	
	void Movement()
	{
		if(CurJumpState == JumpState.Grounded)
		{
			VVelocity = Vector3.zero;
		}
		else
		{
			this.transform.position += VVelocity * Time.deltaTime;
			VVelocity += 0.5f*Vector3.down;

		}
	}

	// JumpControl() checks the player's jump button
	// and changes CurJumpState accordingly to either Jumping or Falling
	void JumpControl()
	{
		int curVValue = RebindableInput.GetAxis("Vertical");

		// pressed jump once
		if((RebindableInput.GetKeyDown("Jump") || (curVValue > 0 && prevVValue == 0))
				&& CurJumpState == JumpState.Grounded)
		{
			CurJumpState = JumpState.Jumping;
			VVelocity = JumpSpeed*Vector3.up;
			VarJumpElapsedTime = 0.0f;
			CanVarJump = true;
		}

		// press and hold jump button
		if((RebindableInput.GetKey("Jump") || curVValue > 0)
				&& CanVarJump)
		{
			VarJumpElapsedTime += Time.deltaTime;
			if(VarJumpElapsedTime < VarJumpTime)
			{
				CurJumpState = JumpState.Jumping;
				VVelocity = JumpSpeed*Vector3.up;
			}
			else // time for variable jump is up
			{
				CurJumpState = JumpState.Falling;
				CanVarJump = false;
			}
		}

		// released Jump Button
		if(RebindableInput.GetKeyUp("Jump") || (curVValue == 0 && prevVValue != 0))
		{
			CurJumpState = JumpState.Falling;
			CanVarJump = false;
		}

		prevVValue = RebindableInput.GetAxis("Vertical");
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
	#endregion
}
