using HarmonyLib;
using NodeCanvas.DialogueTrees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static MapMagic.ObjectPool;

namespace SideLoader_ExtendedEffects.Patches
{
    [HarmonyPatch(typeof(Trainer))]
    public static class TrainerPatches
    {
        [HarmonyPatch(nameof(Trainer.GetSkillTree)), HarmonyPrefix]
        public static bool GetSkillTreeOverridePrefix(Trainer __instance, ref SkillSchool __result)
        {
            SNPC snpc = __instance.GetComponentInParent<SNPC>();
            string TrainerUID = string.Empty;

            //fix for DLC trainers
            if (snpc == null)
            {
                snpc = __instance.transform.parent.parent.GetComponentInChildren<SNPC>();
                TrainerUID = __instance.HolderUID.m_value;
            }
            else
            {
                TrainerUID = snpc.HolderUID.m_value;
            }

            if (snpc != null)
            {
                if (ExtendedEffects.HasSkillTreeOverride(TrainerUID))
                {
                    string SkillTreeUID = ExtendedEffects.SkillTreeOverrides[TrainerUID];
                    SkillSchool skillSchool = SkillTreeHolder.Instance.GetSkillTreeFromUID(SkillTreeUID);
                    if (skillSchool != null)
                    {
                        __result = skillSchool;
                        return false;
                    }
                    else ExtendedEffects._Log.LogMessage($"Could not find a valid SkillSchool with UID {SkillTreeUID}");

                }
                else ExtendedEffects._Log.LogMessage($"No Override For Trainer with UID {TrainerUID}");

            }
            else ExtendedEffects._Log.LogMessage($"Character isn't a trainer.");

            return true;
        }
    }
}
