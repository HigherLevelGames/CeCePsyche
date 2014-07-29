using UnityEngine;
using System.Collections;

public class HMovementController : MonoBehaviour
{
	// hspeed for CeciAnimControl.cs
	public int hSpeed
	{
		get
		{
			if(Mathf.Abs(newX-prevX) < 0.1f)
			{
				return 0;
			}
			else
			{
				return (int)Mathf.Sign(newX-prevX);
			}
		}
	}

	public bool isFacingRight = true; // default
	float MaxSpeed = 10.0f;
	private Quaternion reverseRotation = new Quaternion(0.0f,180.0f,0.0f,0.0f);
	private float prevX = 0.0f;
	private float newX = 0.0f;

	void Start() {}
	
	void FixedUpdate()
	{
		//call these function every frame
		Movement();
	}
	
	void Movement()
	{
		prevX = this.transform.position.x;

		// velocity = speed + direction
		newX = prevX + Input.GetAxis("Horizontal") * MaxSpeed * Time.deltaTime;
		if(Input.GetAxis("Horizontal") != 0) // check needed in case standing still
		{
			isFacingRight = Input.GetAxis("Horizontal") > 0;
		}
		
		this.transform.position = new Vector2(newX, this.transform.position.y);

		if(isFacingRight)
		{
			this.transform.rotation = Quaternion.identity;
		}
		else
		{
			this.transform.rotation = reverseRotation;
		}
	}
}
