using UnityEngine;
using System.Collections;

public class ConveyorBeltIntro : MonoBehaviour {
	public AudioClip buzzer;
	public AudioSource speaker;
	public GameObject explosion;
	void OnTriggerEnter(Collider collider)
	{
		if(collider.tag == "Dog")
		{
			speaker.PlayOneShot(buzzer);
		}
	}
	void OnTriggerExit(Collider collider)
	{
		if(collider.tag == "Dog")
		{
			Instantiate(explosion,this.transform.position, Quaternion.identity);
		}
	}
}
