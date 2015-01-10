using UnityEngine;
using System.Collections;

public class Transition : MonoBehaviour
{
    Animator anim;
    float waitTime;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (waitTime > 0)
        {
            waitTime -= Time.deltaTime;
            if(waitTime <= 0)
            {
                waitTime = 0;
                SaveStateManager.data.LoadQueuedData();
                TransitionIn();
            }
        }
    }

    public void TransitionIn()
    {
        anim.SetTrigger("TrigIn");
    }

    public void TransitionOut()
    {
        anim.SetTrigger("TrigOut");
    }
    public void TransitionFinish()
    {

    }
    public void Wait(float seconds)
    {
        SaveStateManager.data.GoToNextArea();
        waitTime = seconds;
    }
}
