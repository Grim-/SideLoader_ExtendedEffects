using SideLoader_ExtendedEffects.Events;
using SideLoader;
using System;

namespace SideLoader_ExtendedEffects.Containers.Triggers
{

    public class SL_OnHitEffect: SL_ParentEffect {}

    public class OnHitEffect : TriggeredEffect<HitEvent>, ICustomModel
    {
        public Type SLTemplateModel => typeof(SL_OnHitEffect);

        public Type GameModel => typeof(OnHitEffect);

        public enum DamageSourceType {
            ALL,
            WEAPON,
            NON_WEAPON
        }

        public DamageSourceType RequiredSourceType;
        public DamageType[] DamageTypes;
        public bool RequireAllTypes;
        public int MinDamage;
        public bool OnlyCountRequiredTypes;

        public override void OnEvent(object sender, HitEvent args)
        {
            try{
                

                if (args.target == this.OwnerCharacter) // Effect owner is the target
                {
                    try {
                        // apply block effects to the source of the hit (usually an enemy hitting the buff owner)
                        this.m_subEffects[0].SynchronizeEffects(EffectSynchronizer.EffectCategories.Block, args.source, args.hitLocation, args.hitDirection);
                        this.m_subEffects[0].StopAllEffects(EffectSynchronizer.EffectCategories.Block, args.source);
                        // apply reference effects to the target of the hit (the owner of the effect)
                        this.m_subEffects[0].SynchronizeEffects(EffectSynchronizer.EffectCategories.Reference, args.target, args.hitLocation, args.hitDirection);
                        this.m_subEffects[0].StopAllEffects(EffectSynchronizer.EffectCategories.Reference, args.target);
                    } catch (Exception e) {
                        SL.Log(e);
                    }
                }
                if (args.source == this.OwnerCharacter) // Effect owner dealt the hit; 
                {
                    try {
                        // apply hit effects to the target
                        this.m_subEffects[0].SynchronizeEffects(EffectSynchronizer.EffectCategories.Hit, args.target, args.hitLocation, args.hitDirection);
                        this.m_subEffects[0].StopAllEffects(EffectSynchronizer.EffectCategories.Hit, args.target);
                        // apply normal effects to the owner
                        this.m_subEffects[0].SynchronizeEffects(EffectSynchronizer.EffectCategories.Normal, args.source, args.hitLocation, args.hitDirection);
                        this.m_subEffects[0].StopAllEffects(EffectSynchronizer.EffectCategories.Normal, args.source);
                    } catch (Exception e) {
                        SL.Log(e);
                    }
                }
            } catch (Exception e) {
                SL.Log("=============Hit Event Error============");
                SL.Log(e);
            }
            
        }

    }

}
