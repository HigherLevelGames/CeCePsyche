using UnityEngine;
using System.Collections;

// temporarily stores information about the level to be saved later into the profile container
public class Recorder : MonoBehaviour
{
	float startTime;
	public GameObject checkpoint1;
	public GameObject checkpoint2;
	public GameObject checkpoint3;

	// Use this for initialization
	void Start () {
		startTime = Time.time;
	}

	// Update is called once per frame
	void Update () { }

	void OnLevelEnd()
	{
		/*
		// save this information into Profile.cs:
		int level = Application.loadedLevel;
		float levelTime = Time.time - startTime;
		
		// get number of times ceci went to each checkpoint
		int cp1;
		int cp2;
		int cp3;

		// get death locations from Respawn.cs
		List<Vector3> deaths;

		// get powers used from AbilityManager.cs
		List<PowerUsed> powers;

		// get test results from HUD.cs?
		List<QuizResult> testResults;

		// actually save the information
		mainProfile.LevelUpdate(level, cp1, cp2, cp3, deaths, powers, testResults);
		//*/
	}
}
