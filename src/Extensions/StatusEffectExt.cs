namespace TravelSpeed.Extensions {
	public class StatusEffectExt {
		private bool timerSuspended = false;
		private bool effectSuspended = false;

		public bool TimerSuspended {
			get => timerSuspended;
			set => timerSuspended = value;
		}

		public bool EffectSuspended {
			get => effectSuspended;
			set => effectSuspended = value;
		}
	}
}