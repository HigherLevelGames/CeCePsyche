using UnityEngine;
using System.Collections;

public class OneWayPlatform : MonoBehaviour
{
	// Use this for initialization
	void Start ()
	{
		transform.parent.collider2D.isTrigger = false; // JIC
	}

	void OnTriggerEnter2D(Collider2D player)
	{
		// let player pass through
		this.transform.parent.collider2D.isTrigger = true;
	}
	
	void OnTriggerExit2D(Collider2D player)
	{
		// make platform solid again
		transform.parent.collider2D.isTrigger = false;
	}
}