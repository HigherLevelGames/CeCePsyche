using UnityEditor;
using UnityEngine;
using System.Collections;
using System;

namespace nTools
{
	public class PrefsEditor : MonoBehaviour 
	{
		[MenuItem ("Tools/Edit Prefs Utility",false,50)]
		static void EditPref ()
		{
			PrefsEditorWindow.ShowWindow();
		}

		[MenuItem ("Tools/Delete PlayerPrefs",false,60)]
		static void ClearPlayerPrefs ()
		{
			if (EditorUtility.DisplayDialog(
				"Delete all player preferences?",
				"Are you sure you want to delete all the player preferences? " +
				"This action cannot be undone.",
				"Yes",
				"No"))
			{
				PlayerPrefs.DeleteAll();
				
				Debug.Log("Successfully deleted all key-value pairs in PlayerPrefs.");
			}
		}
		
		[MenuItem ("Tools/Delete EditorPrefs",false,70)]
		static void ClearEditorPrefs ()
		{
			if (EditorUtility.DisplayDialog(
				"Delete all editor preferences?",
				"WARNING: Editor preferences are not project specific. " +
				"Deleting them will affect all other projects as well.\n\n" +
				"Are you sure you want to delete all the editor preferences? " +
				"This action cannot be undone.",
				"Yes",
				"No"))
			{
				EditorPrefs.DeleteAll();
				
				Debug.Log("Successfully deleted all key-value pairs in EditorPrefs.");
			}
		}
	}

	public class PrefsEditorWindow : EditorWindow
	{
		const int _numOfLines = 7;
		const float _lineHeight = 20f;
		const float _height = _lineHeight * (float)_numOfLines;
		const float _width = 320f;

		private enum PrefType {@PlayerPrefs = 0, @EditorPrefs = 1}
		private enum PrefDataType {Int = 0, Float = 1, @String = 2};

		private PrefType _prefType;
		private PrefDataType _prefDataType;
		private string _key, _value;

		private static PrefsEditorWindow _window = null;
		
		public static void ShowWindow ()
		{
			if (_window == null)
			{
				_window = CreateInstance(typeof(PrefsEditorWindow)) as PrefsEditorWindow;
			
				float left = (Screen.width - _width) * 0.5f;
				float top = (Screen.height - _height) * 0.5f;

				_window.title = "Edit Prefs Utility";
				_window.position = new Rect(left,top,_width,_height);
				_window.maxSize = new Vector2(_width,_height);
				_window.minSize = _window.maxSize;

				_window._key = string.Empty;
				_window._value = string.Empty;
			}

			_window.ShowUtility();
		}

		void OnGUI ()
		{
			_prefType = (PrefType)EditorGUILayout.EnumPopup("Prefs Type",_prefType);
			_prefDataType = (PrefDataType)EditorGUILayout.EnumPopup("Data Type",_prefDataType);
			_key = EditorGUILayout.TextField("Key",_key);
			_value = EditorGUILayout.TextField("Value",_value);

			GUI.enabled = false;
			//TODO: Get Value Button
			// - test if key is empty.
			// - get value as int, float, and string.
			// - test each value for deviation from default values.
			// - update _prefDataType to match value type.
			// GUI.enabled = HasKey(_key);
			if (GUILayout.Button("Get Value")) {}
			GUI.enabled = true;

			if (GUILayout.Button("Save Changes")) 
			{
				string errorMessage = string.Empty;
				string formatErrorMessage = "Please re-enter an appropriate value or change the Date Type to string.";
				string rangeErrorMessage = "The value is outside the range of the selected Date Type. " + formatErrorMessage;

				if (IsKeyEmpty(_key)) return;

				if (!HasKey(_key))
				{
					if (!EditorUtility.DisplayDialog(
						"Key Not Found",
						"Would you like to create a new key-value pair?",
						"Yes",
						"No"))
					{
						return;
					}
				}

				switch (_prefDataType)
				{
				case PrefDataType.Int:
					try
					{
						int iValue = 0;
						if (!string.IsNullOrEmpty(_value)) 
						{
							Convert.ToInt32(_value);
						}
						else 
						{
							_value = iValue.ToString();
							_window.Repaint();
						}

						SetInt(_key,iValue);
						goto Success;
					}
					catch (FormatException)
					{
						errorMessage = formatErrorMessage;
						goto ConversionFailure;
					}
					catch (OverflowException)
					{
						errorMessage = rangeErrorMessage;
						goto ConversionFailure;
					}
				case PrefDataType.Float:
					try
					{
						float fValue = 0f;
						if (!string.IsNullOrEmpty(_value))
						{
							Convert.ToSingle(_value);
						}
						else 
						{
							_value = fValue.ToString();
							_window.Repaint();
						}

						SetFloat(_key,fValue);
						goto Success;
					}
					catch (FormatException)
					{
						errorMessage = formatErrorMessage;
						goto ConversionFailure;
					}
					catch (OverflowException)
					{
						errorMessage = rangeErrorMessage;
						goto ConversionFailure;
					}
				case PrefDataType.@String:
					SetString(_key,_value);
					goto Success;
				}

			ConversionFailure:
				if (EditorUtility.DisplayDialog(
					"Error",
					string.Format("Failed to convert the Value field content to type {0}. {1}",_prefDataType.ToString(),errorMessage),
					"Ok"))
				{
					return;
				}

			Success:
				EditorUtility.DisplayDialog(
					"Success",
					string.Format("The key-value pair was succesfully saved to {0}.",_prefType.ToString()),
					"Ok");
			}

			if (GUILayout.Button("Delete Key-Value Pair")) 
			{
				if (IsKeyEmpty(_key)) return;

				if (HasKey(_key))
				{
					DeleteKey(_key);

					EditorUtility.DisplayDialog(
						"Success",
						string.Format("The key-value pair was succesfully deleted from {0}.",_prefType.ToString()),
						"Ok");

					_key = string.Empty;
					_value = string.Empty;

					_window.Repaint();
				}
				else
				{
					EditorUtility.DisplayDialog(
						"Key Not Found",
						"Unable to delete the specified key-value pair.",
						"Ok");
				}
			}
		}

		private bool IsKeyEmpty (string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				if (EditorUtility.DisplayDialog(
					"Key is Null or Empty",
					"The field entry for Key must not be left blank.",
					"Ok"))
				{
					return true;
				}
			}

			return false;
		}

		private bool HasKey (string key)
		{
			switch (_prefType)
			{
			case PrefType.@PlayerPrefs:
				return PlayerPrefs.HasKey(key);
			case PrefType.@EditorPrefs:
				return EditorPrefs.HasKey(key);
			}

			return false;
		}

		private void DeleteKey (string key)
		{
			switch (_prefType)
			{
			case PrefType.@PlayerPrefs:
				PlayerPrefs.DeleteKey(key);
				break;
			case PrefType.@EditorPrefs:
				EditorPrefs.DeleteKey(key);
				break;
			}			
		}

		private void SetInt (string key, int value)
		{
			switch (_prefType)
			{
			case PrefType.@PlayerPrefs:
				PlayerPrefs.SetInt(key,value);
				break;
			case PrefType.@EditorPrefs:
				EditorPrefs.SetInt(key,value);
				break;
			}
		}

		private void SetFloat (string key, float value)
		{
			switch (_prefType)
			{
			case PrefType.@PlayerPrefs:
				PlayerPrefs.SetFloat(key,value);
				break;
			case PrefType.@EditorPrefs:
				EditorPrefs.SetFloat(key,value);
				break;
			}
		}

		private void SetString (string key, string value)
		{
			switch (_prefType)
			{
			case PrefType.@PlayerPrefs:
				PlayerPrefs.SetString(key,value);
				break;
			case PrefType.@EditorPrefs:
				EditorPrefs.SetString(key,value);
				break;
			}
		}
	}

	/*
	 * Missing functionality neccessary for a Show Prefs utility:
	 * No utility exists for listing all existing PlayerPrefs or EditorPrefs.
	 * 
	public class PlayerPrefsWindow : PrefsWindow
	{
		public static void ShowWindow ()
		{
			PlayerPrefsWindow window = EditorWindow.GetWindow(typeof(PlayerPrefsWindow),false,"PlayerPrefs") as PlayerPrefsWindow;

			window.label = "Player Preferences";
		}
	}

	public class EditorPrefsWindow : PrefsWindow
	{
		public static void ShowWindow ()
		{
			EditorPrefsWindow window = EditorWindow.GetWindow(typeof(EditorPrefsWindow),false,"EditorPrefs") as EditorPrefsWindow;

			window.label = "Editor Preferences";
		}
	}

	public class PrefsWindow : EditorWindow
	{
		protected string label { get; set; }

		void OnGUI ()
		{
			GUILayout.Label(label, EditorStyles.boldLabel);
		}
	}
	/* END */
}