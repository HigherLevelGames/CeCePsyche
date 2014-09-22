using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimpleClouds : MonoBehaviour
{
		
		public int MaxClouds = 20;
		public Camera Cam;
		public GameObject[] Clonables;
		List<Cloud> Clouds = new List<Cloud> ();
		float timeBetweenClouds;

		void Start ()
		{
				timeBetweenClouds = 4 + Random.value * 4f;	
		}

		void Update ()
		{
				if (Clouds.Count < MaxClouds) {
						timeBetweenClouds -= Time.deltaTime;
						if (timeBetweenClouds < 0.0f) {
								CreateCloudAtRandomLocation (true);
								timeBetweenClouds = 2 + Random.value * 4f;
						}
				}
				for (int i = 0; i < Clouds.Count; i++) {
						if (Clouds [i].Exists)
								Clouds [i].Update (Cam.transform.position);
						else {
								Destroy (Clouds [i].CloudObject, 0.5f);
								Clouds.RemoveAt (i);
						}
				}

		}

		void CreateCloudAtRandomLocation (bool offscreen)
		{
				Vector3 p = new Vector3 ();	
				float x = 0;
				float y = 0;
				if (offscreen) {
						x = 10 + Random.value * 20f;
			y = Random.value * 15f - 7.5f;
				} else {
						x = Random.value * 20f - 10f;
						y = Random.value * 15f - 7.5f;
				}
				p = new Vector3 (Cam.transform.position.x + x, Cam.transform.position.y + y, 0);
				int idx = Mathf.FloorToInt (Random.value * (Clonables.Length - 0.0001f));
				Cloud c = new Cloud ();
				c.CloudObject = Instantiate (Clonables [idx]) as GameObject;
				c.CloudObject.transform.position = p;
		c.CloudObject.transform.localScale = new Vector3 (c.Size, c.Size, 1);
				c.CloudObject.transform.parent = this.transform;
				Clouds.Add (c);
				
				
		}
}

public class Cloud
{
		public Vector3 Velocity;
		public GameObject CloudObject;
		public bool Exists;
	public float Size;
		public Cloud ()
		{
		float val = Random.value;
				Exists = true;
				Velocity = new Vector3 (-val, 0f);
		Size = (1 / (val + 0.1f));
		}
	
		public void Update (Vector3 pos)
		{
				CloudObject.transform.position += Velocity * Time.deltaTime;
				if (Vector3.Distance (pos, CloudObject.transform.position) > 100)
						Exists = false;
		}
}
