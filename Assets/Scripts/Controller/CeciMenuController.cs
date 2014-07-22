using UnityEngine;
using System.Collections;

public class CeciMenuController : MonoBehaviour
{
	GameObject target;
	float speed = 5.0f;
	Vector3 direction
	{
		get
		{
			return (target.transform.position - this.transform.position).normalized;
		}
	}

	public Renderer enterButton; // "Press Enter to Begin" png

	// Use this for initialization
	void Start ()
	{
		target = this.gameObject;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Vector3.Distance(this.transform.position, target.transform.position) > 0.5f)
		{
			this.transform.position += direction*speed*Time.deltaTime;
		}
	}

	void GoTo(GameObject position)
	{
		target = position;
	}

	/* // I prefer this code to be here
	void OnTriggerEnter2D(Collider2D other)
	{
		if(enterButton != null)
		{
			enterButton.enabled = true;
		}
	}
	
	void OnTriggerStay2D(Collider2D other)
	{
		if (Input.GetKeyDown(KeyCode.Return))
		{
			other.SendMessage("loadTheLevel");
		}
	}
	
	void OnTriggerExit2D(Collider2D other)
	{
		if(enterButton != null)
		{
			enterButton.enabled = false;
		}
	}//*/
}
