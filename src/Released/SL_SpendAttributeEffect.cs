using SideLoader;
using System;

namespace SideLoader_ExtendedEffects.Containers
{

    public class SL_SpendAttributeEffect: SL_Effect {

        public float Cost; // Cost to spend
        public Attributes Attr; // Attribute to spend
        public bool Relative; // Whether to base off of fraction of max attribute
        public bool BurnedMax; // Whether the relative comparison should respect the burned max
        public override void ApplyToComponent<T>(T component)
        {
            var comp = component as SpendAttributeEffect;
            comp.Attr = this.Attr;
            comp.Relative = this.Relative;
            comp.BurnedMax = this.BurnedMax;
            comp.Cost = this.Cost;
        }
        public override void SerializeEffect<T>(T effect)
        {
            var comp = effect as SpendAttributeEffect;
            this.Attr = comp.Attr;
            this.Relative = comp.Relative;
            this.BurnedMax = comp.BurnedMax;
            this.Cost = comp.Cost;
        }

    }


    public class SpendAttributeEffect : Effect, ICustomModel
    {

        public Type SLTemplateModel => typeof(SL_SpendAttributeEffect);
        public Type GameModel => typeof(SpendAttributeEffect);

        public float Cost;
        public Attributes Attr;
        public bool Relative; 
        public bool BurnedMax; 
        public override void ActivateLocally(Character _affectedCharacter, object[] _infos)
        {
            Character character =  this.m_parentSynchronizer.OwnerCharacter;
            if (!(character && character.Stats)) return;
            float realCost = Cost * this.m_totalPotency;
            switch (Attr){
                case Attributes.HEALTH:
                    if (Relative) realCost *= BurnedMax ? character.Stats.ActiveMaxHealth : character.Stats.MaxHealth;
                    character.Stats.ReceiveDamage(realCost);
                    break;
                case Attributes.MANA:
                    if (Relative) realCost *= BurnedMax ? character.Stats.ActiveMaxMana : character.Stats.MaxMana;
                    character.Stats.UseMana(null, realCost);
                    break;
                case Attributes.STAMINA:
                    if (Relative) realCost *= BurnedMax ? character.Stats.ActiveMaxStamina : character.Stats.MaxStamina;
                    character.Stats.UseStamina(null, realCost, 1);
                    break;
                case Attributes.BURNT_HEALTH:
                    if (Relative) realCost *= BurnedMax ? character.Stats.ActiveMaxHealth : character.Stats.MaxHealth;
                    character.Stats.IncreaseBurntHealth(realCost);
                    break;
                case Attributes.BURNT_MANA:
                    if (Relative) realCost *= BurnedMax ? character.Stats.ActiveMaxMana : character.Stats.MaxMana;
                    character.Stats.IncreaseBurntMana(realCost);
                    break;
                case Attributes.BURNT_STAMINA:
                    if (Relative) realCost *= BurnedMax ? character.Stats.ActiveMaxStamina : character.Stats.MaxStamina;
                    character.Stats.IncreaseBurntStamina(realCost);
                    break;
                default:
                    return; // Misconfigured effect
            }
        }
    }

}