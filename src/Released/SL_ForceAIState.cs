namespace SideLoader_ExtendedEffects
{
    using SideLoader;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Random = UnityEngine.Random;


    /// <summary>
    /// Switches any Affected CharacterAI to the AIState Specified
    /// </summary>
    public class SL_ForceAIState : SL_Effect, ICustomModel
    {
        public Type SLTemplateModel => typeof(SL_ForceAIState);
        public Type GameModel => typeof(SLEx_ForceAIStateForTime);

        /// <summary>
        /// The ID Of the AIState
        /// </summary>
        public int AIState;

        public override void ApplyToComponent<T>(T component)
        {
            SLEx_ForceAIStateForTime comp = component as SLEx_ForceAIStateForTime;
            comp.AIState = AIState;
        }

        public override void SerializeEffect<T>(T effect)
        {
            SLEx_ForceAIStateForTime comp = effect as SLEx_ForceAIStateForTime;
            this.AIState = comp.AIState;
        }
    }

    public class SLEx_ForceAIStateForTime : Effect, ICustomModel
    {
        public Type SLTemplateModel => typeof(SL_ForceAIState);
        public Type GameModel => typeof(SLEx_ForceAIStateForTime);

        public int AIState;

        public override void ActivateLocally(Character _affectedCharacter, object[] _infos)
        {
            if (AIState > 3)
            {
                //ai state doesnt exist
                return;
            }

            CharacterAI characterAI = _affectedCharacter.GetComponent<CharacterAI>();

            if (characterAI)
            {
                ForceCharacterAIState(characterAI, AIState);
            }

        }

        private void ForceCharacterAIState(CharacterAI CharacterAI, int state)
        {
            CharacterAI.SwitchAiState(state);
        }
    }

}
//public enum SLEx_AISTATES
//{
//    WANDER = 0,
//    SUSPICIOUS = 1,
//    COMBAT = 2,
//    COMBAT_FLEE = 3
//}


//State Name : 1_Wander ID : 0
//State Name : 2_Suspicious ID : 1
//State Name : 3_Combat ID : 2
//State Name : 4_CombatFlee ID : 3