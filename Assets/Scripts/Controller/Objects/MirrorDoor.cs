using UnityEngine;
using System.Collections;

public class MirrorDoor : MonoBehaviour
{
	public string levelName;

	void Interact()
	{
		Application.LoadLevel(levelName);
	}
}
