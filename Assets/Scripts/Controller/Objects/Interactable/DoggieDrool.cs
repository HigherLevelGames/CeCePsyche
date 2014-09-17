using UnityEngine;
using System.Collections;

public class DoggieDrool : MonoBehaviour {
	public GameObject dog;
	public ParticleSystem drool;
	public bool isDrooling;
	public FollowTargetX followScript; 

	// Use this for initialization
	void Start () {
		isDrooling = false;
		followScript.enabled = false;
	
	}
	
	// Update is called once per frame
	void Update () {
		if(isDrooling)
		{
			followScript.enabled = true;
			drool.Play();
			float distance = followScript.distance;
			if (Mathf.Abs(distance) < 2.5)
			{
				drool.Stop();
				isDrooling = false;
				followScript.enabled = false;
				dog.rigidbody2D.velocity = Vector2.zero;
			}
		}
	}

	void Interact() {
		isDrooling = true;
	}
}
