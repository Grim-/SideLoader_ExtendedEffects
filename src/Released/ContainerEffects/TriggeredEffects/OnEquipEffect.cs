using SideLoader_ExtendedEffects.Events;
using SideLoader;
using System;
using System.Collections.Generic;

namespace SideLoader_ExtendedEffects.Containers.Triggers
{

    public class SL_OnEquipEffect: SL_ParentEffect {
        public EquipmentSlot.EquipmentSlotIDs[] AllowedSlots;

        public override void ApplyToComponent<T>(T component)
        {
            base.ApplyToComponent<T>(component);
            var comp = component as OnEquipEffect;
            comp.AllowedSlots = this.AllowedSlots;
        }
        public override void SerializeEffect<T>(T effect)
        {
            base.SerializeEffect<T>(effect);
            var comp = effect as OnEquipEffect;
            this.AllowedSlots = comp.AllowedSlots;
        }

    }

    public class OnEquipEffect : TriggeredEffect<EquipEvent>, ICustomModel
    {
        public Type SLTemplateModel => typeof(SL_OnEquipEffect);
        public Type GameModel => typeof(OnEquipEffect);
        public EquipmentSlot.EquipmentSlotIDs[] AllowedSlots;

        public override void OnEvent(object sender, EquipEvent args)
        {
            if (!AllowedSlots.Contains(args.item.EquipSlot)) {
                return; // Not one of the tracked slots
            }

            if (args.equipped) {
                StartApply(new EffectSynchronizer.EffectCategories[]{EffectSynchronizer.EffectCategories.Activation, EffectSynchronizer.EffectCategories.Normal},
                    args.character, args.character.m_lastPosition, args.character.m_lastForward);
            }
            else
            {
                StartApply(new EffectSynchronizer.EffectCategories[]{EffectSynchronizer.EffectCategories.Reference, EffectSynchronizer.EffectCategories.Normal},
                    args.character, args.character.m_lastPosition, args.character.m_lastForward);
            }

        }

    }

}
