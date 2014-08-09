using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("PlayerProfile")]
public class Profile
{
	// Basic Vars
	public string PlayerName; // name of the player
	public int UnlockedLevel = 0; // player’s progress through the game
	public bool EduMode = false; // whether or not player is playing in edu mode

	// [XmlAttribute("PowerUsed")]

	public float TotalTimePlayed
	{
		get
		{
			float TotalTime = 0.0f;
			foreach(float i in TotalLevelTime)
			{
				TotalTime+=i;
			}
			return TotalTime;
		}
	}

	#region Analytical Vars
	// Amount of time spent in each level
	public float[] TotalLevelTime = new float[6];

	// Number of times sent to a checkpoint
	public Vector3[] checkpointReturn = new Vector3[6];

	// Where the player dies/fails
	[XmlArray("Deaths"), XmlArrayItem("Vector3")]
	public List<Vector3>[] Deaths = new List<Vector3>[6];

	//which powers were used and how often and where
	[XmlArray("PowerUsage"), XmlArrayItem("PowerUsed")]
	public List<PowerUsed>[] PowerUsage = new List<PowerUsed>[6];

	// CS quiz failures and successes
	[XmlArray("Test"), XmlArrayItem("QuizResult")]
	public List<QuizResult>[] Test = new List<QuizResult>[6];
	#endregion

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
		checkpointReturn[level].x = checkpoint1;
		checkpointReturn[level].y = checkpoint2;
		checkpointReturn[level].z = checkpoint3;
		Deaths[level] = deaths;
		PowerUsage[level] = powers;
		Test[level] = testResults;
	}

	public void Save(string path)
	{
		path = Path.Combine(path, PlayerName + ".xml");
		XmlSerializer serializer = new XmlSerializer(typeof(Profile));
		using(FileStream stream = new FileStream(path, FileMode.Create))
		{
			serializer.Serialize(stream, this);
		}
	}
	
	// Profile player = Profile.Load(Path.Combine(Application.dataPath, "saveFile.xml"));
	// Application.dataPath points to your asset/project directory.
	// If you have your xml file stored in your project at DataFiles/test/saveFile.xml
	// you can access it by using Path.Combine(Application.dataPath, "DataFiles/test/saveFile.xml");
	public static Profile Load(string path)
	{
		XmlSerializer serializer = new XmlSerializer(typeof(Profile));
		using(FileStream stream = new FileStream(path, FileMode.Open))
		{
			return serializer.Deserialize(stream) as Profile;
		}
	}
}
