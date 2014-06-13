using UnityEngine;
using System.Collections;

public class Notifications : MonoBehaviour {
	
	public GameObject player; 
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//transform.position += new Vector3(player.transform.position.x, player.transform.position.y, -1f); 
		
		Vector3 dir = player.transform.position - transform.position;
		dir.y = 0f;
		dir.z = 0f;
		if(dir.magnitude < Time.deltaTime * 5f){
			transform.position = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
		}
		else 	
			transform.position += dir.normalized * Time.deltaTime * Mathf.Max(6,dir.magnitude);
		
		//StartCoroutine(Example());
 
		
		
		
		
		
	}
	
	
	IEnumerator Example() {
		//Debug.Log(Time.time);
		yield return new WaitForSeconds(1);
		//print(Time.time);
		//GetComponent<PlayerControl>().enabled = true;
	}
	
}
