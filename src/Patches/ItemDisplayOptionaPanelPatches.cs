using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SideLoader_ExtendedEffects.Patches
{
    [HarmonyPatch(typeof(ItemDisplayOptionPanel))]
    public static class ItemDisplayOptionPanelPatches
    {
        [HarmonyPatch(nameof(ItemDisplayOptionPanel.GetActiveActions)), HarmonyPostfix]
        private static void EquipmentMenu_GetActiveActions_Postfix(ItemDisplayOptionPanel __instance, GameObject pointerPress, ref List<int> __result)
        {
            foreach (var current in ExtendedEffects.Instance.CustomItemMenuManager.CustomItemOptions)
            {
                if (!__result.Contains(current.Key))
                {
                    //if predicate isnt null then use that to determine if this action should be added to this item
                    if (current.Value.ShouldAddActionDelegate != null)
                    {
                        //if true add it
                        if (current.Value.ShouldAddActionDelegate(__instance.LocalCharacter, __instance.m_pendingItem, __instance, current.Key))
                        {
                            __result.Add(current.Key);
                        }
                    }
                    else
                    {
                        //otherwise just add it
                        __result.Add(current.Key);
                    }
                }
            }
        }



        [HarmonyPatch(nameof(ItemDisplayOptionPanel.ActionHasBeenPressed)), HarmonyPrefix]
        private static void EquipmentMenu_ActionHasBeenPressed_Prefix(ItemDisplayOptionPanel __instance, int _actionID)
        {
            Character owner = __instance.m_characterUI.TargetCharacter;
            Item CurrentItem = __instance.m_pendingItem;

            if (owner && CurrentItem && owner.Inventory.OwnsOrHasEquipped(CurrentItem.ItemID))
            {
                foreach (var CustomAction in ExtendedEffects.Instance.CustomItemMenuManager.CustomItemOptions)
                {
                    if (_actionID == CustomAction.Key)
                    {
                        CustomAction.Value?.OnCustomActionPressed(owner, CurrentItem, __instance, _actionID);
                    }
                }
            }

        }


        [HarmonyPatch(nameof(ItemDisplayOptionPanel.GetActionText)), HarmonyPrefix]
        private static bool EquipmentMenu_GetActionText_Prefix(ItemDisplayOptionPanel __instance, int _actionID, ref string __result)
        {
            foreach (var CustomAction in ExtendedEffects.Instance.CustomItemMenuManager.CustomItemOptions)
            {
                if (_actionID == CustomAction.Key)
                {
                    __result = CustomAction.Value.ActionString;
                    return false;
                }
            }
            return true;
        }
    }
}
