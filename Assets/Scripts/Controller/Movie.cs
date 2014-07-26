using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Requires Unity Pro
public class Movie : MonoBehaviour
{
	public MovieTexture movie; // NOTE could make this an array for all the cutscenes we'll be having
	public string nextLevel; // NOTE could put this in playerprefs
	// NOTE could create a map, (cutscene to play/level to load) so everything's in one scene
	//Dictionary<int, string> map = new Dictionary<string, int>();
	//0 = happiness intro
	//1 = happiness outro
	//2 = sadness intro
	//3 = sadness outro
	//etc.

	// Use this for initialization
	void Start ()
	{
		if(movie != null)
		{
			// NOTE could use 3D plane (rotation.x = -90.0f) or draw directly on 2D GUI
			//this.renderer.material.mainTexture = movie;
			movie.Play(); // play at start
			// NOTE possibly have somewhere the ability to replay cutscenes after unlocked the first time
		}
		else
		{
			Debug.Log ("Missing Movie Texture, unable to play clip");
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(movie != null)
		{
			if(Time.time > movie.duration && Application.CanStreamedLevelBeLoaded(nextLevel))
			{
				// NOTE could give player ability to pause cutscene, but assuming how short they'll be, I don't think we'll need to
				Application.LoadLevel(nextLevel);
			}
		}

		// skip button
		if(Input.GetButtonDown("Jump") && Application.CanStreamedLevelBeLoaded(nextLevel))
		{
			Application.LoadLevel(nextLevel);
		}
	}

	void OnGUI()
	{
		GUILayout.Label("Cutscene: Press Spacebar to Skip.");

		if(movie != null)
		{
			// NOTE talk to Jyordana whether or not we're using a border
			GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height), movie, ScaleMode.StretchToFill);
		}
	}
}
