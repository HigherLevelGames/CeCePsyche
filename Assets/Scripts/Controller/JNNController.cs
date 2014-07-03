using UnityEngine;
using System.Collections;
using Common;

public class JNNController : MonoBehaviour
{
	public bool isFacingRight = true;
	float MaxSpeed = 10.0f;

	// Jump Variables
	float JumpSpeed = 5.0f;
	public enum JumpState
	{
		Grounded,
		Jumping,
		Falling
	}
	JumpState CurJumpState = JumpState.Grounded;

	// Variable Jump Variables
	bool CanVarJump = true;
	float VarJumpTime = 0.5f;//half a second, for when the player holds down jump key
	private float VarJumpElapsedTime = 0.0f;
	//float JumpTime, JumpDelay = 0.3f;

	void Start() { }

	void Update()
	{
		//call these function every frame
		GroundCheck();
		Movement2();
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
	
	float HangTime = 0.0f;
	void Movement2()
	{
		float newX = this.transform.position.x;
		float newY = this.transform.position.y;

		// velocity = speed + direction
		newX += Input.GetAxis("Horizontal") * MaxSpeed * Time.deltaTime;
		if(Input.GetAxis("Horizontal") != 0) // check needed in case standing still
		{
			isFacingRight = Input.GetAxis("Horizontal") > 0;
		}

		// Set CurJumpState
		JumpControl();

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
			float t = HangTime - VarJumpElapsedTime;
			newY -= 0.5f * t * t;
		}

		this.transform.position = new Vector2(newX, newY);
	}

	// JumpControl() checks the player's jump button
	// and changes CurJumpState accordingly to either Jumping or Falling
	void JumpControl()
	{
		// pressed jump once
		if(Input.GetButtonDown("Jump") && CurJumpState == JumpState.Grounded)
		{
			CurJumpState = JumpState.Jumping;
			VarJumpElapsedTime = 0.0f;
			CanVarJump = true;

			//JumpTime = JumpDelay;
			//anim.SetTrigger("Jump");
			//hasJumped = true;
		}
		
		// press and hold jump button
		if(Input.GetButton("Jump") && CanVarJump)
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
	}
	
	void Movement() //function that stores all the movement
	{
		// Horizontal Movement
		rigidbody2D.velocity = new Vector2(Input.GetAxis("Horizontal") * MaxSpeed, rigidbody2D.velocity.y);
		/*if(CurJumpState == JumpState.Grounded)
		{
			//float y = Mathf.Sin(ground.transform.rotation.z * Mathf.Deg2Rad);
			rigidbody2D.velocity = new Vector2(Input.GetAxis("Horizontal") * MaxSpeed, rigidbody2D.velocity.y);
		}
		else
		{
			rigidbody2D.velocity = new Vector2(Input.GetAxis("Horizontal") * MaxSpeed, rigidbody2D.velocity.y);
		}//*/

		// Set CurJumpState
		JumpControl();

		// Vertical Movement
		if(CurJumpState == JumpState.Jumping)
		{
			rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, JumpSpeed);
			CurJumpState = JumpState.Falling;
		}
	}

	#region Tempporary OnGUI Display for testing
	int numMemories = 0;
	int numNeurons = 0;
	void OnGUI()
	{
		GUI.Label(new Rect(0,0,100,50),
		          "Memories: " + numMemories +
		          "\nNeurons: " + numNeurons);
	}
	#endregion

	/*
	void OnCollisionEnter2D(Collision2D col)
	{
		// if(collided object is the floor && CeCe is above it)
		//CurJumpState = JumpState.Grounded;
	}//*/

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.tag == "Collectable")
		{
			// collect the item, e.g. memories, neurons, emotion items, etc.
			//PlayerPrefs.SetInt(col.gameObject.name, PlayerPrefs.GetInt(col.gameObject.name) + 1);
			//*
			if(col.gameObject.name == "Neuron")
			{
				numNeurons++;
			}
			if(col.gameObject.name == "Memory")
			{
				numMemories++;
			}
			//*/
			Destroy(col.gameObject);
		}
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

	// When CeCe gets close enough to an interactable object, i.e. she enter's the object's trigger zone
	void OnTriggerStay2D(Collider2D col)
	{
		// TODO: Jason's particle system or something that helps the player know the object is interactable

		// pressed interact key
		if(Input.GetButtonDown("Interact") && col.gameObject.tag == "Interactable")
		{
			// tell the other object to perform some action, e.g. open doors, treasure chests, use item/switches, etc.
			col.gameObject.SendMessage("Interact", SendMessageOptions.DontRequireReceiver);
		}

		// Vertical Movement
		if(col.gameObject.tag == "Ladder")
		{
			if(Input.GetAxis("Vertical") != 0)
			{
				this.transform.position = new Vector3(col.transform.position.x, this.transform.position.y + Input.GetAxis("Vertical") * MaxSpeed * Time.deltaTime, this.transform.position.z);
			}
			else
			{
				rigidbody2D.velocity = Vector2.zero;
			}
		}

	}
}