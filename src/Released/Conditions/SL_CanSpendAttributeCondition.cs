using SideLoader;
using System;
namespace SideLoader_ExtendedEffects.Released.Conditions
{
    public class SL_CanSpendAttributeCondition : SL_EffectCondition, ICustomModel
    {
        public Type SLTemplateModel => typeof(SL_CanSpendAttributeCondition);
        public Type GameModel => typeof(CanSpendAttributeCondition);

        public Attributes Attr; // Attribute to compare to
        public float Value; // Value to compare
        public bool Relative; // If the comparison should be relative to max
        public bool BurnedMax; // Whether the relative comparison should respect the burned max. Ignored for burned attributes
        public override void ApplyToComponent<T>(T component)
        {
            var comp = component as CanSpendAttributeCondition;
            comp.Attr = this.Attr;
            comp.Value = this.Value;
            comp.Relative = this.Relative;
            comp.BurnedMax = this.BurnedMax;
        }
        public override void SerializeEffect<T>(T effect)
        {
            var comp = effect as CanSpendAttributeCondition;
            this.Attr = comp.Attr;
            this.Value = comp.Value;
            this.Relative = comp.Relative;
            this.BurnedMax = comp.BurnedMax;
        }

    }

    public class CanSpendAttributeCondition : EffectCondition
    {
        public Attributes Attr;
        public float Value;
        public bool Relative; 
        public bool BurnedMax;

        public override bool CheckIsValid(Character _affectedCharacter)
        {
            Character character =  this.m_parentSynchronizer.OwnerCharacter;
            if (!(character && character.Stats)) return false;
            float realValue = Value;
            float currentPool;
            switch (Attr){
                case Attributes.HEALTH:
                    if (Relative) realValue = (realValue / 100) * (BurnedMax ? character.Stats.ActiveMaxHealth : character.Stats.MaxHealth);
                    currentPool = character.Stats.CurrentHealth;
                    break;
                case Attributes.MANA:
                    if (Relative) realValue = (realValue / 100) * (BurnedMax ? character.Stats.ActiveMaxMana : character.Stats.MaxMana);
                    realValue = character.Stats.GetFinalManaConsumption(null, realValue);
                    currentPool = character.Stats.CurrentMana;
                    break;
                case Attributes.STAMINA:
                    if (Relative) realValue = (realValue / 100) * (BurnedMax ? character.Stats.ActiveMaxStamina : character.Stats.MaxStamina);
                    realValue = character.Stats.GetFinalStaminaConsumption(null, realValue);
                    currentPool = character.Stats.CurrentStamina;
                    break;
                case Attributes.BURNT_HEALTH:
                    if (Relative) realValue =  (realValue / 100) * (BurnedMax ? character.Stats.ActiveMaxHealth : character.Stats.MaxHealth);
                    currentPool = character.Stats.BurntHealth;
                    break;
                case Attributes.BURNT_MANA:
                    if (Relative) realValue =  (realValue / 100) * (BurnedMax ? character.Stats.ActiveMaxMana : character.Stats.MaxMana);
                    currentPool = character.Stats.BurntMana;
                    break;
                case Attributes.BURNT_STAMINA:
                    if (Relative) realValue =  (realValue / 100) * (BurnedMax ? character.Stats.ActiveMaxStamina : character.Stats.MaxStamina);
                    currentPool = character.Stats.BurntStamina;
                    break;
                default:
                    return false; // Misconfigured effect
            }
            return realValue <= currentPool;
        }
    }
}
