using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(AbilityManager))]
[RequireComponent(typeof(MovementController))]
[RequireComponent(typeof(Animator))]
public class NattiAnimControl : MonoBehaviour
{
	private Animator anim;
	private MovementController moveControl;

	
	// Use this for initialization
	void Start ()
	{
		anim = this.GetComponent<Animator>();
		moveControl = this.GetComponent<MovementController>();
	}
	

	
	// Update is called once per frame
	void Update ()
	{
		/*
		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
		if(Input.GetKeyDown(KeyCode.Space) && stateInfo.nameHash == runStateHash)
		{
			anim.SetTrigger (jumpHash);
		}//*/

		anim.SetInteger("hspeed", moveControl.hSpeed);
		anim.SetInteger("vspeed", moveControl.vSpeed);
		anim.SetBool("grounded", moveControl.isGrounded);
	}

	void OnTriggerEnter2D(Collider2D col)
	{

		if(col.gameObject.tag == "Player")
		{
			print ("entered");
			anim.SetTrigger("TrigWake");
		}
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if(col.gameObject.tag == "Player")
		{
			print ("collided");
			anim.SetTrigger("TrigAttention");
		}
	}
	/*
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
		case 3: // Throw
			
			break;
		default:
			break;
		}
	}*/
}
