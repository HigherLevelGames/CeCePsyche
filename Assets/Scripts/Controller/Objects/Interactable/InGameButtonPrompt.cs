using UnityEngine;
using System.Collections;

public class InGameButtonPrompt : MonoBehaviour {
	public AudioClip prompt;
	public AudioClip farewell;
	void OnTriggerEnter2D(Collider2D other) {
		renderer.enabled = true;
		audio.PlayOneShot (prompt);
	}
	void OnTriggerExit2D(Collider2D other) {
		renderer.enabled = false;
		audio.PlayOneShot (farewell);
	}
}
