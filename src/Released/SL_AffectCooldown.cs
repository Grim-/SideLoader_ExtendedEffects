using SideLoader;
using System;
using UnityEngine;

namespace SideLoader_ExtendedEffects
{
        /// <summary>
        /// Dan
        /// </summary>
    public class SL_AffectCooldown: SL_Effect, ICustomModel
    {
        public Type SLTemplateModel => typeof(SL_AffectCooldown);
        public Type GameModel => typeof(AffectCooldown);

        public float Amount;
        public bool IsModifier;

        public override void ApplyToComponent<T>(T component)
        {
            AffectCooldown effect = component as AffectCooldown;
            effect.Amount = this.Amount;
            effect.IsModifier = this.IsModifier;
        }

        public override void SerializeEffect<T>(T effect)
        {
            AffectCooldown comp = effect as AffectCooldown;
            this.Amount = comp.Amount;
            this.IsModifier = comp.IsModifier;
        }
    }

    public class AffectCooldown: Effect, ICustomModel {
        public Type SLTemplateModel => typeof(SL_AffectCooldown);
        public Type GameModel => typeof(AffectCooldown);

        public float Amount;
        public bool IsModifier;

        public override void ActivateLocally(Character _affectedCharacter, object[] _infos)
        {
            var skills = _affectedCharacter.Inventory.SkillKnowledge.GetLearnedActiveSkillUIDs();
            foreach (string uid in skills)
            {
                Skill skill = ItemManager.Instance.GetItem(uid) as Skill;
                float amount = IsModifier ? skill.RealCooldown * Amount/100 : Amount;
                SL.Log("Cooldown on " + skill.name + " reduced by " + Amount);
                skill.m_remainingCooldownTime -= amount;
                skill.UpdateCooldownRatio();
                SL.Log("Cooldown on " + skill.name + " now " + skill.m_remainingCooldownTime);
            }
        }
    }
}