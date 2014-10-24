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
			VMovementController vControl = col.gameObject.GetComponent<VMovementController>();
			if(vControl.VVelocity.y < 0)
			{
				float temp = vControl.VVelocity.y * multiplier;
				vControl.VVelocity.y = Mathf.Clamp(temp, 0.0f, maxVelocity);
			}
		}
	}

}
