using UnityEngine;
using System.Collections;

public class Conveyor : MonoBehaviour
{
	public bool isLeft = true;
	public float speed = 5.0f;
	
	void OnTriggerStay2D(Collider2D col)
	{
		Vector3 transpose = (isLeft) ? Vector3.left : Vector3.right;
		col.gameObject.transform.position += transpose * speed * Time.deltaTime;
	}
}
