using System;
using HarmonyLib;
using UnityEngine;

namespace SideLoader_ExtendedEffects.Events {

    public struct EquipEvent {
        public Character character;
        public Equipment item;
        public bool equipped;
        public EquipmentSlot.EquipmentSlotIDs slot;
    }

    [HarmonyPatch(typeof(EquipmentSlot), nameof(EquipmentSlot.Equip))]
    public class EquipmentSlot_Equip
    {
        static void Postfix(
            EquipmentSlot __instance,
            Item _item
        )
        {
            var e =  new EquipEvent {
                character   = __instance.m_character,
                item  = _item as Equipment,
                equipped = true,
                slot = __instance.SlotType,
            };
            Publisher<EquipEvent>.RaiseEvent(e);
        }
    }

    [HarmonyPatch(typeof(EquipmentSlot), nameof(EquipmentSlot.Unequip))]
    public class EquipmentSlot_Unequip
    {
        static void Postfix(
            EquipmentSlot __instance
        )
        {
            var e =  new EquipEvent {
                character   = __instance.m_character,
                item = null, //The slot is always empty after unequipping
                equipped = false,
                slot = __instance.SlotType,
            };
            Publisher<EquipEvent>.RaiseEvent(e);
        }
    }

}