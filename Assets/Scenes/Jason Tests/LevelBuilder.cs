using UnityEngine;
using UnityEditor;
using System.Collections;

public class LevelBuilder : EditorWindow
{
		enum EditMode
		{
				Walls = 0,
				Floors = 1,
				None = 2
		}
		EditMode mode = EditMode.Walls;

		[MenuItem("Window/2D Editor")]
		public static void ShowWindow ()
		{
				EditorWindow.GetWindow (typeof(LevelBuilder));

		}

		void Update ()
		{
		switch (mode) {
				case EditMode.Walls:
						break;
				case EditMode.Floors:
						EditFloors ();
						break;
				case EditMode.None:
						break;
				}
		}

		void OnGUI ()
		{
				GUILayout.Label ("Modes", EditorStyles.boldLabel);
				mode = (EditMode)EditorGUILayout.EnumPopup ("Editing Mode", mode);
		}
		void EditFloors ()
		{
				if (Input.GetMouseButtonDown (0)) {
						GameObject obj = new GameObject ();
						obj.AddComponent<BoxCollider2D> ();
						obj.transform.position = new Vector3 ();

						Instantiate (obj);
				}
				if (Input.GetMouseButton (0))
						;
		}

		void EditWalls ()
		{

		}
}