using System;
using UnityEngine;

namespace AssemblyCSharp
{
    public class ClassicalConditioningGame : PointNClickGame
    {
        void Start()
        {
            Initialize();
            Prompt = "Use a conditioned stimulus to cause the dog to salivate.";
            Hint = "Try using a neutral stimulus right before an unconditioned stimulus";
            Lose = "You failed to condition the desired behavior.";
            Win = "You win! You got the dog to drool!";
            this.gameObject.SetActive(false);
        }

        public override void CheckWinCondition(ItemActions action)
        {
            if (GetCondition.ConditionedStimulus > -1)
                WinConditionMet = true;
        }
    }
}

