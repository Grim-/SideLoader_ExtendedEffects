using System;
using HarmonyLib;
using UnityEngine;

namespace SideLoader_ExtendedEffects.Events {

    public struct EquipEvent {
        public Character character;
        public Equipment item;
        public CharacterEquipment equipment;
        public bool equipped;
        public ItemContainer targetInventory;
    }

    [HarmonyPatch(typeof(CharacterEquipment), nameof(CharacterEquipment.EquipWithoutAssociating), argumentTypes:new Type[]{
        typeof(Equipment),
        typeof(bool)
    })]
    public class CharacterEquipment_EquipWithoutAssociating
    {
        static void Postfix(
            CharacterEquipment __instance,
            Equipment _itemToEquip,
            bool _playAnim = false
        )
        {
            var e =  new EquipEvent {
                character   = __instance.m_character,
                item  = _itemToEquip,
                equipment = __instance,
                equipped = true,
                targetInventory = null
            };
            Publisher<EquipEvent>.RaiseEvent(e);
        }
    }

    [HarmonyPatch(typeof(CharacterEquipment), nameof(CharacterEquipment.UnequipItem))]
    public class CharacterEquipment_UnequipItem
    {
        static void Postfix(
            CharacterEquipment __instance,
            Equipment _itemToUnequip,
            bool _playAnim,
            ItemContainer _targetContainer
        )
        {
            var e =  new EquipEvent {
                character   = __instance.m_character,
                item  = _itemToUnequip,
                equipment = __instance,
                equipped = false,
                targetInventory = _targetContainer
            };
            Publisher<EquipEvent>.RaiseEvent(e);
        }
    }

}