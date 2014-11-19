using System;
using UnityEngine;

namespace AssemblyCSharp
{
    public class SpontaneousRecoveryGame : PointNClickGame
    {
        public ItemActions previousConditioned;
        public ItemActions previousUnconditioned;

        void Start()
        {
            Initialize();
            GetCondition.PreviousConditionedStimulus = (int)previousConditioned;
            GetCondition.PreviousUnonditionedStimulus = (int)previousUnconditioned;
            GetCondition.PreConditioned = true;
        }

        public override void CheckWinCondition()
        {
            if (GetCondition.ConditionedStimulus == GetCondition.PreviousConditionedStimulus && GetCondition.ConditionedResponse == GetCondition.PreviousUnonditionedStimulus)
                WinConditionMet = true;
        }
    }
}

