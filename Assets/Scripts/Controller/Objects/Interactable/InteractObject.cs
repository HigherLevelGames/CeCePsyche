using UnityEngine;
using System.Collections;

public class InteractObject : MonoBehaviour
{
	bool activated = false;

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
