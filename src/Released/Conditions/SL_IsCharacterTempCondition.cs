using SideLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SideLoader_ExtendedEffects.Released.Conditions
{
    public class SL_IsCharacterTempCondition : SL_EffectCondition, ICustomModel
    {
        public Type SLTemplateModel => typeof(SL_IsCharacterTempCondition);
        public Type GameModel => typeof(IsCharacterTempCondition);

        public float Temp;
        public int ConditionType;

        public override void ApplyToComponent<T>(T component)
        {
            IsCharacterTempCondition comp = component as IsCharacterTempCondition;
            comp.Temp = Temp;
            comp.ConditionType = (TempreatureOperation)ConditionType;
        }

        public override void SerializeEffect<T>(T component)
        {
            IsCharacterTempCondition comp = component as IsCharacterTempCondition;
            this.Temp = comp.Temp;
            this.ConditionType = (int)comp.ConditionType;
        }
    }

    public class IsCharacterTempCondition : EffectCondition
    {
        public float Temp;
        public TempreatureOperation ConditionType;

        public override bool CheckIsValid(Character _affectedCharacter)
        {

            //return _affectedCharacter.PlayerStats.UpdateEnvironmentalCorruption(Time.deltaTime) > 0;

            switch (ConditionType)
            {
                case TempreatureOperation.LESS_THAN:
                    return _affectedCharacter.PlayerStats.Temperature <= Temp;
                case TempreatureOperation.EQUAL_TO:
                    return Mathf.RoundToInt(_affectedCharacter.PlayerStats.Temperature) == Mathf.RoundToInt(Temp);
                case TempreatureOperation.GREATER_THAN:
                    return _affectedCharacter.PlayerStats.Temperature >= Temp;
            }

            return false;
        }
    }

    public enum TempreatureOperation
    {
        LESS_THAN,
        EQUAL_TO,
        GREATER_THAN
    }
}
