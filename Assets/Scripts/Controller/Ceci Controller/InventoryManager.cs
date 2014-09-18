using UnityEngine;
using System.Collections;

public class InventoryManager : MonoBehaviour
{
	void Start() { }
	
	void Update() { }
	
	#region Tempporary OnGUI Display for testing
	int numTreats = 0;

	void OnGUI()
	{
		GUI.Label(new Rect(0,0,100,50),
		          "Treats: " + numTreats);
	}
	#endregion
	
	/*
	void OnCollisionEnter2D(Collision2D col)
	{
		// if(collided object is the floor && CeCe is above it)
		//CurJumpState = JumpState.Grounded;
	}//*/
	
	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.tag == "Collectable")
		{
			// collect the item, e.g. memories, neurons, emotion items, etc.
			//PlayerPrefs.SetInt(col.gameObject.name, PlayerPrefs.GetInt(col.gameObject.name) + 1);
			//*
			if(col.gameObject.name == "Treat")
			{
				numTreats++;
			}

			//*/
			Destroy(col.gameObject);
		}
	}

	// When CeCe gets close enough to an interactable object, i.e. she enter's the object's trigger zone
	void OnTriggerStay2D(Collider2D col)
	{
		// TODO: Jason's particle system or something that helps the player know the object is interactable
		
		// pressed interact key
		if(RebindableInput.GetKeyDown("Interact") && col.gameObject.tag == "Interactable")
		{
			// tell the other object to perform some action, e.g. open doors, treasure chests, use item/switches, etc.
			col.gameObject.SendMessage("Interact", SendMessageOptions.DontRequireReceiver);
		}
	}
}
