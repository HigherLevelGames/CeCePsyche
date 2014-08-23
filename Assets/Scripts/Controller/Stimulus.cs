using UnityEngine;
using System.Collections;

public class Stimulus : MonoBehaviour
{
	public AbilityManager.Emotion EmotionTrigger;
	public string[] TextToSay;
	private GameObject player;
	
	// Use this for initialization
	void Start ()
	{
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.tag == "Player")
		{
			// activate some script animation sequence to show how Ceci's unconditioned response
			//player.SendMessage("TriggerEmotionAnim", (int)EmotionTrigger);

			// tell controller to call certain ability on Ceci
			player.SendMessage("SetEmotion", EmotionTrigger);
			
			// set the new checkpoint
			player.SendMessage("SetCheckpoint", this.transform.position);
			
			// have subconcious say something (if applicable)
			if(TextToSay != null)
			{
				GameObject subconcious = GameObject.FindGameObjectWithTag("Subconcious");
				subconcious.SendMessage("Say", TextToSay);
			}
			
			// after three stimuli, Ceci will gain the conditioned stimulus
			
			// deactivate this stimulus
			Destroy(this.gameObject);
		}
	}
}
