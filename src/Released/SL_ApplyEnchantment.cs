using SideLoader;
using System;
using UnityEngine;

namespace SideLoader_ExtendedEffects
{
        public class SL_ApplyEnchantment: SL_Effect, ICustomModel {
        public Type SLTemplateModel => typeof(SL_ApplyEnchantment);
        public Type GameModel => typeof(ApplyEnchantment);

        public EquipmentSlot.EquipmentSlotIDs EquipmentSlot;
        public int EnchantmentID;
        public bool ApplyPermanently;

        public override void ApplyToComponent<T>(T component)
        {
            ApplyEnchantment effect = component as ApplyEnchantment;
            effect.EquipmentSlot = this.EquipmentSlot;
            effect.EnchantmentId = this.EnchantmentID;
        }

        public override void SerializeEffect<T>(T effect)
        {
            ApplyEnchantment comp = effect as ApplyEnchantment;
            this.EnchantmentID = comp.EnchantmentId;
            this.EquipmentSlot = comp.EquipmentSlot;
        }
    }

    public class ApplyEnchantment: Effect, ICustomModel {
        public Type SLTemplateModel => typeof(SL_ApplyEnchantment);
        public Type GameModel => typeof(ApplyEnchantment);

        public EquipmentSlot.EquipmentSlotIDs EquipmentSlot;
        public int EnchantmentId;

        [SerializeField]
        private UID affectedItem = UID.Empty;

        public override void ActivateLocally(Character _affectedCharacter, object[] _infos)
        {
            if (_affectedCharacter == null) //No character
            {
                return;
            }
            if (_affectedCharacter.Inventory.Equipment.EquipmentSlots[(int)EquipmentSlot].HasItemEquipped)
            {
                var itemInSlot = _affectedCharacter.Inventory.Equipment.EquipmentSlots[(int)EquipmentSlot].EquippedItem;
                if (itemInSlot.UID != affectedItem) { // Item in hand has changed, clean up old one if any and enchant new one
                    if (!affectedItem.IsNull) {
                        CleanAffectedItem();
                    }
                    if (!itemInSlot.m_enchantmentIDs.Contains(EnchantmentId)) {
                        affectedItem = itemInSlot.UID;
                        itemInSlot.AddEnchantment(EnchantmentId);
                    }
                } else {
                    // Item still in hand, do nothing
                }
            } else { // no item, gotta clean up
                if (!affectedItem.IsNull) {
                    CleanAffectedItem();
                    return;
                }
            }
        }

        public override void StopAffectLocally(Character _affectedCharacter)
        {
            if (!affectedItem.IsNull) {
                CleanAffectedItem();
            }
            base.StopAffectLocally(_affectedCharacter);
        }

        private void CleanAffectedItem() {
            var item = ItemManager.Instance.GetItem(affectedItem) as Equipment;
            item.m_enchantmentIDs.Remove(EnchantmentId);
            foreach (var ench in item.m_activeEnchantments) {
                if (ench.PresetID == EnchantmentId) {
                    item.m_activeEnchantments.Remove(ench);
                    ench.UnapplyEnchantment();
                    float durabilityRatio = item.DurabilityRatio;
					item.RefreshEnchantmentModifiers();
					if (durabilityRatio != item.DurabilityRatio)
					{
						item.SetDurabilityRatio(durabilityRatio);
					}
                }
            }
            affectedItem = UID.Empty;
        }
    }
}