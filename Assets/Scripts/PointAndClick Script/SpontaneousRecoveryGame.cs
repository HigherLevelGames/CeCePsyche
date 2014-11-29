using System;
using UnityEngine;

public class SpontaneousRecoveryGame : PointNClickGame
{
    public ItemActions previousConditioned;
    public ItemActions previousUnconditioned;

    void Start()
    {
        Initialize();
        GetCondition.PreviousConditionedStimulus = (int)previousConditioned;
        GetCondition.PreviousUnonditionedResponse = (int)previousUnconditioned;
        GetCondition.PreConditioned = true;
        Prompt = "Find a way to recover extinct behavior.";
        Hint = "Using past methods will achieve swifter results.";
        Lose = "You failed to recover the behavior.";
        Win = "You win! The behavior was successfully recovered.";
        ClickToMove = true;
        this.gameObject.SetActive(false);
    }

    public override void CheckWinCondition(ItemActions action)
    {
        if ((int)action == GetCondition.PreviousConditionedStimulus && 
            GetCondition.ConditionedResponse == GetCondition.PreviousUnonditionedResponse)
            WinConditionMet = true;
    }
}