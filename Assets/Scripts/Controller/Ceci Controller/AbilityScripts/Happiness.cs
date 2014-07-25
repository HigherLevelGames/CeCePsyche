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
		//this.GetComponent<VMovementController>().isFloatingActive = true;
		this.rigidbody2D.gravityScale = 0.0f;
		isHappy = true;
	}

	public override void EndAbility()
	{
		// stop particle FX
		// Jason TODO

		// stop ability to float/fly
		this.GetComponent<VMovementController>().lockVertical = false;
		//this.GetComponent<VMovementController>().isFloatingActive = false;
		this.rigidbody2D.gravityScale = 1.0f;
		isHappy = false;
	}

	// Does not work with ladders
	float prevVValue = 0.0f;
	void JumpControl()
	{
		float curVValue = Input.GetAxis("Vertical");
		
		// pressed jump once
		if(Input.GetButtonDown("Jump") || (curVValue > 0.0f && prevVValue == 0.0f))
		{
		}
		
		// press and hold jump button
		if(Input.GetButton("Jump") || curVValue > 0.0f)
		{
			this.rigidbody2D.gravityScale = -0.1f;
		}
		
		// released Jump Button
		if(Input.GetButtonUp("Jump") || (curVValue == 0.0f && prevVValue != 0.0f))
		{
			this.rigidbody2D.gravityScale = 0.0f;
		}

		prevVValue = Input.GetAxis("Vertical");
	}
}
