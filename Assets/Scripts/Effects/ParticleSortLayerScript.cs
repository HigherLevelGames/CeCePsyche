using UnityEngine;
using System.Collections;

public class ParticleSortLayerScript : MonoBehaviour {
	void Start () {
		//name the layer particles that needs be in front of the background
		particleSystem.renderer.sortingLayerName = "Particles";
	}
}