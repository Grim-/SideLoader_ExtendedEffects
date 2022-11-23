using SideLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SideLoader_ExtendedEffects.Released
{
    /// <summary>
    /// An Effect that deals damage to the _affectedCharacter based on percentage of its current HP.
    /// </summary>
    public class SL_AffectCurrentHP : SL_Effect, ICustomModel
    {
        public Type SLTemplateModel => typeof(SL_AffectCurrentHP);
        public Type GameModel => typeof(AffectCurrentHP);

        public float Modifier;

        public override void ApplyToComponent<T>(T component)
        {
            AffectCurrentHP comp = component as AffectCurrentHP;
            comp.Modifier = Modifier;
        }

        public override void SerializeEffect<T>(T effect)
        {
            AffectCurrentHP comp = effect as AffectCurrentHP;
            this.Modifier = comp.Modifier;
        }
    }

    public class AffectCurrentHP : Effect
    {
        public float Modifier;

        public override void ActivateLocally(Character _affectedCharacter, object[] _infos)
        {
            if (Modifier <= 0)
            {
                //cant negatively modifiy, use decimals for that eg 0.1 for 10%
                return;
            }

            float HealthValue = _affectedCharacter.Health * Modifier;
            _affectedCharacter.Stats.AffectHealth(HealthValue);
        }
    }
}
