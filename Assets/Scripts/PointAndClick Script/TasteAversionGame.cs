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
            GetCondition.PreviousEnjoyedStimulus = (int)EnjoyedStimulus;
        }
        
        public override void CheckWinCondition(ItemActions action)
        {
            if (GetCondition.TasteAvertedStimulus > -1 && 
                GetCondition.PreviousEnjoyedStimulus == GetCondition.TasteAvertedStimulus)
                WinConditionMet = true;
        }
    }
}