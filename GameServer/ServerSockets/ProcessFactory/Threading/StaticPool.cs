using System;
using System.Collections.Generic;
using System.Security;
using System.Threading;
using VirusX;
using VirusX.Threading.Generic;

namespace VirusX.Threading
{
	// Token: 0x02000056 RID: 86
	public class StaticPool : IDisposable
	{
		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060002FE RID: 766 RVA: 0x00039B08 File Offset: 0x00037D08
		public int Threads
		{
			get
			{
				return this.threads;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060002FF RID: 767 RVA: 0x00039B20 File Offset: 0x00037D20
		public int IdleThreads
		{
			get
			{
				return this.idleThreads;
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000300 RID: 768 RVA: 0x00039B38 File Offset: 0x00037D38
		public int InUseThreads
		{
			get
			{
				return this.inUseThreads;
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000301 RID: 769 RVA: 0x00039B50 File Offset: 0x00037D50
		public int Treshold
		{
			get
			{
				return this.queue.Count;
			}
		}

		// Token: 0x06000302 RID: 770 RVA: 0x00039B70 File Offset: 0x00037D70
		public StaticPool(int maximumPoolSize = 32, ThreadPriority basePriority = ThreadPriority.Normal)
		{
			bool flag = !StaticPool.IsHandlerCreated;
			if (flag)
			{
				StaticPool.IsHandlerCreated = true;
				AppDomain.CurrentDomain.UnhandledException += this.CurrentDomain_UnhandledException;
			}
			this.disposed = false;
			this.dSyncRoot = new object();
			this.qSyncRoot = new object();
			this.subscribers = new Dictionary<int, ISubscription>();
			this.queue = new Queue<ISubscription>();
			this.pool = new List<Thread>();
			this.minimumThreadCount = maximumPoolSize;
			this.maximumThreadCount = maximumPoolSize;
			this.basePriority = basePriority;
		}

		// Token: 0x06000303 RID: 771 RVA: 0x000288D1 File Offset: 0x00026AD1
		private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
		}

		// Token: 0x06000304 RID: 772 RVA: 0x00039C0C File Offset: 0x00037E0C
		~StaticPool()
		{
			this.cleanUp(false);
		}

		// Token: 0x06000305 RID: 773 RVA: 0x00039C40 File Offset: 0x00037E40
		void IDisposable.Dispose()
		{
			this.cleanUp(true);
		}

		// Token: 0x06000306 RID: 774 RVA: 0x00039C4C File Offset: 0x00037E4C
		[SecuritySafeCritical]
		internal void enrollWorker()
		{
			bool flag = this.disposed;
			if (!flag)
			{
				Interlocked.Increment(ref this.threads);
				Interlocked.Increment(ref this.idleThreads);
				Thread thread = new Thread(new ThreadStart(this.work), 1048576);
				this.pool.Add(thread);
				thread.Priority = this.basePriority;
				thread.Start();
			}
		}

		// Token: 0x06000307 RID: 775 RVA: 0x00039CB8 File Offset: 0x00037EB8
		internal void cleanUp(bool forcefully)
		{
			bool flag = !this.disposed;
			if (flag)
			{
				this.disposed = true;
				this.doWork = false;
				if (forcefully)
				{
					foreach (Thread thread in this.pool)
					{
						ThreadEx.Abort(thread);
					}
				}
				this.subscribers.Clear();
				this.subscribers = null;
				this.queue = null;
				this.pool = null;
			}
		}

		// Token: 0x06000308 RID: 776 RVA: 0x00039D58 File Offset: 0x00037F58
		internal void work()
		{
			Thread currentThread;
			for (currentThread = Thread.CurrentThread; currentThread == null; currentThread = Thread.CurrentThread)
			{
			}
			while (this.doWork)
			{
				Thread.Sleep(StaticPool.SleepTime);
				ISubscription subscription;
				bool flag = this.tryDequeue(out subscription);
				if (flag)
				{
					bool viable = subscription.Viable;
					if (viable)
					{
						Interlocked.Decrement(ref this.idleThreads);
						Interlocked.Increment(ref this.inUseThreads);
						currentThread.Priority = subscription.GetPriority();
						try
						{
							subscription.Invoke();
						}
						catch (Exception e)
						{
							MyConsole.WriteException(e);
						}
						finally
						{
							subscription.Enqueued = false;
						}
						currentThread.Priority = this.basePriority;
						Interlocked.Decrement(ref this.inUseThreads);
						Interlocked.Increment(ref this.idleThreads);
					}
					else
					{
						this.removeSubscriber(subscription.GetHashCode());
					}
				}
				subscription = null;
			}
			Interlocked.Decrement(ref this.idleThreads);
		}

		// Token: 0x06000309 RID: 777 RVA: 0x00039E68 File Offset: 0x00038068
		internal bool tryDequeue(out ISubscription sub)
		{
			sub = null;
			object obj = this.qSyncRoot;
			lock (obj)
			{
				bool flag2 = this.queue.Count != 0;
				if (flag2)
				{
					ISubscription subscription = this.queue.Dequeue();
					sub = subscription;
				}
			}
			return sub != null;
		}

		// Token: 0x0600030A RID: 778 RVA: 0x00039ED8 File Offset: 0x000380D8
		internal void removeSubscriber(int hash)
		{
			object obj = this.dSyncRoot;
			lock (obj)
			{
				this.subscribers.Remove(hash);
			}
		}

		// Token: 0x0600030B RID: 779 RVA: 0x00039F24 File Offset: 0x00038124
		public IDisposable Subscribe(TimerRule instruction)
		{
			ISubscription subscription = null;
			object obj = this.dSyncRoot;
			lock (obj)
			{
				subscription = new Subscription(instruction);
				bool flag2 = instruction is LazyDelegate;
				if (flag2)
				{
					subscription.Set(instruction.Period);
				}
				this.subscribers[subscription.GetHashCode()] = subscription;
			}
			return subscription;
		}

		// Token: 0x0600030C RID: 780 RVA: 0x00039FA0 File Offset: 0x000381A0
		public IDisposable Subscribe<T>(TimerRule<T> instruction, T param)
		{
			ISubscription subscription = null;
			object obj = this.dSyncRoot;
			lock (obj)
			{
				subscription = new Subscription<T>(instruction, param);
				bool flag2 = instruction is LazyDelegate<T>;
				if (flag2)
				{
					subscription.Set(instruction.Period);
				}
				this.subscribers[subscription.GetHashCode()] = subscription;
			}
			return subscription;
		}

		// Token: 0x0600030D RID: 781 RVA: 0x0003A020 File Offset: 0x00038220
		public StaticPool Run()
		{
			this.doWork = true;
			for (int i = 0; i < this.minimumThreadCount; i++)
			{
				this.enrollWorker();
			}
			this.propagationThread = new Thread(new ThreadStart(this.propagate));
			this.propagationThread.Start();
			return this;
		}

		// Token: 0x0600030E RID: 782 RVA: 0x0003A07C File Offset: 0x0003827C
		private void propagate()
		{
			while (this.doWork)
			{
				Queue<ISubscription> queue = new Queue<ISubscription>();
				Queue<int> queue2 = new Queue<int>();
				object obj = this.dSyncRoot;
				lock (obj)
				{
					foreach (ISubscription subscription in this.subscribers.Values)
					{
						bool viable = subscription.Viable;
						if (viable)
						{
							bool flag2 = !subscription.Enqueued && subscription.Next;
							if (flag2)
							{
								subscription.Enqueued = true;
								queue.Enqueue(subscription);
							}
						}
						else
						{
							queue2.Enqueue(subscription.GetHashCode());
						}
					}
					while (queue2.Count != 0)
					{
						this.subscribers.Remove(queue2.Dequeue());
					}
				}
				bool flag3 = queue.Count != 0;
				if (flag3)
				{
					object obj2 = this.qSyncRoot;
					lock (obj2)
					{
						while (queue.Count != 0)
						{
							this.queue.Enqueue(queue.Dequeue());
						}
					}
				}
				Thread.Sleep(StaticPool.SleepTime);
			}
		}

		// Token: 0x0600030F RID: 783 RVA: 0x0003A204 File Offset: 0x00038404
		public void Clear()
		{
			object obj = this.qSyncRoot;
			lock (obj)
			{
				this.queue.Clear();
			}
		}

		// Token: 0x06000310 RID: 784 RVA: 0x0003A250 File Offset: 0x00038450
		public override string ToString()
		{
			int count = this.subscribers.Count;
			int count2 = this.queue.Count;
			return string.Format("{0} waiting exec, {1} subscriptions, {2} threads: {3} in use, {4} idle", new object[]
			{
				count2,
				count,
				this.threads,
				this.inUseThreads,
				this.idleThreads
			});
		}

		// Token: 0x040001D6 RID: 470
		public static int SleepTime = 1;

		// Token: 0x040001D7 RID: 471
		internal object qSyncRoot;

		// Token: 0x040001D8 RID: 472
		internal object dSyncRoot;

		// Token: 0x040001D9 RID: 473
		internal Dictionary<int, ISubscription> subscribers;

		// Token: 0x040001DA RID: 474
		internal Queue<ISubscription> queue;

		// Token: 0x040001DB RID: 475
		internal ThreadPriority basePriority = ThreadPriority.Normal;

		// Token: 0x040001DC RID: 476
		internal List<Thread> pool;

		// Token: 0x040001DD RID: 477
		protected internal Thread propagationThread;

		// Token: 0x040001DE RID: 478
		internal volatile bool doWork;

		// Token: 0x040001DF RID: 479
		internal volatile bool disposed;

		// Token: 0x040001E0 RID: 480
		internal int threads;

		// Token: 0x040001E1 RID: 481
		internal int idleThreads;

		// Token: 0x040001E2 RID: 482
		internal int inUseThreads;

		// Token: 0x040001E3 RID: 483
		internal int minimumThreadCount;

		// Token: 0x040001E4 RID: 484
		internal int maximumThreadCount;

		// Token: 0x040001E5 RID: 485
		private static bool IsHandlerCreated = false;
	}
}
