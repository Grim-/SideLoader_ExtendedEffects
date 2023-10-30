using SideLoader;
using System;
namespace SideLoader_ExtendedEffects.Released.Conditions
{
    public class SL_CurrentAttributeRelativeCondition : SL_EffectCondition, ICustomModel
    {
        public Type SLTemplateModel => typeof(SL_CurrentAttributeRelativeCondition);
        public Type GameModel => typeof(CurrentAttributeRelativeCondition);

        public Attributes LeftAttr; // Left hand attribute to compare
        public Attributes RightAttr; // Right hand attribute to compare
        public AICondition.NumericCompare CompareType;
        public bool Owner; // If the comparison should target the owner or the target
        public bool Relative; // If the comparison should be relative to max
        public bool BurnedMax; // Whether the relative comparison should respect the burned max. Ignored for burned attributes
        public override void ApplyToComponent<T>(T component)
        {
            var comp = component as CurrentAttributeRelativeCondition;
            comp.LeftAttr = this.LeftAttr;
            comp.RightAttr = this.RightAttr;
            comp.CompareType = this.CompareType;
            comp.Owner = this.Owner;
            comp.Relative = this.Relative;
            comp.BurnedMax = this.BurnedMax;
        }
        public override void SerializeEffect<T>(T effect)
        {
            var comp = effect as CurrentAttributeRelativeCondition;
            this.LeftAttr = comp.LeftAttr;
            this.RightAttr = comp.RightAttr;
            this.CompareType = comp.CompareType;
            this.Owner = comp.Owner;
            this.Relative = comp.Relative;
            this.BurnedMax = comp.BurnedMax;
        }

    }

    public class CurrentAttributeRelativeCondition : EffectCondition
    {
        public Attributes LeftAttr;
        public Attributes RightAttr;
        public AICondition.NumericCompare CompareType;
        public bool Owner;
        public bool Relative; 
        public bool BurnedMax;

        public override bool CheckIsValid(Character _affectedCharacter)
        {
            Character character = Owner ? this.m_parentSynchronizer.OwnerCharacter : _affectedCharacter;
            if (!(character && character.Stats)) return false;
            float leftVal, rightVal;
            switch (LeftAttr){
                case Attributes.HEALTH:
                    leftVal = character.Stats.CurrentHealth;
                    if (Relative) leftVal /= BurnedMax ? character.Stats.ActiveMaxHealth : character.Stats.MaxHealth;
                    break;
                case Attributes.MANA:
                    leftVal = character.Stats.CurrentMana;
                    if (Relative) leftVal /= BurnedMax ? character.Stats.ActiveMaxMana : character.Stats.MaxMana;
                    break;
                case Attributes.STAMINA:
                    leftVal = character.Stats.CurrentStamina;
                    if (Relative) leftVal /= BurnedMax ? character.Stats.ActiveMaxStamina : character.Stats.MaxStamina;
                    break;
                case Attributes.BURNT_HEALTH:
                    leftVal = character.Stats.CurrentHealth;
                    if (Relative) leftVal /= character.Stats.BurntHealth;
                    break;
                case Attributes.BURNT_MANA:
                    leftVal = character.Stats.CurrentMana;
                    if (Relative) leftVal /= character.Stats.BurntMana;
                    break;
                case Attributes.BURNT_STAMINA:
                    leftVal = character.Stats.CurrentStamina;
                    if (Relative) leftVal /= character.Stats.BurntStamina;
                    break;
                default:
                    return false; // Misconfigured condition
            }
            switch (RightAttr){
                case Attributes.HEALTH:
                    rightVal = character.Stats.CurrentHealth;
                    if (Relative) rightVal /= BurnedMax ? character.Stats.ActiveMaxHealth : character.Stats.MaxHealth;
                    break;
                case Attributes.MANA:
                    rightVal = character.Stats.CurrentMana;
                    if (Relative) rightVal /= BurnedMax ? character.Stats.ActiveMaxMana : character.Stats.MaxMana;
                    break;
                case Attributes.STAMINA:
                    rightVal = character.Stats.CurrentStamina;
                    if (Relative) rightVal /= BurnedMax ? character.Stats.ActiveMaxStamina : character.Stats.MaxStamina;
                    break;
                case Attributes.BURNT_HEALTH:
                    rightVal = character.Stats.BurntHealth;
                    if (Relative) rightVal /= character.Stats.MaxHealth;
                    break;
                case Attributes.BURNT_MANA:
                    rightVal = character.Stats.BurntMana;
                    if (Relative) rightVal /= character.Stats.MaxMana;
                    break;
                case Attributes.BURNT_STAMINA:
                    rightVal = character.Stats.BurntStamina;
                    if (Relative) rightVal /= character.Stats.MaxStamina;
                    break;
                default:
                    return false; // Misconfigured condition
            }
            return AICondition.CompareFloats(leftVal, rightVal, this.CompareType);
        }
    }
}
