using SideLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SideLoader_ExtendedEffects.Released.Conditions
{
    public class SL_IsTimeBetweenCondition : SL_EffectCondition, ICustomModel
    {
        public Type SLTemplateModel => typeof(SL_IsTimeBetweenCondition);
        public Type GameModel => typeof(IsTimeBetweenCondition);

        public float StartHour;
        public float EndHour;


        public override void ApplyToComponent<T>(T component)
        {
            IsTimeBetweenCondition comp = component as IsTimeBetweenCondition;
            comp.StartHour = StartHour;
            comp.EndHour = EndHour;
        }

        public override void SerializeEffect<T>(T component)
        {
            IsTimeBetweenCondition comp = component as IsTimeBetweenCondition;
            this.StartHour = comp.StartHour;
            this.EndHour = comp.EndHour;
        }
    }

    public class IsTimeBetweenCondition : EffectCondition
    {
        public float StartHour;
        public float EndHour;

        public override bool CheckIsValid(Character _affectedCharacter)
        {
            ExtendedEffects.Instance.DebugLogMessage($"IsTimeBetweenCondition StartHour : {StartHour} End Hour : {EndHour} result : {EnvironmentConditions.Instance.TimeOfDay >= StartHour && EnvironmentConditions.Instance.TimeOfDay <= EndHour}");
            return EnvironmentConditions.Instance.TimeOfDay >= StartHour && EnvironmentConditions.Instance.TimeOfDay <= EndHour;
        }
    }



}
