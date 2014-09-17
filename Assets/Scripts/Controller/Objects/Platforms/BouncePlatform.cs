using UnityEngine;
using System.Collections;

public class BouncePlatform : MonoBehaviour
{
	public float maxVelocity = 10.0f;
	public float multiplier = -1.5f;

	void OnCollisionEnter2D(Collision2D col)
	{
		if(col.gameObject.tag == "Player")
		{
			VMovementController vControl = col.gameObject.GetComponent<VMovementController>();
			if(vControl.VVelocity < 0)
			{
				vControl.VVelocity *= multiplier;
				Mathf.Clamp(vControl.VVelocity, 0.0f, maxVelocity);
			}
		}
	}

}
