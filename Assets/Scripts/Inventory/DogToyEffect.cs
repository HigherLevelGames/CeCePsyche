using UnityEngine;
using System.Collections;

public class DogToyEffect : MonoBehaviour
{
	void Start()
	{
		//toCeciScript = this.GetComponent<FollowTargetX>();
		//toCeciScript.enabled = false;
	}
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Conditionable")
        {
			print ("Collider found");
            other.GetComponent<ResponseSet>().RespondAfter(ItemActions.SqueakyToy, 0.5f);
            gameObject.collider2D.enabled = false;
        }
    }
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Conditionable")
		{
			other.GetComponent<ResponseSet>().RespondAfter(ItemActions.SqueakyToy, 0.5f);
			other.rigidbody2D.velocity = Vector2.zero;
			gameObject.collider2D.enabled = false;
		}
	}
}
