using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class MovementController : MonoBehaviour
{
    #region Speed Variables
	public float MaxHSpeed = 10.0f;// The fastest the player can travel in the x axis.
	public float ClimbSpeed = 10.0f;
    public float JumpSpeed = 10.0f; // Amount of force added when the player jumps.
	public float VarJumpTime = 0.5f;//half a second, how long player can hold down jump key
    public float MaxFallSpeed = -1000.0f;
	public float moveForce = 100f;			// Amount of force added to move the player left and right.
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
    public enum CState
    {
        Grounded,
        Jumping, // moving upwards
        Falling, // moving downwards
    }
    public CState state = CState.Grounded;// so can see in inspector
    
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
            if (Mathf.Abs(transform.position.x - prevPos.x) < 0.05f)
            {
                return 0;
            } else
            {
                return (int)Mathf.Sign(transform.position.x - prevPos.x);
            }
        }
    }
    
    // vspeed for CeciAnimControl.cs
    public int vSpeed
    {
        get
        {
            if (isGrounded || Mathf.Abs(transform.position.y - prevPos.y) < 0.05f)
            { // if(isGrounded || velocity.y == 0.0f)
                return 0;
            } else
            {
                return (int)Mathf.Sign(transform.position.y - prevPos.y); // velocity.y
            }
        }
    }
    
    // grounded for CeciAnimControl.cs
    public bool isGrounded
    {
        get
        {
            return state == CState.Grounded;
        }
    }
    #endregion

	void Start()
    {
		// do use rigidbody2D's weird 2D physics for collision detection
        this.rigidbody2D.gravityScale = 1.0f;
        this.rigidbody2D.isKinematic = false;
        state = CState.Jumping;
    }

    float friction = 1.0f;

	/*
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
	 */

    void FixedUpdate()
    {
        Vector3 position = prevPos = transform.position;
		velocity = rigidbody2D.velocity;
        float dt = Time.fixedDeltaTime;

		#region Player Controls
		if(checker.check(transform))
		{
			state = CState.Grounded;
		}
		else if(!isClimbing) // fall
		{
			state = CState.Falling;
			rigidbody2D.AddForce(new Vector2(0.0f,-0.5f*moveForce));
		}
		
		// If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
		int h = Left? -1: Right? 1: 0;
		if(h * rigidbody2D.velocity.x < MaxHSpeed)
		{
			// ... add a force to the player.
			rigidbody2D.AddForce(Vector2.right * h * moveForce);
		}

		if(isClimbing)
		{
			if(Up)
			{
				this.transform.position += Vector3.up * ClimbSpeed * dt;
				//rigidbody2D.AddForce(Vector2.up * ClimbSpeed*moveForce);
			}
			if(Down)
			{
				this.transform.position += Vector3.down * ClimbSpeed * dt;
				//rigidbody2D.AddForce(-Vector2.up * ClimbSpeed*moveForce);
			}

			// need so Ceci can go on and off the ladder
			if(Left)
			{
				this.transform.position += Vector3.left * ClimbSpeed * dt;
			}
			if(Right)
			{
				this.transform.position += Vector3.right * ClimbSpeed * dt;
			}
		}
		else
		{
			// Jump
			if (state == CState.Grounded && UpPress)
				Jump();
		}
		#endregion

		#region Clamp Velocity
		float clampX = 0.0f;
		float clampY = 0.0f;

		// clamp maximum/minimum speed
		if (!lockHorizontal)
			clampX = Mathf.Clamp(rigidbody2D.velocity.x, -MaxHSpeed, MaxHSpeed);
		if (!lockVertical)
			clampY = Mathf.Clamp(rigidbody2D.velocity.y, MaxFallSpeed, rigidbody2D.velocity.y);

		rigidbody2D.velocity = new Vector2(clampX, clampY);
		#endregion

		#region Update Facing Direction
		isFacingRight = Left? false: Right? true: isFacingRight;
		/*if(rigidbody2D.velocity.x != 0)
		{
			isFacingRight = rigidbody2D.velocity.x > 0;
		}//*/
		if (isFacingRight)
            this.transform.rotation = Quaternion.identity;
        else
            this.transform.rotation = reverseRotation;
        #endregion

        PrevUp = UpPress;
    }

    void Jump()
    {
		rigidbody2D.AddForce(Vector2.up * JumpSpeed * moveForce);
        state = CState.Jumping;
    }

    #region Ladder Trigger Zone
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Ladder")
        {
            lockVertical = true;
			rigidbody2D.isKinematic = true;
			isStartClimb = true;
        }
    }
	/*
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
    */
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Ladder")
        {
            lockVertical = false;
			rigidbody2D.isKinematic = false;
			isStartClimb = false;
            isClimbing = false;
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Ladder")
        {
            lockVertical = true;
			rigidbody2D.isKinematic = true;
			isClimbing = true;
        }
    }
    #endregion*/
}
