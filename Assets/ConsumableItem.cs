using UnityEngine;
using System.Collections;

public class ConsumableItem : MonoBehaviour
{
    public ItemActions item = ItemActions.DogBone;
    public AudioClip consumedSound;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Conditionable")
        {
            other.gameObject.audio.PlayOneShot(consumedSound);
            Conditionable c = other.gameObject.GetComponent<Conditionable>();
            c.ConditionedResponse = (int)ItemActions.DogBone;
            c.Consuming = true;
            Destroy(gameObject);
        }
    }
}
