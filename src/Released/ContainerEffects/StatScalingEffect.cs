using SideLoader;
using System;

namespace SideLoader_ExtendedEffects.Containers
{

    public class SL_StatScalingEffect: SL_ParentEffect {

        public String Stat; // Name of tag to read value from
        public float BaselineValue; // Value of Stat at which effects will be applied at 100% potency
        public bool Round; // Whether the scaling modifier should round down to integer multiples of the baseline or not
        public bool Owner; // Whether owner or target attribute should be used

        public override void ApplyToComponent<T>(T component)
        {
            base.ApplyToComponent<T>(component);
            var comp = component as StatScalingEffect;
            comp.Stat = OutwardHelpers.GetTagFromName(this.Stat);
            comp.BaselineValue = this.BaselineValue;
            comp.Round = this.Round;
            comp.Owner = this.Owner;
        }
        public override void SerializeEffect<T>(T effect)
        {
            base.SerializeEffect<T>(effect);
            var comp = effect as StatScalingEffect;
            this.Stat = comp.Stat.TagName;
            this.BaselineValue = comp.BaselineValue;
            this.Round = comp.Round;
            this.Owner = comp.Owner;
        }

    }


    public class StatScalingEffect : ParentEffect, ICustomModel
    {

        public Type SLTemplateModel => typeof(SL_StatScalingEffect);
        public Type GameModel => typeof(StatScalingEffect);

        public Tag Stat; // Tag to read value from
        public float BaselineValue; // Value of Stat at which effects will be applied at 100% potency
        public bool Round; // Whether the scaling modifier should round down to integer multiples of the baseline or not
        public bool Owner; // Whether owner or target attribute should be used

        public override void ActivateLocally(Character _affectedCharacter, object[] _infos)
        {
            Character character = Owner ? this.m_parentSynchronizer.OwnerCharacter : _affectedCharacter;
            float scale = character.Stats.GetStatCurrentValue(Stat) / BaselineValue;
            if (Round)
            {
                scale = (float)Math.Floor((double)scale); // ew
            }
            this.m_subEffects[0].EffectPotency = scale;
            this.m_subEffects[0].SynchronizeEffects(EffectSynchronizer.EffectCategories.Normal, character);
        }
        public override void StopAffectLocally(Character _affectedCharacter)
        {
            Character character = Owner ? this.m_parentSynchronizer.OwnerCharacter : _affectedCharacter;
            this.m_subEffects[0].StopAllEffects(EffectSynchronizer.EffectCategories.Normal, character);
            base.StopAffectLocally(character);
        }
    }

}