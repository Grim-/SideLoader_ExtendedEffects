using SideLoader;
using System;

namespace SideLoader_ExtendedEffects.Containers
{

    public class SL_EffectLifecycleEffect: SL_ParentEffect {
    }

    public class EffectLifecycleEffect : ParentEffect, ICustomModel
    {

        public Type SLTemplateModel => typeof(SL_EffectLifecycleEffect);
        public Type GameModel => typeof(EffectLifecycleEffect);

        private bool activated = false;
        public override void ActivateLocally(Character _affectedCharacter, object[] _infos)
        {
            if (!activated) {
                StartApply(new EffectSynchronizer.EffectCategories[]{EffectSynchronizer.EffectCategories.Activation}, this.OwnerCharacter);
                activated = true;
            }
            StartApply(new EffectSynchronizer.EffectCategories[]{EffectSynchronizer.EffectCategories.Normal}, this.OwnerCharacter);
        }
        public override void StopAffectLocally(Character _affectedCharacter)
        {
            StartApply(new EffectSynchronizer.EffectCategories[]{EffectSynchronizer.EffectCategories.Reference}, this.OwnerCharacter);
            this.activated = false; 
            
            StopApply(new EffectSynchronizer.EffectCategories[]{EffectSynchronizer.EffectCategories.Reference}, this.OwnerCharacter);
            StopApply(new EffectSynchronizer.EffectCategories[]{EffectSynchronizer.EffectCategories.Activation}, this.OwnerCharacter);
            StopApply(new EffectSynchronizer.EffectCategories[]{EffectSynchronizer.EffectCategories.Normal}, this.OwnerCharacter);
            base.StopAffectLocally(_affectedCharacter);
        }
    }

}