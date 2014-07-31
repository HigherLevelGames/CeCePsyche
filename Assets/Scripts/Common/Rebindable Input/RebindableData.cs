using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class RebindableData : MonoBehaviour
{
	[SerializeField]
	List<RebindableKey> defaultRebindableKeys;
	[SerializeField]
	List<RebindableAxis> defaultRebindableAxes;
	
	List<RebindableKey> savedRebindableKeys;
	List<RebindableAxis> savedRebindableAxes;
	
	List<RebindableKey> rebindableKeys;
	List<RebindableAxis> rebindableAxes;
	
	void Awake ()
	{
		savedRebindableKeys = LoadSavedKeys ();
		savedRebindableAxes = LoadSavedAxes ();
		rebindableKeys = new List<RebindableKey>(savedRebindableKeys);
		rebindableAxes = new List<RebindableAxis>(savedRebindableAxes);
	}
	
	public static RebindableData GetRebindableManager ()
	{
		return (RebindableData)(GameObject.Find ("Rebindable Manager").GetComponent (typeof(RebindableData)));
	}
	
	List<RebindableKey> LoadSavedKeys ()
	{
		string rebindPrefs = PlayerPrefs.GetString ("RebindableKeyPrefs", "");
		
		if (rebindPrefs == "")
		{
			return CopyKeyList (defaultRebindableKeys);
		}
		else
		{
			string[] keybindPrefsSplit = rebindPrefs.Split ("\n".ToCharArray ());
			
			string[] keyNames = keybindPrefsSplit[0].Split ("*".ToCharArray ());
			string[] keyValues = keybindPrefsSplit[1].Split ("*".ToCharArray ());
			// JNN: added
			string[] altKeyValues = keybindPrefsSplit[2].Split ("*".ToCharArray ());

			List <RebindableKey> keys = new List<RebindableKey> ();
			
			for (int i = 0; i < keyNames.Length; i++)
			{
				//keys.Add (new RebindableKey(keyNames[i], (KeyCode)int.Parse (keyValues[i])));
				// JNN: replaced
				keys.Add(new RebindableKey(keyNames[i],
				                           (KeyCode)int.Parse(keyValues[i]),
				                           (KeyCode)int.Parse(altKeyValues[i])));
			}
			
			return keys;
		}
	}
	
	List<RebindableAxis> LoadSavedAxes ()
	{
		string axisPrefs = PlayerPrefs.GetString ("RebindableAxisPrefs", "");
		
		if (axisPrefs == "")
		{
			return CopyAxisList (defaultRebindableAxes);
		}
		else
		{
			string[] axisPrefsSplit = axisPrefs.Split ("\n".ToCharArray ());
			
			string[] axisNames = axisPrefsSplit[0].Split ("*".ToCharArray ());
			string[] axisPoses = axisPrefsSplit[1].Split ("*".ToCharArray ());
			string[] axisNegss = axisPrefsSplit[2].Split ("*".ToCharArray ());
			//JNN: added
			string[] altAxisPoses = axisPrefsSplit[3].Split ("*".ToCharArray ());
			string[] altAxisNegss = axisPrefsSplit[4].Split ("*".ToCharArray ());

			List<RebindableAxis> axes = new List<RebindableAxis> ();
			
			for (int i = 0; i < axisNames.Length; i++)
			{
				//axes.Add (new RebindableAxis(axisNames[i], (KeyCode)int.Parse (axisPoses[i]), (KeyCode)int.Parse (axisNegss[i])));
				// JNN: replaced
				axes.Add (new RebindableAxis(axisNames[i],
				                             (KeyCode)int.Parse (axisPoses[i]),
				                             (KeyCode)int.Parse (axisNegss[i]),
				                             (KeyCode)int.Parse (altAxisPoses[i]),
				                             (KeyCode)int.Parse (altAxisNegss[i])));
			}		
			
			return axes;
		}
	}
	
	public List<RebindableKey> GetCurrentKeys ()
	{
		return rebindableKeys;
	}
	
	public List<RebindableAxis> GetCurrentAxes ()
	{
		return rebindableAxes;
	}
	
	public void ActivateDefaultKeys ()
	{		
		rebindableKeys = CopyKeyList (defaultRebindableKeys);
	}
	
	public void ActivateDefaultAxes ()
	{		
		rebindableAxes = CopyAxisList (defaultRebindableAxes);
	}
	
	public void ActivateSavedKeys ()
	{		
		rebindableKeys = CopyKeyList (savedRebindableKeys);
	}
	
	public void ActivateSavedAxes ()
	{		
		rebindableAxes = CopyAxisList (savedRebindableAxes);
	}
	
	public void SaveKeys ()
	{
		string keyNames = "";
		string keyValues = "";
		// JNN: added
		string altKeyValues = "";
		
		savedRebindableKeys = new List<RebindableKey> (rebindableKeys);
		
		for (int i = 0; i < rebindableKeys.Count; i++)
		{
			if (i < rebindableKeys.Count - 1)
			{
				keyNames += rebindableKeys[i].inputName + "*";
				keyValues += ((int)rebindableKeys[i].input).ToString () + "*";

				// JNN: added
				altKeyValues += ((int)rebindableKeys[i].altInput).ToString () + "*";
			}
			else
			{
				keyNames += rebindableKeys[i].inputName;
				keyValues += ((int)rebindableKeys[i].input).ToString ();

				// JNN: added
				altKeyValues += ((int)rebindableKeys[i].altInput).ToString ();
			}
		}
		
		//string prefsToSave = keyNames + "\n" + keyValues;
		// JNN: replaced
		string prefsToSave = keyNames + "\n" + keyValues + "\n" + altKeyValues;
		
		PlayerPrefs.SetString ("RebindableKeyPrefs", prefsToSave);
	}
	
	public void SaveAxes ()
	{
		string axisNames = "";
		string axisPoses = "";
		string axisNegss = "";
		// JNN: added
		string altAxisPoses = "";
		string altAxisNegss = "";
		
		savedRebindableAxes = new List<RebindableAxis> (rebindableAxes);
		
		for (int i = 0; i < rebindableAxes.Count; i++)
		{
			if (i < rebindableAxes.Count - 1)
			{
				axisNames += rebindableAxes[i].axisName + "*";
				axisPoses += ((int)rebindableAxes[i].axisPos).ToString () + "*";
				axisNegss += ((int)rebindableAxes[i].axisNeg).ToString () + "*";
				// JNN: added
				altAxisPoses += ((int)rebindableAxes[i].altAxisPos).ToString () + "*";
				altAxisNegss += ((int)rebindableAxes[i].altAxisNeg).ToString () + "*";
			}
			else
			{
				axisNames += rebindableAxes[i].axisName;
				axisPoses += ((int)rebindableAxes[i].axisPos).ToString ();
				axisNegss += ((int)rebindableAxes[i].axisNeg).ToString ();
				// JNN: added
				altAxisPoses += ((int)rebindableAxes[i].altAxisPos).ToString ();
				altAxisNegss += ((int)rebindableAxes[i].altAxisNeg).ToString ();
			}
		}
		
		//string prefsToSave = axisNames + "\n" + axisPoses + "\n" + axisNegss;
		// JNN: replaced
		string prefsToSave = axisNames + "\n" + axisPoses + "\n" + axisNegss + "\n" + altAxisPoses + "\n" + altAxisNegss;
		
		PlayerPrefs.SetString ("RebindableAxisPrefs", prefsToSave);
	}
	
	List<RebindableKey> CopyKeyList (List<RebindableKey> listToCopy)
	{
		List<RebindableKey> listToReturn = new List<RebindableKey> (listToCopy.Count);
		
		foreach (RebindableKey key in listToCopy)
		{
			//listToReturn.Add (new RebindableKey(key.inputName, key.input));
			// JNN: replaced
			listToReturn.Add (new RebindableKey(key.inputName, key.input, key.altInput));
		}
		
		return listToReturn;
	}
	
	List<RebindableAxis> CopyAxisList (List<RebindableAxis> listToCopy)
	{
		List<RebindableAxis> listToReturn = new List<RebindableAxis> (listToCopy.Count);
		
		foreach (RebindableAxis axis in listToCopy)
		{
			//listToReturn.Add (new RebindableAxis(axis.axisName, axis.axisPos, axis.axisNeg));
			// JNN: replaced
			listToReturn.Add (new RebindableAxis(axis.axisName, axis.axisPos, axis.axisNeg, axis.altAxisPos, axis.altAxisNeg));
		}
		
		return listToReturn;
	}
}