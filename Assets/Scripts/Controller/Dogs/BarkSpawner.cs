using UnityEngine;
using System.Collections;

public class BarkSpawner : MonoBehaviour {
	public GameObject objectToSpawn;
	public Transform spawnPos;


	void Update()
	{
		if( Input.GetKeyDown(KeyCode.B))
			CreateSB();
	}

	public void CreateSB () 
	{
			if(objectToSpawn != null && spawnPos != null)
			{
				Vector3 spawnSpot = new Vector3(spawnPos.position.x, spawnPos.position.y, spawnPos.position.z);
				Instantiate(objectToSpawn,spawnSpot, Quaternion.identity);
			}
	}
}
