using UnityEngine;
using System.Collections;
using Common;

public class CeciMenuController : MonoBehaviour
{
	public float speed = 5.0f;
	private GameObject target;
	private Vector3 direction
	{
		get
		{
			return (target.transform.position - this.transform.position).normalized;
		}
	}

	// Use this for initialization
	void Start ()
	{
		target = new GameObject();
		target.transform.parent = this.gameObject.transform.parent;
	}
	
	// Update is called at regular intervals
	void FixedUpdate ()
	{
		Vector3 dir = new Vector3(RebindableInput.GetAxis("Horizontal"), RebindableInput.GetAxis("Vertical"));
		if(dir!=Vector3.zero)
		{
			// using arrow keys
			this.transform.position += dir*speed*Time.deltaTime;
			//this.transform.position = Utility.Clamp(
			//		this.transform.position,
			//		new Vector3(-6.0f,-4.0f,0.0f), // min
			//		new Vector3(6.0f,4.0f,0.0f)); // max
			target.transform.position = this.transform.position;
		}

		// Move to target
		if(Vector3.Distance(this.transform.position, target.transform.position) > 0.5f)
		{
			this.transform.position += direction*speed*Time.deltaTime;
		}

		// Make it so Ceci always stays on screen
		Vector3 lowerLeft =
				Camera.main.ScreenToWorldPoint(new Vector3(0.2f*Screen.width, 0.2f*Screen.height, 10.0f));
		Vector3 upperRight =
				Camera.main.ScreenToWorldPoint(new Vector3(0.8f*Screen.width, 0.8f*Screen.height, 10.0f));
		this.transform.position = Utility.Clamp(this.transform.position, lowerLeft, upperRight);
		target.transform.position = Utility.Clamp(target.transform.position, lowerLeft, upperRight);
	}
	
	void GoTo(GameObject newTarget)
	{
		target.transform.position = newTarget.transform.position;
	}
}
