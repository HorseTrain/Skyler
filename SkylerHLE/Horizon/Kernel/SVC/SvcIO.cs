using SkylerCommon.Debugging;
using SkylerCommon.Globals;
using SkylerCommon.Memory;
using SkylerCommon.Timing;
using SkylerCommon.Utilities.Tools;
using SkylerCPU;
using SkylerHLE.Horizon.Execution;
using SkylerHLE.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Kernel.SVC
{
    static class SvcIO
    {
        //<-- https://switchbrew.org/wiki/SVC#OutputDebugString -->
        public static void OutputDebugString(ObjectIndexer<ulong> X)
        {
            ulong Address = X[0];
            ulong Size = X[1];

            MemoryReader reader = GlobalMemory.RamReader;

            reader.Seek(Address);

            SupervisorCallCollection.SvcLog(reader.ReadString(Size));

            X[0] = 0;
        }

        //<-- https://switchbrew.org/wiki/SVC#GetSystemTick -->
        public static void GetSystemTick(ObjectIndexer<ulong> X) => X[0] = GlobalCounter.cntpct_el0;

        //<-- https://switchbrew.org/wiki/SVC#GetInfo -->
        public static void GetInfo(ObjectIndexer<ulong> X)
        {
            ulong InfoType = X[1];
            ulong Handle = X[2];
            ulong InfoSubType = X[3];

            //Kernel too new (for now)
            if (InfoType >= 19)
            {
                X[0] = 61441;

                return;
            }

            X[0] = 0;

            switch (InfoType)
            {
                case 2: X[1] = 0x10000000; break;   //TODO: Put this in a constant.
                case 3: X[1] = 0x20000000; break;   //TODO: Put this in a constant.
                case 4: X[1] = 0x10000000 + 0x20000000; break;  //TODO: Put this in a constant.
                case 5: X[1] = 0xCFEE0000; break;   //TODO: Put this in a constant.

                case 11: X[1] = 0; break; //No Rng here :)
                case 12: X[1] = MemoryMetaData.AddressSpaceBegin; break;
                case 13: X[1] = 4160749568; break;  //TODO: Put this in a constant.
                case 14: X[1] = 0x10000000; break;  //TODO: Put this in a constant.
                case 15: X[1] = 0x20000000; break;  //TODO: Put this in a constant.
                case 16: X[1] = 1; break;

                default: Debug.ThrowNotImplementedException($"Info: {InfoType}"); break;
            }
        }

        //<-- https://switchbrew.org/wiki/SVC#CloseHandle -->
        public static void CloseHandle(ObjectIndexer<ulong> X)
        {
            //TODO:

            X[0] = 0;
        }

        //<-- https://switchbrew.org/wiki/SVC#SleepThread -->
        public static void SleepThread(ObjectIndexer<ulong> X)
        {
            ulong Nanoseconds = X[0];

            //TODO: Move this to sched.

           Thread.Sleep((int)(Nanoseconds / 1000000));
        }
    }
}
