
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Service.NV
{
    public struct gpu_z_cull_get_info
    {
        public int WidthAlignPixels;
        public int HeightAlignPixels;
        public int PixelSquaresByAliquots;
        public int AliquotTotal;
        public int RegionByteMultiplier;
        public int RegionHeaderSize;
        public int SubregionHeaderSize;
        public int SubregionWidthAlignPixels;
        public int SubregionHeightAlignPixels;
        public int SubregionCount;
    }
}
