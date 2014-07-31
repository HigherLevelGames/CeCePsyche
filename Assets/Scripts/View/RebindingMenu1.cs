using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Common;

public class RebindingMenu1 : Menu
{
	public RebindableData rebindableManager;

	private List<RebindableKey> rebindKeys;
	private List<RebindableAxis> rebindAxes;

	// JNN TODO: easy way to do this using 4-bits, but for now, this is good enough
	private bool isAlt = false;
	private bool rebinding = false;
	private bool rebindingAxPo = false;
	private bool rebindingAxNe = false;
	
	private string objToRebind = "";
	
	public RebindingMenu1(Rect menuArea) : base(menuArea)
	{
		rebindableManager = GameObject.Find("Rebindable Manager").GetComponent<RebindableData>();
		rebindKeys = rebindableManager.GetCurrentKeys();
		rebindAxes = rebindableManager.GetCurrentAxes();
	}

	// May use this: http://forum.unity3d.com/threads/wait-for-input.74034/
	public override void Update ()
	{
		if (rebinding && Input.anyKeyDown)
		{
			KeyCode reboundKey = FetchPressedKey();
			
			if (reboundKey == KeyCode.None)
			{
				if (Input.GetKeyDown(KeyCode.LeftAlt)) { reboundKey = KeyCode.LeftAlt; }
				if (Input.GetKeyDown(KeyCode.RightAlt)) { reboundKey = KeyCode.RightAlt; }
				if (Input.GetKeyDown(KeyCode.LeftShift)) { reboundKey = KeyCode.LeftShift; }
				if (Input.GetKeyDown(KeyCode.RightShift)) { reboundKey = KeyCode.RightShift; }
				if (Input.GetKeyDown(KeyCode.LeftControl)) { reboundKey = KeyCode.LeftControl; }
				if (Input.GetKeyDown(KeyCode.RightControl)) { reboundKey = KeyCode.RightControl; }
			}
			
			if (rebindingAxPo || rebindingAxNe)
			{
				for (int k = 0; k < rebindAxes.Count; k++)
				{
					if (rebindAxes[k].axisName == objToRebind)
					{
						if (rebindingAxPo)
						{
							if(isAlt)
							{
								rebindAxes[k].altAxisPos = reboundKey;
							}
							else
							{
								rebindAxes[k].axisPos = reboundKey;
							}
						}
						else
						{
							if(isAlt)
							{
								rebindAxes[k].altAxisNeg = reboundKey;
							}
							else
							{
								rebindAxes[k].axisNeg = reboundKey;
							}
						}
					}
				}
			}
			else
			{
				for (int l = 0; l < rebindKeys.Count; l++)
				{
					if (rebindKeys[l].inputName == objToRebind)
					{
						if(isAlt)
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
			rebinding = false;
			rebindingAxPo = false;
			rebindingAxNe = false;
		}
	}
	
	public override void ShowMe ()
	{
		GUILayout.BeginArea(Utility.adjRect(box));
		GUILayout.BeginVertical ("box");

		ShowKeyBindOptions();
		
		GUILayout.Label ("");

		ShowAxisBindOptions();

		if (rebinding)
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
				isAlt = false;
				rebinding = true;
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
				isAlt = true;
				rebinding = true;
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
				isAlt = false;
				rebinding = true;
				rebindingAxPo = true;
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
				isAlt = false;
				rebinding = true;
				rebindingAxNe = true;
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
				isAlt = true;
				rebinding = true;
				rebindingAxPo = true;
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
				isAlt = true;
				rebinding = true;
				rebindingAxNe = true;
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
		return KeyCode.None;
	}
}
