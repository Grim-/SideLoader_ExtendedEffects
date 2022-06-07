using SideLoader;
using System;
using System.Collections.Generic;
using TravelSpeed.Extensions;

namespace SideLoader_ExtendedEffects {
	public class SL_SuspendStatusTimer : SL_Effect, ICustomModel {
		public Type SLTemplateModel => typeof(SL_SuspendStatusTimer);
		public Type GameModel => typeof(SLEx_SuspendStatusTimer);

		public string[] StatusEffectIdentifiers;

		public override void ApplyToComponent<T>(T component) {
			SLEx_SuspendStatusTimer effect = component as SLEx_SuspendStatusTimer;
			effect.StatusEffectIdentifiers = StatusEffectIdentifiers;
		}
		
		public override void SerializeEffect<T>(T effect) { }
	}
	
	public class SLEx_SuspendStatusTimer : Effect, ICustomModel {
		public Type SLTemplateModel => typeof(SL_SuspendStatusTimer);
		public Type GameModel => typeof(SLEx_SuspendStatusTimer);
		
		public string[] StatusEffectIdentifiers;

		private void SetSuspendedTimers(Character _affectedCharacter, bool suspended) {
			if (StatusEffectIdentifiers == null || StatusEffectIdentifiers.Length == 0) {
				ExtendedEffects.Log.LogDebug("SLEx_SuspendStatusTimer defined without effects. Please specify the StatusEffectIdentifiers to suspend.");
				return;
			}
			foreach (StatusEffect statusEffect in _affectedCharacter.StatusEffectMngr.Statuses) {
				if (StatusEffectIdentifiers.Contains(statusEffect.IdentifierName) && statusEffect.IsTimerSuspended()!=suspended) {
					ExtendedEffects.Log.LogDebug($"Suspending timer of {statusEffect.IdentifierName} = {suspended}");
					statusEffect.SetTimerSuspended(suspended);
				}
			}
		}

		public override void ActivateLocally(Character _affectedCharacter, object[] _infos) {
			SetSuspendedTimers(_affectedCharacter, true);
		}
		
		public override void StopAffectLocally(Character _affectedCharacter) {
			SetSuspendedTimers(_affectedCharacter, false);
		}
	}
}