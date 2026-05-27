using System;
using System.Threading;

namespace VirusX.Threading.Generic
{
	// Token: 0x0200005B RID: 91
	public class TimerRule<T>
	{
		// Token: 0x0600031F RID: 799 RVA: 0x0003A500 File Offset: 0x00038700
		public TimerRule(Action<T> action, uint period, ThreadPriority priority = ThreadPriority.Normal)
		{
			this.Action = action;
			this.Period = period;
			this.Repeat = true;
			this.Priority = priority;
		}

		// Token: 0x06000320 RID: 800 RVA: 0x0003A528 File Offset: 0x00038728
		~TimerRule()
		{
			this.Action = null;
		}

		// Token: 0x040001ED RID: 493
		internal Action<T> Action;

		// Token: 0x040001EE RID: 494
		internal uint Period;

		// Token: 0x040001EF RID: 495
		internal bool Repeat;

		// Token: 0x040001F0 RID: 496
		internal ThreadPriority Priority;
	}
}
