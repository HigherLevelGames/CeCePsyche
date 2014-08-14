using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// class kept persistent throughout game
// gets updated during scene transitions (i.e. cutscenes)
public class ProfileContainer : MonoBehaviour
{
	private Profile mainProfile;

	// Use this for initialization
	void Start ()
	{
		DontDestroyOnLoad(this.transform.gameObject);
	}

	// called when creating a new game or loading a saved game
	void SetProfile(Profile p)
	{
		mainProfile = p;
	}

	/*
	// called at the end of each level by Recorder.cs to record information
	public void LevelUpdate(int level,
	                        float time,
	                        int checkpoint1, int checkpoint2, int checkpoint3,
	                        List<Vector3> deaths,
	                        List<PowerUsed> powers,
	                        List<QuizResult> testResults)
	{
		mainProfile.TotalLevelTime[level] = time;
		mainProfile.checkpointReturn[level].x = checkpoint1;
		mainProfile.checkpointReturn[level].y = checkpoint2;
		mainProfile.checkpointReturn[level].z = checkpoint3;
		mainProfile.Deaths[level] = deaths;
		mainProfile.PowerUsage[level] = powers;
		mainProfile.Test[level] = testResults;
	}//*/
}
