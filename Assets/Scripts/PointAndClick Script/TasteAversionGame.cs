using System;
using UnityEngine;

namespace AssemblyCSharp
{
    public class TasteAversionGame : PointNClickGame
    {
        public ItemActions EnjoyedStimulus;

        void Start()
        {
            Initialize();
            GetCondition.CurrentEnjoyedBehavior = (int)EnjoyedStimulus;
            Prompt = "Find a way to cause aversion to unwanted behavior.";
            Hint = "Stinky squirrels aren't any fun.";
            Lose = "You failed to avert the undesired behavior.";
            Win = "You win! The behavior was successfully averted.";
            this.gameObject.SetActive(false);
        }
        
        public override void CheckWinCondition(ItemActions action)
        {
            if (GetCondition.TasteAvertedBehavior == (int)action)
                WinConditionMet = true;
        }
    }
}