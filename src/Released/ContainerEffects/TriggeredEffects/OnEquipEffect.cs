using SideLoader_ExtendedEffects.Events;
using SideLoader;
using System;
using System.Collections.Generic;

namespace SideLoader_ExtendedEffects.Containers.Triggers
{

    public class SL_OnEquipEffect: SL_ParentEffect {
        public EquipmentSlot.EquipmentSlotIDs[] AllowedSlots;

        public OnEquipEffect.Action AllowedAction;

        public override void ApplyToComponent<T>(T component)
        {
            base.ApplyToComponent<T>(component);
            var comp = component as OnEquipEffect;
            comp.AllowedSlots = this.AllowedSlots;
            comp.AllowedAction = this.AllowedAction;
        }
        public override void SerializeEffect<T>(T effect)
        {
            base.SerializeEffect<T>(effect);
            var comp = effect as OnEquipEffect;
            this.AllowedSlots = comp.AllowedSlots;
            this.AllowedAction = comp.AllowedAction;
        }

    }

    public class OnEquipEffect : TriggeredEffect<EquipEvent>, ICustomModel
    {
        public enum Action {
            EQUIP,
            UNEQUIP,
            ANY
        }

        public Type SLTemplateModel => typeof(SL_OnEquipEffect);
        public Type GameModel => typeof(OnEquipEffect);
        public Action AllowedAction;
        public EquipmentSlot.EquipmentSlotIDs[] AllowedSlots;

        public override void OnEvent(object sender, EquipEvent args)
        {
            if (!AllowedSlots.Contains(args.item.EquipSlot)) {
                return; // Not one of the tracked slots
            }

            if (
                (AllowedAction == Action.EQUIP && !args.equipped) ||
                (AllowedAction == Action.UNEQUIP && args.equipped)
            )
            {
                return; //Not a tracked action
            }

            List<EffectSynchronizer.EffectCategories> effectList = new List<EffectSynchronizer.EffectCategories>();
            ApplyTo(effectList.ToArray(), args.character, args.character.m_lastPosition, args.character.m_lastForward);

        }

    }

}
