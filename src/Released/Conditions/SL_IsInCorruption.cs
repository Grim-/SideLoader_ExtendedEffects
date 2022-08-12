using SideLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SideLoader_ExtendedEffects.Released.Conditions
{
    public class SL_IsInCorruptionCondition : SL_EffectCondition, ICustomModel
    {
        public Type SLTemplateModel => typeof(SL_IsInCorruptionCondition);
        public Type GameModel => typeof(IsInCorruptionCondition);

        public override void ApplyToComponent<T>(T component)
        {

        }

        public override void SerializeEffect<T>(T component)
        {

        }
    }

    public class IsInCorruptionCondition : EffectCondition
    {
        public override bool CheckIsValid(Character _affectedCharacter)
        {
            //not sure what happens here for AI, we'll find out I guess.
            return _affectedCharacter.PlayerStats.UpdateEnvironmentalCorruption(Time.deltaTime) > 0;
        }
    }
}
