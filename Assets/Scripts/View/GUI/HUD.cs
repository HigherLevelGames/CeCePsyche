using UnityEngine;
using System.Collections;
using System.Collections.Generic; // for List<T>
using Common;

public class HUD : MonoBehaviour
{
	Rect currentItemBox = new Rect(45,5,10,5);
	Rect currentItemBtnBox = new Rect(45,10,10,10);
	Rect leftSideBar = new Rect(80,0,20,100);
	Rect ResponseBox1 = new Rect(80,20,20,20);
	Rect ResponseBox2 = new Rect(80,40,20,20);
	Rect ResponseBox3 = new Rect(80,60,20,20);
	Rect ResponseBox4 = new Rect(80,80,20,20);

	List<string> responses = new List<string>();

	// Use this for initialization
	void Start ()
	{
		responses.Add("Happiness Icon");
		responses.Add("Sadness Icon");
		responses.Add("Fear Icon");
		responses.Add("Anger Icon");
	}

	// custom shuffling method
	void Shuffle(ref List<string> items)
	{
		for(int i = 0; i < items.Count; i++)
		{
			// choose two random indices
			int m = Random.Range(0,items.Count); // inclusive, exclusive
			int n = Random.Range(0,items.Count);

			// swap indices
			string temp = items[m];
			items[m] = items[n];
			items[n] = temp;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Input.GetMouseButtonDown(0) && !Utility.adjRect(currentItemBox).Contains(Input.mousePosition))
		{
			Utility.SetBool("EduMode", false);
		}
	}

	void OnGUI()
	{
		GUI.Box(Utility.adjRect(currentItemBox), "Conditioned\nStimulus");
		if(GUI.Button (Utility.adjRect(currentItemBtnBox), "Icon"))
		{
			Utility.SetBool("EduMode", true);
			if(Utility.GetBool("EduMode"))
			{
				// show responses/quiz
				Shuffle(ref responses);
			}
			else
			{
				// just use the gosh darn item
			}
		}

		if(Utility.GetBool("EduMode"))
		{
			GUI.Box (Utility.adjRect(leftSideBar), "Conditioned Response");
			GUI.Button (Utility.adjRect(ResponseBox1), responses[0]);
			GUI.Button (Utility.adjRect(ResponseBox2), responses[1]);
			GUI.Button (Utility.adjRect(ResponseBox3), responses[2]);
			GUI.Button (Utility.adjRect(ResponseBox4), responses[3]);
		}
	}
}
