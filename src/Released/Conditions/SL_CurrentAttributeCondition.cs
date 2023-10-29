using SideLoader;
using System;
namespace SideLoader_ExtendedEffects.Released.Conditions
{
    public class SL_CurrentAttributeCondition : SL_EffectCondition, ICustomModel
    {
        public Type SLTemplateModel => typeof(SL_CurrentAttributeCondition);
        public Type GameModel => typeof(CurrentAttributeCondition);

        public Attributes Attr; // Attribute to compare
        public float Value; // Value to compare to
        public AICondition.NumericCompare CompareType;
        public bool Owner; // If the comparison should target the owner or the target
        public bool Relative; // If the comparison should be relative to max
        public bool BurnedMax; // Whether the relative comparison should respect the burned max. Ignored for burned attributes
        public override void ApplyToComponent<T>(T component)
        {
            var comp = component as CurrentAttributeCondition;
            comp.Attr = this.Attr;
            comp.Value = this.Value;
            comp.CompareType = this.CompareType;
            comp.Owner = this.Owner;
            comp.Relative = this.Relative;
            comp.BurnedMax = this.BurnedMax;
        }
        public override void SerializeEffect<T>(T effect)
        {
            var comp = effect as CurrentAttributeCondition;
            this.Attr = comp.Attr;
            this.Value = comp.Value;
            this.CompareType = comp.CompareType;
            this.Owner = comp.Owner;
            this.Relative = comp.Relative;
            this.BurnedMax = comp.BurnedMax;
        }

    }

    public class CurrentAttributeCondition : EffectCondition
    {
        public Attributes Attr;
        public float Value;
        public AICondition.NumericCompare CompareType;
        public bool Owner;
        public bool Relative; 
        public bool BurnedMax;

        public override bool CheckIsValid(Character _affectedCharacter)
        {
            Character character = Owner ? this.m_parentSynchronizer.OwnerCharacter : _affectedCharacter;
            if (!(character && character.Stats)) return false;
            float attrVal;
            switch (Attr)
            {
                case Attributes.HEALTH:
                    attrVal = character.Stats.CurrentHealth;
                    if (Relative) attrVal /= BurnedMax ? character.Stats.ActiveMaxHealth : character.Stats.MaxHealth;
                    break;
                case Attributes.MANA:
                    attrVal = character.Stats.CurrentMana;
                    if (Relative) attrVal /= BurnedMax ? character.Stats.ActiveMaxMana : character.Stats.MaxMana;
                    break;
                case Attributes.STAMINA:
                    attrVal = character.Stats.CurrentStamina;
                    if (Relative) attrVal /= BurnedMax ? character.Stats.ActiveMaxStamina : character.Stats.MaxStamina;
                    break;
                case Attributes.BURNT_HEALTH:
                    attrVal = character.Stats.BurntHealth;
                    if (Relative) attrVal /= character.Stats.MaxHealth;
                    break;
                case Attributes.BURNT_MANA:
                    attrVal = character.Stats.BurntMana;
                    if (Relative) attrVal /= character.Stats.MaxMana;
                    break;
                case Attributes.BURNT_STAMINA:
                    attrVal = character.Stats.BurntStamina;
                    if (Relative) attrVal /= character.Stats.MaxStamina;
                    break;
                default:
                    return false; // Misconfigured condition
            }
            return AICondition.CompareFloats(attrVal, Relative ? this.Value / 100 : this.Value, this.CompareType);
        }
    }
}
