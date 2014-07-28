/*
coded by Jordan Nguyen (Jordan.N.Nguyen@asu.edu)
modified by Jesse McIntosh for profile selection and loading (jmmcinto@asu.edu)
*/

using UnityEngine;
using System.Collections;

public class ComboBoxTest : MonoBehaviour
{
    public Rect boundingBox = new Rect(0, 0, 610, 60);
    public Profile[] profiles;
	public GUISkin gSkin;
	//public Texture backgroundImage;
    private Profile selectedProfile;
	private int selectedProfileNum;
    private string startDropText = "Choose Your Profile";
    private bool startDropOn;
	private bool displayInfo;
	private string buttonText = "Go Back";
	Vector2 startScrollPosition = Vector2.zero;
	Vector2 startScrollPosition1 = Vector2.zero;
	void Start()
	{

		profiles = new Profile[PlayerPrefs.GetInt("TotalProfiles")];
		print (profiles.Length);
		for(int i = 1; i<profiles.Length +1; i++)
		{
			profiles[i-1] = new Profile();
			profiles[i-1].profileNum = i;
			profiles[i-1].name = PlayerPrefs.GetString( "Profile " + i.ToString() + " Name");
			profiles[i-1].cupsHighScore = PlayerPrefs.GetInt("Profile " + i.ToString() + " Cups Highscore");
			profiles[i-1].cupsHighCombo = PlayerPrefs.GetInt("Profile " + i.ToString() + " Cups Highest Combo"); 	 
			profiles[i-1].QuizHighScore = PlayerPrefs.GetInt("Profile " + i.ToString() + " Quiz Highscore");
			profiles[i-1].quizAccuracy = PlayerPrefs.GetFloat("Profile " + i.ToString() + " Total Quiz Accuracy"); 
			profiles[i-1].completeQuizNum = PlayerPrefs.GetInt("Profile " + i.ToString() + " Total Completed Quizzes"); 	 
			profiles[i-1].avgQuizTime = PlayerPrefs.GetFloat("Profile " + i.ToString() + " Average Quiz Completion"); 
			profiles[i-1].totalQuizTime = PlayerPrefs.GetFloat("Profile " + i.ToString() + " Total Time In Quiz");
			profiles[i-1].bakingHighScore = PlayerPrefs.GetInt("Profile " + i.ToString() + " Baking Highscore"); 	 
			profiles[i-1].idNum = PlayerPrefs.GetString("Profile " + i.ToString() + " Student ID number");
		}
	}

    void SetCurrentProfile(Profile b)
    {
		startDropText = b.name;
        selectedProfile = b;
		buttonText = "Play";
		displayInfo = true;
    }

    void OnGUI()
    {

		if (gSkin)
			GUI.skin = gSkin; 
		
		//GUI.DrawTexture(new Rect(0,0,1024,768),backgroundImage);

        // get bounding box dimensions used for determining area of GUI elements
        // box separated into four quadrants
        float x = boundingBox.x;
        float y = boundingBox.y;
        float w = boundingBox.width / 2;
        float h = boundingBox.height / 2;
        float buf = 5; // buffer between quadrants
        GUI.depth = -1;


		//profile creation button
		if(GUI.Button(new Rect(810,560,200,150),"New Profile"))
		{
			Application.LoadLevel("Profile Creation");
		}

		if(GUI.Button(new Rect(110,560,200,150), buttonText))
		{
			Application.LoadLevel("Title Screen");
		}

        // dropdown box
        if (GUI.Button(new Rect(x + w + buf, y + h + buf, w, h), startDropText))
        {
            if (startDropOn) startDropOn = false;
            else startDropOn = true;
        }

        if (startDropOn)
        {
            GUI.Box(new Rect(x + w + buf, y + 2 * h + buf, w, Mathf.Min(100, profiles.Length * h)), "");
            startScrollPosition = GUI.BeginScrollView(
                    new Rect(x + w + buf, y + 2 * h + buf, w, Mathf.Min(100, profiles.Length * h)), // same as GUI.Box (i.e. viewable area on screen)
                    startScrollPosition, // zero
                    new Rect(0, 0, w - 16, profiles.Length * h), // size of content area
                    false, // do not horizontal scrollbar unless necessary
                    true // always show vertical scrollbar
                    );

            for (int i = 0; i < profiles.Length; i++)
            {
                string temp = profiles[i].name;
                if (GUI.Button(new Rect(0, i * h, w - 16, h), temp))
                {
					selectedProfileNum = i + 1;
					PlayerPrefs.SetInt("SelectedProfile", selectedProfileNum);
                    SetCurrentProfile(profiles[i]);
                    startDropOn = false;
					displayInfo = true;
                }
            }

            GUI.EndScrollView();
        }


		if(displayInfo)
		{
			startScrollPosition1 = GUI.BeginScrollView(
				new Rect(2*(x + w + buf), y + h + buf, w*1.5f, h*12), 
				startScrollPosition1, // zero
				new Rect(2*(x + w + buf), y + h + buf, w*1.5f-16, h*24), // size of content area
				false, // do not horizontal scrollbar unless necessary
				true // always show vertical scrollbar
				);

			string speedPerSecond = "Average Quiz Completion Time: " + selectedProfile.avgQuizTime.ToString("F2") + "/sec";
			string speedPerMinute = "Average Quiz Completion Time: " + (selectedProfile.avgQuizTime/60).ToString("F2") + "/min";
			string displayedString;
			if(selectedProfile.avgQuizTime < 60f)
				displayedString = speedPerSecond;
			else
				displayedString = speedPerMinute;
			GUI.Label (new Rect(2*(x + w + buf), y + h + buf, w*1.5f-16, h), "Profile Name: " + selectedProfile.name.ToString());
			GUI.Label (new Rect(2*(x + w + buf), (y + h + buf)*2, w*1.5f-16, h*2), "Profile ID Number: " + selectedProfile.idNum.ToString());
			GUI.Label (new Rect(2*(x + w + buf), (y + h + buf)*3, w*1.5f-16, h*2), "Primitive Coffee Cups" );
			GUI.Label (new Rect(2*(x + w + buf), (y + h + buf)*4, w*1.5f-16, h*2), "High Score: " + selectedProfile.cupsHighScore.ToString());
			GUI.Label (new Rect(2*(x + w + buf), (y + h + buf)*5, w*1.5f-16, h*2), "Highest Combo: " + selectedProfile.cupsHighCombo.ToString());
			GUI.Label (new Rect(2*(x + w + buf), (y + h + buf)*6, w*1.5f-16, h*2), "The Coffee Hutt Quiz Show");
			GUI.Label (new Rect(2*(x + w + buf), (y + h + buf)*7, w*1.5f-16, h*2), "Total Score: " + selectedProfile.QuizHighScore.ToString());
			GUI.Label (new Rect(2*(x + w + buf), (y + h + buf)*8, w*1.5f-16, h*2), "Total Completed Quizzes " + selectedProfile.completeQuizNum.ToString());
			GUI.Label (new Rect(2*(x + w + buf), (y + h + buf)*9, w*1.5f-16, h*2), "Total Accuracy: " + selectedProfile.quizAccuracy.ToString("F1") + "%");
			GUI.Label (new Rect(2*(x + w + buf), (y + h + buf)*10, w*1.5f-16, h*2), displayedString);
			GUI.Label (new Rect(2*(x + w + buf), (y + h + buf)*11, w*1.5f-16, h*2), "Baking 3000 High Score: " + selectedProfile.bakingHighScore.ToString());
			GUI.EndScrollView();
		}

    }
}

[System.Serializable]
public class Profile
{

	public string name;
	public int profileNum;
	public int cupsHighScore;
	public int cupsHighCombo;
	public int QuizHighScore;
	public int completeQuizNum;
	public float avgQuizTime;
	public float totalQuizTime;
	public int bakingHighScore;
	public float quizAccuracy;
	public string idNum;
}