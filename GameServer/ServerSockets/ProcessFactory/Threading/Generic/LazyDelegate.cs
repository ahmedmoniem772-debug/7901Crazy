using System;
using System.Threading;

namespace VirusX.Threading.Generic
{
	// Token: 0x02000059 RID: 89
	public class LazyDelegate<T> : TimerRule<T>
	{
		// Token: 0x06000319 RID: 793 RVA: 0x0003A3FC File Offset: 0x000385FC
		public LazyDelegate(Action<T> action, uint dueTime, ThreadPriority priority = ThreadPriority.Normal) : base(action, dueTime, priority)
		{
			this.Repeat = false;
		}
	}
}
