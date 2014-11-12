using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class MovementController : MonoBehaviour
{
    #region Speed Variables
    public float MaxHSpeed = 10.0f;
    public float ClimbSpeed = 10.0f;
    public float JumpSpeed = 10.0f;
    public float VarJumpTime = 0.5f;//half a second, how long player can hold down jump key
    public float MaxFallSpeed = -50.0f;
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
    private enum CState
    {
        Grounded,
        Jumping, // moving upwards
        Falling, // moving downwards
    }
    CState state = CState.Grounded;
    
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
        // don't use rigidbody2D's weird 2D physics
        this.rigidbody2D.gravityScale = 0.0f;
        this.rigidbody2D.isKinematic = true;
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
        float dt = Time.fixedDeltaTime;
        #region Command Input
        if (Right)
            velocity.x = MaxHSpeed;
        else if (Left)
            velocity.x = -MaxHSpeed;
        else
            velocity.x = 0.0f;

        if (state == CState.Grounded)
        if (UpPress)
            Jump();
        #endregion
        #region Check Locks
        if (lockVertical)
            velocity.y = 0.0f;
        if (lockHorizontal)
            velocity.x = 0.0f;
        #endregion
        #region Velocity
        if (state == CState.Jumping)
            velocity.y -= gravity;
        else if (state == CState.Grounded)
        {
            if (velocity.x > 0) // Update x velocity
            {
                velocity.x -= friction * dt;
                if (velocity.x < 0)
                    velocity.x = 0;
            } else if (velocity.x < 0)
            {
                velocity.x += friction * dt;
                if (velocity.x > 0)
                    velocity.x = 0;
            }
        }
        #endregion
        #region Position
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        float halfh = collider.bounds.size.y * 0.5f;
        position.x += velocity.x * dt;
        if (state == CState.Jumping)
        {
            if (velocity.y < 0)
            {
                Vector2 v = velocity * dt;
                Vector2 foot = new Vector2(position.x, position.y - halfh);
                RaycastHit2D groundhit = Physics2D.Raycast(foot, v, v.magnitude + 1, 1 << LayerMask.NameToLayer("Ground"));
                p1 = foot;
                p2 = p1 + v;

                if (groundhit.collider != null)
                {
                    groundedge = (EdgeCollider2D)groundhit.collider;
                    position.y = groundhit.point.y + halfh;
                    state = CState.Grounded;
                    velocity.y = 0;
                    p3 = groundhit.point;
                }
            }
        } else
        {
            int ptid = GetLedgeSec(position.x);
            if (ptid > -1)
                position.y = GetLedgeY(position.x, ptid) + halfh;
            else
                state = CState.Jumping;
        }
        position.y += velocity.y * dt;
        transform.position = position;
        #endregion
        #region Update Facing Direction
        if (velocity.x != 0)
            isFacingRight = velocity.x > 0;
        if (isFacingRight)
            this.transform.rotation = Quaternion.identity;
        else
            this.transform.rotation = reverseRotation;
        #endregion
        PrevUp = UpPress;

    }

    int GetLedgeSec(float x)
    {
        if (groundedge == null)
            return -1;
        for (int i = 0; i < groundedge.points.Length - 1; i++)
        {
            float x1 = groundedge.transform.position.x + groundedge.points [i].x;
            float x2 = groundedge.transform.position.x + groundedge.points [i + 1].x;
            if (x > x1 && x <= x2)
                return i;
        }
        return -1;
    }

    float GetLedgeY(float x, int i)
    {
        float y1 = groundedge.transform.position.y + groundedge.points [i].y;
        float y2 = groundedge.transform.position.y + groundedge.points [i + 1].y;
        float x1 = groundedge.transform.position.x + groundedge.points [i].x;
        float x2 = groundedge.transform.position.x + groundedge.points [i + 1].x;
        return (y2 - y1) * ((x - x1) / (x2 - x1)) + y1;
    }

    EdgeCollider2D groundedge;
    Vector2 p1, p2, p3;
    float gravity = 1f;

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(p1, p2);
        Gizmos.DrawIcon(p3, "icon.tiff");
    }

    void Jump()
    {
        velocity.y = 20.0f;
        state = CState.Jumping;
    }

    #region Ladder Trigger Zone
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Ladder")
        {
            lockVertical = true;
            isStartClimb = true;
        }
    }

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
    
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Ladder")
        {
            lockVertical = false;
            isStartClimb = false;
            isClimbing = false;
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Ladder")
        {
            lockVertical = true;
            isClimbing = true;
        }
    }
    #endregion*/
}
