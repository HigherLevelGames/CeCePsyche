using UnityEngine;
using System.Collections;

public class SonicBark: MonoBehaviour {
	public float expoDelay = 1f;
	public float expoRate = 1f;
	public float expoMaxSize = 10f;
	public float curRadius = 0f;
	public float forceMultiplier = 1f;

	public bool exploded = true;
	Vector3 expoRadius = new Vector3(0f,0f,0f);


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

		/*expoDelay -= Time.deltaTime;
		if (expoDelay < 0)
		{
			exploded = true;
		}*/
		if(Input.GetKeyDown(KeyCode.B))
		   exploded = true;
	
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
				exploded = false;
				curRadius = 0f;
				Destroy(this.gameObject);
			}
			expoRadius = new Vector3(curRadius,curRadius, 1);
			this.gameObject.transform.localScale = expoRadius;
		}
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if((col.gameObject.rigidbody2D != null && col.gameObject.layer.ToString() == "Destructible") && exploded ||
		   (col.gameObject.rigidbody2D != null && col.gameObject.tag.ToString() == "Destructible" && exploded))
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
