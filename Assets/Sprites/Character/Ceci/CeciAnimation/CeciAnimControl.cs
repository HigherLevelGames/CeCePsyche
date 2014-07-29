using UnityEngine;
using System.Collections;

[RequireComponent(typeof(HMovementController))]
[RequireComponent(typeof(VMovementController))]
[RequireComponent(typeof(Animator))]
public class CeciAnimControl : MonoBehaviour
{
	Animator anim;
	HMovementController hControl;
	VMovementController vControl;

	// Use this for initialization
	void Start ()
	{
		anim = this.GetComponent<Animator>();
		hControl = this.GetComponent<HMovementController>();
		vControl = this.GetComponent<VMovementController>();
	}

	//int jumpHash = Animator.StringToHash("Jump");
	//int runStateHash = Animator.StringToHash("Base Layer.Run");

	// Update is called once per frame
	void Update ()
	{
		//float move = Input.GetAxis ("Vertical");
		//anim.SetFloat("Speed", move);

		/*
		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
		if(Input.GetKeyDown(KeyCode.Space) && stateInfo.nameHash == runStateHash)
		{
			anim.SetTrigger (jumpHash);
		}//*/

		anim.SetInteger("hspeed",hControl.hSpeed);
		anim.SetInteger("vspeed",vControl.vSpeed);
		anim.SetBool("grounded", vControl.isGrounded);

		/*
		anim.SetBool("floating");
		anim.SetBool("grounded");
		anim.SetBool("climbing");
		anim.SetTrigger("CryTrigger");
		anim.SetTrigger("RageTrigger");
		anim.SetTrigger("ShrinkTrigger");
		anim.SetTrigger("GrowTrigger");
		anim.SetTrigger("FloatTrigger");
		anim.SetInteger("vspeed");
		//*/
	}
}
