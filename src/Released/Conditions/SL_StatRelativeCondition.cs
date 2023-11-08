using SideLoader;
using System;
namespace SideLoader_ExtendedEffects.Released.Conditions
{
    public class SL_StatRelativeCondition : SL_EffectCondition, ICustomModel
    {
        public Type SLTemplateModel => typeof(SL_StatRelativeCondition);
        public Type GameModel => typeof(StatRelativeCondition);

        public String StatLeft; // Stat on the left hand side of the comparator
        public String StatRight; // Stat on the right hnd side of the comparator
        public AICondition.NumericCompare CompareType;
        public bool Owner; // If the comparison should target the owner or the target
        public bool Modifier; // Check the modifier instead.
        public override void ApplyToComponent<T>(T component)
        {
            var comp = component as StatRelativeCondition;
            comp.StatLeft = OutwardHelpers.GetTagFromName(this.StatLeft);
            comp.StatRight = OutwardHelpers.GetTagFromName(this.StatRight);
            comp.CompareType = this.CompareType;
            comp.Owner = this.Owner;
            comp.Modifier = this.Modifier;
        }
        public override void SerializeEffect<T>(T effect)
        {
            var comp = effect as StatRelativeCondition;
            this.StatLeft = comp.StatLeft.TagName;
            this.StatRight = comp.StatRight.TagName;
            this.CompareType = comp.CompareType;
            this.Owner = comp.Owner;
            this.Modifier = comp.Modifier;
        }

    }

    public class StatRelativeCondition : EffectCondition
    {
        public Tag StatLeft;
        public Tag StatRight;
        public AICondition.NumericCompare CompareType;
        public bool Owner;
        public bool Modifier;

        public override bool CheckIsValid(Character _affectedCharacter)
        {
            Character character = Owner ? this.m_parentSynchronizer.OwnerCharacter : _affectedCharacter;
            Tag[] tags = this.GetParentTags();
            return character && character.Stats
                && AICondition.CompareFloats(
                    Modifier ? character.GetTaggedStatModifier(StatLeft, tags) : character.GetTaggedStat(StatLeft, tags),
                    Modifier ? character.GetTaggedStatModifier(StatRight, tags) : character.GetTaggedStat(StatRight, tags), 
                    this.CompareType);
        }
    }
}
