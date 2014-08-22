using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleEmitter))]
public class Geyser : MonoBehaviour
{
	private GameObject player;
	private AbilityManager emoControl;
	private float xDis
	{
		get
		{
			return Mathf.Abs(player.transform.position.x - this.transform.position.x);
		}
	}
	private float yDis
	{
		get
		{
			return Mathf.Abs(player.transform.position.y - this.transform.position.y);
		}
	}
	private bool isShootingWater = false;
	public float TimeToEmit = 5.0f;
	private float StartTime = 0.0f;
	private float StartHeight = 0.0f;
	private float Age
	{
		get
		{
			return Time.time - StartTime;
		}
	}

	// Use this for initialization
	void Start ()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		emoControl = player.GetComponent<AbilityManager>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(!emoControl.isNeutral && xDis < 2.0f)
		{
			ShootWater();
		}

		if(isShootingWater)
		{
			float newY = Mathf.SmoothStep(StartHeight,0.0f,Age/TimeToEmit);
			this.particleEmitter.localVelocity = new Vector3(0,newY,0);
			if(newY <= 0.0f)
			{
				isShootingWater = false;
			}
		}
		else if(this.particleEmitter.emit)
		{
			this.particleEmitter.emit = false;
		}
	}

	void ShootWater()
	{
		if(isShootingWater)
		{
			return;
		}
		// else
		StartTime = Time.time;
		isShootingWater = true;
		emoControl.SendMessage("Neutralize");
		StartHeight = yDis * 10.0f;
		this.particleEmitter.localVelocity = new Vector3(0,StartHeight,0);
		this.particleEmitter.emit = true;
	}
}
