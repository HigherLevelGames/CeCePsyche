using UnityEngine;
using System.Collections;

public class Dynamite : MonoBehaviour
{
	public GameObject explosionPrefab;
	public float effectArea;
	GameObject[] dogs;
	bool beenThrown = false;

	void Start()
	{
		dogs = GameObject.FindGameObjectsWithTag("Dog");
		for( int i = 0; i < dogs.Length; i++)
		{
			print (dogs[i].ToString());
		}
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

			//Get Responses from nearby dogs
			for( int i = 0; i < dogs.Length; i++)
			{
				float distance = Vector3.Distance(dogs[i].transform.position, this.transform.position);
				if (distance <= effectArea)
				{
					//ellicit strong response from dog
					Dog thisDog = dogs[i].GetComponent<Dog>();
					thisDog.SendMessage("PerformResponse", true);
			
				}
			}

				Destroy(this.gameObject);
		}

	}


}
