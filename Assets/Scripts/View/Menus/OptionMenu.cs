using UnityEngine;
using System.Collections;
using System;
using Common;

// Referenced: http://wiki.unity3d.com/index.php?title=PauseMenu
public class OptionMenu : Menu
{
	public OptionMenu(Rect menuArea) : base(menuArea)
	{
		options = new string[] {
			"Audio",
			"Graphics",
			"Key Mapping",
			"Back"
		};
	}
	
	protected override void PressedEnter()
	{
		switch(selected)
		{
		case 0: // Audio
			OnChanged(EventArgs.Empty, 2);
			break;
		case 1: // Graphics
			OnChanged(EventArgs.Empty, 3);
			break;
		case 2: // Key Mapping
			OnChanged(EventArgs.Empty, 4);
			break;
		case 3: // Back
			OnChanged(EventArgs.Empty, 0);
			break;
		default:
			break;
		}
		selected = -1;
	}
}
