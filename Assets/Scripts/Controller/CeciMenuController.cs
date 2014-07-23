using UnityEngine;
using System.Collections;

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
}
