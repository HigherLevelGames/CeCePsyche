using UnityEngine;
using System.Collections;

public class Respawn : MonoBehaviour
{
	public float groundPos = -5.0f;
	private Vector3 checkpoint;

	// Use this for initialization
	void Start ()
	{
		checkpoint = this.transform.position;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(this.transform.position.y < groundPos)
		{
			GoToCheckpoint();
		}
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.tag == "CheckPoint")
		{
			checkpoint = col.gameObject.transform.position;
			// JASON TODO add some particle effect or other indicator that the checkpoint is active
		}
	}

	// enemies call this method
	void GoToCheckpoint()
	{
		this.transform.position = checkpoint;
	}
}
