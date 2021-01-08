using SkylerHLE.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Handles
{
    public class TransferMemory
    {
        public ulong Handle                 { get; set; } //Maybe rename this to ID?
        public ulong Address                { get; set; }
        public ulong Size                   { get; set; }
        public MemoryPermission Permission  { get; set; }
        
        public TransferMemory(ulong Address, ulong Size, MemoryPermission Permission)
        {
            this.Address = Address;
            this.Size = Size;
            this.Permission = Permission;

            Handle = Switch.MainOS.Handles.AddObject(this);
        }
    }
}
