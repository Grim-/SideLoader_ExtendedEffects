using System.Runtime.CompilerServices;

namespace TravelSpeed.Extensions {
	public static class StatusEffectExtensions {
		
		private static ConditionalWeakTable<StatusEffect, StatusEffectExt> SuspendedEffects = new ConditionalWeakTable<StatusEffect, StatusEffectExt>();

		public static void SetTimerSuspended(this StatusEffect statusEffect, bool suspended) {
			StatusEffectExt ext = SuspendedEffects.GetValue(statusEffect, key => new StatusEffectExt());
			ext.TimerSuspended = suspended;
		}
		
		public static bool IsTimerSuspended(this StatusEffect statusEffect) {
			if (SuspendedEffects.TryGetValue(statusEffect, out StatusEffectExt ext)) {
				return ext.TimerSuspended;
			}
			return false;
		}
		
		public static void SetEffectSuspended(this StatusEffect statusEffect, bool suspended) {
			StatusEffectExt ext = SuspendedEffects.GetValue(statusEffect, key => new StatusEffectExt());
			ext.EffectSuspended = suspended;
		}
		
		public static bool IsEffectSuspended(this StatusEffect statusEffect) {
			if (SuspendedEffects.TryGetValue(statusEffect, out StatusEffectExt ext)) {
				return ext.EffectSuspended;
			}
			return false;
		}
	}
}