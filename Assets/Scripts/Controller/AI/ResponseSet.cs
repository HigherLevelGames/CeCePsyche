using UnityEngine;
using System.Collections;

public class ResponseSet : MonoBehaviour {

    ItemActions itemaction;
    float waittime;

	protected void TimerUpdate()
	{
		if (waittime > 0)
		{
			print (waittime);
			
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
		Debug.Log (seconds);
        itemaction = action;
        waittime = seconds;
    }
}
