using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Memory
{
    public class PageTable
    {
        public ulong ID { get; set; }
        public PageEntry[] Entries { get; set; }

        public PageTable(ulong ID)
        {
            this.ID = ID;
            Entries = new PageEntry[MemoryManager.PageTableLength];
        }

        public ref PageEntry this[ulong index] => ref Entries[index];
    }
}
