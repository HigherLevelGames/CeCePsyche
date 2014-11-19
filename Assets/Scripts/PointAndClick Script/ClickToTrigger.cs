using UnityEngine;
using System.Collections;

public class ClickToTrigger : MonoBehaviour {
    public string trigger_name;
    void Start()
    {
        Debug.Log("started");
    }
    void OnMouseDown()
    {
        Debug.Log("Hello");
        Animator ani = GetComponentInChildren<Animator>();
        ani.SetTrigger(trigger_name);
    }
}
