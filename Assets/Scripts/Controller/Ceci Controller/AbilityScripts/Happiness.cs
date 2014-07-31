using UnityEngine;
using System.Collections;

[RequireComponent (typeof(VMovementController))]
public class Happiness : Ability
{
	private bool isHappy = false;

	// Use this for initialization
	void Start () { }
	
	// Update is called once per frame
	void Update ()
	{
		if(isHappy)
		{
			JumpControl();
		}
	}

	public override void UseAbility()
	{
		// play giggling anim
		// Jyordana TODO

		// create happy particle FX
		// Jason TODO

		// set flag in controller to have CeCi float in the air
		this.GetComponent<VMovementController>().lockVertical = true;
		this.rigidbody2D.gravityScale = 0.0f;
		isHappy = true;
	}

	public override void EndAbility()
	{
		// stop particle FX
		// Jason TODO

		// stop ability to float/fly
		this.GetComponent<VMovementController>().lockVertical = false;
		this.rigidbody2D.gravityScale = 1.0f;
		isHappy = false;
	}

	// Does not work with ladders
	float prevVValue = 0.0f;
	void JumpControl()
	{
		float curVValue = Input.GetAxis("Vertical");
		
		// pressed jump once
		if(RebindableInput.GetKeyDown("Jump") || (curVValue > 0.0f && prevVValue == 0.0f))
		{
		}
		
		// press and hold jump button
		if(RebindableInput.GetKey("Jump") || curVValue > 0.0f)
		{
			this.rigidbody2D.gravityScale = -0.1f;
		}

		// player trying to move downwards for whatever reason
		if(curVValue < 0.0f)
		{
			this.rigidbody2D.gravityScale = 0.1f;
		}
		
		// released Jump Button
		if(RebindableInput.GetKeyUp("Jump") || (curVValue == 0.0f && prevVValue != 0.0f))
		{
			this.rigidbody2D.gravityScale = 0.0f;
		}

		prevVValue = RebindableInput.GetAxis("Vertical");
	}
}
