using OpenTK.Windowing.GraphicsLibraryFramework;
using SkylerCommon.Debugging;
using SkylerCommon.Memory;
using SkylerCommon.Utilities.Tools;
using SkylerCPU;
using SkylerHLE.Horizon.Service.Sessions;
using System;
using System.Collections.Generic;
using System.Threading;
using static SkylerHLE.Switch;

namespace SkylerHLE.Horizon.Execution
{
    public class Scheduler
    {
        public ulong ID { get; set; }

        public KThread GetThread(ulong ID) => (KThread)MainOS.Handles.GetObject(ID);

        public Scheduler()
        {
            ID = MainOS.Handles.AddObject(this);
            SuspendedThreads = new List<WaitHandle>();
            MutexSyncLock = new object();
            MutexLocks = new Dictionary<ulong, MutexLock>();
        }

        public KThread CreateThread<T>(T cpu, Process process, ulong PC, ulong SP, ulong TLS, ulong Arguments, ulong Priority) where T : CpuContext
        {
            KThread Guest = new KThread(cpu, process, Priority);

            Guest.Cpu.PC = PC;
            Guest.Cpu.SP = SP;
            Guest.Cpu.tpidrro_el0 = TLS;

            Guest.Cpu.X[0] = Arguments;
            Guest.Cpu.X[1] = Guest.ID;

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

        object MutexSyncLock { get; set; }

        Dictionary<ulong,MutexLock> MutexLocks { get; set; }

        public void MutexLockAddress(ulong KeyAddress, ulong TagAddress, int Tag,ulong TimeOut,KThread thread)
        {
            MutexLock Lock = new MutexLock()
            {
                KeyAddress = KeyAddress,
                TagAddress = TagAddress,
                Tag = Tag,
                TimeOut = TimeOut,
                GuestThread = thread,
                Event = new ManualResetEvent(false)
            };

            lock (MutexSyncLock)
            {
                MutexLocks.Add(KeyAddress,Lock);
            }

            if (TimeOut == ulong.MaxValue)
            {
                Lock.Event.WaitOne();
            }
            else
            {
                Lock.Event.WaitOne(GetTimeMS(TimeOut));

                MutexUnlockAddress(KeyAddress,Tag);
            }
        }

        public void MutexUnlockAddress(ulong KeyAddress,int Tag)
        {
            lock (MutexSyncLock)
            {
                if (MutexLocks.ContainsKey(KeyAddress))
                {
                    MutexLocks[KeyAddress].Event.Set();

                    MutexLocks.Remove(KeyAddress);
                }
            }
        }
    }
}
