using HarmonyLib;
using SideLoader_ExtendedEffects;
using System;
using System.Collections.Generic;
using TravelSpeed.Extensions;
using UnityEngine;

namespace TravelSpeed.Patches
{

	[HarmonyPatch(typeof(Effect))]
	public static class EffectPatches
	{
		[HarmonyPatch(nameof(Effect.TryActivateLocally)), HarmonyPrefix]
		public static bool Effect_TryActivateLocally_Prefix(Effect __instance, Character _affectedCharacter, object[] _infos) {
			return __instance.m_parentStatusEffect == null || !__instance.m_parentStatusEffect.IsEffectSuspended();
		}
	}
}