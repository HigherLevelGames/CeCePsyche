using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// temporarily stores information about the level to be saved later into the profile container
public class Recorder : MonoBehaviour
{
	int level = -1;
	float startTime = 0.0f;
	public GameObject checkpoint1;
	public GameObject checkpoint2;
	public GameObject checkpoint3;

	// Use this for initialization
	void Start ()
	{
		startTime = Time.time;
	}

	// Update is called once per frame
	void Update () { }

	void OnLevelEnd()
	{
		// save this information into Profile.cs:
		int level = Application.loadedLevel;
		float levelTime = Time.time - startTime;
		
		// get number of times ceci went to each checkpoint
		int cp1 = 0; // checkpoint1.jfiewdks
		int cp2 = 0;
		int cp3 = 0;

		// get death locations from Respawn.cs
		//List<Vector3> deaths = new List<Vector3>(); // jfdkls

		// get powers used from AbilityManager.cs
		//List<PowerUsed> powers = new List<PowerUsed>(); // jedswds

		// get test results from HUD.cs?
		//List<QuizResult> testResults = new List<QuizResult>();//wj ojkdcs

		// actually save the information
		//ProfileContainer mainProfile = GameObject.Find("ProfileContainer").GetComponent<ProfileContainer>();
		//mainProfile.LevelUpdate(level, cp1, cp2, cp3, deaths, powers, testResults);
	}
}
