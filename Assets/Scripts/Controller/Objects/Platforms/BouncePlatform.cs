using UnityEngine;
using System.Collections;

public class BouncePlatform : MonoBehaviour
{
	public float maxVelocity = 30.0f;
	public float multiplier = -1.5f;

	void OnCollisionEnter2D(Collision2D col)
	{
		if(col.gameObject.tag == "Player")
		{
			MovementController vControl = col.gameObject.GetComponent<MovementController>();
			if(vControl.velocity.y < 0)
			{
				float temp = vControl.velocity.y * multiplier;
				vControl.velocity.y = Mathf.Clamp(temp, 0.0f, maxVelocity);
			}
		}
	}

}
