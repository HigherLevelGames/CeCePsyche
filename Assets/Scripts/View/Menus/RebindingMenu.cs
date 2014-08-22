using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Common;

public class RebindingMenu : Menu
{
	public RebindableData rebindableManager;
	
	private List<RebindableKey> rebindKeys;
	private List<RebindableAxis> rebindAxes;
	
	[Flags]
	private enum RebindFlag
	{
		stopRebinding = 0,
		isRebinding = 1 << 0,
		isAlternate = 1 << 1,
		isAxes = 1 << 2,
		isAxisPos = 1 << 3,
		
		// multiple flags shortcut
		isAltAxisPos = isAlternate | isAxisPos // | isAxis
	}
	RebindFlag flag = RebindFlag.stopRebinding;
	
	private string objToRebind = "";
	
	public RebindingMenu(Rect menuArea) : base(menuArea)
	{
		rebindableManager = GameObject.Find("Rebindable Manager").GetComponent<RebindableData>();
		rebindKeys = rebindableManager.GetCurrentKeys();
		rebindAxes = rebindableManager.GetCurrentAxes();
	}
	
	// May use this: http://forum.unity3d.com/threads/wait-for-input.74034/
	public override void Update ()
	{
		if (((flag & RebindFlag.isRebinding) != 0) && Input.anyKeyDown)
		{
			KeyCode reboundKey = FetchPressedKey();
			
			if((flag & RebindFlag.isAxes) != 0)
			{
				for (int k = 0; k < rebindAxes.Count; k++)
				{
					if (rebindAxes[k].axisName == objToRebind)
					{
						if ((flag & RebindFlag.isAltAxisPos) == RebindFlag.isAltAxisPos)
						{
							rebindAxes[k].altAxisPos = reboundKey;
						}
						else if((flag & RebindFlag.isAxisPos) != 0)
						{
							rebindAxes[k].axisPos = reboundKey;
						}
						else if((flag & RebindFlag.isAlternate) != 0) // rebinding alt negative axis
						{
							rebindAxes[k].altAxisNeg = reboundKey;
						}
						else // rebinding negative axis
						{
							rebindAxes[k].axisNeg = reboundKey;
						}
					}
				}
			}
			else // rebinding key
			{
				for (int l = 0; l < rebindKeys.Count; l++)
				{
					if (rebindKeys[l].inputName == objToRebind)
					{
						if((flag & RebindFlag.isAlternate) != 0)
						{
							rebindKeys[l].altInput = reboundKey;
						}
						else
						{
							rebindKeys[l].input = reboundKey;
						}
					}
				}
			}
			objToRebind = "";
			flag = RebindFlag.stopRebinding;
		}
	}
	
	public override void ShowMe ()
	{
		GUILayout.BeginArea(Utility.adjRect(box));
		GUILayout.BeginVertical ("box");
		
		ShowKeyBindOptions();
		
		GUILayout.Label ("");
		
		ShowAxisBindOptions();
		
		if ((flag & RebindFlag.isRebinding) != 0)
		{
			GUILayout.Label("<color=cyan>Press any key to rebind.</color>");
		}
		else
		{
			GUILayout.Label("");
		}
		
		GUILayout.BeginHorizontal();
		
		if (GUILayout.Button("Save to File"))
		{
			rebindableManager.SaveKeys();
			rebindableManager.SaveAxes();
		}
		
		if (GUILayout.Button("Load Defaults"))
		{
			rebindableManager.ActivateDefaultKeys();
			rebindableManager.ActivateDefaultAxes();
			rebindKeys = rebindableManager.GetCurrentKeys();
			rebindAxes = rebindableManager.GetCurrentAxes();
		}
		
		GUILayout.EndHorizontal();
		
		GUILayout.Label("");
		
		if(GUILayout.Button("Back"))
		{
			OnChanged(EventArgs.Empty, 1);
		}
		
		GUILayout.EndVertical ();
		GUILayout.EndArea();
	}
	
	void ShowKeyBindOptions()
	{
		GUILayout.Label ("Normal Keybinds");
		
		GUILayout.BeginHorizontal ();
		
		// Column 1
		GUILayout.BeginVertical();
		GUILayout.Label ("Key Name:");
		for (int i = 0; i < rebindKeys.Count; i++)
		{
			GUILayout.Label (rebindKeys[i].inputName);
		}
		GUILayout.EndVertical();
		
		// Column 2
		GUILayout.BeginVertical();
		GUILayout.Label ("Key Code:");
		for (int i = 0; i < rebindKeys.Count; i++)
		{
			if (GUILayout.Button (rebindKeys[i].input.ToString ()))
			{
				flag = RebindFlag.isRebinding;
				objToRebind = rebindKeys[i].inputName;
			}
		}
		GUILayout.EndVertical();
		
		// Column 3
		GUILayout.BeginVertical();
		GUILayout.Label ("Alt Key Code:");
		for (int i = 0; i < rebindKeys.Count; i++)
		{
			if (GUILayout.Button (rebindKeys[i].altInput.ToString ()))
			{
				flag = RebindFlag.isRebinding | RebindFlag.isAlternate;
				objToRebind = rebindKeys[i].inputName;
			}
		}
		GUILayout.EndVertical();
		
		GUILayout.EndHorizontal ();		
	}
	
	void ShowAxisBindOptions()
	{
		GUILayout.Label ("Axis Keybinds");
		
		GUILayout.BeginHorizontal ();
		
		// Column 1
		GUILayout.BeginVertical();
		GUILayout.Label ("Axis Name:");
		for (int j = 0; j < rebindAxes.Count; j++)
		{
			GUILayout.Label (rebindAxes[j].axisName);
		}
		GUILayout.EndVertical();
		
		// Column 2
		GUILayout.BeginVertical();
		GUILayout.Label ("Positive:");
		for (int j = 0; j < rebindAxes.Count; j++)
		{
			if (GUILayout.Button (rebindAxes[j].axisPos.ToString ()))
			{
				flag = RebindFlag.isRebinding | RebindFlag.isAxes | RebindFlag.isAxisPos;
				objToRebind = rebindAxes[j].axisName;
			}
		}
		GUILayout.EndVertical();
		
		// Column 3
		GUILayout.BeginVertical();
		GUILayout.Label ("Negative:");
		for (int j = 0; j < rebindAxes.Count; j++)
		{
			if (GUILayout.Button (rebindAxes[j].axisNeg.ToString ()))
			{
				flag = RebindFlag.isRebinding | RebindFlag.isAxes;
				objToRebind = rebindAxes[j].axisName;
			}
		}
		GUILayout.EndVertical();
		
		// Column 4
		GUILayout.BeginVertical();
		GUILayout.Label ("Alt Positive:");
		for (int j = 0; j < rebindAxes.Count; j++)
		{
			if (GUILayout.Button (rebindAxes[j].altAxisPos.ToString ()))
			{
				flag = RebindFlag.isRebinding | RebindFlag.isAlternate | RebindFlag.isAxes | RebindFlag.isAxisPos;
				objToRebind = rebindAxes[j].axisName;
			}
		}
		GUILayout.EndVertical();
		
		// Column 5
		GUILayout.BeginVertical();
		GUILayout.Label ("Alt Negative:");
		for (int j = 0; j < rebindAxes.Count; j++)
		{
			if (GUILayout.Button (rebindAxes[j].altAxisNeg.ToString ()))
			{
				flag = RebindFlag.isRebinding | RebindFlag.isAlternate | RebindFlag.isAxes;
				objToRebind = rebindAxes[j].axisName;
			}
		}
		GUILayout.EndVertical();
		
		GUILayout.EndHorizontal ();		
	}
	
	KeyCode FetchPressedKey ()
	{
		int e = 330;
		for (int i = 0; i < e; i++)
		{
			if (i < 128 || i > 255)
			{
				KeyCode key = (KeyCode)i;
				if (Input.GetKeyDown(key))
				{
					return key;
				}
			}
		}
		/* // JNN: I don't think this is needed...
	if (Input.GetKeyDown(KeyCode.LeftAlt)) { return KeyCode.LeftAlt; }
	if (Input.GetKeyDown(KeyCode.RightAlt)) { return KeyCode.RightAlt; }
	if (Input.GetKeyDown(KeyCode.LeftShift)) { return KeyCode.LeftShift; }
	if (Input.GetKeyDown(KeyCode.RightShift)) { return KeyCode.RightShift; }
	if (Input.GetKeyDown(KeyCode.LeftControl)) { return KeyCode.LeftControl; }
	if (Input.GetKeyDown(KeyCode.RightControl)) { return KeyCode.RightControl; }
	//*/
		return KeyCode.None;
	}
}
