using UnityEngine;
using System.Collections;

public class MoveToPlayerLayer : MonoBehaviour {

	void Start ()
	{
		//Change Foreground to the layer you want it to display on
		//You could prob. make a public variable for this
		particleSystem.renderer.sortingLayerName = "Playground";
		particleSystem.renderer.sortingOrder = 9001;
	}
}
