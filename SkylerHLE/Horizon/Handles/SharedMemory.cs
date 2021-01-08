using SkylerCommon.Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Handles
{
    public unsafe class SharedMemory
    {
        public ulong ID                 { get; set; }
        public ulong RealPosition       { get; set; }
        public ulong VirtualPosition    { get; set; }
        public bool Mapped              { get; set; }

        public SharedMemory(ulong VirtualPosition) 
        {
            this.VirtualPosition = VirtualPosition;
            this.RealPosition = (ulong)GlobalMemory.BaseMemoryPointer + VirtualPosition;

            ID = Switch.MainOS.Handles.AddObject(this);
        }

        public void Map() => Mapped = true;
        public void Unmap() => Mapped = false;
    }
}
