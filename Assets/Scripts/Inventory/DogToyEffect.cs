using UnityEngine;
using System.Collections;

public class DogToyEffect : MonoBehaviour
{
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
			print ("Collider found - enter");
			other.GetComponent<ResponseSet>().RespondAfter(ItemActions.SqueakyToy, 0.5f);
			gameObject.collider2D.enabled = false;
		}
	}
}
