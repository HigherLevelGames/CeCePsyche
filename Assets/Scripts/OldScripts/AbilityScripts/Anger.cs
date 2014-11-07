using UnityEngine;
using System.Collections;

//[RequireComponent (typeof(HMovementController))]
public class Anger : Ability
{
	float speed = -10.0f;

	// Use this for initialization
	void Start () { }
	
	// Update is called once per frame
	void Update () { }

	public override void UseAbility()
	{
		// play raging anim
		// Jyordana TODO

		// create firey particle FX
		// Jason TODO

		// Deion's dash ability
		float force = speed;
		if(this.GetComponent<MovementController>().isFacingRight)
		{
			force *= -1.0f;
		}
		this.rigidbody2D.velocity = new Vector2(force, this.rigidbody2D.velocity.y);
	}

	public override void EndAbility()
	{
		// stop particle FX
		// Jason TODO
	}
}
