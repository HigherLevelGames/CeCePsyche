using UnityEngine;
using System.Collections;

namespace Common
{
	// class to hold helper functions
	public class Utility
	{
		public static Vector2 GUIMousePos
		{
			get
			{
				Vector2 temp = Input.mousePosition;
				temp.y = Screen.height - temp.y;
				return temp;
			}
		}

		// adjusts a rectangle using percentages to screen coordinates
		public static Rect adjRect(Rect r)
		{
			return new Rect(r.x * Screen.width / 100.0f,
			                r.y * Screen.height / 100.0f,
			                r.width * Screen.width / 100.0f,
			                r.height * Screen.height / 100.0f);
		}

		// inverse of adjRect
		public static Rect unadjRect(Rect r)
		{
			return new Rect(r.x * 100.0f / Screen.width,
			                r.y * 100.0f / Screen.height,
			                r.width * 100.0f / Screen.width,
			                r.height * 100.0f / Screen.height);
		}

		public static Vector3 Clamp(Vector3 value, Vector3 min, Vector3 max)
		{
			return new Vector3(
				Mathf.Clamp(value.x, min.x, max.x),
				Mathf.Clamp(value.y, min.y, max.y),
				Mathf.Clamp(value.z, min.z, max.z));
		}

		public static Vector2 toVector2(Vector3 v)
		{
			return new Vector2(v.x, v.y);
		}

		// Wrapper for setting bool in PlayerPrefs
		// 0 = false
		// 1 = true
		public static void SetBool(string key, bool value)
		{
			int val = (value) ? 1 : 0;
			PlayerPrefs.SetInt(key, val);
		}

		// Wrapper for getting bool in PlayerPrefs
		public static bool GetBool(string key)
		{
			return (PlayerPrefs.GetInt(key) != 0);
		}

		// Wrapper for setting Vector3 in PlayerPrefs
		public static void SetVector3(string key, Vector3 value)
		{
			PlayerPrefs.SetFloat(key + "_x", value.x);
			PlayerPrefs.SetFloat(key + "_y", value.y);
			PlayerPrefs.SetFloat(key + "_z", value.z);
		}

		// Wrapper for getting Vector3 in PlayerPrefs;
		public static Vector3 GetVector3(string key)
		{
			Vector3 temp = Vector3.zero;
			temp.x = PlayerPrefs.GetFloat(key + "_x");
			temp.y = PlayerPrefs.GetFloat(key + "_y");
			temp.z = PlayerPrefs.GetFloat(key + "_z");
			return temp;
		}
	}
}
