using UnityEngine;
using System.Collections;

public class Stimulus1 : MonoBehaviour
{
	public GameObject bully;
	private AbilityManager emoControl;

	// Use this for initialization
	void Start ()
	{
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		emoControl = player.GetComponent<AbilityManager>();
	}
	
	// Update is called once per frame
	void Update () { }

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.tag == "Player")
		{
			// activate some script animation sequence to show how Ceci's unconditioned response
			bully.SetActive(true);

			// tell controller (most likely subconcious) to call certain ability on Ceci
			emoControl.SetEmotion(AbilityManager.Emotion.Happy);
			emoControl.SendMessage("SetCheckpoint", this.transform.position);

			// after three stimuli, Ceci will gain the conditioned stimulus

			// deactivate this stimulus
			Destroy(this.gameObject);
		}
	}
}
