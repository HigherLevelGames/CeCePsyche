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
        }
        
        public override void CheckWinCondition(ItemActions action)
        {
            if (GetCondition.TasteAvertedBehavior == (int)action)
                WinConditionMet = true;
        }
    }
}