using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
	#region Class Variables
	// bool for interact, so that player can only interact when in range of thing to interact with
	bool interact;

	public float speed = 6.0f;

	// Jump variables
	float jumpTime, jumpDelay = 0.3f;
	bool grounded; // if on ground
	bool jumped; // if pressed jump key

	// transform variable for the end points of the linecasts
	public Transform jumpCheck; // interactCheck;

	//a variable type that stores a collider that was hit during linecast
	//RaycastHit2D interacted;

	// CeCe's state variables
	bool crying = true;
	public enum CharacterState
	{
		Neutral,
		Sad,
		Happy,
		Scared, // Afraid/Fear
		Mad // Angry
	}
	CharacterState CurrentEmotion = CharacterState.Neutral;

	// for CeCe's thoughts
	public GameObject message;
	 
	// for character animation
	Animator anim;
	#endregion

	void Start()
	{
		anim = GetComponent<Animator>();
	}

	void Update()
	{
		//call these function every frame
		Movement();
		RaycastStuff();
	}

	void RaycastStuff()
	{
		//Just a debug visual representation of the Linecast, can only see this in scene view! Doesn't actually do anything!
//		Debug.DrawLine(transform.position, jumpCheck.position, Color.magenta);
//		Debug.DrawLine(transform.position, interactCheck.position, Color.magenta);

		//we assign the bool 'ground' with a linecast, that returns true or false when the end of line 'jumpCheck' touches the ground
		grounded = Physics2D.Linecast(transform.position, jumpCheck.position, 1 << LayerMask.NameToLayer("Ground"));  

		//Using linecast which takes (start point, end point, layermask) so we can make it only detect objects with specified layers
		//its wrapped in an if statement, so that while the tip of the Linecast (interactCheck.position) is touching an object with layer 'Guard', the code inside executes
	/*	if(Physics2D.Linecast(transform.position, interactCheck.position, 1 << LayerMask.NameToLayer("Guard")))
		{
			//we store the collider object the Linecast hit so that we can do something with that specific object, ie. the guard
			//each time the linecast touches a new object with layer "guard", it updates 'interacted' with that specific object instance
			interacted = Physics2D.Linecast(transform.position, interactCheck.position, 1 << LayerMask.NameToLayer("Guard")); 
			interact = true; //since the linecase is touching the guard and we are in range, we can now interact!
		}
		else
		{
			interact = false; //if the linecast is not touching a guard, we cannot interact
		}
		 */
		Physics2D.IgnoreLayerCollision(8, 10); //if we want certain layers to ignore each others collision, we use this! the number is the layer number in the layers list
	}


	void Movement() //function that stores all the movement
	{
		anim.SetFloat("speed", Mathf.Abs(Input.GetAxis ("Horizontal")));

		if(Input.GetAxisRaw("Horizontal") > 0 && crying)
		{
			transform.Translate(Vector3.right * speed * Time.deltaTime); 
			transform.eulerAngles = new Vector2(0, 0); //this sets the rotation of the gameobject
		}

		if(Input.GetAxisRaw("Horizontal") < 0 && crying)
		{
			transform.Translate(Vector3.right * speed * Time.deltaTime);
			transform.eulerAngles = new Vector2(0, 180); //this sets the rotation of the gameobject
		}

		//KeyCode.Space
		if(Input.GetKeyDown (KeyCode.W) && grounded) // If the jump button is pressed and the player is grounded then the player jumps 
		{
			rigidbody2D.AddForce(transform.up * 50000f);
			jumpTime = jumpDelay;
			anim.SetTrigger("Jump");
			jumped = true;
		}

		jumpTime -= Time.deltaTime;
		if(jumpTime <= 0 && grounded && jumped)
		{
			anim.SetTrigger("Land");
			jumped = false;
		}
	}
	
	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag == "Dog") 
		{
			crying = false;
			message.renderer.enabled = false; 
			audio.Play ();
			anim.SetTrigger ("Cry");
			StartCoroutine(Example());
			//Here we would like to add IgnoreCollision but it is not a feature yet.
			//Physics2D.IgnoreCollision2D(col.collider, collider); 
		}
	}
	
	IEnumerator Example()
	{
		yield return new WaitForSeconds(2.7f);
		GetComponent<PlayerControl>().enabled = true;
		crying = true;
	}
	
}