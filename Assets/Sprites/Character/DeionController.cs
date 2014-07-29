using UnityEngine;
using System.Collections;

public class DeionController : MonoBehaviour
{
	[HideInInspector]
	public bool facingRight = true;			// For determining which way the player is currently facing.
	[HideInInspector]
	public bool jump = false;				// Condition for whether the player should jump.
	public bool doubleJump = false;			// Condition for whether the player should double jump
	public bool canDoubleJump = false;
	public bool dash = false;				// Condition for whether the player should dash
	public bool canDash = false;			// Condition for whether the player can dash

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

	private Transform groundCheck;			// A position marking where to check if the player is grounded.
	private bool grounded = false;			// Whether or not the player is grounded.
	private bool inWater = false;
	private Animator anim;					// Reference to the player's animator component.

	void Awake()
	{
		// Setting up references.
		groundCheck = transform.Find("groundCheck");
		anim = GetComponent<Animator>();

	}

	void Update()
	{
		// The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
		grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));  

		inWater = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Water"));  
		// If the jump button is pressed and the player is grounded then the player should jump.
		if(grounded && Input.GetButtonDown("Jump"))
		{
			jump = true;

		}

		if(canDoubleJump && Input.GetButtonDown("Jump") && !grounded)
		{
			doubleJump = true;
		}

		if(grounded)
		{
			canDash = true;
			canDoubleJump = true;
			anim.SetBool("Grounded", true);
		}
		else
		{
			anim.SetBool("Grounded", false);
		}



		if(canDash && Input.GetButtonDown("Dash"))
		{
			dash = true;
			if(!grounded)
			{
				canDash = false;

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
			canDash = false;
			canDoubleJump = true;
	
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
			AudioSource.PlayClipAtPoint(oneJump, transform.position);
			
			// Add a vertical force to the player.
			rigidbody2D.AddForce(new Vector2(0f, jumpForce));
			
			// Make sure the player can't jump again until the jump conditions from Update are satisfied.
			jump = false;

		}

		if(doubleJump)
		{
			// Set the Jump animator trigger parameter.
			anim.SetTrigger("Jump");
			
			// Play double jump audio clip.
			AudioSource.PlayClipAtPoint(twoJump, transform.position);
			
			// Add a vertical force to the player.
			rigidbody2D.AddForce(new Vector2(0f, jumpForce));
			
			// Make sure the player can't jump again until the jump conditions from Update are satisfied.
			doubleJump = false;
			canDoubleJump = false;
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
/*hspeed = 0
//gravity = 0.5
if (keyboard_check_direct(ord('A'))) // moving left
{ 
    hspeed = -6 
    if (mouse_check_button(mb_right)) {hspeed += -4}
    if obj_controller.Insane = 1 {hspeed += -4}
    if obj_controller.Insiculous = 1 {hspeed += -4}
    if not place_free(x + hspeed,y - 1) { hspeed = 0 } 
}
if  (keyboard_check_direct(ord('D'))) // moving right
{ 
    hspeed = 6 
    if hspeed > 14 {hspeed = 14}
    if (mouse_check_button(mb_right)) {hspeed +=4}
    //if obj_controller.Insane = 1 {hspeed += 4}  		//Insane Speed Boost
    //if obj_controller.Insiculous = 1 {hspeed += 4}	//Insiculous Speed Boost
    if not place_free(x + hspeed,y - 1) { hspeed = 0 } 
}
if (keyboard_check_direct (ord('W'))) // jump
{ 
    if not place_free(x,y + 1) and place_free(x + hspeed,y - 10) 
    { 
        sound_play(snd_Deion_jump)
        vspeed -= 10 
    } 
}

// i.e. if there's something above/below you
if not place_free(x + hspeed*-1,y+1 + vspeed)
{
    // if falling, stop on top of object
    if vspeed >= 0 { vspeed = 0 } //J = 1 gravity = 0 } 
    // if you bump your head, start falling
    else { vspeed = 1; }//gravity = 0.5; }
}

// if in the air
if place_free(x+hspeed*-1, y+vspeed) //if place_free(x,y+sprite_height/2)
{
    gravity = 0.5; // fall
}
else
{
    gravity = 0; // standing on something

    // if Deion's standing on movable platform
    if place_meeting(x,y+sprite_height/2,obj_moveblock_h)
    {
        standingOn = instance_nearest(x,y,obj_moveblock_h);
        hspeed += standingOn.hspeed;
        if not place_free(x+hspeed,y-1){ hspeed = 0 }
        //x = obj_moveblock_h.x -16 -sprite_height/2;
    }
    else if place_meeting(x,y+sprite_height/2,obj_moveblock_v)
    {
        standingOn = instance_nearest(x,y,obj_moveblock_v);
        vspeed = standingOn.vspeed;
        //if not place_free(x,y+vspeed){ //deion gets squished? }
    
        //if(obj_moveblock_v.vspeed < 0)
          //  y = obj_moveblock_v.y -16 - sprite_height/2;
    }
}

// if fall off screen
if y > room_height 
{
    lives -= 1
    sound_play(snd_Deion_Death)
    if (lives = 0) { game_restart()}
    else {
    x = obj_spawn_point.x;
    y = obj_spawn_point.y;
    instance_change(obj_Spawn,1);
    }
}

// if Deion is stuck, get Deion unstuck!!
if((hspeed==0 && vspeed==0) && not place_free(x,y-1))
{
    y-=1; // temporary fix...
}*/

}