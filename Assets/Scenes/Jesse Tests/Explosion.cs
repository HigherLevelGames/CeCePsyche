using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {
	public float expoDelay = 1f;
	public float expoRate = 1f;
	public float expoMaxSize = 10f;
	public float curRadius = 0f;
	public float forceMultiplier = 1f;

	bool exploded = false;
	CircleCollider2D expoRadius;

	// Use this for initialization
	void Start () {
		expoRadius = this.gameObject.GetComponent<CircleCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {

		expoDelay -= Time.deltaTime;
		if (expoDelay < 0)
		{
			exploded = true;
		}
	
	}

	void FixedUpdate() {

		if(exploded)
		{
			if(curRadius < expoMaxSize)
			{
				curRadius += expoRate;
			}
			else
			{
				Destroy(this.gameObject);
			}
			expoRadius.radius = curRadius;
		}
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(exploded)
		{
			if((col.gameObject.rigidbody2D != null && col.gameObject.layer.ToString() == "Destructible") ||
			   (col.gameObject.rigidbody2D != null && col.gameObject.tag.ToString() == "Destructible"))
			{
				Vector2 target = col.gameObject.transform.position;
				Vector2 boom = gameObject.transform.position;

				Vector2 direction = target - boom;

				float distance = Mathf.Abs(Vector2.Distance(boom, target)); 

				float power = expoMaxSize - distance;
				if (power < 0)
					power = 0;
				Vector2 explosiveForce = direction.normalized * power * forceMultiplier; 
				col.gameObject.rigidbody2D.AddForce(explosiveForce);
			}
		}
	}
}
