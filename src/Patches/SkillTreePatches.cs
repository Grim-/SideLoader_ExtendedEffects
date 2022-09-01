using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SideLoader_ExtendedEffects.Patches
{
	[HarmonyPatch(typeof(Trainer))]
	public static class TrainerPatches
	{
		[HarmonyPatch(nameof(Trainer.GetSkillTree)), HarmonyPrefix]
		public static bool GetSkillTreeOverridePrefix(Trainer __instance, ref SkillSchool __result)
		{
			SNPC snpc = __instance.GetComponentInParent<SNPC>();
            if (snpc != null)
			{
				string TrainerUID = snpc.HolderUID.m_value;
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
