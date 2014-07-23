using UnityEngine;
using System.Collections;

public class CameraFollowPlayer : MonoBehaviour {
	
	public GameObject player; 
	
	
	// Use this for initialization
	void Start () {
	
		
	}
	
	// Update is called once per frame
	void Update () {
		//transform.position += new Vector3((player.transform.position.x - transform.position.x) * Time.deltaTime * 2f, 0, 0); 
		Vector3 dir = player.transform.position - transform.position;
		dir.y = 0f;
		dir.z = 0f;
		if(dir.magnitude < Time.deltaTime * 10f){
			transform.position = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
		}
		else 	
			transform.position += dir.normalized * Time.deltaTime * Mathf.Max(5,dir.magnitude/2);
		
	}
}
