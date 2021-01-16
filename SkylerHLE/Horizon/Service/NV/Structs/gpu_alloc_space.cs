using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Service.NV.Structs
{
    public struct gpu_alloc_space
    {
        public int Pages;
        public int PageSize;
        public int Flags;
        public int Padding;
        public long Offset;
    }
}
