using UnityEngine;
using System.Collections;

public class FollowAndTransport : MonoBehaviour {

	private GameObject player;
	float speed = 3f;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {

		float direction = player.transform.position.x - this.transform.position.x;
		this.transform.position += new Vector3(direction, 0, 0) * speed * Time.deltaTime;
	}
}
