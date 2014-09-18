using UnityEngine;
using System.Collections;

public class IdleOnOff : MonoBehaviour {
	public ParticleSystem particles;
	public GameObject subconscious;
	Vector3 pastPosition;
	public bool playing;
	// Use this for initialization
	void Start () {
		renderer.sortingLayerName = "Playground";
		renderer.sortingOrder = 10000;
		pastPosition = Vector3.zero;
		playing = false;
		particles.Play ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		float deltaPosition = Vector3.Distance(pastPosition,subconscious.transform.position);
		if (deltaPosition != 0.0f) 
		{
			particles.enableEmission = false;
				playing = false;
		} 
		else 
		{
			particles.enableEmission = true;
				playing = true;
		}
		pastPosition = subconscious.transform.position;
	}
}
