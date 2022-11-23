using SideLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SideLoader_ExtendedEffects.Released.Conditions
{
    /// <summary>
    /// An EffectCondition that returns if the _AffectedCharacter is blocking or not.
    /// </summary>
    public class SL_IsHitTargetBlocking : SL_EffectCondition, ICustomModel
    {
        public Type SLTemplateModel => typeof(SL_IsHitTargetBlocking);

        public Type GameModel => typeof(IsHitTargetBlocking);

        public override void ApplyToComponent<T>(T component)
        {
           
        }

        public override void SerializeEffect<T>(T component)
        {
           
        }
    }

    public class IsHitTargetBlocking : EffectCondition
    {
        public override bool CheckIsValid(Character _affectedCharacter)
        {
            return _affectedCharacter.Blocking;
        }
    }
}
