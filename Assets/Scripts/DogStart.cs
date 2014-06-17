using UnityEngine;
using System.Collections;

public class DogStart : MonoBehaviour 
{
	private Animator anim;
	private PlayerControl playerControl;
	public GameObject message; 


	//ATTACB THIS TO PLAYER CHARACTER!!!!!!!
	
	void Awake()
	{
		playerControl = GetComponent<PlayerControl>();
		anim = GetComponent<Animator>();
		PlayerPrefs.SetInt ("spawn", 0);
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		// If the player hits the trigger...
		if (col.gameObject.tag == "DogStart") {
			// .. stop the camera tracking the player
			message.renderer.enabled = true;
			GetComponent<PlayerControl>().enabled = false;
			anim.SetFloat("speed", 0);
			//collider2D.enabled = false; 
			//Destroy (rigidbody2D);
			PlayerPrefs.SetInt("spawn", 1);
			StartCoroutine(Example());
			Destroy (col.gameObject);
		}
	}
	
	
	void OnTriggerExit2D(Collider2D col){
		//Add Rigidbody back on	
	}
	

	IEnumerator Example() {
		yield return new WaitForSeconds(5);
		GetComponent<PlayerControl>().enabled = true;
	}
}