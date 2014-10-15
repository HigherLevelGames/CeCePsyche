using System; // for EventArgs
using UnityEngine;
using System.Collections;

// Unconditioned Stimuli will be thrown
// and used to condition an unconditioned response.
// ("UseItem" stored in inventory)
//
// Conditioned Stimuli will be part of the environment
// and will illicit no response.
// Once the subject has been conditioned,
// then the environment can illicit the conditioned response.
// ("Interact" with environment)
[RequireComponent(typeof(Bar))]
public class Dog : MonoBehaviour
{
	//Response Actions
	public delegate void WeakResponseEventHandler(object sender, EventArgs e);
	public static event WeakResponseEventHandler WeakResponseEvent;
	protected virtual void OnWeakResponseEvent(EventArgs e)
	{
		if (WeakResponseEvent != null)
		{
			WeakResponseEvent(this, e);
		}
	}

	public delegate void StrongResponseEventHandler(object sender, EventArgs e);
	public static event StrongResponseEventHandler StrongResponseEvent;
	protected virtual void OnStrongResponseEvent(EventArgs e)
	{
		if (StrongResponseEvent != null)
		{
			StrongResponseEvent(this, e);
		}
	}

	//Response Level
	//protected Bar responseLevel;

	// Conditioned State
	enum ActionState
	{
		Neutral, // idle
		Susceptible, // conditioned stimulus
		RespondingStrong, // unconditioned response
		RespondingWeak // conditioned response
	}
	private ActionState curState = ActionState.Neutral;

	private int timesConditioned = 0;
	private bool isConditioned
	{
		get
		{
			return timesConditioned >= 3;
		}
	}

	// Use this for initialization
	void Start ()
	{
		//responseLevel = this.GetComponent<Bar>();
	}
	
	// Update is called once per frame
	void Update () { }

	void PerformResponse(bool isUnconditionedStimulus)
	{
		if(isUnconditionedStimulus) // unconditioned stimulus (throwable items)
		{
			if(curState == ActionState.Susceptible)
			{
				timesConditioned++;
			}
			// response level = high
			curState = ActionState.RespondingStrong;
			OnStrongResponseEvent(EventArgs.Empty);
			curState = ActionState.Neutral;
		}
		else // conditioned stimulus (environment)
		{
			curState = ActionState.Susceptible;
			if(isConditioned)
			{
				// response level = low
				curState = ActionState.RespondingWeak;
				OnWeakResponseEvent(EventArgs.Empty);
				curState = ActionState.Neutral;
			}
		}
	}
}
