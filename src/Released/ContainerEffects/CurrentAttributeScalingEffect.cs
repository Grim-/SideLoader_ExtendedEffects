using SideLoader;
using System;

namespace SideLoader_ExtendedEffects.Containers
{

    public class SL_CurrentAttributeScalingEffect: SL_ParentEffect {

        public float BaselineValue; // Value of attribute at which effects will be applied at 100% potency
        public bool Round; // Whether the scaling modifier should round down to integer multiples of the baseline or not
        public Attributes Attr; // Attribute to base off of
        public bool Owner; // Whether owner or target attribute should be used
        public bool Relative; // Whether to base off of fraction of max attribute
        public bool BurnedMax; // Whether the relative comparison should respect the burned max
        public override void ApplyToComponent<T>(T component)
        {
            var comp = component as CurrentAttributeScalingEffect;
            comp.Attr = this.Attr;
            comp.Owner = this.Owner;
            comp.Relative = this.Relative;
            comp.BurnedMax = this.BurnedMax;
            comp.Round = this.Round;
            comp.BaselineValue = this.BaselineValue;
        }
        public override void SerializeEffect<T>(T effect)
        {
            var comp = effect as CurrentAttributeScalingEffect;
            this.Attr = comp.Attr;
            this.Owner = comp.Owner;
            this.Relative = comp.Relative;
            this.BurnedMax = comp.BurnedMax;
            this.Round = comp.Round;
            this.BaselineValue = comp.BaselineValue;
        }

    }


    public class CurrentAttributeScalingEffect : ParentEffect, ICustomModel
    {

        public Type SLTemplateModel => typeof(SL_CurrentAttributeScalingEffect);
        public Type GameModel => typeof(CurrentAttributeScalingEffect);

        public float BaselineValue; // Value of attribute at which effects will be applied at 100% potency
        public bool Round; // Whether the scaling modifier should round down to integer multiples of the baseline or not
        public Attributes Attr; // Attribute to base off of
        public bool Owner; // Whether owner or target attribute should be used
        public bool Relative; // Whether to base off of fraction of max attribute
        public bool BurnedMax; // Whether the relative comparison should respect the burned max
        public override void ActivateLocally(Character _affectedCharacter, object[] _infos)
        {
            Character character = Owner ? this.m_parentSynchronizer.OwnerCharacter : _affectedCharacter;
            if (!(character && character.Stats)) return;
            float scale;
            switch (Attr){
                case Attributes.HEALTH:
                    scale = character.Stats.CurrentHealth;
                    if (Relative) scale /= BurnedMax ? character.Stats.ActiveMaxHealth : character.Stats.MaxHealth;
                    break;
                case Attributes.MANA:
                    scale = character.Stats.CurrentMana;
                    if (Relative) scale /= BurnedMax ? character.Stats.ActiveMaxMana : character.Stats.MaxMana;
                    break;
                case Attributes.STAMINA:
                    scale = character.Stats.CurrentStamina;
                    if (Relative) scale /= BurnedMax ? character.Stats.ActiveMaxStamina : character.Stats.MaxStamina;
                    break;
                case Attributes.BURNT_HEALTH:
                    scale = character.Stats.CurrentHealth;
                    if (Relative) scale /= character.Stats.MaxHealth;
                    break;
                case Attributes.BURNT_MANA:
                    scale = character.Stats.CurrentMana;
                    if (Relative) scale /= character.Stats.MaxMana;
                    break;
                case Attributes.BURNT_STAMINA:
                    scale = character.Stats.CurrentStamina;
                    if (Relative) scale /= character.Stats.MaxStamina;
                    break;
                default:
                    return; // Misconfigured effect
            }
            scale /= Relative ? BaselineValue / 100 : BaselineValue;
            if (Round)
            {
                scale = (float)Math.Floor((double)scale); // ew
            }
            this.m_subEffects[0].EffectPotency = scale;
            StopApply(new EffectSynchronizer.EffectCategories[]{EffectSynchronizer.EffectCategories.Normal}, character);
        }
        public override void StopAffectLocally(Character _affectedCharacter)
        {
            Character character = Owner ? this.m_parentSynchronizer.OwnerCharacter : _affectedCharacter;
            StopApply(new EffectSynchronizer.EffectCategories[]{EffectSynchronizer.EffectCategories.Normal}, character);
            base.StopAffectLocally(character);
        }
    }

}