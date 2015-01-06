using UnityEngine;
using System.Collections;

public class DogToyEffect : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Conditionable")
        {
            other.GetComponent<ResponseSet>().RespondAfter(ItemActions.SqueakyToy, 0.5f);
            gameObject.collider2D.enabled = false;
        }
    }
}
