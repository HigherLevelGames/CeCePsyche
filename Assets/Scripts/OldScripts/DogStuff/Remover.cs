using UnityEngine;
using System.Collections;

public class Remover : MonoBehaviour
{
	//THIS GOES ON DOG END!!!
	void OnTriggerEnter2D(Collider2D col) //the killTrigger object is set as a rectangle at the bottom of the screen
	{
		// If the player hits the trigger...
		if(col.gameObject.tag == "Player")
		{
			//Destroy (col.gameObject);
		}
		else
		{
		// ... instantiate the splash where the enemy falls in.
		// Destroy the enemy.
		Destroy (col.gameObject);		
		}
	}
}