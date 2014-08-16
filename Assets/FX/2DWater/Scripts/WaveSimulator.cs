using UnityEngine;
using System.Collections;

public class WaveSimulator : MonoBehaviour
{
	public float WaveSpeed;
	public float WaveHeight;
	private float OriginalY;

	void  Start ()
	{
		OriginalY = this.transform.localPosition.y;
	}

	void  FixedUpdate ()
	{
		this.transform.localPosition = new Vector3(this.transform.localPosition.x, OriginalY + Mathf.Sin(Time.time * WaveSpeed) * WaveHeight, this.transform.localPosition.z);
		//if(transform.position.y < 0.5f){
		//	transform.position.y = 0.5f;
		//}
	}
}