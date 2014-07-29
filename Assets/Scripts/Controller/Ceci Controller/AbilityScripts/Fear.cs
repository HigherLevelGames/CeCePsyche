using UnityEngine;
using System.Collections;

public class Fear : Ability
{
	public float Percentage = 0.5f;
	//public float TimeToShrink = 3.0f;
	private float TimeToShrink = 0.5f;
	private float StartTime = 0.0f;
	private bool isSmall = false;

	// Use this for initialization
	void Start () { }

	// LateUpdate so we can override animation transform.localScale values
	void LateUpdate ()
	{
		if(isSmall && this.transform.localScale != Percentage*Vector3.one)
		{
			Shrink(1.0f, Percentage);
		}
		else if(!isSmall && this.transform.localScale != Vector3.one)
		{
			Shrink(Percentage, 1.0f);
		}
	}

	public override void UseAbility()
	{
		// play frightened anim
		// Jyordana TODO (Done?)

		// create frightened particle FX
		// Jason TODO

		// half transform size
		StartTime = Time.time;
		isSmall = !isSmall;
	}

	public override void EndAbility()
	{
		// stop particle FX
		// Jason TODO

		// double transform size
		StartTime = Time.time;
		isSmall = !isSmall;
	}

	void Shrink(float start, float end)
	{
		float amount = Mathf.SmoothStep(start, end, (Time.time-StartTime)/TimeToShrink);
		this.transform.localScale = new Vector3(amount, amount, amount);
	}
}
