using UnityEngine;
using System.Collections;

public class FlyAroundTarget : MonoBehaviour
{
	public GameObject target;
	public float speed = 5.0f;
	public float height = 5.0f;
	public float xDis = 2.0f;
	public float yDis = 1.0f;
	private bool isFacingRight = true;
	private Quaternion reverseRotation = new Quaternion(0.0f,180.0f,0.0f,0.0f);
	private Vector2 direction
	{
		get
		{
			return new Vector2(target.transform.position.x - this.transform.position.x, 0);
		}
	}
	private Vector2 TargetPos
	{
		get
		{
			return new Vector2(target.transform.position.x, target.transform.position.y + height);
		}
	}
	
	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(direction.x > 0)
		{
			isFacingRight = true;
		}
		else if(direction.x < 0)
		{
			isFacingRight = false;
		}
		
		// face left or right by changing the y rotation value
		if(isFacingRight)
		{
			this.transform.rotation = Quaternion.identity;
		}
		else
		{
			this.transform.rotation = reverseRotation;
		}

		float theta = Time.time;
		this.transform.position = TargetPos + new Vector2(xDis * Mathf.Cos(theta), yDis * Mathf.Sin(theta));
	}
}
