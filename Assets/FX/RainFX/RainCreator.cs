using UnityEngine;
using System.Collections;

public class RainCreator : MonoBehaviour {
	
	// rain type	0 = soft rain, 1 = normal rain, 2 = heavy rain
	public int rainLevel = 0;
	
	// local position
	public Vector3 localPosition;
	
	// rain prefabs
	public GameObject softRain;
	public GameObject rain;
	public GameObject heavyRain;

	public void Awake () {

		softRain = (GameObject) Resources.Load("Soft Rain");
		rain = (GameObject) Resources.Load ("Rain");
		heavyRain = (GameObject) Resources.Load ("Heavy Rain");

	}

	
	public void OnEnable () {

		// instantiate the prefab and detach the collision object
		switch (rainLevel) {
			
			default:	softRain = Instantiate(softRain) as GameObject;
						softRain.transform.DetachChildren();
						softRain.transform.parent = transform;
						softRain.transform.localPosition = localPosition;
						break;
			
			case 1:		rain = Instantiate(rain) as GameObject;
						rain.transform.DetachChildren();
						rain.transform.parent = transform;
						rain.transform.localPosition = localPosition;
						break;
			
			case 2:		heavyRain = Instantiate(heavyRain) as GameObject;
						heavyRain.transform.DetachChildren();
						heavyRain.transform.parent = transform;
						heavyRain.transform.localPosition = localPosition;
						break;
			
		}
		
	}
	
}
