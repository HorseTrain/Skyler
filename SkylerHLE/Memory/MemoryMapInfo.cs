using SkylerCommon.Utilities.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Memory
{
    public class MemoryMapInfo
    {
        public ulong Address                { get; set; }
        public ulong Size                   { get; set; }
        public uint Attr                    { get; set; }
        public MemoryPermission Permission  { get; set; }
        public MemoryType Type              { get; set; }

        public MemoryMapInfo(ulong Address, ulong Size,uint Attr, MemoryPermission permission,MemoryType type)
        {
            this.Address = Address;
            this.Size = Size;
            this.Attr = Attr;
            this.Permission = permission;
            this.Type = type;
        }

        public override string ToString() => $"Address: {StringTools.FillStringBack(Address, ' ', 15)} Size: {StringTools.FillStringBack(Size, ' ', 15)}Attr: {StringTools.FillStringBack(Attr, ' ', 15)}Permission: {StringTools.FillStringBack(Permission, ' ', 15)}Type: {StringTools.FillStringBack(Type, ' ', 15)}";
    }
}
