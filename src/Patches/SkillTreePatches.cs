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
			string TrainerUID = TryGetCharacterUID(__instance);

			if (TrainerUID != String.Empty)
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
                    else
                    {
                        ExtendedEffects._Log.LogMessage($"Could not find a valid SkillSchool with UID {SkillTreeUID}");
                    }
                }
                else
                {
                    //ExtendedEffects._Log.LogMessage($"No Override For Trainer with UID {TrainerUID}");
                }
            }
            else
            {
                ExtendedEffects._Log.LogMessage($"Failed to find a CharacterUID for {__instance.name}.");
            }
				
			return true;
		}


		private static string TryGetCharacterUID(Trainer Trainer)
		{
            SNPC snpc = Trainer.GetComponentInParent<SNPC>();

			if (snpc == null)
			{
				snpc = Trainer.GetComponentInChildren<SNPC>();
            }

			if (snpc == null)
			{
				if (Trainer == null)
				{
					return string.Empty;
				}

				return Trainer.HolderUID;
			}
			else
			{
				return snpc.HolderUID.m_value;
			}
        }


    }
}
