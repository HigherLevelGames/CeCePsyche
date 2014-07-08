using UnityEngine;
using System.Collections;

public class FollowAndTransport : MonoBehaviour {

	public Transform playerPos;

	// Use this for initialization
	void Start () {
		playerPos = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
