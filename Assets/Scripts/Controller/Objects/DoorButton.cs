using UnityEngine;
using System.Collections;

public class DoorButton : MonoBehaviour {
	public Door door;

	private Animator anim;

	void Start()
	{
		anim = this.GetComponent<Animator>();
	}
	void OnTriggerEnter2D(Collider2D col)
	{
		anim.SetBool("Pressed", true);
	}

	void OnTriggerExit2D(Collider2D col)
	{
		anim.SetBool("Pressed", false);
	}
}
