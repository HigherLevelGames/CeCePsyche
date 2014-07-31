using UnityEngine;
using System.Collections;

[System.Serializable]
public class RebindableAxis
{
	public string axisName = "";
	public string axisPosName = "";
	public string axisNegName = "";
	
	public KeyCode axisPos = KeyCode.W;
	public KeyCode axisNeg = KeyCode.S;
	// JNN: added
	public KeyCode altAxisPos = KeyCode.None;
	public KeyCode altAxisNeg = KeyCode.None;
	
	public RebindableAxis () { }
	
	public RebindableAxis (string name, KeyCode positive, KeyCode negative)
	{
		axisName = name;
		axisPos = positive;
		axisNeg = negative;
	}

	// JNN: added
	public RebindableAxis (string name, KeyCode positive, KeyCode negative, KeyCode altPositive, KeyCode altNegative)
	{
		axisName = name;
		axisPos = positive;
		axisNeg = negative;
		altAxisPos = altPositive;
		altAxisNeg = altNegative;
	}
}
