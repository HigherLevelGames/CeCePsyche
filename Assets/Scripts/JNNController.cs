using UnityEngine;
using System.Collections;

public class JNNController : MonoBehaviour
{
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
	bool isHoldingJump = false;
	float VarJumpTime = 0.5f;//half a second, for when the player holds down jump key
	private float VarJumpElapsedTime = 0.0f;
	//float JumpTime, JumpDelay = 0.3f;

	void Start() { }

	bool showPause = false; // pause/inventory flag
	void Update()
	{
		//call these function every frame
		Movement();

		// grounded = Physics2D.Linecast(transform.position, jumpCheck.position, 1 << LayerMask.NameToLayer("Ground"));
		if(Input.GetKeyDown(KeyCode.P))
		{
			showPause = !showPause;
			if(showPause)
			{
				Time.timeScale = 0.0f;
			}
			else
			{
				Time.timeScale = 1.0f;
			}
		}
	}
	
	void Movement() //function that stores all the movement
	{
		// Horizontal Movement
		rigidbody2D.velocity = new Vector2(Input.GetAxis("Horizontal") * MaxSpeed, rigidbody2D.velocity.y);

		// Check Jump Key
		if(Input.GetAxis("Jump") != 0)
		{
			if(!isHoldingJump) // pressed once
			{
				isHoldingJump = true;
				VarJumpElapsedTime = 0.0f;
				if(CurJumpState == JumpState.Grounded)
				{
					CurJumpState = JumpState.Jumping;
				}
			}
			else if(CanVarJump)// press and hold
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

			//JumpTime = JumpDelay;
			//anim.SetTrigger("Jump");
			//hasJumped = true;
		}

		if(isHoldingJump && Input.GetAxis ("Jump") == 0)
		{
			CanVarJump = false;
		}

		//Debug.Log(CurJumpState);

		// Vertical Movement
		if(CurJumpState == JumpState.Jumping)
		{
			rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, JumpSpeed);
			CurJumpState = JumpState.Falling;
		}

		/*
		jumpTime -= Time.deltaTime;
		if(jumpTime <= 0 && grounded && jumped)
		{
			anim.SetTrigger("Land");
			jumped = false;
		}//*/
	}

	int numMemories = 0;
	int numNeurons = 0;
	void OnGUI()
	{
		GUI.Label(new Rect(0,0,100,50),
		          "Memories: " + numMemories +
		          "\nNeurons: " + numNeurons);

		if(showPause)
		{
			GUI.Box(new Rect(200,200,200,200),"Paused");
		}
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		// if(collided object is the floor && CeCe is above it)
		CurJumpState = JumpState.Grounded;
		isHoldingJump = false;
		CanVarJump = true;
	}

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

	// When CeCe gets close enough to an interactable object
	bool pressedInteract = false;
	void OnTriggerStay2D(Collider2D col)
	{
		// TODO: Jason's particle system or something that helps the player know the object is interactable

		// pressed interact key
		if(Input.GetAxis("Interact") != 0 && col.gameObject.tag == "Interactable" && !pressedInteract)
		{
			pressedInteract = true;
			// tell the other object to perform some action, e.g. open doors, treasure chests, use item/switches, etc.
			col.gameObject.SendMessage("Interact", SendMessageOptions.DontRequireReceiver);
		}
		if(Input.GetAxis("Interact") == 0)
		{
			pressedInteract = false;
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