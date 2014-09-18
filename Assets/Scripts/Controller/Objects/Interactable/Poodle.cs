using UnityEngine;
using System.Collections;

public class Poodle : MonoBehaviour
{
	bool activated = false;
	public FlyAroundTarget hoverScript;
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
}
