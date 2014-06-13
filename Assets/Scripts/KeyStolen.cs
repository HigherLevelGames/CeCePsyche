using UnityEngine;
using System.Collections;

public class KeyStolen : MonoBehaviour {

	// Use this for initialization
	void Start () {
		PlayerPrefs.SetInt ("stolenKey", 1);
	}
	
	// Update is called once per frame
	void Update () {
		
		if(PlayerPrefs.GetInt("stolenKey") == 0)
		{
			renderer.enabled = true; 
			StartCoroutine(Example());
			PlayerPrefs.SetInt ("stolenKey", 1);
		}		
	
	}
	
	IEnumerator Example() {
	yield return new WaitForSeconds(5);
	renderer.enabled = false;
	}
}
