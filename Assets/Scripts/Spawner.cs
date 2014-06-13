using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
	public float spawnTime = 5f;		// The amount of time between each spawn.
	public float spawnDelay = 3f;		// The amount of time before spawning starts.
	//DogStart dogStart = trigger.GetComponent<DogStart> ();
	//public bool shouldSpawn = dogStart.spawn;
	
	public GameObject dog;
	
	void Start ()
	{

	}

	void Update ()
	{

		if (PlayerPrefs.GetInt ("spawn") == 1) {
			Spawn ();
			PlayerPrefs.SetInt ("spawn", 0);
		}
	}


	void Spawn ()
	{
		//SPAWN DOG HERE INSTEAD OF ENEMIES!!!!!!!

		// Instantiate a random enemy.
		
		Instantiate(dog, transform.position, transform.rotation);
		//spawn the chosen enemy at the object's standard position and rotation

		// Play the spawning effect from all of the particle systems.
		foreach(ParticleSystem p in GetComponentsInChildren<ParticleSystem>())
		{
			p.Play();
		}
	}
}
