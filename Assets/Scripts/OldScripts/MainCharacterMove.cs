using UnityEngine;
using System.Collections;

public class MainCharacterMove : MonoBehaviour {

	public float maxSpeed = 10f;
	bool facingRight = true;

	//Animator anim;

	void Start () {

		//anim = GetComponent<Animator> ();
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float move = Input.GetAxis ("Horizontal");

		//anim.setFloat ("Speed", Mathf.Abs (move));

		rigidbody2D.velocity = new Vector2(move * maxSpeed, rigidbody2D.velocity.y);

		if(move > 0 &&!facingRight)
			Flip ();
		else if(move < 0 && facingRight)
			Flip();
	
	}

	void Flip(){
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

}
