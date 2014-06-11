using UnityEngine;
using System.Collections;

public class InteractObject : MonoBehaviour {
	bool activated = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Interact()
	{
		activated = !activated;
		if(activated)
		{
			this.renderer.material.color = Color.green;
		}
		else
		{
			this.renderer.material.color = Color.blue;
		}
	}
}
