using UnityEngine;
using System.Collections;

public class Door: MonoBehaviour {
	
	public bool isOpen;
	public bool autoClose = false;
	public float closeTime = 5f;
	public string sceneToLoad;
	private bool charEntered;
	private GameObject player;


	Animator anim;
	float timeLeft;

	// Use this for initialization
	void Start () {
		anim = this.GetComponent<Animator>();
		timeLeft = closeTime;
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if(anim != null)
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

		if(charEntered)
		{

			timeLeft -= Time.deltaTime;
			if (timeLeft < 1.3)
			{
				isOpen = false;
			}

			if(timeLeft < 0)
			{
				Application.LoadLevel(sceneToLoad);
			}
		}

	}

	void LateUpdate()
	{
		//if(charEntered)
		//	Shrink(1f,.8f, player);
	}

	void OnTriggerStay2D(Collider2D col)
	{
		if(col.gameObject.tag == "Player" && isOpen && RebindableInput.GetKeyDown("Interact"))
		{
			Animator charAnim = col.gameObject.GetComponent<Animator>();
			charAnim.SetTrigger("WalkInTrigger");
			charEntered = true;
			timeLeft = 2f;
			this.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 3;

		}
	}

	void Shrink(float start, float end, GameObject tobeShrunk)
	{
		float amount = Mathf.SmoothStep(start, end, 1000f);
		tobeShrunk.transform.localScale = new Vector3(amount, amount, amount);
	}
}
