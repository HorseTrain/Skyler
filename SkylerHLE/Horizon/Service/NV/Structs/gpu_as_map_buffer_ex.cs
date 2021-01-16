using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Service.NV.Structs
{
    public struct gpu_as_map_buffer_ex
    {
        public int Flags;
        public int Kind;
        public int NvMapHandle;
        public int PageSize;
        public long BufferOffset;
        public long MappingSize;
        public long Offset;
    }
}
