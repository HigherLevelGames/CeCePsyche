using UnityEngine;
using System.Collections;

public class Dynamite : MonoBehaviour
{
	public GameObject explosionPrefab;
	public float effectArea;
	bool beenThrown = false;

	void Start()
	{
		effectArea = 10;
	}

	void UseMe(bool isFacingRight)
	{
		beenThrown = true;
		if(isFacingRight)
		{
			this.rigidbody2D.velocity = new Vector2(5.0f,5.0f);
		}
		else
		{
			this.rigidbody2D.velocity = new Vector2(-5.0f,5.0f);
		}
	}

	void OnCollisionEnter2D(Collision2D coll) 
	{
		if(beenThrown)
		{
			//Turn into an explosion
			Instantiate(explosionPrefab, this.transform.position, Quaternion.identity);
			Destroy(this.gameObject);
		}

	}


}
