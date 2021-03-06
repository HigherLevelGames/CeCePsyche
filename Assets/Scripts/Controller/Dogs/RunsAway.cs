﻿using System; // Need this!
using UnityEngine;
using System.Collections;

public class RunsAway : Conditionable //MonoBehaviour
{
	public Transform endPos;
	public float speed = 1.0f;

	private Bar responseLevel;
	private Vector3 pos1 = Vector3.zero; // start position
	private Vector3 pos2 = Vector3.zero; // end position
	private Vector3 target;
	private bool isMovingToEnd = true;
	private bool blastoff = false;
	private Vector3 Direction
	{
		get
		{
			return (target - this.transform.position).normalized;
		}
	}
	
	// Use this for initialization
	void Start ()
	{
		pos1 = this.transform.position;
		pos2 = endPos.position;
		target = pos2;
		blastoff = false;
		responseLevel = this.GetComponent<Bar>();
		WeakResponseEvent += travelShort; // add whatever methods you want
		StrongResponseEvent += travelFar; // add whatever methods you want
	}

	// EventArgs is in System
	void travelShort(object sender, EventArgs e)
	{
		responseLevel.SendMessage("increaseBar", 0.5f);
		blastoff = true;
	}

	void travelFar(object sender, EventArgs e)
	{
		responseLevel.SendMessage("increaseBar", 1.0f);
		blastoff = true;
	}
	
	void FixedUpdate ()
	{
		if(responseLevel.BarFill > 0)
			responseLevel.decreaseBar(.1f);
		if(Vector3.Distance(this.transform.position, target) <= 1.0f)
		{
			// reached target, swap target
			isMovingToEnd = !isMovingToEnd;
			target = isMovingToEnd ? pos2 : pos1;
			blastoff = false;
		}
		
		// move to target
		if(blastoff)
			this.transform.position += Direction * speed * Time.deltaTime;
	}


	void Interact()
	{
		//this.SendMessage("PerformResponse", true);
		// technically false, since we're using interact,
		// but we want to trigger the the strong unconditioned response for demonstration purposes
		// true = Unconditioned Stimulus (Interact)
		// false = Conditioned Stimulus (UseMe)

		//print ("blastoff!");
		//blastoff = true;
	}
}
