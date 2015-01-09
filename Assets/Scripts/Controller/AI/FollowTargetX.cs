using UnityEngine;
using System.Collections;

public class FollowTargetX : MonoBehaviour
{
	public float speed = 10f;
	private Vector2 direction;
	private float distance;
	private GameObject natti;
	private GameObject ceci;
	
	// Update is called once per frame
	void Update ()
	{

		if(Mathf.Abs(distance) > 10.0f)
		{
			// Chase target
			if(natti != null)
			{
				natti.rigidbody2D.velocity = direction * speed;// * Time.deltaTime;
			}
		}
		else if (natti != null)
		{
			natti.rigidbody2D.velocity = Vector2.zero;
		}
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Conditionable")
		{
			natti = col.gameObject;
		}
		if (col.gameObject.tag == "Player")
		{
			ceci = col.gameObject; 
		}

		distance = ceci.transform.position.x - natti.transform.position.x;
		direction = new Vector2(distance, -2).normalized;

	}

}
