using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AbilityManager))]
[RequireComponent(typeof(HMovementController))]
[RequireComponent(typeof(VMovementController))]
[RequireComponent(typeof(Animator))]
public class CeciAnimControl : MonoBehaviour
{
	Animator anim;
	HMovementController hControl;
	VMovementController vControl;
	AbilityManager emoControl;

	// Use this for initialization
	void Start ()
	{
		anim = this.GetComponent<Animator>();
		hControl = this.GetComponent<HMovementController>();
		vControl = this.GetComponent<VMovementController>();
		emoControl = this.GetComponent<AbilityManager>();
	}

	//int jumpHash = Animator.StringToHash("Jump");
	//int runStateHash = Animator.StringToHash("Base Layer.Run");

	// Update is called once per frame
	void Update ()
	{
		/*
		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
		if(Input.GetKeyDown(KeyCode.Space) && stateInfo.nameHash == runStateHash)
		{
			anim.SetTrigger (jumpHash);
		}//*/

		anim.SetInteger("hspeed", hControl.hSpeed);
		anim.SetInteger("vspeed", vControl.vSpeed);
		anim.SetBool("grounded", vControl.isGrounded);
		anim.SetBool("floating", emoControl.isFloating);
		// anim.SetBool("climbing");
	}

	void TriggerEmotionAnim(int index)
	{
		switch(index)
		{
		case 0: // Happy
			anim.SetTrigger("FloatTrigger");
			break;
		case 1: // Sad
			anim.SetTrigger("CryTrigger");
			break;
		case 2: // Scared
			if(emoControl.isFrightened)
			{
				anim.SetTrigger("GrowTrigger");
			}
			else
			{
				anim.SetTrigger("ShrinkTrigger");
			}
			break;
		case 3: // Mad
			anim.SetTrigger("RageTrigger");
			break;
		default:
			break;
		}
	}
}
