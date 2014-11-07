using UnityEngine;
using System.Collections;

//[RequireComponent (typeof(HMovementController))]
//[RequireComponent (typeof(VMovementController))]
public class Fear : Ability
{
	public float Percentage = 0.5f;
	//public float TimeToShrink = 3.0f;
	private float TimeToShrink = 0.5f;
	private float StartTime = 0.0f;
	private bool isSmall = false;
	//private HMovementController hControl;
	//private VMovementController vControl;
	private MovementController control;

	// Use this for initialization
	void Start ()
	{
		control = this.GetComponent<MovementController>();
		//hControl = this.GetComponent<HMovementController>();
		//vControl = this.GetComponent<VMovementController>();
	}

	// LateUpdate so we can override animation transform.localScale values
	void LateUpdate ()
	{
		if(isSmall && this.transform.localScale != Percentage*Vector3.one)
		{
			//Shrink(1.0f, Percentage);
		}
		else if(!isSmall && this.transform.localScale != Vector3.one)
		{
			//Shrink(Percentage, 1.0f);
		}
	}

	public override void UseAbility()
	{
		// create frightened particle FX
		// Jason TODO

		// half transform size
		StartTime = Time.time;
		isSmall = !isSmall;
		control.MaxSpeed *= 0.5f;
		control.JumpSpeed *= 0.5f;
	}

	public override void EndAbility()
	{
		// stop particle FX
		// Jason TODO

		// double transform size
		StartTime = Time.time;
		isSmall = !isSmall;
		control.MaxSpeed *= 2.0f;
		control.JumpSpeed *= 2.0f;
	}

	void Shrink(float start, float end)
	{
		float amount = Mathf.SmoothStep(start, end, (Time.time-StartTime)/TimeToShrink);
		this.transform.localScale = new Vector3(amount, amount, amount);
	}
}
