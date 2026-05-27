using VirusX;
using System;
using System.Reflection;
using System.Threading;

namespace VirusX.Threading.Generic
{
	// Token: 0x0200005A RID: 90
	internal class Subscription<T> : ISubscription
	{
		// Token: 0x0600031A RID: 794 RVA: 0x0003A410 File Offset: 0x00038610
		public Subscription(TimerRule<T> instruction, T param)
		{
			this.Instruction = instruction;
			this.Priority = this.Instruction.Priority;
			this.Param = param;
		}

		// Token: 0x0600031B RID: 795 RVA: 0x0003A43C File Offset: 0x0003863C
		internal override void Invoke()
		{
			try
			{
				bool flag = this.Instruction != null;
				if (flag)
				{
					this.Instruction.Action(this.Param);
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
			catch(Exception ex)
			{
				MyConsole.WriteLine(ex.ToString());
			}
		}

		// Token: 0x0600031C RID: 796 RVA: 0x0003A4AC File Offset: 0x000386AC
		internal override void CleanUp()
		{
			this.Instruction = null;
			this.Param = default(T);
		}

		// Token: 0x0600031D RID: 797 RVA: 0x0003A4C4 File Offset: 0x000386C4
		internal override MethodInfo GetMethodInfo()
		{
			return this.Instruction.Action.Method;
		}

		// Token: 0x0600031E RID: 798 RVA: 0x0003A4E8 File Offset: 0x000386E8
		internal override ThreadPriority GetPriority()
		{
			return this.Priority;
		}

		// Token: 0x040001EB RID: 491
		private TimerRule<T> Instruction;

		// Token: 0x040001EC RID: 492
		private T Param;
	}
}
