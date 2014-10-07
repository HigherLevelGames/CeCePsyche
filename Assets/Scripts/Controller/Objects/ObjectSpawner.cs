using UnityEngine;
using System.Collections;

public class ObjectSpawner : MonoBehaviour {
	public GameObject objectToSpawn;
	public float spawnFrequency;
	float elapsedTime = 0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		elapsedTime += Time.deltaTime; 
		if(elapsedTime >= spawnFrequency)
		{
			Instantiate(objectToSpawn,this.transform.position, Quaternion.identity);
			elapsedTime = 0f;
		}
	}
}
