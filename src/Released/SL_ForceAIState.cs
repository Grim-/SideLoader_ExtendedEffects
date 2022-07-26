namespace SideLoader_ExtendedEffects
{
    using SideLoader;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Random = UnityEngine.Random;



    //Example of a Custom SL_Effect

    //This class is your SL Definition of the effect (For the XML)
    public class SL_ForceAIState : SL_Effect, ICustomModel
    {
        //implementing ICustomModel, means you have to define these two variables below, SLTemplateModel is this file, GameModel is going to be where your code is
        public Type SLTemplateModel => typeof(SL_ForceAIState);
        public Type GameModel => typeof(SLEx_ForceAIStateForTime);

        /// <summary>
        /// The ID Of the AIState
        /// </summary>
        public int AIState;

        //here we take the values from XML and apply them to our component below
        public override void ApplyToComponent<T>(T component)
        {
            //cast the component to our GameModel type
            SLEx_ForceAIStateForTime comp = component as SLEx_ForceAIStateForTime;
            //apply the values from XML to it
            comp.AIState = AIState;
        }

        //this takes values from an already defined in-game GameObject and sets this classes member variables to them
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

        //here is when the Effect is actually called, here goes your logic
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