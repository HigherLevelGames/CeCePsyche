using UnityEngine;
using System.Collections;

public class DeionController : MonoBehaviour
{

	public bool facingRight = true;			// For determining which way the player is currently facing.

	public bool jump = false;				// Condition for whether the player should jump.
	public bool airJump = false;
	public int numJumps = 3;
	public int jumpsLeft;			// Condition for whether the player should double jump
	public bool dash = false;				// Condition for whether the player should dash
	public int numDash = 3;
	private int dashLeft;
	public bool shieldOut;
	public bool useBubble;
	public bool cBaconEquip;
	public bool pushEquip;
	public bool insane = false;				// Conditions for Deions Powered up modes
	public bool ridiculous = false;
	public bool insiculous = false;

	public float moveForce = 365f;			// Amount of force added to move the player left and right.
	public float maxSpeed = 7f;				// The fastest the player can travel in the x axis.
	public float speedBoost = 3f;			// Amount of force added to max speed after Deion gets a speed Boost
	public AudioClip oneJump;				// Sound clip for the players first jump.
	public AudioClip twoJump;				// Sound clip for the players second jump
	public AudioClip deathSound;
	public AudioClip dashSound;
	public float jumpForce = 1000f;			// Amount of force added when the player jumps.
	public float dashForce = 1000f;

	private Transform gCheck1, gCheck2, gCheck3;			// A position marking where to check if the player is grounded.
	public bool grounded = false;			// Whether or not the player is grounded.
	//private bool inWater = false;
	private Animator anim;					// Reference to the player's animator component.

	void Awake()
	{
		// Setting up references.
		gCheck1 = transform.Find("check1");
		gCheck2 = transform.Find("check2");
		gCheck3 = transform.Find("check3");
		anim = GetComponent<Animator>();

	}

	void Update()
	{
		// The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
		if( Physics2D.Linecast(transform.position, gCheck1.position, 1 << LayerMask.NameToLayer("ground")) ||
		   Physics2D.Linecast(transform.position, gCheck2.position, 1 << LayerMask.NameToLayer("ground")) ||
		   Physics2D.Linecast(transform.position, gCheck3.position, 1 << LayerMask.NameToLayer("ground")))
		{
			grounded = true;
		}
		else
		{
			grounded = false;
		}


		// If the jump button is pressed and the player is grounded then the player should jump.
		if(grounded && Input.GetButtonDown("Jump"))
		{
			jump = true;

		}

		if(jumpsLeft > 0 && Input.GetButtonDown("Jump") && !grounded)
		{
			jumpsLeft--;
			airJump = true;
		}

		if(grounded)
		{
			jumpsLeft = numJumps;
			dashLeft = numDash;
			anim.SetBool("Grounded", true);
		}
		else
		{
			anim.SetBool("Grounded", false);
		}



		if(dashLeft>0 && Input.GetButtonDown("Dash"))
		{
			dash = true;
			if(!grounded)
			{
				dashLeft--;

			}
		}

		/*if(Input.GetButtonUp("Dash"))
		{
			dash = false;

		}*/



	}

	void FixedUpdate ()
	{
		// Cache the horizontal input.
		float h = Input.GetAxis("Horizontal");
		float vSpeed = rigidbody2D.velocity.y;
		float hSpeed = Mathf.Abs(rigidbody2D.velocity.x);


	
		
		// The Speed animator parameter is set to the absolute value of the horizontal input.
	
		anim.SetFloat("Hspeed", hSpeed);
		anim.SetFloat("Vspeed", vSpeed);
		
		// If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
		if(h * rigidbody2D.velocity.x < maxSpeed)
			// ... add a force to the player.
			rigidbody2D.AddForce(Vector2.right * h * moveForce);
		
		// If the player's horizontal velocity is greater than the maxSpeed...
		if(Mathf.Abs(rigidbody2D.velocity.x) > maxSpeed)
			// ... set the player's velocity to the maxSpeed in the x axis.
			rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x) * maxSpeed, rigidbody2D.velocity.y);

		if(dash)
		{
			// Set the dash animator trigger parameter.
			anim.SetTrigger("Dash");
			
			// Play dash audio clip.
			//int i = Random.Range(0, jumpClips.Length);
			AudioSource.PlayClipAtPoint(dashSound, transform.position);
			
			// Add a horizontal force to the player.
			if(facingRight)
			{
				rigidbody2D.velocity = new Vector2(dashForce,-vSpeed);
			}
			if(!facingRight)
			{
				rigidbody2D.velocity = new Vector2(-dashForce,-vSpeed);
			}
		
			
			// Make sure the player can't dash again until the jump conditions from Update are satisfied.
			dash = false;
	
		}

		if (!dash)
		{
			anim.SetTrigger("StopDash");
		}
		
		// If the input is moving the player right and the player is facing left...
		if(h > 0 && !facingRight)
			// ... flip the player.
			Flip();
		// Otherwise if the input is moving the player left and the player is facing right...
		else if(h < 0 && facingRight)
			// ... flip the player.
			Flip();
		
		// If the player should jump...
		if(jump)
		{
			// Set the Jump animator trigger parameter.
			anim.SetTrigger("Jump");
			
			// Play jump audio clip.
			//int i = Random.Range(0, jumpClips.Length);
			if(oneJump != null)
			AudioSource.PlayClipAtPoint(oneJump, transform.position);
			
			// Add a vertical force to the player.
			rigidbody2D.AddForce(new Vector2(0f, jumpForce));
			
			// Make sure the player can't jump again until the jump conditions from Update are satisfied.
			jump = false;

		}

		if(airJump)
		{
			// Set the Jump animator trigger parameter.
			anim.SetTrigger("Jump");

			// Play double jump audio clip.
			if(twoJump != null)
			AudioSource.PlayClipAtPoint(twoJump, transform.position);

			//Offset Y back to 0 for better jump
			float yV = rigidbody2D.velocity.y;
			rigidbody2D.AddForce(new Vector2(0f, -yV));

			// Add a vertical force to the player.
			rigidbody2D.AddForce(new Vector2(0f, jumpForce));

			// Make sure the player can't jump again until the jump conditions from Update are satisfied.
			airJump = false;
		}


	}

	void Flip ()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;
		
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}