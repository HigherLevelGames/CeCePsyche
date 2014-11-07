using UnityEngine;
using System.Collections;

public class SpringScript : MonoBehaviour
{
	public float TargetY; //This is the YSurface in "Water" which is the height of the mesh
	public float Speed;
	public float Displacement;
	public float Damping;
	public float Tension;
	public int ID;
	public Water Water; //The "Water" script set this upon instantiating of each spring.
	public Vector3 OriginalPosition;
	public float MaxDecrease;
	public float MaxIncrease;
	public float WaveHeight;
	public float WaveSpeed;

	void  Start ()
	{
		OriginalPosition = this.transform.localPosition;//transform.position;
	}

	void  FixedUpdate ()
	{
		//This is the Spring effect that makes the water bounce and stuff
		Displacement = TargetY - this.transform.localPosition.y;
		Speed += Tension * Displacement - Speed * Damping;
		this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y + Speed, this.transform.localPosition.z);
		
		//Limiting the waves
		if(this.transform.localPosition.y < OriginalPosition.y + MaxDecrease)
		{
			this.transform.localPosition = new Vector3(this.transform.localPosition.x, OriginalPosition.y + MaxDecrease, this.transform.localPosition.z);
			Speed = 0;
		}
		if(this.transform.localPosition.y > OriginalPosition.y + MaxIncrease)
		{
			this.transform.localPosition = new Vector3(this.transform.localPosition.x, OriginalPosition.y + MaxIncrease, this.transform.localPosition.z);
			Speed = 0;
		}
	}

	//Create a splash effect by calling Splash() function in the "Water" script.
	void  OnTriggerEnter2D ( Collider2D other  )
	{
		MovementController vControl = other.gameObject.GetComponent<MovementController>();
		if(vControl != null)
		{
			Water.Splash(vControl.velocity.y, ID, other.transform);
		}
		else
		{
			Water.Splash(other.collider2D.rigidbody2D.velocity.y,ID,other.transform);
		}

		//Here you can access the script on the "other" object and call a specific function
		//ScriptName ScriptName;
		//ScriptName = other.transform.GetComponent<"ScriptName">() as ScriptName;
		//ScriptName.FunctionName(); //Ex Call ChangeWaterState();
	}
	//Velocity of the body, The ID is used to identify this specific spring and other.transform is sent for preventing the object from continuous collision with the water.
}