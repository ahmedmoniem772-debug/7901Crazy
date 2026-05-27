using System;

namespace VirusX.Threading
{
	// Token: 0x02000055 RID: 85
	public struct NativeTime32
	{
		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060002DC RID: 732 RVA: 0x00039718 File Offset: 0x00037918
		public static NativeTime32 Now
		{
			get
			{
				return new NativeTime32((uint)Time32.Now.Value);
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060002DD RID: 733 RVA: 0x0003973C File Offset: 0x0003793C
		public uint Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x060002DE RID: 734 RVA: 0x00039754 File Offset: 0x00037954
		public NativeTime32(uint Value)
		{
			this.value = Value;
		}

		// Token: 0x060002DF RID: 735 RVA: 0x00039754 File Offset: 0x00037954
		public NativeTime32(int Value)
		{
			this.value = (uint)Value;
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x0003975E File Offset: 0x0003795E
		public NativeTime32(long Value)
		{
			this.value = (uint)Value;
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0003976C File Offset: 0x0003796C
		public NativeTime32 AddMilliseconds(int Amount)
		{
			return new NativeTime32((long)((ulong)this.value + (ulong)((long)Amount)));
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x00039790 File Offset: 0x00037990
		public NativeTime32 AddMilliseconds(uint Amount)
		{
			return new NativeTime32(this.value + Amount);
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x000397B0 File Offset: 0x000379B0
		public uint AllMilliseconds()
		{
			return this.value;
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x000397C8 File Offset: 0x000379C8
		public NativeTime32 AddSeconds(int Amount)
		{
			return this.AddMilliseconds(Amount * 1000);
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x000397E8 File Offset: 0x000379E8
		public NativeTime32 AddSeconds(uint Amount)
		{
			return this.AddMilliseconds(Amount * 1000U);
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x00039808 File Offset: 0x00037A08
		public uint AllSeconds()
		{
			return this.AllMilliseconds() / 1000U;
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x00039828 File Offset: 0x00037A28
		public NativeTime32 AddMinutes(int Amount)
		{
			return this.AddSeconds(Amount * 60);
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x00039844 File Offset: 0x00037A44
		public NativeTime32 AddMinutes(uint Amount)
		{
			return this.AddSeconds(Amount * 60U);
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x00039860 File Offset: 0x00037A60
		public uint AllMinutes()
		{
			return this.AllSeconds() / 60U;
		}

		// Token: 0x060002EA RID: 746 RVA: 0x0003987C File Offset: 0x00037A7C
		public NativeTime32 AddHours(int Amount)
		{
			return this.AddMinutes(Amount * 60);
		}

		// Token: 0x060002EB RID: 747 RVA: 0x00039898 File Offset: 0x00037A98
		public NativeTime32 AddHours(uint Amount)
		{
			return this.AddMinutes(Amount * 60U);
		}

		// Token: 0x060002EC RID: 748 RVA: 0x000398B4 File Offset: 0x00037AB4
		public uint AllHours()
		{
			return this.AllMinutes() / 60U;
		}

		// Token: 0x060002ED RID: 749 RVA: 0x000398D0 File Offset: 0x00037AD0
		public NativeTime32 AddDays(int Amount)
		{
			return this.AddHours(Amount * 24);
		}

		// Token: 0x060002EE RID: 750 RVA: 0x000398EC File Offset: 0x00037AEC
		public NativeTime32 AddDays(uint Amount)
		{
			return this.AddHours(Amount * 24U);
		}

		// Token: 0x060002EF RID: 751 RVA: 0x00039908 File Offset: 0x00037B08
		public uint AllDays()
		{
			return this.AllHours() / 24U;
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x00039924 File Offset: 0x00037B24
		public bool Next(uint due = 0U, uint time = 0U)
		{
			bool flag = time == 0U;
			if (flag)
			{
				time = (uint)Environment.TickCount;
			}
			return this.value + due <= time;
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x00039954 File Offset: 0x00037B54
		public void Set(uint due, uint time = 0U)
		{
			bool flag = time == 0U;
			if (flag)
			{
				time = (uint)Environment.TickCount;
			}
			this.value = time + due;
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0003997A File Offset: 0x00037B7A
		public void SetSeconds(uint due, uint time = 0U)
		{
			this.Set(due * 1000U, time);
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0003998C File Offset: 0x00037B8C
		public override bool Equals(object obj)
		{
			bool flag = obj is NativeTime32;
			bool result;
			if (flag)
			{
				result = ((NativeTime32)obj == this);
			}
			else
			{
				result = base.Equals(obj);
			}
			return result;
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x000399D0 File Offset: 0x00037BD0
		public override string ToString()
		{
			return this.value.ToString();
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x000399F0 File Offset: 0x00037BF0
		public override int GetHashCode()
		{
			return (int)this.value;
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x00039A08 File Offset: 0x00037C08
		public static bool operator ==(NativeTime32 t1, NativeTime32 t2)
		{
			return t1.value == t2.value;
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x00039A28 File Offset: 0x00037C28
		public static bool operator !=(NativeTime32 t1, NativeTime32 t2)
		{
			return t1.value != t2.value;
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x00039A4C File Offset: 0x00037C4C
		public static bool operator >(NativeTime32 t1, NativeTime32 t2)
		{
			return t1.value > t2.value;
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x00039A6C File Offset: 0x00037C6C
		public static bool operator <(NativeTime32 t1, NativeTime32 t2)
		{
			return t1.value < t2.value;
		}

		// Token: 0x060002FA RID: 762 RVA: 0x00039A8C File Offset: 0x00037C8C
		public static bool operator >=(NativeTime32 t1, NativeTime32 t2)
		{
			return t1.value >= t2.value;
		}

		// Token: 0x060002FB RID: 763 RVA: 0x00039AB0 File Offset: 0x00037CB0
		public static bool operator <=(NativeTime32 t1, NativeTime32 t2)
		{
			return t1.value <= t2.value;
		}

		// Token: 0x060002FC RID: 764 RVA: 0x00039AD4 File Offset: 0x00037CD4
		public static NativeTime32 operator -(NativeTime32 t1, NativeTime32 t2)
		{
			return new NativeTime32(t1.value - t2.value);
		}

		// Token: 0x040001D4 RID: 468
		private uint value;

		// Token: 0x040001D5 RID: 469
		public static readonly NativeTime32 NULL = new NativeTime32(0);
	}
}
