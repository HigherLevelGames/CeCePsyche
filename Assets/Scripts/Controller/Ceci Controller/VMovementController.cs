using UnityEngine;
using System.Collections;
using Common;

public class VMovementController : MonoBehaviour
{
	float ClimbSpeed = 10.0f;
	float prevVValue = 0.0f; // for Input.GetAxis()

	// Jump Variables
	float JumpSpeed = 5.0f;
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
	float HangTime = 0.0f;
	//float JumpTime, JumpDelay = 0.3f;

	// See:  http://answers.unity3d.com/questions/576044/one-way-platform-using-2d-colliders.html

	void Start() { }
	
	void Update()
	{
		//call these function every frame
		GroundCheck();
		JumpControl();
		Movement();
	}
	
	void GroundCheck()
	{
		BoxCollider2D col = this.collider2D as BoxCollider2D;
		Vector2 groundPos = Utility.toVector2(this.transform.position) - Vector2.up * col.size.y;
		
		// middle
		bool grounded = Physics2D.Linecast(this.transform.position, groundPos, 1 << LayerMask.NameToLayer("Ground"));
		Debug.DrawLine(transform.position, groundPos, Color.magenta);
		
		// right
		Vector2 temp = Utility.toVector2(transform.position) + Vector2.right * col.size.x / 2.0f;
		Vector2 temp2 = groundPos + Vector2.right * col.size.x / 2.0f;
		Debug.DrawLine(temp, temp2, Color.magenta);
		
		// left
		temp = Utility.toVector2(transform.position) - Vector2.right * col.size.x / 2.0f;
		temp2 = groundPos - Vector2.right * col.size.x / 2.0f;
		Debug.DrawLine(temp, temp2, Color.magenta);
		
		if(grounded)
		{
			CurJumpState = JumpState.Grounded;
		}
	}
	
	void Movement()
	{
		float newY = this.transform.position.y;

		JumpSpeed = 10.0f;
		// Vertical Movement
		if(CurJumpState == JumpState.Jumping)
		{
			newY += JumpSpeed * Time.deltaTime;
			newY -= 0.5f * HangTime * HangTime;
			// need to subtract Gravity
			CurJumpState = JumpState.Falling;
		}
		
		if(CurJumpState != JumpState.Grounded)
		{
			HangTime += Time.deltaTime;
		}
		else
		{
			HangTime = 0.0f;
		}
		
		if(CurJumpState == JumpState.Falling)
		{
			float t = (HangTime - VarJumpElapsedTime) * Time.deltaTime;
			newY -= 0.5f * t * t;
		}
		
		this.transform.position = new Vector2(this.transform.position.x, newY);
	}

	// JumpControl() checks the player's jump button
	// and changes CurJumpState accordingly to either Jumping or Falling
	void JumpControl()
	{
		// pressed jump once
		if((Input.GetButtonDown("Jump") || (Input.GetAxis("Vertical") > 0 && prevVValue == 0.0f)) && CurJumpState == JumpState.Grounded)
		{
			CurJumpState = JumpState.Jumping;
			VarJumpElapsedTime = 0.0f;
			CanVarJump = true;
			
			//JumpTime = JumpDelay;
			//anim.SetTrigger("Jump");
			//hasJumped = true;
		}
		
		// press and hold jump button
		if((Input.GetButton("Jump") || Input.GetAxis("Vertical") > 0) && CanVarJump)
		{
			VarJumpElapsedTime += Time.deltaTime;
			if(VarJumpElapsedTime < VarJumpTime)
			{
				CurJumpState = JumpState.Jumping;
			}
			else
			{
				CurJumpState = JumpState.Falling;
				CanVarJump = false;
			}
		}		
		
		// released Jump Button
		if(Input.GetButtonUp("Jump"))
		{
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
		CurJumpState = JumpState.Grounded;
		// Vertical Movement
		if(col.gameObject.tag == "Ladder")
		{
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
