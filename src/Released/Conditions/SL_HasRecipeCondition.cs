using SideLoader;
using System;

namespace SideLoader_ExtendedEffects.Released.Conditions
{
    public class SL_HasRecipeCondition : SL_EffectCondition, ICustomModel
    {
        public Type SLTemplateModel => typeof(SL_HasRecipeCondition);
        public Type GameModel => typeof(HasRecipeCondition);

        public string RecipeUID;

        public override void ApplyToComponent<T>(T component)
        {
            HasRecipeCondition learnRecipeEffect = component as HasRecipeCondition;

            learnRecipeEffect.RecipeUID = RecipeUID;
        }

        public override void SerializeEffect<T>(T effect)
        {
            HasRecipeCondition learnRecipeEffect = effect as HasRecipeCondition;
            this.RecipeUID = learnRecipeEffect.RecipeUID;
        }
    }

    public class HasRecipeCondition : EffectCondition
    {
        public string RecipeUID;

        public override bool CheckIsValid(Character _affectedCharacter)
        {
            return _affectedCharacter.Inventory.RecipeKnowledge.IsRecipeLearned(RecipeUID);

        }
    }


}
