using System;
using System.Threading;

namespace VirusX.Threading
{
	// Token: 0x02000054 RID: 84
	public class LazyDelegate : TimerRule
	{
		// Token: 0x060002DB RID: 731 RVA: 0x00039702 File Offset: 0x00037902
		public LazyDelegate(Action action, uint dueTime, ThreadPriority priority = ThreadPriority.Normal) : base(action, dueTime, priority)
		{
			this.Repeat = false;
		}
	}
}
