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
            GetCondition.PreviousUnonditionedResponse = (int)previousUnconditioned;
            GetCondition.PreConditioned = true;
        }

        public override void CheckWinCondition(ItemActions action)
        {
            if ((int)action == GetCondition.PreviousConditionedStimulus && 
                GetCondition.ConditionedResponse == GetCondition.PreviousUnonditionedResponse)
            {
                WinConditionMet = true;
                Debug.Log(GetCondition.ConditionedStimulus);
                Debug.Log(GetCondition.ConditionedResponse);
                Debug.Log(GetCondition.PreviousConditionedStimulus);
                Debug.Log(GetCondition.PreviousUnonditionedResponse);
            }
        }
    }
}

