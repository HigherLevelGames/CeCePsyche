using UnityEngine;
using System; // for EventArgs
using System.Collections;
using Common;

public class TestMenuManager : MonoBehaviour
{
	public Rect menuBox = new Rect(10, 10, 20, 80);
	private LoadMenu menu;

	// Use this for initialization
	void Start ()
	{
		menu = new LoadMenu(menuBox);
		menu.Changed += new ChangedEventHandler(SwapMenu);
	}

	// Update is called once per frame
	void Update ()
	{
		menu.Update();
	}

	void OnGUI()
	{
		menu.ShowMe();
	}

	// Event Handler Method (this is called whenever the event is fired)
	private void SwapMenu(object sender, EventArgs e, int index) 
	{
		//curMenu = index;
		/* 0 = start menu
		 * 1 = options menu
		 * 2 = audio menu
		 * 3 = graphics menu
		 * 4 = keymapping menu
		 */
	}
}