using UnityEngine;
using System.Collections;

// See:  http://answers.unity3d.com/questions/576044/one-way-platform-using-2d-colliders.html
public class OneWayPlatform : MonoBehaviour
{

	void OnCollisionStay2D(Collision2D player)
	{
		if(player.gameObject.tag == "Player")
		{
			if(RebindableInput.GetAxisDown("Vertical") < 0)
			{
				this.gameObject.collider2D.isTrigger = true;
			}

		}
	}
	
}