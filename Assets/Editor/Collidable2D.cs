using UnityEngine;
using UnityEditor;
using System.Collections;

public class Collidable2D : MonoBehaviour {
	[MenuItem ("GameObject/2D Object/Collidable 2D Sprite")]
	static void Collidable2DSprite () {
		GameObject obj = new GameObject ();
		Vector3 point = SceneView.currentDrawingSceneView.camera.ViewportToWorldPoint (new Vector3 (0.5f, 0.5f, 0f));
		obj.transform.position = new Vector3(point.x, point.y, 0);
		obj.name = "New Collidable Sprite";
		obj.AddComponent<SpriteRenderer> ();
		obj.AddComponent<BoxCollider2D> ();
		Selection.activeGameObject = obj;
	}
}