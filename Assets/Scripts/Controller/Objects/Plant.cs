using UnityEngine;
using System.Collections;

public class Plant : MonoBehaviour
{
	bool hasGrown = false;

	// Use this for initialization
	void Start () { }
	
	// Update is called once per frame
	void Update () { }

	void OnTriggerStay2D(Collider2D col)
	{
		if(!hasGrown && col.gameObject.tag == "Player")
		{
			if(col.gameObject.GetComponent<AbilityManager>().isCrying)
			{
				Debug.Log ("growing");
				hasGrown = true;
				// instantiate platforms during/after animation ends
				// play growing SFX
				// play growing anim
			}
		}
	}
}
