using SideLoader_ExtendedEffects.Events;
using SideLoader;
using System;
using System.Collections.Generic;

namespace SideLoader_ExtendedEffects.Containers.Triggers
{

    public class SL_OnBlockEffect: SL_ParentEffect {
        public DamageSourceType RequiredSourceType;

        public override void ApplyToComponent<T>(T component)
        {
            base.ApplyToComponent<T>(component);
            var comp = component as OnBlockEffect;
            comp.RequiredSourceType = this.RequiredSourceType;
        }
        public override void SerializeEffect<T>(T effect)
        {
            base.SerializeEffect<T>(effect);
            var comp = effect as OnBlockEffect;
            this.RequiredSourceType = comp.RequiredSourceType;
        }

    }

    public class OnBlockEffect : TriggeredEffect<BlockEvent>, ICustomModel
    {
        public Type SLTemplateModel => typeof(SL_OnBlockEffect);

        public Type GameModel => typeof(OnBlockEffect);

        public DamageSourceType RequiredSourceType;

        public override void OnEvent(object sender, BlockEvent args)
        {
            try{
                //Check if the hit was made with a weapon
                if (
                    (RequiredSourceType == DamageSourceType.NON_WEAPON && args.source is Weapon) ||
                    (RequiredSourceType == DamageSourceType.WEAPON && !(args.source is Weapon))
                )
                {
                    ExtendedEffects.Instance.DebugLogMessage("Wrong Hit Type");
                    return; // Wrong type of hit
                }

                List<EffectSynchronizer.EffectCategories> dealerList = new List<EffectSynchronizer.EffectCategories>();
                List<EffectSynchronizer.EffectCategories> targetList = new List<EffectSynchronizer.EffectCategories>();
                if (args.target == this.OwnerCharacter) // Effect owner is the target
                {
                    try {
                        // apply block effects to the source of the hit (usually an enemy hitting the buff owner)
                        dealerList.Add(EffectSynchronizer.EffectCategories.Block);
                        // apply reference effects to the target of the hit (the owner of the effect)
                        targetList.Add(EffectSynchronizer.EffectCategories.Reference);
                    } catch (Exception e) {
                        ExtendedEffects.Instance.DebugLogMessage(e);
                    }
                }
                if (args.dealer == this.OwnerCharacter) // Effect owner dealt the hit; 
                {
                    // Apply Hit effects to target
                    targetList.Add(EffectSynchronizer.EffectCategories.Hit);
                    // Apply Normal effects to source
                    dealerList.Add(EffectSynchronizer.EffectCategories.Normal);
                }
                // Actually apply all of the required effects.
                // Uses apply helper to make sure only one application per update happens
                if (args.dealer == args.target)
                {
                    dealerList.AddRange(targetList);
                    Apply(dealerList.ToArray(), args.dealer, args.dealer.CenterPosition, args.hitDirection);
                }
                else
                {
                    Apply(dealerList.ToArray(), args.dealer, args.dealer.CenterPosition, args.hitDirection);
                    Apply(targetList.ToArray(), args.target, args.target.CenterPosition, args.hitDirection);
                }
            } catch (Exception e) {
                ExtendedEffects.Instance.DebugLogMessage("=============Hit Event Error============");
                ExtendedEffects.Instance.DebugLogMessage(e);
            }
            
        }

    }

}
