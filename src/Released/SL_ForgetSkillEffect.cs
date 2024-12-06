using SideLoader;
using System;
using System.Collections.Generic;

namespace SideLoader_ExtendedEffects.Containers
{
    public class SL_ForgetSkillEffect : SL_Effect
    {
        public List<int> skillIDs;

        public override void ApplyToComponent<T>(T component)
        {
            var comp = component as ForgetSkillEffect;
            comp.skillIDs = this.skillIDs;
        }
        public override void SerializeEffect<T>(T effect)
        {
            var comp = effect as ForgetSkillEffect;
            this.skillIDs = comp.skillIDs;
        }
    }

    public class ForgetSkillEffect : Effect, ICustomModel
    {
        public List<int> skillIDs;

        public Type SLTemplateModel => typeof(SL_ForgetSkillEffect);
        public Type GameModel => typeof(ForgetSkillEffect);

        public override void ActivateLocally(Character _affectedCharacter, object[] _infos)
        {
            Character character = this.m_parentSynchronizer.OwnerCharacter;

            if (character == null) return;


            foreach (var item in skillIDs)
            {
                if (character.Inventory.SkillKnowledge.IsItemLearned(item))
                {
                    character.Inventory.SkillKnowledge.RemoveItem(item);
                }
            }
 
        }
    }
}