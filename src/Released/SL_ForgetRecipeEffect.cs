using SideLoader;
using System;
using System.Collections.Generic;

namespace SideLoader_ExtendedEffects.Containers
{
    public class SL_ForgetRecipeEffect : SL_Effect
    {
        public List<string> recipeIDs;

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
        public List<string> recipeIDs;

        public Type SLTemplateModel => typeof(SL_ForgetRecipeEffect);
        public Type GameModel => typeof(ForgetRecipeEffect);

        public override void ActivateLocally(Character _affectedCharacter, object[] _infos)
        {
            Character character = this.m_parentSynchronizer.OwnerCharacter;

            if (character == null) return;

            foreach (var item in recipeIDs)
            {
                if (character.Inventory.RecipeKnowledge.IsRecipeLearned(item))
                {
                    Item knownRecipe = GetRecipeFromUID(character, item);
                    character.Inventory.SkillKnowledge.RemoveItem(knownRecipe);

                    int index = character.Inventory.RecipeKnowledge.m_learnedItemUIDs.IndexOf(item);
                    if (index >= 0)
                    {
                        character.Inventory.RecipeKnowledge.m_learnedItemUIDs.RemoveAt(index);
                    }
                }
                else
                {
                    ExtendedEffects._Log.LogMessage($"{character.Name} does not know  Recipe UID {item} cannot unlearn.");
                }
            }
        }

        private Item GetRecipeFromUID(Character character, string recipeUID)
        {
            foreach (var item in character.Inventory.RecipeKnowledge.m_learnedItems)
            {
                if (item.UID == recipeUID)
                {
                    return item;
                }
            }
            ExtendedEffects._Log.LogMessage($"Cannot find recipe with UID {recipeUID} on {character.Name} {character.UID}");
            return null;
        }
    }
}