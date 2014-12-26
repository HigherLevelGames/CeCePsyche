using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(AbilityManager))]
[RequireComponent(typeof(MovementController))]
[RequireComponent(typeof(Animator))]
public class CeciAnimControl : MonoBehaviour
{
	private Animator anim;
	private MovementController moveControl;
	//private AbilityManager emoControl;
	
	// Use this for initialization
	void Start ()
	{
		anim = this.GetComponent<Animator>();
		moveControl = this.GetComponent<MovementController>();
		//emoControl = this.GetComponent<AbilityManager>();
	}
	
	//int jumpHash = Animator.StringToHash("Jump");
	//int runStateHash = Animator.StringToHash("Base Layer.Run");
	int climbingStateHash = Animator.StringToHash("Base Layer.Ceci_Climbing");
	
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
		//anim.SetBool("floating", emoControl.isFloating);
		anim.SetBool("climbing", moveControl.isClimbing);
		anim.SetBool("startClimbing", moveControl.isStartClimb);

		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
		if(stateInfo.nameHash == climbingStateHash && moveControl.vSpeed == 0.0f)
		{
			anim.speed = 0.0f;
		}/*
		else if(stateInfo.nameHash == climbingStateHash && moveControl.vSpeed < 0.0f)
		{
			anim.speed = -1.0f;
			//Debug.Log(anim.animation["Ceci_Climb"].wrapMode);//"Hi");
			//anim.animation["Ceci_Climb"].time = animation["Ceci_Climb"].length;
			//anim.animation["Ceci_Climb"].speed = -1;
			//anim.animation.Play("Ceci_Climb");
			//anim.animation["Base Layer.Ceci_Climbing"].wrapMode = WrapMode.PingPong;
		}*/
		else
		{
			anim.speed = 1.0f;
		}
		
		//Jesse Hack - using use F to trigger throw animation
		if(Input.GetKeyDown(KeyCode.F))
		{
			anim.SetTrigger("TossTrigger");
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
