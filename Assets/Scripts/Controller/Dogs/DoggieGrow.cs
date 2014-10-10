using UnityEngine;
using System.Collections;

public class DoggieGrow : MonoBehaviour {
	public GameObject dog;
	public float growSize = 0.01f;
	public float growFrame = -1f;
	private bool big;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(growFrame >= 0)
		{
			if( growFrame >= 1.0f)
			{
				growFrame -= Time.deltaTime;
					dog.transform.localScale += new Vector3 (growSize,-growSize,0);
				if(growFrame < 1.0f)
					growFrame = -1.0f;
			}
			else {
				dog.transform.localScale += new Vector3 (-growSize,growSize,0);
				growFrame += Time.deltaTime;
			if(growFrame > 1.0f)
				growFrame = -1.0f;
			}
		}

	}

	void Interact() {
		if(big) {
			print ("small");
			growFrame = 2;
			big = false;

			//Vector3 growth = new Vector3 (-growSize,-growSize,0);
			//dog.transform.localScale +=growth;


		}

		else {
			print ("big");
			growFrame = 0;
			//Vector3 growth = new Vector3 (growSize,growSize,0);
			//dog.transform.localScale += growth;
			big = true;
		}
	}
} 
