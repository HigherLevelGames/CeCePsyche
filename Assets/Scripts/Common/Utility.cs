using UnityEngine;
using System.Collections;

namespace Common
{
	// class to hold helper functions
	public class Utility
	{
		// adjusts a rectangle using percentages to screen coordinates
		public static Rect adjRect(Rect r)
		{
			return new Rect(r.x * Screen.width / 100,
			                r.y * Screen.height / 100,
			                r.width * Screen.width / 100,
			                r.height * Screen.height / 100);
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
	}
}
