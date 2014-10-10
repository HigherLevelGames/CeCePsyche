using UnityEngine;
using System.Collections;

public class Conveyor : MonoBehaviour
{
	public bool isLeft = true;
	public float speed = 5.0f;
	public GameObject wheel1;
	public GameObject wheel2;

	void Update()
	{
		if(wheel1 != null)
		{
			wheel1.transform.RotateAround(
					wheel1.transform.position,
					Vector3.forward,
					(isLeft) ? speed : -speed);
		}
		if(wheel2 != null)
		{
			wheel2.transform.RotateAround(
					wheel2.transform.position,
					Vector3.forward,
					(isLeft) ? speed : -speed);
		}
	}

	void OnTriggerStay2D(Collider2D col)
	{
		Vector3 transpose = (isLeft) ? Vector3.left : Vector3.right;
		col.gameObject.transform.position += transpose * speed * Time.deltaTime;
	}
}
