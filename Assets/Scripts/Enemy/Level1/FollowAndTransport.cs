using UnityEngine;
using System.Collections;

public class FollowAndTransport : MonoBehaviour {

	public GameObject player;
	private Transform playerPos;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		playerPos = player.transform;
	}
}
