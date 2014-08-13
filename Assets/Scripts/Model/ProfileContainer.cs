using UnityEngine;
using System.Collections;

// class kept persistent throughout game
public class ProfileContainer : MonoBehaviour
{
	private Profile mainProfile;

	// Use this for initialization
	void Start ()
	{
		DontDestroyOnLoad(this.transform.gameObject);
	}
	
	// Update is called once per frame
	void Update () { }

	void SetProfile(Profile p)
	{
		mainProfile = p;
	}
}
