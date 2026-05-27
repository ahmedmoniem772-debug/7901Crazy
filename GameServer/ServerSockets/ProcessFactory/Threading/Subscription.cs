using System;
using System.Reflection;
using System.Threading;

namespace VirusX.Threading
{
	// Token: 0x02000057 RID: 87
	internal class Subscription : ISubscription
	{
		// Token: 0x06000312 RID: 786 RVA: 0x0003A2D4 File Offset: 0x000384D4
		public Subscription(TimerRule instruction)
		{
			this.Instruction = instruction;
			this.Priority = instruction.Priority;
		}

		// Token: 0x06000313 RID: 787 RVA: 0x0003A2F4 File Offset: 0x000384F4
		internal override void Invoke()
		{
			bool flag = this.Instruction != null;
			if (flag)
			{
				this.Instruction.Action();
				bool flag2 = this.Instruction != null;
				if (flag2)
				{
					bool flag3 = !this.Instruction.Repeat;
					if (flag3)
					{
						((IDisposable)this).Dispose();
					}
					else
					{
						base.Set(this.Instruction.Period);
					}
				}
			}
		}

		// Token: 0x06000314 RID: 788 RVA: 0x0003A35E File Offset: 0x0003855E
		internal override void CleanUp()
		{
			this.Instruction = null;
		}

		// Token: 0x06000315 RID: 789 RVA: 0x0003A368 File Offset: 0x00038568
		internal override MethodInfo GetMethodInfo()
		{
			return this.Instruction.Action.Method;
		}

		// Token: 0x06000316 RID: 790 RVA: 0x0003A38C File Offset: 0x0003858C
		internal override ThreadPriority GetPriority()
		{
			return this.Priority;
		}

		// Token: 0x040001E6 RID: 486
		private TimerRule Instruction;
	}
}
