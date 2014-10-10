using UnityEngine;
using System.Collections;

public class ControlPanel : MonoBehaviour {
	Animator anim;
	public AudioClip beepSound;
	public AudioClip buzzerSound;
	public AudioSource buzzer;
	public Door door;
	public bool isActive;
	public float activeTime = 1f;
	float timeLeft;

	// Use this for initialization
	void Start () {
		anim = this.GetComponent<Animator>();
		timeLeft = activeTime;
	}
	
	// Update is called once per frame
	void Update () {
		anim.SetBool("isActive",isActive);
		if(isActive)
		{
			timeLeft -= Time.deltaTime;
			if (timeLeft < 0)
			{
				isActive = false;
				timeLeft = activeTime;
			}
		}
	}

	void Interact () {
		if(!isActive)
		{
			isActive = true;
		}
		if(door != null)
		{
			bool isOpen = door.isOpen;
			bool autoClose = door.autoClose;
			if(isOpen && !autoClose)
			{
				door.isOpen = false;
			}

			if(!isOpen)
			{
				door.isOpen = true;
			}
		}

		if(buzzer != null)
		{
		}
	}
}
