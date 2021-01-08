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

        public static MemoryReader GetReader(ulong Address = 0)
        {
            MemoryReader Out = new MemoryReader(BaseMemoryPointer);

            Out.Seek(Address);

            return Out;
        }

        public static MemoryWriter GetWriter(ulong Address = 0)
        {
            MemoryWriter Out = new MemoryWriter(BaseMemoryPointer);

            Out.Seek(Address);

            return Out;
        }

        public static void SetBaseAddress(void* Address)
        {
            BaseMemory = (IntPtr)Address;
        }
    }
}
