using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Profile
{
	// basic vars
	public string PlayerName; // name of the player
	public int UnlockedLevel = 0; // player’s progress through the game
	public bool EduMode = false; // whether or not player is playing in edu mode

	// analytical vars
	public float[] TotalLevelTime = new float[6]; // Amount of time spent in each level
	public int[,] checkpointReturn = new int[6,3]; // Number of times sent to a checkpoint
	public List<Vector3>[] Deaths = new List<Vector3>[6]; //Where the player dies/fails
	public List<PowerUsed>[] PowerUsage = new List<PowerUsed>[6]; //which powers were used and how often and where
	public List<QuizResult>[] Test = new List<QuizResult>[6]; //CS quiz failures and successes

	// Constructor
	public Profile() { }			

	public struct PowerUsed
	{
		string PowerName;
		float TimeStamp;
		Vector3 Location;
	}			
			
	public struct QuizResult
	{
		string PowerName;
		int NumRight;
		int NumWrong;
	}

	// LevelUpdate() called at the end of each level
	public void LevelUpdate(int level,
	            float time,
	            int checkpoint1, int checkpoint2, int checkpoint3,
	            List<Vector3> deaths,
	            List<PowerUsed> powers,
	            List<QuizResult> testResults)
	{
		TotalLevelTime[level] = time;
		checkpointReturn[level,0] = checkpoint1;
		checkpointReturn[level,1] = checkpoint2;
		checkpointReturn[level,2] = checkpoint3;
		Deaths[level] = deaths;
		PowerUsage[level] = powers;
		Test[level] = testResults;
	}

	public void Save() { }

	public void Load() { }
}
