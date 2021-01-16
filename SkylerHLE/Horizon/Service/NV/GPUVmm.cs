using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Service.NV
{
    public class GPUVmm
    {
        public const ulong AddressSize = 1UL << 40;

        public const int PageTableP = 14;
        public const int PageSizeBit = 12;

        public const ulong PageTableSize = 1UL << PageTableP;
        public const ulong PageSize = 1UL << PageSizeBit;

        public const ulong PageTableMask = PageTableSize - 1;
        public const ulong PageMask = PageSize - 1;

        public long[][] PageTable { get; set; }

        public GPUVmm()
        {
            PageTable = new long[PageTableSize][];
        }
    }
}
