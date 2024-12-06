using SideLoader;
using System;

namespace SideLoader_ExtendedEffects.Released
{
    public class SL_LearnRecipeEffect : SL_Effect, ICustomModel
    {
        public Type SLTemplateModel => typeof(SL_LearnRecipeEffect);
        public Type GameModel => typeof(LearnCustomRecipeEffect);

        public string RecipeUID;

        public override void ApplyToComponent<T>(T component)
        {
            LearnCustomRecipeEffect learnRecipeEffect = component as LearnCustomRecipeEffect;
            learnRecipeEffect.RecipeUID = RecipeUID;
        }

        public override void SerializeEffect<T>(T effect)
        {
            LearnCustomRecipeEffect learnRecipeEffect = effect as LearnCustomRecipeEffect;
            RecipeUID = learnRecipeEffect.RecipeUID;
        }
    }

    public class LearnCustomRecipeEffect : Effect
    {
        public string RecipeUID;
        public override void ActivateLocally(Character _affectedCharacter, object[] _infos)
        {
            if (!_affectedCharacter.Inventory.RecipeKnowledge.IsRecipeLearned(RecipeUID))
            {
                Recipe FoundRecipe = TryFindRecipe(RecipeUID);

                if (FoundRecipe != null)
                {
                    _affectedCharacter.Inventory.RecipeKnowledge.LearnRecipe(FoundRecipe);
                }
                else
                {
                    //OutwardModTemplate.StormCancer_MasterChef.Log.LogMessage($"LearnRecipeEffect :: Cannot find Recipe with UID {RecipeUID}");
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
    }

}
