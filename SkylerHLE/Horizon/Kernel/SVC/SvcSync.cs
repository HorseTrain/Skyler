using SkylerCommon.Debugging;
using SkylerCommon.Globals;
using SkylerCommon.Utilities.Tools;
using SkylerHLE.Horizon.Execution;
using SkylerHLE.Horizon.Service.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Kernel.SVC
{
    public static class SvcSync
    {
        public static void ResetSignal(ObjectIndexer<ulong> X)
        {
            ulong Handle = X[0];

            KEvent Event = (KEvent)Switch.MainOS.Handles[Handle];

            Event.WaitEvent.Reset();

            X[0] = 0;
        }

        public static void WaitSynchronization(ObjectIndexer<ulong> X)
        {
            //TODO:

            ulong HandlePointer = X[1];
            ulong HandleCount = X[2];
            ulong TimeOut = X[3];

            if (HandleCount > 64)
            {
                X[0] = 0xEE01;
            }

            //KThread thread = (KThread)((CpuContext)X.parent).ThreadInformation;

            Scheduler scheduler = Switch.MainOS.scheduler;

            WaitHandle[] WaitHandles = scheduler.GetEventHandles(GlobalMemory.GetReader(HandlePointer), HandleCount);

            scheduler.AddSuspendedThread(WaitHandles[HandleCount]);

            int HandleResult = 0;
            ulong Result = 0;

            if (TimeOut != ulong.MaxValue)
            {
                HandleResult = WaitHandle.WaitAny(WaitHandles, Scheduler.GetTimeMS(TimeOut));
            }
            else
            {
                HandleResult = WaitHandle.WaitAny(WaitHandles);
            }

            scheduler.UnSuspendThread(WaitHandles[HandleCount]);

            if (HandleResult == WaitHandle.WaitTimeout)
            {
                Result = 59905;
            }
            else if (HandleResult == WaitHandles.Length + 1)
            {
                //TODO:

                Debug.ThrowNotImplementedException();
            }

            X[0] = Result;

            if (Result == 0)
            {
                X[1] = (ulong)HandleResult;
            }
        }

        public static void SignalProcessWideKey(ObjectIndexer<ulong> X)
        {
            ulong ConditionalVar = X[0];
            ulong Count = X[1];

            KThread thread = SupervisorCallCollection.GetThread(X);

            Scheduler scheduler = Switch.MainOS.scheduler;

            scheduler.CondVarSignal(thread,ConditionalVar,(int)Count);

            X[0] = 0;
        }

        public static void WaitProcessWideKeyAtomic(ObjectIndexer<ulong> X)
        {
            ulong MutexAddress = X[0];
            ulong CondVarAddress = X[1];
            ulong ThreadHandle = X[2];
            ulong TimeOut = X[3];

            Scheduler scheduler = Switch.MainOS.scheduler;

            scheduler.CondVarWait(SupervisorCallCollection.GetThread(X),(int)ThreadHandle,MutexAddress,CondVarAddress,TimeOut);            

            X[0] = 0;
        }

        public static void ArbitrateLock(ObjectIndexer<ulong> X)
        {
            int OwnerThreadHandle = (int)X[0];
            ulong MutexAddress = X[1];
            int WaitThreadHandle = (int)X[2];

            Scheduler scheduler = Switch.MainOS.scheduler;

            scheduler.MutexLock(SupervisorCallCollection.GetThread(X), SupervisorCallCollection.GetThread((ulong)WaitThreadHandle),OwnerThreadHandle,WaitThreadHandle,MutexAddress);

            X[0] = 0;
        }

        public static void ArbitrateUnlock(ObjectIndexer<ulong> X)
        {
            ulong Address = X[0];

            Scheduler scheduler = Switch.MainOS.scheduler;

            scheduler.MutexUnlock(SupervisorCallCollection.GetThread(X),Address);

            X[0] = 0;
        }
    }
}
