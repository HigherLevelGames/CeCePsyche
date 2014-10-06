using UnityEngine;
using System.Collections;

public class TransformCamera : MonoBehaviour {
	public Transform cameraSpot;
	public Vector3 moveEnterPos;
	public Vector3 moveExitPos;

	void OnTriggerEnter2D( Collider2D collider)
	{
		if(collider.tag == "Player"){
			//print("entered");
			cameraSpot.position += moveEnterPos;
		}
	}

	void OnTriggerExit2D( Collider2D collider)
	{
		if(collider.tag == "Player"){
			//print("entered");
			cameraSpot.position += moveExitPos;
		}
	}
}
