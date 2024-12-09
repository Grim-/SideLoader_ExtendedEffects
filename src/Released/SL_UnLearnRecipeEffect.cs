using SideLoader;
using System;

namespace SideLoader_ExtendedEffects.Released
{
    public class SL_UnLearnRecipeEffect : SL_Effect, ICustomModel
    {
        public Type SLTemplateModel => typeof(SL_UnLearnRecipeEffect);
        public Type GameModel => typeof(UnLearnRecipeEffect);

        public string RecipeUID;

        public override void ApplyToComponent<T>(T component)
        {
            UnLearnRecipeEffect learnRecipeEffect = component as UnLearnRecipeEffect;
            learnRecipeEffect.RecipeUID = RecipeUID;
        }

        public override void SerializeEffect<T>(T effect)
        {
            UnLearnRecipeEffect learnRecipeEffect = effect as UnLearnRecipeEffect;
            this.RecipeUID = learnRecipeEffect.RecipeUID;
        }
    }


    public class UnLearnRecipeEffect : Effect, ICustomModel
    {
        public Type SLTemplateModel => typeof(SL_UnLearnRecipeEffect);
        public Type GameModel => typeof(UnLearnRecipeEffect);
        public string RecipeUID;

        public override void ActivateLocally(Character _affectedCharacter, object[] _infos)
        {
            Character character = this.m_parentSynchronizer.OwnerCharacter;

            if (character == null) return;

            if (character.Inventory.RecipeKnowledge.IsRecipeLearned(RecipeUID))
            {
                Item knownRecipe = GetRecipeFromUID(character, RecipeUID);
                character.Inventory.SkillKnowledge.RemoveItem(knownRecipe);

                int index = character.Inventory.RecipeKnowledge.m_learnedItemUIDs.IndexOf(RecipeUID);
                if (index >= 0)
                {
                    character.Inventory.RecipeKnowledge.m_learnedItemUIDs.RemoveAt(index);
                }
            }
            else
            {
                ExtendedEffects._Log.LogMessage($"{character.Name} does not know  Recipe UID {RecipeUID} cannot unlearn.");
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
