using UnityEngine;
using System.Collections;

public class FollowBae : MonoBehaviour {
 
        public GameObject player;
        public float movementSpeed = 2f;
		public float targetX;
		public float targetY = 0;
		public Transform target;
		
		public Vector2 targetPos;
	
		public int lockRot = 0; 
	
		public bool facingRight = true;	
	
	
           // Use this for initialization
        void Start () 
		{
           player = GameObject.Find ("Player");
        }
 
        // Update is called once per frame
        void FixedUpdate () {
			
			//transform.rotation = Quaternion.Euler (lockRot,lockRot,lockRot);
		
			//targetX = target.transform.position.x;
      		//targetY = target.transform.position.y;
 
       		//Vector2 targetPos = new Vector2(targetX,targetY );
 
       		//transform.LookAt(targetPos);
 
       		//rigidbody2D.velocity= transform.forward*movementSpeed;
 		
			/*		
             if(player.transform.position.x < transform.position.x){
              rigidbody2D.velocity = new Vector2 ((transform.position.x*movementSpeed)*-1,rigidbody2D.velocity.y);
 
             }
             if(player.transform.position.x > transform.position.x){
              rigidbody2D.velocity = new Vector2 (transform.position.x*movementSpeed,rigidbody2D.velocity.y);
 
                }
             if(player.transform.position.y < transform.position.y){
              rigidbody2D.velocity = new Vector2 ((transform.position.x*movementSpeed)*-1,rigidbody2D.velocity.x);
 
                }
             if(player.transform.position.x > transform.position.x){
              rigidbody2D.velocity = new Vector2 (transform.position.x*movementSpeed,rigidbody2D.velocity.x);
 			*/
		
		if(player.transform.position.x > transform.position.x && facingRight){
			// ... flip the player.
		Flip();
		}
		// Otherwise if the input is moving the player left and the player is facing right...
		else if(player.transform.position.x < transform.position.x && !facingRight){
			// ... flip the player.
			Flip();
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

 
 
           
 
	void OnCollisionEnter2D (Collision2D coll) 
	{
		if (coll.gameObject.tag == "Player")
		{
			//collider2D.enabled = false;
			//renderer.enabled = false; 
			PlayerPrefs.SetInt("hasKey",0);
		}	
	}
 
}