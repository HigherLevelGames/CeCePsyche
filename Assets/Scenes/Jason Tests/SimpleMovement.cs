using UnityEngine;
using System.Collections;

public class SimpleMovement : MonoBehaviour {


	void Start () {
	
	}
	

	void Update () {
		Vector3 p = new Vector3 ();
		if (Input.GetKey (KeyCode.W))
			 p += new Vector3 (0, 10f, 0);
		if(Input.GetKey (KeyCode.A))
			p += new Vector3 (-10f, 0, 0);
		if(Input.GetKey (KeyCode.S))
			 p += new Vector3 (0, -10f, 0);
		if(Input.GetKey (KeyCode.D))
			p += new Vector3 (10f, 0, 0);
		this.transform.position += p * Time.deltaTime;
	}
}
