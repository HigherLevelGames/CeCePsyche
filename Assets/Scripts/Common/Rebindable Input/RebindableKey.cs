using UnityEngine;
using System.Collections;

[System.Serializable]
public class RebindableKey
{
	public string inputName = "";
	public KeyCode input = KeyCode.A;
	// JNN: added
	public KeyCode altInput = KeyCode.None;
	
	public RebindableKey () { }
	
	public RebindableKey (string name, KeyCode key)
	{
		inputName = name;
		input = key;
	}

	// JNN: added
	public RebindableKey (string name, KeyCode key, KeyCode key2)
	{
		inputName = name;
		input = key;
		altInput = key2;
	}
}
