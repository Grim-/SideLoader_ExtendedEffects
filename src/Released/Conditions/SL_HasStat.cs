using SideLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SideLoader_ExtendedEffects.Released.Conditions
{
    public class SL_HasStat : SL_EffectCondition, ICustomModel
    {
        public Type SLTemplateModel => typeof(SL_HasStat);
        public Type GameModel => typeof(HasStat);

        public string TagName;
        public float StatValue;
        public bool IsAbsolute = true;
        public AICondition.NumericCompare Comparator = AICondition.NumericCompare.Equal;

        public override void ApplyToComponent<T>(T component)
        {
            HasStat comp = component as HasStat;
            comp.TagName = TagName;
            comp.StatValue = StatValue;
            comp.IsAbsolute = IsAbsolute;
        }

        public override void SerializeEffect<T>(T component)
        {
            HasStat comp = component as HasStat;
            this.TagName = comp.TagName;
            this.StatValue = comp.StatValue;
            this.IsAbsolute = comp.IsAbsolute;
        }
    }

    public class HasStat : EffectCondition, ICustomModel
    {
        public Type SLTemplateModel => typeof(SL_HasStat);
        public Type GameModel => typeof(HasStat);

        public string TagName;
        public float StatValue;
        public bool IsAbsolute = true;
        public AICondition.NumericCompare Comparator;
        public override bool CheckIsValid(Character _affectedCharacter)
        {
            Tag FoundTag = OutwardHelpers.GetTagFromName(TagName);

            if (FoundTag != default(Tag))
            {
                Stat FoundStat = _affectedCharacter.Stats.GetStat(FoundTag);

                if (FoundStat == null)
                {
                    ExtendedEffects._Log.LogMessage("FoundStat was null");
                    return false;
                }

                float ValueToCompare;
                if (IsAbsolute)
                {
                    ValueToCompare = FoundStat.CurrentValue;
                }
                else
                {
                    ValueToCompare = (FoundStat.CurrentValue / FoundStat.MaxRange) * 100;
                }

                // Perform the comparison using the switch statement
                switch (Comparator)
                {
                    case AICondition.NumericCompare.Less:
                        return ValueToCompare < StatValue;

                    case AICondition.NumericCompare.Greater:
                        return ValueToCompare > StatValue;

                    case AICondition.NumericCompare.LEqual:
                        return ValueToCompare <= StatValue;

                    case AICondition.NumericCompare.GEqual:
                        return ValueToCompare >= StatValue;

                    case AICondition.NumericCompare.Equal:
                        return ValueToCompare == StatValue;

                    case AICondition.NumericCompare.NotEqual:
                        return ValueToCompare != StatValue;
                }

            }

            ExtendedEffects._Log.LogMessage($"Stat Tag ({TagName}) could not be found");
            return false;
        }
    }
}
