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
							rebindAxes[k].axisPos = reboundKey;
						}
						else
						{
							rebindAxes[k].axisNeg = reboundKey;
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
						rebindKeys[l].input = reboundKey;
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
		GUILayout.Label ("Normal Keybinds");

		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Key Name:");
		GUILayout.Label ("Key Code:");
		GUILayout.EndHorizontal ();
		
		for (int i = 0; i < rebindKeys.Count; i++)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label (rebindKeys[i].inputName);
			
			if (GUILayout.Button (rebindKeys[i].input.ToString ()))
			{
				rebinding = true;
				objToRebind = rebindKeys[i].inputName;
			}
			
			GUILayout.EndHorizontal();
		}
		
		GUILayout.Label ("");
		GUILayout.Label ("Axis Keybinds");
		
		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Axis Name:");
		GUILayout.Label ("Positive:");
		GUILayout.Label ("Negative:");
		GUILayout.EndHorizontal ();
		
		for (int j = 0; j < rebindAxes.Count; j++)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label (rebindAxes[j].axisName);
			
			if (GUILayout.Button (rebindAxes[j].axisPos.ToString ()))
			{
				rebinding = true;
				rebindingAxPo = true;
				objToRebind = rebindAxes[j].axisName;
			}
			
			if (GUILayout.Button (rebindAxes[j].axisNeg.ToString ()))
			{
				rebinding = true;
				rebindingAxNe = true;
				objToRebind = rebindAxes[j].axisName;
			}
			
			GUILayout.EndHorizontal();
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
		
		if (rebinding)
		{
			GUILayout.Label("Press any key to rebind.");
		}
		else
		{
			GUILayout.Label("");
		}

		GUILayout.Label("");
		if(GUILayout.Button("Back"))
		{
			OnChanged(EventArgs.Empty, 1);
		}
		GUILayout.EndVertical ();

		GUILayout.EndArea();
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
