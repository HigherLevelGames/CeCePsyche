using UnityEngine;
using System.Collections;

// See:  http://answers.unity3d.com/questions/576044/one-way-platform-using-2d-colliders.html
public class OneWayPlatform : MonoBehaviour
{
	// Use this for initialization
	void Start ()
	{
		transform.parent.collider2D.isTrigger = false; // JIC

	}

	void OnTriggerEnter2D(Collider2D player)
	{
		if(player.tag == "Player")
		{
			// let player pass through
			this.transform.parent.collider2D.isTrigger = true;

		}
	}
	
	void OnTriggerExit2D(Collider2D player)
	{
		if(player.tag == "Player")
		{
			// make platform solid again
			transform.parent.collider2D.isTrigger = false;

		}
	}
}