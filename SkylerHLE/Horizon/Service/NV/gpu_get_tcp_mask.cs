using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Service.NV
{
    public struct gpu_get_tcp_mask
    {
        public uint MaskBufferSize;
        public uint Reserved;
        public ulong MaskBufferAddress;
        public uint TpcMask;
        public uint Padding;
    }
}
