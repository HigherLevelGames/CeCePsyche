using System;
using UnityEngine;

namespace AssemblyCSharp
{
    public class ClassicalConditioningGame : PointNClickGame
    {
        void Start()
        {
            Initialize();
        }

        public override void CheckWinCondition()
        {
            if (GetCondition.ConditionedStimulus > -1)
                WinConditionMet = true;
        }
    }
}

