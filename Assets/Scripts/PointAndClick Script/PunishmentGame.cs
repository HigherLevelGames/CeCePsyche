using System;
using UnityEngine;

public class PunishmentGame : PointNClickGame
{
    public ItemActions EnjoyedActivity = ItemActions.Squirrel;
    
    void Start()
    {
        Initialize();
        GetCondition.CurrentEnjoyedBehavior = (int)EnjoyedActivity;
        Prompt = "Find a way to deter persistent behavior.";
        Hint = "If an activity is followed by a punishment, it becomes less desirable.";
        Lose = "You failed to stop the unwanted behavior.";
        Win = "You win! The behavior has ceased!";
        this.gameObject.SetActive(false);
    }
    
    public override void CheckWinCondition(ItemActions action)
    {
        if ((int)action == GetCondition.ConditionedStimulus && 
            GetCondition.CurrentEnjoyedBehavior == -1)
            WinConditionMet = true;
    }
    public override void ClickFunction(int id)
    {
        FireMenuItemAction(id);
    }
}
