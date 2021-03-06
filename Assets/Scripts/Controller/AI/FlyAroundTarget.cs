﻿using UnityEngine;
using System.Collections;

public class FlyAroundTarget : MonoBehaviour
{
	public GameObject target;
	public float speed = 5.0f;
	public Vector2 pivot = new Vector2(0.0f, 5.0f); // xOffset, height
	public Vector2 pivotDistance = new Vector2(2.0f, 1.0f); // xDis, yDis
	private bool isFacingRight = true;
	private Quaternion reverseRotation = new Quaternion(0.0f,180.0f,0.0f,0.0f);
	private Vector2 direction
	{
		get
		{
			return new Vector2(target.transform.position.x - this.transform.position.x, 0);
		}
	}
	private Vector2 prevPos = Vector2.zero;
	private Vector2 TargetPos
	{
		get
		{
			return new Vector2(target.transform.position.x + pivot.x, target.transform.position.y + pivot.y);
		}
	}

	// Update is called once per frame
	void Update ()
	{
		float theta = Time.time;
		Vector3 newPos = TargetPos + new Vector2(pivotDistance.x * Mathf.Cos(theta), pivotDistance.y * Mathf.Sin(theta));
		if(newPos.x < prevPos.x)
		{
			isFacingRight = false;
		}
		else if(newPos.x > prevPos.x)
		{
			isFacingRight = true;
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

		this.transform.position = newPos;
		prevPos = this.transform.position;
	}
}
