using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class RebindingMenu : MonoBehaviour
{
	public RebindableData rebindableManager;

	private List<RebindableKey> rebindKeys;
	private List<RebindableAxis> rebindAxes;
	
	private bool showingMenu = false;
	private bool rebinding = false;
	private bool rebindingAxPo = false;
	private bool rebindingAxNe = false;
	
	private string objToRebind = "";
	
	void Start ()
	{
		rebindKeys = rebindableManager.GetCurrentKeys();
		rebindAxes = rebindableManager.GetCurrentAxes();
	}
	
	void Update ()
	{
		if (RebindableInput.GetKeyDown("RebindMenu") && !rebinding)
		{
			showingMenu = !showingMenu;
		}
		
		if (rebinding)
		{
			if (Input.anyKeyDown)
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
	}
	
	void OnGUI ()
	{
		if (showingMenu)
		{
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
			
			GUILayout.EndVertical ();
			
			if (rebinding)
			{
				float cX = Screen.width / 2;
				float cY = Screen.height / 2;
				GUIStyle centeredStyle = GUI.skin.GetStyle("Label");
				centeredStyle.alignment = TextAnchor.MiddleCenter;
				GUI.Box(new Rect(cX - 100, cY - 17, 200, 34), "");
				GUI.Label(new Rect(cX - 100, cY - 17, 200, 34), "Press any key to rebind.", centeredStyle);
			}
		}
		else
		{
			GUILayout.BeginVertical("box");
			GUILayout.Label("Press '" + RebindableInput.GetKeyFromBinding("RebindMenu").ToString() + "' to rebind.");
			GUILayout.EndVertical();
		}
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
