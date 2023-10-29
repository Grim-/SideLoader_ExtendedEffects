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
        public bool OnOwner;
        public int[] AllowedSkills;

        public override void ApplyToComponent<T>(T component)
        {
            AffectCooldown effect = component as AffectCooldown;
            effect.Amount = this.Amount;
            effect.IsModifier = this.IsModifier;
            effect.OnOwner = this.OnOwner;
            effect.AllowedSkills = this.AllowedSkills;
        }

        public override void SerializeEffect<T>(T effect)
        {
            AffectCooldown comp = effect as AffectCooldown;
            this.Amount = comp.Amount;
            this.IsModifier = comp.IsModifier;
            this.OnOwner = comp.OnOwner;
            this.AllowedSkills = comp.AllowedSkills;
        }
    }

    public class AffectCooldown: Effect, ICustomModel {
        public Type SLTemplateModel => typeof(SL_AffectCooldown);
        public Type GameModel => typeof(AffectCooldown);

        public float Amount;
        public bool IsModifier;
        public bool OnOwner;
        public int[] AllowedSkills;

        public override void ActivateLocally(Character _affectedCharacter, object[] _infos)
        {
            var character = OnOwner ? this.OwnerCharacter : _affectedCharacter;
            var skills = character.Inventory.SkillKnowledge.GetLearnedActiveSkillUIDs();
            foreach (string uid in skills)
            {
                Skill skill = ItemManager.Instance.GetItem(uid) as Skill;
                if (AllowedSkills != null && AllowedSkills.Length > 0 && !AllowedSkills.Contains(skill.ItemID))
                {
                    continue; // Not an allowed skill
                }

                float amount = IsModifier ? skill.RealCooldown * Amount/100 : Amount;
                skill.m_remainingCooldownTime -= amount;
                skill.UpdateCooldownRatio();
            }
        }
    }
}