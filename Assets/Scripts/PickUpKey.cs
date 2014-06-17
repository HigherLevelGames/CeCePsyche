using UnityEngine;
using System.Collections;

public class PickUpKey : MonoBehaviour {
	
	public GameObject message; 
	
	void Start(){
		message.renderer.enabled = false;
		PlayerPrefs.SetInt ("hasKey", 0);
	}
	
	// Use this for initialization
	void OnCollisionEnter2D (Collision2D coll) 
	{
		if (coll.gameObject.tag == "Player")
		{
			audio.Play();
			collider2D.enabled = false;
			renderer.enabled = false; 
			PlayerPrefs.SetInt("hasKey",1);
			message.renderer.enabled = true; 
			StartCoroutine(Example());
			//Destroy(message, 2);
			//instead of deleting you can just turn the renderer off. 
		}	
	}
	
	
	IEnumerator Example() {
		yield return new WaitForSeconds(2);
		message.renderer.enabled = false;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
