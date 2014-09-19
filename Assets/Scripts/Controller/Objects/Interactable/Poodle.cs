using UnityEngine;
using System.Collections;

public class Poodle : MonoBehaviour
{
	bool activated = false;
	//public FlyAroundTarget hoverScript;
	private FlyAroundTarget hoverScript;

	void Start()
	{
		hoverScript = this.GetComponent<FlyAroundTarget>();
	}

	void Interact()
	{
		if(!activated)
		{
			//Destroy(this.GetComponent<FlyAroundTarget>());
			hoverScript.enabled = false;
			BoxCollider2D boxCol = (BoxCollider2D)this.collider2D;
			//boxCol.size = Vector2.one;
			boxCol.isTrigger = false;
			this.gameObject.layer = LayerMask.NameToLayer("Ground");
			activated = true;
		}
		else
		{
			hoverScript.enabled = true;
			BoxCollider2D boxCol = (BoxCollider2D)this.collider2D;
			//boxCol.size = Vector2.one;
			boxCol.isTrigger = true;
			this.gameObject.layer = LayerMask.NameToLayer("Default");
			activated = false;
		}



	}

	void OnTriggerEnter2D(Collider2D player)
	{
		if(player.tag == "Player")
		{
			// let player pass through
			hoverScript.enabled = true;
			
		}
	}
}
