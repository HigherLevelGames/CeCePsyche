using UnityEngine;
using System; // for EventArgs
using System.Collections;
using Common;

namespace Common
{
	// A delegate type for hooking up change notifications.
	public delegate void ChangedEventHandler(object sender, EventArgs e, int index);
}

public class Menu
{
	// Menu gets shown in this box area
	protected Rect box = new Rect(0,0,0,0);

	// selection grid vars for default menu
	protected int selected = -1;
	protected string[] options = { "" };

	// Constructor
	public Menu(Rect menuArea)
	{
		box = menuArea;
	}

	// An event MenuManager can use to be notified whenever the menu should change
	public event ChangedEventHandler Changed;
	
	// Invoke the Changed event; called whenever menu changes
	protected virtual void OnChanged(EventArgs e, int index)
	{
		if (Changed != null)
		{
			Changed(this, e, index);
		}
	}

	// Overridden by AudioMenu.cs, GraphicsMenu.cs, and RebindingMenu1.cs
	public virtual void Update()
	{
		// up and down keys to control menu
		int vInput = RebindableInput.GetAxisDown("Vertical");
		if(vInput != 0)
		{
			if(vInput < 0)
			{
				selected = (selected + 1) % options.Length;
			}
			else if(selected > 0) // vInput > 0
			{
				selected--;
			}
			else // selected <= 0 && vInput > 0
			{
				selected = options.Length-1;
			}
		}
		
		// space/enter to select
		if(RebindableInput.GetKeyDown("Jump") || RebindableInput.GetKeyDown("Interact"))
		{
			PressedEnter();
		}
	}

	// To Show the Menu
	public virtual void ShowMe()
	{
		// draw selection grid buttons
		selected = GUI.SelectionGrid(Utility.adjRect(box), selected, options, 1);

		if(Utility.adjRect(box).Contains(Input.mousePosition))
		{
			Debug.Log("Hello");
		}
		else
		{
			Debug.Log("World");
		}

		// left click event same as enter event
		if(Input.GetMouseButtonUp(0) && Utility.adjRect(box).Contains(Input.mousePosition))
		{
			// http://docs.unity3d.com/Manual/ExecutionOrder.html
			// the check is done here due to the ordering of Unity Events
			// i.e. OnGUI() draws multiple times before Update() is,
			// so the click detection might be lost if we don't check for it immediately
			PressedEnter();
		}
	}

	protected virtual void PressedEnter() { }
}