

namespace SideLoader_ExtendedEffects.Containers
{

    public class SL_EffectLifecycleEffect: SL_ParentEffect {
    }

    public class EffectLifecycleEffect : ParentEffect
    {
        private bool activated = false;
        public override void ActivateLocally(Character _affectedCharacter, object[] _infos)
        {
            if (!activated) {
                this.m_subEffects[0].SynchronizeEffects(EffectSynchronizer.EffectCategories.Activation, this.OwnerCharacter);
                activated = true;
            }
            this.m_subEffects[0].SynchronizeEffects(EffectSynchronizer.EffectCategories.Normal, this.OwnerCharacter);
        }
        public override void StopAffectLocally(Character _affectedCharacter)
        {
            this.m_subEffects[0].SynchronizeEffects(EffectSynchronizer.EffectCategories.Reference, this.OwnerCharacter);
            this.activated = false;
            
            this.m_subEffects[0].StopAllEffects(EffectSynchronizer.EffectCategories.Activation, this.OwnerCharacter);
            this.m_subEffects[0].StopAllEffects(EffectSynchronizer.EffectCategories.Normal, this.OwnerCharacter);
            this.m_subEffects[0].StopAllEffects(EffectSynchronizer.EffectCategories.Reference, this.OwnerCharacter);
            base.StopAffectLocally(_affectedCharacter);
        }
    }

}