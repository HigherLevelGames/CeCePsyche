/*
coded by Jordan Nguyen (Jordan.N.Nguyen@asu.edu)
modified by Jesse McIntosh for profile selection and loading (jmmcinto@asu.edu)
*/

using UnityEngine;
using System.Collections;

public class ComboBoxTest //: MonoBehaviour
{
    public Rect boundingBox = new Rect(0, 0, 610, 60);
    public Profile[] profiles;
	public GUISkin gSkin;
    private Profile selectedProfile;
	private int selectedProfileNum;
    private string startDropText = "Choose Your Profile";
    private bool startDropOn;
	Vector2 startScrollPosition = Vector2.zero;

	void SetCurrentProfile(Profile b)
    {
		startDropText = b.name;
        selectedProfile = b;
    }

	int DrawComboBox(Rect position, Rect scrollPosition, int selected, string[] texts)
	{
		//GUI.Button(position, texts[selected]) startDropOn = !startDropOn;
		/*
		GUI.Box(scrollPosition,"");
		startScrollPosition = GUI.BeginScrollView(
			scrollPosition, // same as GUI.Box (i.e. viewable area on screen)
			startScrollPosition, // zero
			new Rect(0, 0, scrollPosition.w - 16, texts.Length * scrollPosition.h), // size of content area
			false, // do not show horizontal scrollbar unless necessary
			true // always show vertical scrollbar
			);
		
		for (int i = 0; i < texts.Length; i++)
		{
			if (GUI.Button(new Rect(0, i * scrollPosition.h, scrollPosition.w - 16, scrollPosition.h), texts[i]))
			{
				//selectedProfileNum = i + 1;
				//SetCurrentProfile(profiles[i]);
				//startDropOn = false;
			}
		}
		GUI.EndScrollView();
		//*/		
		return selectedProfileNum;
	}
	
	public void OnGUI()
	{
		if(profiles == null)
		{
			profiles = new Profile[20];
			for(int i = 0; i < 20; i++)
			{
				profiles[i] = new Profile();
				profiles[i].name += i.ToString();
			}
		}
		if (gSkin)
		{
			GUI.skin = gSkin;
		}

        // get bounding box dimensions used for determining area of GUI elements
        // box separated into four quadrants
        float x = boundingBox.x;
        float y = boundingBox.y;
        float w = boundingBox.width / 2;
        float h = boundingBox.height / 2;
        float buf = 5; // buffer between quadrants

        // dropdown box
        if (GUI.Button(new Rect(x + w + buf, y + h + buf, w, h), startDropText))
        {
			startDropOn = !startDropOn;
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
                    SetCurrentProfile(profiles[i]);
                    startDropOn = false;
                }
            }

            GUI.EndScrollView();
        }
    }
}

[System.Serializable]
public class Profile
{
	public string name = "Hello World";
	public Profile() { }
}