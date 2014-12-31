using UnityEngine;
using System.Collections;

public class HiddenObject : MonoBehaviour {
	public MonoBehaviour inactiveScript;
	public GameObject activationObj;

	void Start ()
	{
		inactiveScript.enabled = false;
	}


	// Update is called once per frame
	void OnTriggerEnter2D(Collider2D col)
	{

		if ( col.gameObject.ToString() == activationObj.ToString())
		{
			inactiveScript.enabled = true;
		}
	}
}
