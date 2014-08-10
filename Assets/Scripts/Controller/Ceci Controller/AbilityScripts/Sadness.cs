using UnityEngine;
using System.Collections;

public class Sadness : Ability
{
	// Use this for initialization
	void Start () { }
	
	// Update is called once per frame
	void Update () { }

	public override void UseAbility()
	{
		// create tear particle FX
		// Jason TODO

		// Instantiate tears objects moving in left & right directions
		// grow nearby flora upon contact see Plant.cs
		// Jordan TODO

		// Push Bully back upon contact
		// Jordan TODO
	}

	public override void EndAbility()
	{
		// stop particle FX
		// Jason TODO
	}
}
