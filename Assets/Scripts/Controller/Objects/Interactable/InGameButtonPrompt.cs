using UnityEngine;
using System.Collections;

public class InGameButtonPrompt : MonoBehaviour {
	public AudioClip prompt;
	public AudioClip farewell;
    public AudioClip activate;

	void OnTriggerEnter2D(Collider2D other) {
        Prompt();
	}
	void OnTriggerExit2D(Collider2D other) {
        Deprompt();
	}
    public void Prompt()
    {
        renderer.enabled = true;
        audio.PlayOneShot(prompt);
    }
    public void Activate()
    {
        renderer.enabled = false;
        audio.PlayOneShot(activate);
    }
    public void Deactivate()
    {
        renderer.enabled = true;
        audio.PlayOneShot(activate);
    }
    public void Deprompt()
    {
        renderer.enabled = false;
        audio.PlayOneShot (farewell);
    }
}
