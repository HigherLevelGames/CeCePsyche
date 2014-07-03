using UnityEngine;
using System.Collections;

public class Stimulus : MonoBehaviour
{
	// Use this for initialization
	void Start () { }
	
	// Update is called once per frame
	void Update () { }

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.tag == "Player")
		{
			// activate some script animation sequence to show how Ceci's unconditioned response
			// tell controller (most likely subconcious) to call certain ability on Ceci
			// after three stimuli, Ceci will gain the conditioned stimulus
			// deactivate this stimulus
			Destroy(this.gameObject);
		}
	}
}
