using UnityEngine;
using System.Collections;

public class Temp : MonoBehaviour {
	public string[] text;
	int index = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI()
	{
		if(text.Length <= 0)
		{
			return;
		}

		GUI.Box (new Rect(0,Screen.height*0.8f, Screen.width,Screen.height*0.2f), text[index%text.Length]);
		if(GUI.Button (new Rect(Screen.width*0.9f,Screen.height*0.85f,Screen.width*0.1f,Screen.height*0.1f), "Next"))
		{
			index++;
			if(index > text.Length-1)
			{
				Application.LoadLevel("BrainMenu");
			}
		}
	}
}
