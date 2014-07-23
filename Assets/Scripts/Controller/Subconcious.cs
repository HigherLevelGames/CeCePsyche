using UnityEngine;
using System.Collections;

public class Subconcious : MonoBehaviour
{
	#region Variables
	private bool isTalking = false;
	public string[] talkText;
	private int index = 0;
	private float scrollIndex = 0;

	// TODO: set offset vars based on Subconcious sprite
	private int offsetX = 20; // sprite.width / 2
	private int offsetY = 20; // sprite.height / 2

	//From: http://www.forbes.com/sites/brettnelson/2012/06/04/do-you-read-fast-enough-to-be-successful/
	//Third-grade students = 150 words per minute (wpm)
	//Eight grade students = 250
	//Average college student = 450
	//Average “high level exec” = 575
	//Average college professor = 675
	//Speed readers = 1,500
	//World speed reading champion = 4,700
	//Average adult: 300 wpm
	
	//(assume 5 char = 1 word)
	//15 chars/sec = 900 char/min ~ 180 words/min
	//20 chars/sec = 1200 char/min ~ 240 words/min
	//25 chars/sec = 1500 char/min ~ 300 words/min
	public float talkSpeed = 20.0f;
	#endregion

	// Use this for initialization
	void Start () { }
	
	// Update is called once per frame
	void Update ()
	{
		// Place Subconcious where mouse position is located
		Vector3 screenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
		screenPos.z = 10.0f; // distance from camera
		Vector3 pos = Camera.main.ScreenToWorldPoint(screenPos);
		this.transform.position = pos;

		// Subconcious speaks by revealing more of the current talkText
		if(isTalking)
		{
			scrollIndex += talkSpeed*Time.deltaTime;
		}

		// Temporary shortcut keys for testing
		if(Input.GetKeyDown(KeyCode.N))
		{
			Say (new string[]{"Hello World", "Some longer text to debug whether or not this works", "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."});
		}
	}

	void OnMouseDown()
	{
		if(isTalking)
		{
			// to progress through multiple lines of text
			index++;
			scrollIndex = 0.0f;
		}
	}

	// Call this method to have the subconcious say stuff
	void Say(string[] text)//, AudioClip clip)
	{
		talkText = text;
		index = 0;
		scrollIndex = 0.0f;
		//audio.clip = clip; // in case we plan for audio
		//audio.Play();
		isTalking = true;
	}

	// OnGUI is called for rendering and handling GUI events
	void OnGUI()
	{
		if(isTalking)
		{
			if(index < talkText.Length) // JIC: to prevent overflow error
			{
				// werid GUI stuff
				bool temp = GUI.skin.box.wordWrap;
				GUI.skin.box.wordWrap = true;
				GUIContent text = new GUIContent(talkText[index]);

				// calculate dimensions
				Vector2 dimensions = GUI.skin.box.CalcSize(text);
				float x = Input.mousePosition.x + offsetX;
				float y = Screen.height - Input.mousePosition.y + offsetY;
				dimensions.x = Mathf.Min(Screen.width*0.25f, dimensions.x);
				float w = (x + dimensions.x > Screen.width) ?
						Screen.width - x:
						dimensions.x;
				float h = GUI.skin.box.CalcHeight(text, w);

				// actually draw the box
				int endIndex = Mathf.Min((int)scrollIndex, talkText[index].Length);
				string wordsToSay = talkText[index].Substring(0, endIndex);
				GUI.Box (new Rect(x, y, w, h), wordsToSay);

				// set weird GUI stuff back to default values
				GUI.skin.box.wordWrap = temp;
			}
		}
	}
}
