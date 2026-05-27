using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VirusX.Threading;
namespace VirusX
{
    public class ProcessFactory
    {
        private const int REST = 2;

        private Action[] actions;

        private Thread[] threads;

        private int pos;
        public ProcessFactory(int workers)
        {
            this.actions = new Action[workers];
            this.threads = new Thread[workers];
            for (int i = 0; i < workers; i++)
                this.threads[i] = new Thread(Process);
        }

        private void Process(object operationObj)
        {
            var idx = (int)operationObj;
            for (; ; )
            {
                if (actions[idx] != null)
                    actions[idx]();
                Thread.Sleep(REST);
            }
        }

        public void Start()
        {
            for (int i = 0; i < threads.Length; i++)
                threads[i].Start(i);
        }

        public void AddProcess(Action process)
        {
            actions[pos] += process;
            pos = (pos + 1) % actions.Length;
        }

    }
    public class ThreadCollection
    {
        private static ProcessFactory processFactory;
        public static void StartThreads()
        {
            processFactory = new ProcessFactory(8);
            processFactory.AddProcess(AutoAttackCallBack.Handle);//1
            processFactory.AddProcess(BuffersCallback.Handle);//2
            processFactory.AddProcess(CharactersCallback.Handle);//3
            processFactory.AddProcess(FloorCallback.Handle);//4
            processFactory.AddProcess(SaveMeleAttack.Handle);//5
            processFactory.AddProcess(Monstercallback.Handle);//6
            processFactory.AddProcess(CheckItemsTime.Handle);//7
            processFactory.Start();

        }
    }

}
