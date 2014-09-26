using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	public Camera MyCamera;
	public Transform PlayerTransform;
	public BoxCollider2D LevelBox;
	Vector2 Target;
	public bool mouseControl;


	void Start () {
		Target = new Vector2 ();
	}
	

	void Update () {
		if (mouseControl) {
			Vector3 mouseWorldPosition = MyCamera.ScreenToWorldPoint (Input.mousePosition + new Vector3(0,0,10));
			Target = new Vector2 (mouseWorldPosition.x, mouseWorldPosition.y);
				} else {
						Target = new Vector2 (PlayerTransform.position.x, PlayerTransform.position.y);
				}
		Vector2 cameraPosition = new Vector2(MyCamera.transform.position.x, MyCamera.transform.position.y);
		cameraPosition += (Target - cameraPosition) * Time.deltaTime * 2.0f;
		if(LevelBox.OverlapPoint(cameraPosition))
			MyCamera.transform.position = new Vector3(cameraPosition.x, cameraPosition.y, MyCamera.transform.position.z);
	}
}
