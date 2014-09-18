using UnityEngine;
using System.Collections;

public class RocketDog : MonoBehaviour
{
	public Transform endPos;
	public float speed = 1.0f;
	
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
	}
	
	void FixedUpdate ()
	{
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
	
	void OnTriggerStay2D(Collider2D player)
	{
		if(player.tag == "Player")
		{
			if(blastoff) 
			{
				// move player who's on top of platform
				player.transform.position += Direction * speed * Time.deltaTime;
			}
		}
	}

	void Interact()
	{
		print ("blastoff!");
		blastoff = true;
	}
}
