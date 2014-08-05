using UnityEngine;
using System; // for EventArgs
using System.Collections;
using Common;

public class TitleMenuManager : MonoBehaviour
{
	public Rect menuBox = new Rect(10, 10, 20, 80);
	private Menu[] menus = new Menu[6];
	private int curMenu = 0;

	// Use this for initialization
	void Start ()
	{
		PlayerPrefs.SetInt("UnlockedLevel", 0);
		menus[0] = new StartMenu(menuBox);
		menus[1] = new OptionMenu(menuBox);
		menus[2] = new AudioMenu(menuBox);
		menus[3] = new GraphicsMenu(menuBox);
		menus[4] = new RebindingMenu(new Rect(10,10,80,80));
		//menus[5] = new ExtrasMenu(menuBox);

		for(int i = 0; i < 5; i++)
		{
			menus[i].Changed += new ChangedEventHandler(SwapMenu);
		}
	}

	// Update is called once per frame
	void Update ()
	{
		// Stretch background to Screen Size
		if(this.guiTexture != null)
		{
			float w = Screen.width;
			float h = Screen.height;
			this.guiTexture.pixelInset = new Rect(-w/2.0f, -h/2.0f, w, h);
		}

		menus[curMenu].Update();
	}

	void OnGUI()
	{
		menus[curMenu].ShowMe();
	}

	// Event Handler Method (this is called whenever the event is fired)
	private void SwapMenu(object sender, EventArgs e, int index) 
	{
		curMenu = index;
		/* 0 = start menu
		 * 1 = options menu
		 * 2 = audio menu
		 * 3 = graphics menu
		 * 4 = keymapping menu
		 */
	}
}