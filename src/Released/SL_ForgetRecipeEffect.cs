using SideLoader;
using System;
using System.Collections.Generic;

namespace SideLoader_ExtendedEffects.Containers
{
    public class SL_ForgetRecipeEffect : SL_Effect
    {
        public List<int> recipeIDs;

        public override void ApplyToComponent<T>(T component)
        {
            var comp = component as ForgetRecipeEffect;
            comp.recipeIDs = this.recipeIDs;
        }
        public override void SerializeEffect<T>(T effect)
        {
            var comp = effect as ForgetRecipeEffect;
            this.recipeIDs = comp.recipeIDs;
        }
    }

    public class ForgetRecipeEffect : Effect, ICustomModel
    {
        public List<int> recipeIDs;

        public Type SLTemplateModel => typeof(SL_ForgetRecipeEffect);
        public Type GameModel => typeof(ForgetRecipeEffect);

        public override void ActivateLocally(Character _affectedCharacter, object[] _infos)
        {
            Character character = this.m_parentSynchronizer.OwnerCharacter;

            if (character == null) return;

            foreach (var item in recipeIDs)
            {
                if (character.Inventory.SkillKnowledge.IsItemLearned(item))
                {
                    character.Inventory.SkillKnowledge.RemoveItem(item);
                }
            }
        }
    }
}