using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour
{
	void UseMe(bool isFacingRight)
	{
		if(isFacingRight)
		{
			this.rigidbody2D.velocity = new Vector2(5.0f,5.0f);
		}
		else
		{
			this.rigidbody2D.velocity = new Vector2(-5.0f,5.0f);
		}
	}
}
