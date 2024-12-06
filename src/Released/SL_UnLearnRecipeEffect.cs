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


    public class UnLearnRecipeEffect : Effect
    {
        public string RecipeUID;

        public override void ActivateLocally(Character _affectedCharacter, object[] _infos)
        {
            if (_affectedCharacter.Inventory.RecipeKnowledge.IsRecipeLearned(RecipeUID))
            {
                Recipe FoundRecipe = TryFindRecipe(RecipeUID);

                if (FoundRecipe != null)
                {
                    _affectedCharacter.Inventory.RecipeKnowledge.RemoveItem(FoundRecipe.RecipeID);
                }
                else
                {
                    //Log.LogMessage($"LearnRecipeEffect :: Cannot find Recipe with UID {RecipeUID}");
                }
            }
        }


        private Recipe TryFindRecipe(string RecipeUID)
        {
            foreach (var item in RecipeManager.Instance.m_recipes)
            {
                if (item.Value.UID == RecipeUID)
                {
                    return item.Value;
                }
            }

            return null;
        }

        private Recipe TryFindRecipeItemID(string RecipeUID)
        {
            foreach (var item in RecipeManager.Instance.m_recipes)
            {
                if (item.Value.UID == RecipeUID)
                {
                    return item.Value;
                }
            }

            return null;
        }
    }
}
