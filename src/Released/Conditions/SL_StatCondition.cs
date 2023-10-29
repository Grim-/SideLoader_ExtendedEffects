using SideLoader;
using System;
namespace SideLoader_ExtendedEffects.Released.Conditions
{
    public class SL_StatCondition : SL_EffectCondition, ICustomModel
    {
        public Type SLTemplateModel => typeof(SL_StatCondition);
        public Type GameModel => typeof(StatCondition);

        public String Stat; // Stat to compare
        public float Value; // Value to compare to
        public AICondition.NumericCompare CompareType;
        public bool Owner; // If the comparison should target the owner or the target

        public override void ApplyToComponent<T>(T component)
        {
            var comp = component as StatCondition;
            comp.Stat = OutwardHelpers.GetTagFromName(this.Stat);
            comp.Value = this.Value;
            comp.CompareType = this.CompareType;
            comp.Owner = this.Owner;
        }
        public override void SerializeEffect<T>(T effect)
        {
            var comp = effect as StatCondition;
            this.Stat = comp.Stat.TagName;
            this.Value = comp.Value;
            this.CompareType = comp.CompareType;
            this.Owner = comp.Owner;
        }

    }

    public class StatCondition : EffectCondition
    {
        public Tag Stat;
        public float Value;
        public AICondition.NumericCompare CompareType;
        public bool Owner;

        public override bool CheckIsValid(Character _affectedCharacter)
        {
            Character character = Owner ? this.m_parentSynchronizer.OwnerCharacter : _affectedCharacter;
            return character && character.Stats && AICondition.CompareFloats(character.Stats.GetStatCurrentValue(Stat), this.Value, this.CompareType);
        }
    }
}
