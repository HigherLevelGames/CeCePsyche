using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour {
	
	public float moveSpeed = 3.5f;		// The speed the enemy moves at.
	
	private Transform frontCheck;		// Reference to the position of the gameobject used for checking if something is in front.
	
	private Collider2D col;
	public GameObject player; 

	
	
	void Start(){
	 player = GameObject.FindGameObjectWithTag("Player");	
	}
	
	void Awake()
	{
		// Setting up the references.
		//ren = transform.Find("body").GetComponent<SpriteRenderer>();
		//frontCheck = transform.Find("frontCheck").transform;
	
	}

	void FixedUpdate (){
		
		rigidbody2D.velocity = new Vector2(-(transform.localScale.x) * moveSpeed, -3.359812f);	
	}
		// Create an array of all the colliders in front of the enemy.
		//Collider2D[] frontHits = Physics2D.OverlapPointAll(frontCheck.position, 1);

		// Check each of the colliders.
		//foreach(Collider2D c)
	 //If any of the colliders is an Obstacle...

			//if (coll.gameObject.tag == "Player")
			
				//PlayerPrefs.SetInt("stolenKey", 0);
				//... Flip the enemy and stop checking the other colliders.
				//Flip ();
				//break;
			
		

		// Set the enemy's velocity to moveSpeed in the x direction.
		
		
	void OnCollisionEnter2D(Collision2D col)
	{
		if(col.gameObject.tag == "Player")
		{
			
			PlayerPrefs.SetInt("stolenKey", 0);
			Destroy (gameObject);
			
			//collider2D.enabled = false;
			//Destroy (rigidbody2D);
		}
	
	}
	public void Flip()
	{
		// Multiply the x component of localScale by -1.
		Vector3 enemyScale = transform.localScale;
		enemyScale.x *= -1;
		transform.localScale = enemyScale;
	}
}
