using UnityEngine;
using System.Collections;

public class FollowAndTransport : MonoBehaviour
{
	private GameObject player;
	public float speed = 3f;
	private Vector2 direction
	{
		get
		{
			return new Vector2(player.transform.position.x - this.transform.position.x, this.rigidbody2D.velocity.y).normalized;
		}
	}
	private bool isFacingRight = true;
	private Quaternion reverseRotation = new Quaternion(0.0f,180.0f,0.0f,0.0f);

	// Use this for initialization
	void Start ()
	{
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(direction.x > 0)
		{
			isFacingRight = true;
		}
		else if(direction.x < 0)
		{
			isFacingRight = false;
		}

		// face left or right by changing the y rotation value
		if(isFacingRight)
		{
			this.transform.rotation = Quaternion.identity;
		}
		else
		{
			this.transform.rotation = reverseRotation;
		}

		this.rigidbody2D.velocity = direction * speed;// * Time.deltaTime;
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		col.gameObject.SendMessage("GoToCheckpoint", SendMessageOptions.DontRequireReceiver);
	}
}
