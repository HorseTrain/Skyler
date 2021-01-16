using SkylerCommon.Debugging;
using SkylerCommon.Globals;
using SkylerCommon.Memory;
using SkylerCommon.Timing;
using SkylerCommon.Utilities.Tools;
using SkylerCPU;
using SkylerHLE.Horizon.Execution;
using SkylerHLE.Horizon.Handles;
using SkylerHLE.Horizon.Service.Sessions;
using SkylerHLE.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace SkylerHLE.Horizon.Kernel.SVC
{
    static class SvcIO
    {
        public static void OutputDebugString(ObjectIndexer<ulong> X)
        {
            ulong Address = X[0];
            ulong Size = X[1];

            MemoryReader reader = GlobalMemory.GetReader(Address);

            SupervisorCallCollection.SvcLog(reader.ReadString(Size));

            X[0] = 0;
        }

        public static void GetSystemTick(ObjectIndexer<ulong> X) => X[0] = GlobalCounter.cntpct_el0;

        static Random rng = new Random();

        public static void GetInfo(ObjectIndexer<ulong> X)
        {
            ulong InfoType = X[1];
            ulong Handle = X[2];
            ulong InfoSubType = X[3];

            //Kernel too new (for now)
            if (InfoType >= 18)
            {
                X[0] = 61441;

                return;
            }

            X[0] = 0;

            switch (InfoType)
            {
                case 0: X[1] = 0b1111; break;
                case 2: X[1] = 0x10000000; break;   //TODO: Put this in a constant.
                case 3: X[1] = 0x20000000; break;   //TODO: Put this in a constant.
                case 4: X[1] = 0x10000000 + 0x20000000; break;  //TODO: Put this in a constant.
                case 5: X[1] = 0xCFEE0000; break;   //TODO: Put this in a constant.
                case 6: X[1] = GlobalMemory.RamSize - MemoryMetaData.AddressSpaceBegin; break;
                case 7: X[1] = 806486016 + MemoryMetaData.CurrentHeapSize; break;

                case 11: X[1] = 0; break;//(ulong)rng.Next() + (ulong)(rng.Next() << 32); break; //No Rng here :)
                case 12: X[1] = MemoryMetaData.AddressSpaceBegin; break;
                case 13: X[1] = 4160749568; break;  //TODO: Put this in a constant.
                case 14: X[1] = 0x10000000; break;  //TODO: Put this in a constant.
                case 15: X[1] = 0x20000000; break;  //TODO: Put this in a constant.
                case 16: X[1] = 1; break;

                default: Debug.ThrowNotImplementedException($"Info: {InfoType}"); break;
            }
        }

        public static void CloseHandle(ObjectIndexer<ulong> X)
        {
            Switch.MainOS.Handles.RemoveObject(X[0]);

            X[0] = 0;
        }

        public static void CreateThread(ObjectIndexer<ulong> X)
        {
            ulong Entry = X[1];
            ulong ThreadContext = X[2];
            ulong StackTop = X[3];
            int Priority = (int)X[4];
            int ProcessorId = (int)X[5];

            if (ProcessorId == -2)
            {
                ProcessorId = 0;
            }
            else if ((uint)ProcessorId > 3)
            {
                Debug.LogError("",true);
            }

            KThread thread = Switch.MainOS.scheduler.CreateThread(Process.MainProcess,Entry,StackTop,ThreadContext,(ulong)Priority);

            X[0] = 0;
            X[1] = thread.ID;
        }

        public static void StartThread(ObjectIndexer<ulong> X)
        {
            ulong Handle = X[0];

            Switch.MainOS.scheduler.DetatchAndStartThread(Handle);

            X[0] = 0;
        }

        public static void SleepThread(ObjectIndexer<ulong> X)
        {
            ulong Nanoseconds = X[0];

            //TODO: Move this to sched.

            Thread.Sleep((int)(Nanoseconds / 1000000));
        }

        public static void GetThreadPriority(ObjectIndexer<ulong> X)
        {
            ulong Handle = X[1];

            KThread thread = (KThread)Switch.MainOS.Handles[Handle];

            X[0] = 0;
            X[1] = thread.ThreadPriority;
        }

        public static void SetThreadPriority(ObjectIndexer<ulong> X)
        {
            ulong Handle = X[0];
            ulong Priority = X[1];

            KThread thread = (KThread)Switch.MainOS.Handles[Handle];

            thread.ThreadPriority = Priority;

            X[0] = 0;
        }

        public static void GetThreadId(ObjectIndexer<ulong> X)
        {
            ulong Handle = X[1];

            X[0] = 0;
            X[1] = ((KThread)Switch.MainOS.Handles[Handle]).ID;
        }

        public static void Break(ObjectIndexer<ulong> X)
        {
            Debug.ThrowException(new Exception($"Pain from {((KThread)((CpuContext)X.parent).ThreadInformation).ID}"));
        }
    }
}
