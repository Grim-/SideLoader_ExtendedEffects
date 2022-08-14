using SideLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SideLoader_ExtendedEffects.Released.Conditions
{
    public class SL_IsAllowedSceneCondition : SL_EffectCondition, ICustomModel
    {
        public Type SLTemplateModel => typeof(SL_IsAllowedSceneCondition);
        public Type GameModel => typeof(IsAllowedSceneCondition);


        public List<string> AllowedScenes;

        public override void ApplyToComponent<T>(T component)
        {
            IsAllowedSceneCondition comp = component as IsAllowedSceneCondition;
            comp.AllowedScenes = this.AllowedScenes;
        }

        public override void SerializeEffect<T>(T component)
        {
            IsAllowedSceneCondition comp = component as IsAllowedSceneCondition;
            this.AllowedScenes = comp.AllowedScenes;
        }
    }

    public class IsAllowedSceneCondition : EffectCondition
    {
        public List<string> AllowedScenes;

        public override bool CheckIsValid(Character _affectedCharacter)
        {
            string CurrentSceneName = SceneManagerHelper.ActiveSceneName;

            foreach (var AllowedScene in AllowedScenes)
            {
                if (CurrentSceneName == AllowedScene)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
