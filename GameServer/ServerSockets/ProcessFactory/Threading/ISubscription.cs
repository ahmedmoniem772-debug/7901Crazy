using System;
using System.Reflection;
using System.Threading;

namespace VirusX.Threading
{
	// Token: 0x02000053 RID: 83
	internal abstract class ISubscription : IDisposable
	{
		// Token: 0x060002D0 RID: 720
		internal abstract void Invoke();

		// Token: 0x060002D1 RID: 721 RVA: 0x00039614 File Offset: 0x00037814
		public ISubscription()
		{
			ISubscription.counter++;
			this.hashCode = ISubscription.counter;
			this.Viable = true;
			this.Enqueued = false;
			this.Set(0U);
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x00039654 File Offset: 0x00037854
		~ISubscription()
		{
			((IDisposable)this).Dispose();
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060002D3 RID: 723 RVA: 0x00039684 File Offset: 0x00037884
		internal bool Next
		{
			get
			{
				return NativeTime32.Now > this.NextInvokation;
			}
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x000396A8 File Offset: 0x000378A8
		internal void Set(uint dueTime)
		{
			this.NextInvokation = NativeTime32.Now.AddMilliseconds(dueTime);
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x000396CA File Offset: 0x000378CA
		void IDisposable.Dispose()
		{
			this.Viable = false;
			this.CleanUp();
		}

		// Token: 0x060002D6 RID: 726
		internal abstract void CleanUp();

		// Token: 0x060002D7 RID: 727
		internal abstract MethodInfo GetMethodInfo();

		// Token: 0x060002D8 RID: 728
		internal abstract ThreadPriority GetPriority();

		// Token: 0x060002D9 RID: 729 RVA: 0x000396DC File Offset: 0x000378DC
		public override int GetHashCode()
		{
			return this.hashCode;
		}

		// Token: 0x040001CE RID: 462
		internal static volatile int counter = int.MinValue;

		// Token: 0x040001CF RID: 463
		internal bool Viable;

		// Token: 0x040001D0 RID: 464
		internal bool Enqueued;

		// Token: 0x040001D1 RID: 465
		internal NativeTime32 NextInvokation;

		// Token: 0x040001D2 RID: 466
		internal ThreadPriority Priority;

		// Token: 0x040001D3 RID: 467
		protected int hashCode;
	}
}
