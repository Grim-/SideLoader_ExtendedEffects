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
        public bool Modifier; // Check the modifier instead.

        public override void ApplyToComponent<T>(T component)
        {
            var comp = component as StatCondition;
            comp.Stat = OutwardHelpers.GetTagFromName(this.Stat);
            comp.Value = this.Value;
            comp.CompareType = this.CompareType;
            comp.Owner = this.Owner;
            comp.Modifier = this.Modifier;
        }
        public override void SerializeEffect<T>(T effect)
        {
            var comp = effect as StatCondition;
            this.Stat = comp.Stat.TagName;
            this.Value = comp.Value;
            this.CompareType = comp.CompareType;
            this.Owner = comp.Owner;
            this.Modifier = comp.Modifier;
        }

    }

    public class StatCondition : EffectCondition
    {
        public Tag Stat;
        public float Value;
        public AICondition.NumericCompare CompareType;
        public bool Owner;
        public bool Modifier;

        public override bool CheckIsValid(Character _affectedCharacter)
        {
            Character character = Owner ? this.m_parentSynchronizer.OwnerCharacter : _affectedCharacter;
            Tag[] tags = this.GetParentTags();
            return character && character.Stats && AICondition.CompareFloats(
                Modifier ? character.GetTaggedStatModifier(Stat, tags) : character.GetTaggedStat(Stat, tags),
                this.Value, this.CompareType);
        }
    }
}
