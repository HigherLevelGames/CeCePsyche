using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	public Camera MyCamera;
	public Transform PlayerTransform;
	public BoxCollider[] LevelBoxes;
	public float SpringSpeed = 2.0f;
	Vector2 Target;
	
	void Start () {
		Target = new Vector2 ();
	}
	void Update () { 
		Target = new Vector2 (PlayerTransform.position.x, PlayerTransform.position.y);
		Vector2 cameraPosition = new Vector2(MyCamera.transform.position.x, MyCamera.transform.position.y);
		cameraPosition += (Target - cameraPosition) * Time.deltaTime * SpringSpeed;
		if (CameraInBounds(new Vector3(cameraPosition.x, cameraPosition.y, 0)))
			MyCamera.transform.position = new Vector3 (cameraPosition.x, cameraPosition.y, MyCamera.transform.position.z);
	}
	bool CameraInBounds(Vector3 campos){
		for (int i = 0; i < LevelBoxes.Length; i++)
			if(LevelBoxes[i].bounds.Contains(campos))
				return true;
		return false;
	}
}
