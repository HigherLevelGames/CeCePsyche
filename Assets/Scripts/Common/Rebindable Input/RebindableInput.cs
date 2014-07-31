using UnityEngine;
using System;
using System.Collections.Generic;

public class RebindableInput : MonoBehaviour
{
	static RebindableData rebindableManager;
	
	// Use this for initialization
	void Start ()
	{
		rebindableManager = RebindableData.GetRebindableManager ();
	}
	
	public static bool GetKey (string inputName)
	{
		List<RebindableKey> keyDatabase = rebindableManager.GetCurrentKeys ();
		
		foreach (RebindableKey key in keyDatabase)
		{
			if (key.inputName == inputName)
			{
				//return Input.GetKey (key.input);
				// JNN: replaced
				return Input.GetKey (key.input) || Input.GetKey(key.altInput);
			}
		}
		
		throw new RebindableNotFoundException ("The rebindable key '" + inputName + "' was not found.\nBe sure you have created it and haven't misspelled it.");
	}
	
	public static bool GetKeyDown (string inputName)
	{
		List<RebindableKey> keyDatabase = rebindableManager.GetCurrentKeys ();
		
		foreach (RebindableKey key in keyDatabase)
		{
			if (key.inputName == inputName)
			{
				//return Input.GetKeyDown (key.input);
				// JNN: replaced
				return Input.GetKeyDown(key.input) || Input.GetKeyDown(key.altInput);
			}
		}
		
		throw new RebindableNotFoundException ("The rebindable key '" + inputName + "' was not found.\nBe sure you have created it and haven't misspelled it.");
	}
	
	public static bool GetKeyUp (string inputName)
	{
		List<RebindableKey> keyDatabase = rebindableManager.GetCurrentKeys ();
		
		foreach (RebindableKey key in keyDatabase)
		{
			if (key.inputName == inputName)
			{
				//return Input.GetKeyUp (key.input);
				// JNN: replaced
				return Input.GetKeyUp (key.input) || Input.GetKeyUp(key.altInput);
			}
		}
		
		throw new RebindableNotFoundException ("The rebindable key '" + inputName + "' was not found.\nBe sure you have created it and haven't misspelled it.");
	}
	
	public static int GetAxis (string axisName)
	{
		List<RebindableAxis> axisDatabase = rebindableManager.GetCurrentAxes ();
		
		foreach (RebindableAxis axis in axisDatabase)
		{
			if (axis.axisName == axisName)
			{
				//bool posPressed = Input.GetKey (axis.axisPos);
				//bool negPressed = Input.GetKey (axis.axisNeg);
				// JNN: replaced
				bool posPressed = Input.GetKey (axis.axisPos) || Input.GetKey(axis.altAxisPos);
				bool negPressed = Input.GetKey (axis.axisNeg) || Input.GetKey(axis.altAxisNeg);

				return 0 + (posPressed ? 1 : 0) - (negPressed ? 1 : 0);
			}
		}
		
		throw new RebindableNotFoundException ("The rebindable axis '" + axisName + "' was not found.\nBe sure you have created it and haven't misspelled it.");
	}

	// JNN: added
	public static int GetAxisDown (string axisName)
	{
		List<RebindableAxis> axisDatabase = rebindableManager.GetCurrentAxes ();
		
		foreach (RebindableAxis axis in axisDatabase)
		{
			if (axis.axisName == axisName)
			{
				bool posPressed = Input.GetKeyDown (axis.axisPos) || Input.GetKeyDown(axis.altAxisPos);
				bool negPressed = Input.GetKeyDown (axis.axisNeg) || Input.GetKeyDown(axis.altAxisNeg);
				
				return 0 + (posPressed ? 1 : 0) - (negPressed ? 1 : 0);
			}
		}
		
		throw new RebindableNotFoundException ("The rebindable axis '" + axisName + "' was not found.\nBe sure you have created it and haven't misspelled it.");
	}
	
	public static KeyCode GetKeyFromBinding (string inputName)
	{
		List<RebindableKey> keyDatabase = rebindableManager.GetCurrentKeys ();
		
		foreach (RebindableKey key in keyDatabase)
		{
			if (key.inputName == inputName)
			{
				return key.input;
			}
		}
		
		throw new RebindableNotFoundException ("The rebindable key '" + inputName + "' was not found.\nBe sure you have created it and haven't misspelled it.");
	}

	// JNN: added
	public static KeyCode GetAltKeyFromBinding (string inputName)
	{
		List<RebindableKey> keyDatabase = rebindableManager.GetCurrentKeys ();
		
		foreach (RebindableKey key in keyDatabase)
		{
			if (key.inputName == inputName)
			{
				return key.altInput;
			}
		}
		
		throw new RebindableNotFoundException ("The rebindable key '" + inputName + "' was not found.\nBe sure you have created it and haven't misspelled it.");
	}
	
	public static KeyCode GetPositiveFromAxis (string axisName)
	{
		List<RebindableAxis> axisDatabase = rebindableManager.GetCurrentAxes ();
		
		foreach (RebindableAxis axis in axisDatabase)
		{
			if (axis.axisName == axisName)
			{
				return axis.axisPos;
			}
		}
		
		throw new RebindableNotFoundException ("The rebindable axis '" + axisName + "' was not found.\nBe sure you have created it and haven't misspelled it.");
	}
	
	public static KeyCode GetNegativeFromAxis (string axisName)
	{
		List<RebindableAxis> axisDatabase = rebindableManager.GetCurrentAxes ();
		
		foreach (RebindableAxis axis in axisDatabase)
		{
			if (axis.axisName == axisName)
			{
				return axis.axisNeg;
			}
		}
		
		throw new RebindableNotFoundException ("The rebindable axis '" + axisName + "' was not found.\nBe sure you have created it and haven't misspelled it.");
	}

	// JNN: added
	public static KeyCode GetAltPositiveFromAxis (string axisName)
	{
		List<RebindableAxis> axisDatabase = rebindableManager.GetCurrentAxes ();
		
		foreach (RebindableAxis axis in axisDatabase)
		{
			if (axis.axisName == axisName)
			{
				return axis.altAxisPos;
			}
		}
		
		throw new RebindableNotFoundException ("The rebindable axis '" + axisName + "' was not found.\nBe sure you have created it and haven't misspelled it.");
	}

	// JNN: added
	public static KeyCode GetAltNegativeFromAxis (string axisName)
	{
		List<RebindableAxis> axisDatabase = rebindableManager.GetCurrentAxes ();
		
		foreach (RebindableAxis axis in axisDatabase)
		{
			if (axis.axisName == axisName)
			{
				return axis.altAxisNeg;
			}
		}
		
		throw new RebindableNotFoundException ("The rebindable axis '" + axisName + "' was not found.\nBe sure you have created it and haven't misspelled it.");
	}
}
