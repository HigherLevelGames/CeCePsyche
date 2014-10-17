using UnityEngine;
using System.Collections;
using Common;

[RequireComponent(typeof(BoxCollider2D))]
public class VMovementController : MonoBehaviour
{
	// vspeed for CeciAnimControl.cs
	public int vSpeed
	{
		get
		{
			if(isGrounded || VVelocity == 0.0f)
			{
				return 0;
			}
			else
			{
				return (int)Mathf.Sign(VVelocity);
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

	public bool isStartClimb = false;
	public bool isClimbing = false;
	float ClimbSpeed = 10.0f;
	int prevVValue = 0; // for Input.GetAxis()

	// Jump Variables
	public float JumpSpeed = 10.0f;
	public float VVelocity = 0.0f;
	public enum JumpState
	{
		Grounded,
		Jumping, // moving upwards
		Falling, // moving downwards
	}
	JumpState CurJumpState = JumpState.Grounded;
	
	// Variable Jump Variables
	bool CanVarJump = true;
	float VarJumpTime = 0.5f;//half a second, for when the player holds down jump key
	private float VarJumpElapsedTime = 0.0f;
	//float JumpTime, JumpDelay = 0.3f;

	public bool lockVertical = false;

	void Start() { }
	
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
			this.rigidbody2D.isKinematic = false;
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
			VVelocity = 0.0f;
		}
		else
		{
			this.transform.position += Vector3.up * VVelocity * Time.deltaTime;
			VVelocity -= 0.5f;// * Time.deltaTime;
		}
	}

	// JumpControl() checks the player's jump button
	// and changes CurJumpState accordingly to either Jumping or Falling
	void JumpControl()
	{
		int curVValue = RebindableInput.GetAxis("Vertical");

		this.rigidbody2D.gravityScale = (curVValue < 0) ? 1.0f : 0.0f;

		// pressed jump once
		if((RebindableInput.GetKeyDown("Jump") || (curVValue > 0 && prevVValue == 0))
				&& CurJumpState == JumpState.Grounded)
		{
			CurJumpState = JumpState.Jumping;
			VVelocity = JumpSpeed;
			VarJumpElapsedTime = 0.0f;
			CanVarJump = true;
			
			//JumpTime = JumpDelay;
			//anim.SetTrigger("Jump");
			//hasJumped = true;
		}

		// press and hold jump button
		if((RebindableInput.GetKey("Jump") || curVValue > 0)
				&& CanVarJump)
		{
			VarJumpElapsedTime += Time.deltaTime;
			if(VarJumpElapsedTime < VarJumpTime)
			{
				CurJumpState = JumpState.Jumping;
				VVelocity = JumpSpeed;
			}
			else
			{
				CurJumpState = JumpState.Falling;
				VVelocity = 0.0f;
				CanVarJump = false;
			}
		}

		// released Jump Button
		if(RebindableInput.GetKeyUp("Jump") || (curVValue == 0 && prevVValue != 0))
		{
			CurJumpState = JumpState.Falling;
			VVelocity = (VVelocity > 0.0f) ? 0.0f : VVelocity;
			CanVarJump = false;
		}
		//Debug.Log(CurJumpState);
		
		/*
		jumpTime -= Time.deltaTime;
		if(jumpTime <= 0 && grounded && jumped)
		{
			anim.SetTrigger("Land");
			jumped = false;
		}//*/
		prevVValue = RebindableInput.GetAxis("Vertical");
	}

	#region Ladder Trigger Zone
	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.tag == "Ladder")
		{
			rigidbody2D.gravityScale = 0.0f;
			lockVertical = true;
			isStartClimb = true;
		}
	}

	void OnTriggerExit2D(Collider2D col)
	{
		if(col.gameObject.tag == "Ladder")
		{
			rigidbody2D.gravityScale = 1.0f;
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
			VVelocity = RebindableInput.GetAxis("Vertical") * ClimbSpeed;//0.0f;

			if(RebindableInput.GetAxis("Vertical") != 0)
			{
				this.transform.position = new Vector3(col.transform.position.x, this.transform.position.y + VVelocity * Time.deltaTime, this.transform.position.z);
			}
			else
			{
				rigidbody2D.velocity = Vector2.zero;
			}
		}
	}
	#endregion
}
