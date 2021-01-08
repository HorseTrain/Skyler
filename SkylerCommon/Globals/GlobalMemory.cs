using SkylerCommon.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkylerCommon.Globals
{
    public static unsafe class GlobalMemory
    {
        public const ulong RamSize = 4UL * 1024 * 1024 * 1024;

        public static IntPtr BaseMemory { get; set; }
        public static unsafe void* BaseMemoryPointer => BaseMemory.ToPointer();

        //TODO: Make these thread safe;
        public static MemoryReader RamReader { get; private set; } 
        public static MemoryWriter RamWriter { get; private set; } 

        public static void SetBaseAddress(void* Address)
        {
            BaseMemory = (IntPtr)Address;

            RamReader = new MemoryReader(Address);
            RamWriter = new MemoryWriter(Address);
        }
    }
}
