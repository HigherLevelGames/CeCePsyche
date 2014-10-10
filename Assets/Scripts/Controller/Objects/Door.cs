using UnityEngine;
using System.Collections;

public class Door: MonoBehaviour {

	Animator anim;
	public bool isOpen;
	// Use this for initialization
	void Start () {
		anim = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		anim.SetBool("isOpen", isOpen);
	}
}
