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
	public float effectArea;
	GameObject[] dogs;
	float timeLeft;

	// Use this for initialization
	void Start () {
		dogs = GameObject.FindGameObjectsWithTag("Dog");
		anim = this.GetComponent<Animator>();
		timeLeft = activeTime;
		effectArea = 10;
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
			//sound buzzer

			for( int i = 0; i < dogs.Length; i++)
			{
				float distance = Vector3.Distance(dogs[i].transform.position, this.transform.position);
				if (distance <= effectArea)
				{
					//ellicit response from dog
					Dog thisDog = dogs[i].GetComponent<Dog>();
					thisDog.SendMessage("PerformResponse", false);
					print ("doggie hears");

				}
			}
		}
	}
}
