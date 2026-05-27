using System;
using System.Reflection;
using System.Threading;

// Token: 0x02000002 RID: 2
public class ThreadEx
{
	// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	public static void Abort(Thread thread)
	{
		MethodInfo methodInfo = null;
		foreach (MethodInfo methodInfo2 in thread.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic))
		{
			bool flag = methodInfo2.Name.Equals("AbortInternal") && methodInfo2.GetParameters().Length == 0;
			if (flag)
			{
				methodInfo = methodInfo2;
			}
		}
		bool flag2 = methodInfo == null;
		if (flag2)
		{
			throw new Exception("Failed to get Thread.Abort method");
		}
		methodInfo.Invoke(thread, new object[0]);
	}
}
