using SideLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SideLoader_ExtendedEffects.Released.Conditions
{
    public class SL_IsCharacterTemp : SL_EffectCondition, ICustomModel
    {
        public Type SLTemplateModel => typeof(SL_IsCharacterTemp);
        public Type GameModel => typeof(IsCharacterTempCondition);

        public float Temp;
        public string ConditionType;

        public override void ApplyToComponent<T>(T component)
        {
            IsCharacterTempCondition comp = component as IsCharacterTempCondition;
            comp.Temp = Temp;
            comp.ConditionType = (TempreatureOperation)Enum.Parse(typeof(TempreatureOperation), ConditionType);
        }

        public override void SerializeEffect<T>(T component)
        {
            IsCharacterTempCondition comp = component as IsCharacterTempCondition;
            this.Temp = comp.Temp;
            this.ConditionType = comp.ConditionType.ToString();
        }
    }

    public class IsCharacterTempCondition : EffectCondition
    {
        public float Temp;
        public TempreatureOperation ConditionType;

        public override bool CheckIsValid(Character _affectedCharacter)
        {
            //AI dont have playerstats so return false if AI
            if (!_affectedCharacter.IsLocalPlayer)
            {
                return false;
            }

            switch (ConditionType)
            {
                case TempreatureOperation.LESSTHAN:
                    return _affectedCharacter.PlayerStats.Temperature <= Temp;
                case TempreatureOperation.EQUALTO:
                    return Mathf.RoundToInt(_affectedCharacter.PlayerStats.Temperature) == Mathf.RoundToInt(Temp);
                case TempreatureOperation.GREATERTHAN:
                    return _affectedCharacter.PlayerStats.Temperature >= Temp;
            }

            return false;
        }
    }

    public enum TempreatureOperation
    {
        LESSTHAN,
        EQUALTO,
        GREATERTHAN
    }
}
