using SkylerCommon.Debugging;
using SkylerCommon.Globals;
using SkylerCommon.Utilities.Tools;
using SkylerCPU;
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

        static int i = 0;

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

            //Debug.Log($"Queried Memory From: {StringTools.FillStringBack(Address,' ',20)} With: {Map}");

            GlobalMemory.GetWriter().WriteStruct<ulong>(Destination, Map.Address); 
            GlobalMemory.GetWriter().WriteStruct<ulong>(Destination + 0x08, Map.Size);
            GlobalMemory.GetWriter().WriteStruct<uint>(Destination + 0x10, (uint)Map.Type);
            GlobalMemory.GetWriter().WriteStruct<uint>(Destination + 0x14, Map.Attr); //Almost Always 0
            GlobalMemory.GetWriter().WriteStruct<uint>(Destination + 0x18, (uint)Map.Permission);
            GlobalMemory.GetWriter().WriteStruct<uint>(Destination + 0x1c, 0);
            GlobalMemory.GetWriter().WriteStruct<uint>(Destination + 0x20, 0);
            GlobalMemory.GetWriter().WriteStruct<uint>(Destination + 0x24, 0);

            //Console.WriteLine(Map.Address);

            i++;

            X[0] = 0;
            X[1] = 0;
        }

        public static void SetHeapSize(ObjectIndexer<ulong> X)
        {
            ulong Size = X[1];

            //TODO: Compare size with process size, then unmap if size is greater.
            Switch.Memory.MapMemory(MemoryMetaData.HeapBase,Size,MemoryPermission.ReadAndWrite,MemoryType.Heap);

            //TODO: Add unmap

            MemoryMetaData.CurrentHeapSize = Size;

            X[1] = MemoryMetaData.HeapBase;
            X[0] = 0;
        }

        public static void SetMemoryAttribute(ObjectIndexer<ulong> X)
        {
            ulong Address = X[0];
            ulong Size = X[1];
            ulong Mask = X[2];
            ulong Value = X[3];

            Debug.LogWarning("Called Stubbed SVC \"SetMemoryAttribute\"");
            
            //TODO:

            X[0] = 0;
        }

        public static void MapMemory(ObjectIndexer<ulong> X)
        {
            ulong Des = X[0];
            ulong Source = X[1];
            ulong Size = X[3];

            MemoryMapInfo Map = Switch.Memory.GetMemoryInfo(Source);

            //Console.WriteLine(Map.tp);

            Switch.Memory.MapMemory(Des,Size,MemoryPermission.None,Map.Type);

            X[0] = 0;
        }

        public static void CreateTransferMemory(ObjectIndexer<ulong> X)
        {
            ulong Address = X[1];
            ulong Size = X[2];
            ulong Permission = X[3]; //TODO: find out if this has any uses.

            //TODO: Reprotect memory ?

            TransferMemory transferMemory = new TransferMemory(Address,Size,Switch.Memory.GetMemoryInfo(Address).Permission);

            X[0] = 0;
            X[1] = transferMemory.Handle;
        }

        public static void MapSharedMemory(ObjectIndexer<ulong> X)
        {
            ulong Handle = X[0];
            ulong Source = X[1];
            ulong Size = X[2];
            ulong Perm = X[3];

            if (!MemoryManager.IsValidPosition(Source))
            {
                Debug.LogError("",true);
            }

            SharedMemory sharedMemory = (SharedMemory)MainOS.Handles[Handle];

            //TODO: Chedk for null 

            if (sharedMemory != null)
            {
                Switch.Memory.MapMemory(Source,Size,(MemoryPermission)Perm,MemoryType.SharedMemory);

                //TODO: Maybe clear map ?

                sharedMemory.VirtualPosition = Source;
                sharedMemory.Map();
            }

            X[0] = 0;
        }

        public static void MapPhysicalMemory(ObjectIndexer<ulong> X)
        {
            ulong Address = X[0];
            ulong Size = X[1];

            Switch.Memory.MapMemory(Address,Size,MemoryPermission.ReadAndWrite,MemoryType.Heap);

            X[0] = 0;
        }
    }
}
