using UnityEngine;
using System.Collections;

public class ResponseSet : MonoBehaviour {

    ItemActions itemaction;
    float waittime;
    void Upate()
    {
        if (waittime > 0)
        {

            waittime -= Time.deltaTime;
            if(waittime < 0)
            {
                Respond(itemaction);
            }
        }
    }

	public virtual void Respond(ItemActions action)
    {
    }
    public void RespondAfter(ItemActions action, float seconds)
    {
        itemaction = action;
        waittime = seconds;
    }
}
