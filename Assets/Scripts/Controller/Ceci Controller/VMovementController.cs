using UnityEngine;
using System.Collections;
using Common;

public class VMovementController : MonoBehaviour
{
	// vspeed for CeciAnimControl.cs
	public int vSpeed
	{
		get
		{
			if(isGrounded)
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

	float ClimbSpeed = 10.0f;
	float prevVValue = 0.0f; // for Input.GetAxis()

	// Jump Variables
	float JumpSpeed = 10.0f;
	float VVelocity = 0.0f;
	public enum JumpState
	{
		Grounded,
		Jumping, // moving upwards
		Falling // moving downwards
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
		if(!lockVertical)
		{
			GroundCheck();
			JumpControl();
			Movement();
		}
	}
	
	void GroundCheck()
	{
		BoxCollider2D col = this.collider2D as BoxCollider2D;

		// middle
		Vector2 myPos = Utility.toVector2(this.transform.position);
		Vector2 groundPos = myPos - Vector2.up * col.size.y;
		bool grounded = Physics2D.Linecast(myPos, groundPos, 1 << LayerMask.NameToLayer("Ground"));
		Debug.DrawLine(myPos, groundPos, Color.magenta);
		
		// right
		Vector2 temp = myPos + Vector2.right * col.size.x / 2.0f;
		Vector2 temp2 = groundPos + Vector2.right * col.size.x / 2.0f;
		bool grounded2 = Physics2D.Linecast(temp, temp2, 1<<LayerMask.NameToLayer("Ground"));
		Debug.DrawLine(temp, temp2, Color.magenta);
		
		// left
		temp = myPos - Vector2.right * col.size.x / 2.0f;
		temp2 = groundPos - Vector2.right * col.size.x / 2.0f;
		bool grounded3 = Physics2D.Linecast(temp, temp2, 1<<LayerMask.NameToLayer("Ground"));
		Debug.DrawLine(temp, temp2, Color.magenta);

		if(grounded || grounded2 || grounded3)
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
		VVelocity -= 0.5f * Time.deltaTime;
		this.transform.position += Vector3.up * VVelocity * Time.deltaTime;
		if(CurJumpState == JumpState.Grounded)
		{
			VVelocity = 0.0f;
		}
	}

	// JumpControl() checks the player's jump button
	// and changes CurJumpState accordingly to either Jumping or Falling
	void JumpControl()
	{
		float curVValue = Input.GetAxis("Vertical");

		// pressed jump once
		if((Input.GetButtonDown("Jump") || (curVValue > 0.0f && prevVValue == 0.0f))
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
		if((Input.GetButton("Jump") || curVValue > 0.0f)
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
		if(Input.GetButtonUp("Jump") || (curVValue == 0.0f && prevVValue != 0.0f))
		{
			CurJumpState = JumpState.Falling;
			VVelocity = 0.0f;
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
		prevVValue = Input.GetAxis("Vertical");
	}

	#region Ladder Trigger Zone
	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.tag == "Ladder")
		{
			rigidbody2D.gravityScale = 0.0f;
		}
	}

	void OnTriggerExit2D(Collider2D col)
	{
		if(col.gameObject.tag == "Ladder")
		{
			rigidbody2D.gravityScale = 1.0f;
		}
	}

	void OnTriggerStay2D(Collider2D col)
	{
		// Vertical Movement
		if(col.gameObject.tag == "Ladder")
		{
			CurJumpState = JumpState.Grounded;
			VVelocity = 0.0f;

			if(Input.GetAxis("Vertical") != 0)
			{
				this.transform.position = new Vector3(col.transform.position.x, this.transform.position.y + Input.GetAxis("Vertical") * ClimbSpeed * Time.deltaTime, this.transform.position.z);
			}
			else
			{
				rigidbody2D.velocity = Vector2.zero;
			}
		}
	}
	#endregion
}
