using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Memory
{
    public struct PageEntry
    {
        public MemoryPermission permission;
        public MemoryType memorytype;
        public uint Attr;
        public bool mapped; //might not be needed ?

        public static bool Compare(PageEntry left,PageEntry right)
        {
            return
                left.permission == right.permission &&
                left.memorytype == right.memorytype &&
                left.mapped == right.mapped &&
                left.Attr == right.Attr;
        }
    }
}