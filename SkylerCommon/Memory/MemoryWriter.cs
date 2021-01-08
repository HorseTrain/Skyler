using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerCommon.Memory
{
    public unsafe class MemoryWriter : Stream
    {
        public MemoryWriter(void* Location) : base(Location)
        {

        }

        public void WriteStruct<T>(T data) where T: unmanaged
        {
            *(T*)CurrentLocation = data;

            Advance((ulong)sizeof(T));
        }

        public void WriteStruct<T>(ulong Address, T data) where T : unmanaged
        {
            Seek(Address);

            *(T*)CurrentLocation = data;

            Advance((ulong)sizeof(T));
        }

        public void WriteStruct<T>(T[] data) where T: unmanaged
        {
            foreach (T t in data)
            {
                WriteStruct(t);
            }
        }

        public void WriteStruct<T>(ulong offset, T[] data) where T : unmanaged
        {
            Seek(offset);

            WriteStruct(data);
        }
    }
}
