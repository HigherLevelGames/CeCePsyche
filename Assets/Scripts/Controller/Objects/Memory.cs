using UnityEngine;
using System.Collections;

public class Memory : MonoBehaviour
{
	public GameObject FogZone; // foggy memory that will disappear once memory has been collected

	// Use this for initialization
	void Start ()
	{
		if(FogZone == null)
		{
			Debug.Log("Error: missing foggy location");
		}
	}
	
	// Update is called once per frame
	void Update () { }

	// Memory had been collected (see JNNController.cs)
	void OnDestroy()
	{
		// deactivate fog zone
		FogZone.SetActive(false);
	}
}
