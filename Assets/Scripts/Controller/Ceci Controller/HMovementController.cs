using UnityEngine;
using System.Collections;

public class HMovementController : MonoBehaviour
{
	public bool isFacingRight = true; // default
	float MaxSpeed = 10.0f;
	
	void Start(){}
	
	void FixedUpdate()
	{
		//call these function every frame
		Movement();
	}
	
	void Movement()
	{
		float newX = this.transform.position.x;

		// velocity = speed + direction
		newX += Input.GetAxis("Horizontal") * MaxSpeed * Time.deltaTime;
		if(Input.GetAxis("Horizontal") != 0) // check needed in case standing still
		{
			isFacingRight = Input.GetAxis("Horizontal") > 0;
		}
		
		this.transform.position = new Vector2(newX, this.transform.position.y);
	}
}
