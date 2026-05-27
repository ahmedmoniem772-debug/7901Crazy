using System;
using System.Threading;

namespace VirusX.Threading
{
	// Token: 0x02000058 RID: 88
	public class TimerRule
	{
		// Token: 0x06000317 RID: 791 RVA: 0x0003A3A4 File Offset: 0x000385A4
		public TimerRule(Action action, uint period, ThreadPriority priority = ThreadPriority.Normal)
		{
			this.Action = action;
			this.Period = period;
			this.Repeat = true;
			this.Priority = priority;
		}

		// Token: 0x06000318 RID: 792 RVA: 0x0003A3CC File Offset: 0x000385CC
		~TimerRule()
		{
			this.Action = null;
		}

		// Token: 0x040001E7 RID: 487
		internal Action Action;

		// Token: 0x040001E8 RID: 488
		internal uint Period;

		// Token: 0x040001E9 RID: 489
		internal bool Repeat;

		// Token: 0x040001EA RID: 490
		internal ThreadPriority Priority;
	}
}
