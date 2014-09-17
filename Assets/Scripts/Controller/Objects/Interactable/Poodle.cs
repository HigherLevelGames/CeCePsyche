using UnityEngine;
using System.Collections;

public class Poodle : MonoBehaviour
{
	bool activated = false;

	void Interact()
	{
		if(!activated)
		{
			Destroy(this.GetComponent<FlyAroundTarget>());
			BoxCollider2D boxCol = (BoxCollider2D)this.collider2D;
			boxCol.size = Vector2.one;
			boxCol.isTrigger = false;
			this.gameObject.layer = LayerMask.NameToLayer("Ground");
			activated = true;
		}
	}
}
