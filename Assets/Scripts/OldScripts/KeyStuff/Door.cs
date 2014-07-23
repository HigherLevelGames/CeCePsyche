using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {
	private PickUpKey key;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
			
	}
	void OnCollisionEnter2D(Collision2D coll)
	{
		if (PlayerPrefs.GetInt("hasKey") ==1)
		{
		   Application.LoadLevel("Sadness"); 
			
		}
	}
}
