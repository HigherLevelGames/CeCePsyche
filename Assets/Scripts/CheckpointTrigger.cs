using UnityEngine;
using System.Collections;

public class CheckpointTrigger : MonoBehaviour
{
    Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if(SaveStateManager.data.SaveCheckpoint())
                anim.SetTrigger("Save");
        }
    }

}
