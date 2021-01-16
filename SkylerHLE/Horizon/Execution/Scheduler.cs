using OpenTK.Windowing.GraphicsLibraryFramework;
using SkylerCommon.Debugging;
using SkylerCommon.Globals;
using SkylerCommon.Memory;
using SkylerCommon.Utilities.Tools;
using SkylerCPU;
using SkylerHLE.Horizon.Service.Sessions;
using SkylerHLE.Memory;
using System;
using System.Collections.Generic;
using System.Threading;
using static SkylerHLE.Switch;

namespace SkylerHLE.Horizon.Execution
{
    //NOTE: Sense i'm not yuzu 2020, I would like to note that almost ALL of the schedular code is HEAVILY referenced from Ryujinx. 
    public class Scheduler
    {
        public ulong ID { get; set; }

        public static CreateThread ThreadGenerator;

        public KThread GetThread(ulong ID) => (KThread)MainOS.Handles.GetObject(ID);

        public Scheduler()
        {
            ID = MainOS.Handles.AddObject(this);
            SuspendedThreads = new List<WaitHandle>();

            KeySyncLock = new object();
            CondVarWaitingThreads = new List<KThread>();
            KeyLocks = new Dictionary<ulong, KeyLock>();
            ThreadArbiterList = new List<KThread>();

            MutexSyncLock = new object();
            MutexLocks = new Dictionary<ulong, MutexLock>();
        }

        public KThread CreateThread(Process process, ulong PC, ulong SP, ulong Arguments, ulong Priority)
        {
            CpuContext cpu = ThreadGenerator();

            KThread Guest = new KThread(cpu, process, Priority);

            Guest.Cpu.PC = PC;
            Guest.Cpu.SP = SP;
            Guest.Cpu.tpidrro_el0 = MemoryMetaData.TlsCollectionAddress + (MemoryMetaData.OpenedTLS * 0x200);

            Guest.Cpu.X[0] = Arguments;
            Guest.Cpu.X[1] = Guest.ID;

            MemoryMetaData.OpenedTLS++;

            return Guest;
        }

        public void DetatchAndStartThread(ulong id)
        {
            KThread GuestThread = GetThread(id);

            GuestThread.Running = true;

            GuestThread.StartThread();
        }

        public KEvent CreateEventHandle() => new KEvent();
        public KEvent GetEventHandle(uint ID) => (KEvent)MainOS.Handles[ID];
        public void RemoveEventHandle(uint ID) => MainOS.Handles.RemoveObject(ID);

        public List<WaitHandle> SuspendedThreads { get; private set; }

        public void AddSuspendedThread(WaitHandle Event)
        {
            lock (SuspendedThreads)
            {
                SuspendedThreads.Add(Event);
            }
        }

        public void UnSuspendThread(WaitHandle Event)
        {
            lock (SuspendedThreads)
            {
                SuspendedThreads.Remove(Event);
            }
        }

        public static int GetTimeMS(ulong NanoSeconds)
        {
            ulong Ms = NanoSeconds / 1000000;

            if (Ms < int.MaxValue)
                return (int)Ms;
            else
                return int.MaxValue;
        }

        public WaitHandle[] GetEventHandles(MemoryReader Reader,ulong Count)
        {
            WaitHandle[] Out = new WaitHandle[Count + 1];

            for (ulong i = 0; i < Count;i++)
            {
                Out[i] = ((KEvent)MainOS.Handles[Reader.ReadStruct<uint>()]).WaitEvent;
            }

            Out[Count] = new ManualResetEvent(false); //This extra is for svc 

            return Out;
        }

        object KeySyncLock      { get; set; }
        object MutexSyncLock    { get; set; }

        Dictionary<ulong,KeyLock> KeyLocks              { get; set; }
        Dictionary<ulong,MutexLock> MutexLocks          { get; set; }

        List<KThread> CondVarWaitingThreads             { get; set; }
        List<KThread> ThreadArbiterList                 { get; set; }

        public void EnterWait(KThread thread)
        {
            lock (KeySyncLock)
            {
                CondVarWaitingThreads.Add(thread);
            }

            Debug.Log($"Halted Thread: {thread.ID}",LogLevel.Low);

            thread.SyncHandler.WaitEvent.WaitOne();

            Debug.Log($"Resumed Thread: {thread.ID}",LogLevel.Low);
        }

        public void WakeThread(KThread thread)
        {
            thread.SyncHandler.WaitEvent.Set();
        }

        private (KThread, int) PopMutexThreadUnsafe(KThread OwnerThread, ulong MutexAddress)
        {
            int Count = 0;

            KThread WakeThread = null;

            foreach (KThread Thread in OwnerThread.MutexWaiters)
            {
                if (Thread.MutexAddress != MutexAddress)
                {
                    continue;
                }

                if (WakeThread == null)
                {
                    WakeThread = Thread;
                }

                Count++;
            }

            if (WakeThread != null)
            {
                OwnerThread.MutexWaiters.Remove(WakeThread);
            }

            return (WakeThread, Count);
        }

        public KThread PopCondVarThreadUnsafe(ulong CondVarAddress)
        {
            KThread WakeThread = null;

            foreach (KThread Thread in ThreadArbiterList)
            {
                if (Thread.CondVarAddress != CondVarAddress)
                {
                    continue;
                }

                if (WakeThread == null)
                {
                    WakeThread = Thread;
                }
            }

            if (WakeThread != null)
            {
                ThreadArbiterList.Remove(WakeThread);
            }

            return WakeThread;
        }

        public void MutexUnlock(KThread CurrThread, ulong MutexAddress)
        {
            (KThread OwnerThread, int Count) = PopMutexThreadUnsafe(CurrThread, MutexAddress);

            if (OwnerThread == CurrThread)
            {
                throw new Exception();
            }

            if (OwnerThread != null)
            {
                int HasListeners = Count >= 2 ? 0x40000000 : 0;

                GlobalMemory.GetWriter(MutexAddress).WriteStruct(HasListeners | OwnerThread.WaitHandle);

                OwnerThread.WaitHandle = 0;
                OwnerThread.MutexAddress = 0;
                OwnerThread.CondVarAddress = 0;
                OwnerThread.MutexOwner = null;

                WakeThread(OwnerThread);
            }
            else
            {
                GlobalMemory.GetWriter(MutexAddress).WriteStruct<uint>(0);
            }
        }

        public void MutexLock(KThread CurrentThread, KThread WaitThread, int OwnerHandle, int WaitHandle,ulong MutexAddress)
        {
            lock (KeySyncLock)
            {
                int MutexValue = GlobalMemory.GetReader(MutexAddress).ReadStruct<int>();

                if (MutexValue != (OwnerHandle | 0x40000000))
                {
                    return;
                }

                CurrentThread.WaitHandle = WaitHandle;
                CurrentThread.MutexAddress = MutexAddress;
            }

            EnterWait(CurrentThread);
        }

        public void CondVarSignal(KThread thread, ulong CondVarAddress, int Count)
        {
            lock (KeySyncLock)
            {
                while (Count == -1 || Count -- > 0)
                {
                    KThread WaitThread = PopCondVarThreadUnsafe(CondVarAddress);

                    if (WaitThread == null)
                    {
                        break;
                    }

                    WaitThread.CondVarSignaled = true;

                    ulong MutexAddress = WaitThread.MutexAddress;

                    int MutexValue = GlobalMemory.GetReader(MutexAddress).ReadStruct<int>();

                    WakeThread(WaitThread);
                }
            }
        }

        //TODO: Move this to process
        public void CondVarWait(KThread WaitThread,int WaitThreadHandle, ulong MutexAddress, ulong CondVarAddress, ulong TimeOut)
        {
            WaitThread.WaitHandle = WaitThreadHandle;
            WaitThread.MutexAddress = MutexAddress;
            WaitThread.CondVarAddress = CondVarAddress;

            lock (KeySyncLock)
            {
                MutexUnlock(WaitThread, MutexAddress);

                WaitThread.CondVarSignaled = false;

                ThreadArbiterList.Add(WaitThread);
            }

            if (TimeOut != ulong.MaxValue)
            {
                Debug.ThrowNotImplementedException();
            }
            else
            {
                EnterWait(WaitThread);
            }
        }
    }
}
