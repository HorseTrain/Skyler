using SkylerCommon.Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Memory
{
    public static class MemoryMetaData
    {
        public const ulong RamSize = GlobalMemory.RamSize;

        public const ulong AddressSpaceBegin = 0x8000000;

        public const ulong HeapBase = 0x10000000;

        public const ulong MainStackSize = 0x100000;
        public const ulong MainStackAddress = RamSize - MainStackSize;
        public const ulong StackTop = MainStackAddress + MainStackSize;

        //Right before stack
        public const ulong TlsSize = 0x20000;
        public const ulong TlsCollectionAddress = MainStackAddress - TlsSize;

        public const int PageBit = 12;

        public const ulong PageSize = 1 << PageBit;
        public const ulong PageMask = PageSize - 1;

        public static ulong PageRoundUp(ulong Address) => (Address + PageMask) & ~PageMask;
        public static ulong PageRoundDown(ulong Address) => Address & ~PageMask;
    }
}
