using UnityEngine;
using System.Collections;

public class Destructable : MonoBehaviour {
	public GameObject destroyedBy;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		print (col.gameObject.ToString());
		print (destroyedBy.gameObject.ToString()+"(Clone)");
		GameObject stuff = col.gameObject;
		stuff.name = stuff.name.Substring(0,stuff.name.Length-7); // remove "(Clone)" at end of name
		if(stuff.ToString() == destroyedBy.ToString())
		{
			Destroy(this.gameObject);
		}
	}
}
