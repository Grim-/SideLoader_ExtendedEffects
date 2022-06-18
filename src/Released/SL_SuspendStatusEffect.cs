using SideLoader;
using System;
using System.Collections.Generic;
using TravelSpeed.Extensions;

namespace SideLoader_ExtendedEffects {
	public class SL_SuspendStatusEffect : SL_Effect, ICustomModel {
		public Type SLTemplateModel => typeof(SL_SuspendStatusEffect);
		public Type GameModel => typeof(SLEx_SuspendStatusEffect);

		public string[] StatusEffectIdentifiers;

		public override void ApplyToComponent<T>(T component)
		{
			SLEx_SuspendStatusEffect effect = component as SLEx_SuspendStatusEffect;
			effect.StatusEffectIdentifiers = StatusEffectIdentifiers;
		}
		
		public override void SerializeEffect<T>(T effect)
		{
			SLEx_SuspendStatusEffect comp = effect as SLEx_SuspendStatusEffect;
			this.StatusEffectIdentifiers = comp.StatusEffectIdentifiers;
		}
	}
	
	public class SLEx_SuspendStatusEffect : Effect, ICustomModel {
		public Type SLTemplateModel => typeof(SL_SuspendStatusEffect);
		public Type GameModel => typeof(SLEx_SuspendStatusEffect);
		
		public string[] StatusEffectIdentifiers;

		private void SetSuspendedEffects(Character _affectedCharacter, bool suspended) {
			if (StatusEffectIdentifiers == null || StatusEffectIdentifiers.Length == 0) {
				ExtendedEffects._Log.LogDebug("SLEx_SuspendStatusEffect defined without effects. Please specify the StatusEffectIdentifiers to suspend.");
				return;
			}
			foreach (StatusEffect statusEffect in _affectedCharacter.StatusEffectMngr.Statuses) {
				if (StatusEffectIdentifiers.Contains(statusEffect.IdentifierName) && statusEffect.IsEffectSuspended()!=suspended) {
					ExtendedEffects._Log.LogDebug($"Suspending effect of {statusEffect.IdentifierName} = {suspended}");
					statusEffect.SetEffectSuspended(suspended);
				}
			}
		}

		public override void ActivateLocally(Character _affectedCharacter, object[] _infos) {
			SetSuspendedEffects(_affectedCharacter, true);
		}
		
		public override void StopAffectLocally(Character _affectedCharacter) {
			SetSuspendedEffects(_affectedCharacter, false);
		}
	}
}