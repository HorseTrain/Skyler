using SkylerCommon.Debugging;
using SkylerCommon.Globals;
using SkylerCommon.Utilities.Tools;
using SkylerHLE.Horizon.Execution;
using SkylerHLE.Horizon.Handles;
using SkylerHLE.Memory;
using System;
using static SkylerHLE.Switch;

namespace SkylerHLE.Horizon.Kernel.SVC
{
    public static class SvcMemory
    {
        //<-- https://switchbrew.org/wiki/SVC#QueryMemory -->

        //Pain 
        public static void QueryMemory(ObjectIndexer<ulong> X)
        {
            ulong Destination = X[0];
            ulong Address = X[2];

            MemoryMapInfo Map = Switch.Memory.GetMemoryInfo(Address);

            if (Map == null)
            {
                Map = new MemoryMapInfo(4294967296, 18446744069414584320, 0,MemoryPermission.None,MemoryType.Reserved); //TODO: Formulate size with constants?
            }

            Debug.Log($"Queried Memory With: {Map}");

            GlobalMemory.GetWriter().WriteStruct<ulong>(Destination, Map.Address); 
            GlobalMemory.GetWriter().WriteStruct<ulong>(Destination + 0x08, Map.Size);
            GlobalMemory.GetWriter().WriteStruct<uint>(Destination + 0x10, (uint)Map.Type);
            GlobalMemory.GetWriter().WriteStruct<uint>(Destination + 0x14, Map.Attr); //Almost Always 0
            GlobalMemory.GetWriter().WriteStruct<uint>(Destination + 0x18, (uint)Map.Permission);
            GlobalMemory.GetWriter().WriteStruct<uint>(Destination + 0x1c, 0);
            GlobalMemory.GetWriter().WriteStruct<uint>(Destination + 0x20, 0);
            GlobalMemory.GetWriter().WriteStruct<uint>(Destination + 0x24, 0);

            X[0] = 0;
            X[1] = 0;
        }

        //<-- https://switchbrew.org/wiki/SVC#SetHeapSize -->
        public static void SetHeapSize(ObjectIndexer<ulong> X)
        {
            ulong Size = X[0];

            //TODO: Compare size with process size, then unmap if size is greater.
            Switch.Memory.MapMemory(MemoryMetaData.HeapBase,Size,MemoryPermission.ReadAndWrite,MemoryType.MappedMemory);

            X[1] = MemoryMetaData.HeapBase;
            X[0] = 0;
        }

        //<-- https://switchbrew.org/wiki/SVC#SetMemoryAttribute -->
        public static void SetMemoryAttribute(ObjectIndexer<ulong> X)
        {
            X[0] = 0;
        }

        //<-- https://switchbrew.org/wiki/SVC#CreateTransferMemory -->
        public static void CreateTransferMemory(ObjectIndexer<ulong> X)
        {
            ulong Address = X[1];
            ulong Size = X[2];
            ulong Permission = X[3]; //TODO: find out if this has any use cases.

            //TODO: Reprotect memory ?

            TransferMemory transferMemory = new TransferMemory(Address,Size,Switch.Memory.GetMemoryInfo(Address).Permission);

            X[0] = 0;
            X[1] = transferMemory.Handle;
        }

        //<-- https://switchbrew.org/wiki/SVC#MapSharedMemory -->
        public static void MapSharedMemory(ObjectIndexer<ulong> X)
        {
            //TODO:

            X[0] = 0;
        }
    }
}
