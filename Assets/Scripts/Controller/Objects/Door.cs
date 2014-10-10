using UnityEngine;
using System.Collections;

public class Door: MonoBehaviour {
	
	public bool isOpen;
	public bool autoClose = false;
	public float closeTime = 5f;

	Animator anim;
	float timeLeft;

	// Use this for initialization
	void Start () {
		anim = this.GetComponent<Animator>();
		timeLeft = closeTime;
	}
	
	// Update is called once per frame
	void Update () {
		anim.SetBool("isOpen", isOpen);
		if(isOpen && autoClose)
		{
			timeLeft -= Time.deltaTime;
			if(timeLeft < 0)
			{
				timeLeft = closeTime;
				isOpen = false;
			}
		}

	}
}
