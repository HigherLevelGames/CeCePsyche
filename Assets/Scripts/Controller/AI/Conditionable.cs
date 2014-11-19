using System;

// for EventArgs
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
//[RequireComponent(typeof(Bar))]
public class Conditionable : MonoBehaviour
{
    #region JordanCodeLand
    //Response Actions
    public delegate void WeakResponseEventHandler(object sender,EventArgs e);

    public static event WeakResponseEventHandler WeakResponseEvent;

    protected virtual void OnWeakResponseEvent(EventArgs e)
    {
        if (WeakResponseEvent != null)
        {
            WeakResponseEvent(this, e);
        }
    }

    public delegate void StrongResponseEventHandler(object sender,EventArgs e);

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

    private bool inExtinction
    {
        get
        {
            return timesConditioned > 3;
        }
    }

    void PerformResponse(bool isUnconditionedStimulus)
    {
        if (isUnconditionedStimulus)
        { // unconditioned stimulus (throwable items)
            if (curState == ActionState.Susceptible)
            {
                timesConditioned++;
            }
            // response level = high
            curState = ActionState.RespondingStrong;
            OnStrongResponseEvent(EventArgs.Empty);
            curState = ActionState.Neutral;
        } else
        { // conditioned stimulus (environment)
            curState = ActionState.Susceptible;
            if (isConditioned)
            {
                // response level = low
                curState = ActionState.RespondingWeak;
                OnWeakResponseEvent(EventArgs.Empty);
                curState = ActionState.Neutral;
            }
        }
    }
    #endregion
    #region JasonCodeKingdom
    float stimulusTimer = 0;
    int counter = 0;
    bool acting = false;
    int neutral;
    int[,] pairs = new int[3, 2];
    public int ConditionedStimulus = -1;
    public int ConditionedResponse = -1;
    public bool Consuming;
    public bool PreConditioned;
    public int PreviousConditionedStimulus = -1;
    public int PreviousUnonditionedStimulus = -1;

    void Start()
    {
        RevertPairs();
    }

    void RevertPairs()
    {
        neutral = -1;
        for (int i = 0; i < pairs.GetLength(0); i++)
        {
            pairs [i, 0] = -1;
            pairs [i, 1] = -1;
        }
        counter = 0;
    }

    void Update()
    {
        if (acting)
        {
            stimulusTimer += Time.deltaTime;
            if (stimulusTimer > 3f)
            {
                stimulusTimer = 0;
                counter = Mathf.Max(0, counter - 1);
                pairs [counter, 0] = -1;
                pairs [counter, 1] = -1;
                neutral = -1;
                acting = false;
            }
        }
    }

    public void AddUnconditioned(int un)
    {
        if (acting)
        {
            if (PreConditioned)
            {
                for (int i = 0; i < 3; i++)
                    Condition(un);
                AttemptSpontaneousRecovery();
            } else if (counter < 3)
                Condition(un);
        }
    }

    void Condition(int un)
    {
        pairs [counter, 0] = neutral;
        pairs [counter, 1] = un; 
        counter++;
        acting = false;
        if (counter == 3)
            AttemptClassicalConditioning();
    }

    void AttemptClassicalConditioning()
    {
        bool ok = true;

        ok = (pairs [0, 0] == pairs [1, 0]) && 
            (pairs [1, 0] == pairs [2, 0]) && 
            (pairs [0, 1] == pairs [1, 1]) && 
            (pairs [1, 1] == pairs [2, 1]);
        if (ok)
        {
            ConditionedStimulus = neutral;
            ConditionedResponse = pairs [0, 1];
        } else
            RevertPairs();
    }

    void AttemptSpontaneousRecovery()
    {
        bool ok;
        ok = pairs [0, 0] == PreviousConditionedStimulus &&
            pairs [0, 1] == PreviousUnonditionedStimulus;
        if (!ok)
            RevertPairs();
    }

    public void AddNeutral(int n)
    {
        neutral = n;
        acting = true;
        stimulusTimer = 0;
    }

    public void Reset()
    {
        stimulusTimer = 0;
        counter = 0;
        Consuming = acting = false;
        ConditionedStimulus = -1;
        ConditionedResponse = -1;
        RevertPairs();
    }
    #endregion
}
