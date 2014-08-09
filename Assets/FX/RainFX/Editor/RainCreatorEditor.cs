using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor (typeof(RainCreator))]
public class RainCreatorEditor : Editor {

	public override void OnInspectorGUI () {

		string[] rainType = { "Soft Rain", "Normal Rain", "Heavy Rain" };

		RainCreator myTarget = (RainCreator) target;

		EditorGUILayout.Space ();

		EditorGUILayout.LabelField ("Base Settings", EditorStyles.boldLabel);

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.PrefixLabel ("Rain Type");
		myTarget.rainLevel = EditorGUILayout.Popup (myTarget.rainLevel, rainType);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.HelpBox ("The amount of rain you want in your scene. Select 'Soft Rain' for optimal performance.", MessageType.None);

		EditorGUILayout.Space ();

		EditorGUILayout.LabelField ("Optional Settings", EditorStyles.boldLabel);

		myTarget.localPosition = EditorGUILayout.Vector3Field ("Local Position", myTarget.localPosition);

		EditorGUILayout.HelpBox ("The position (local space) at which the rain will be instantiated. If the RainCreator is parented to a First Person Camera I recommend setting the y-value to '9'.", MessageType.None);

	}

}
