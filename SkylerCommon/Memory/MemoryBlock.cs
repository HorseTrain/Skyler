using SkylerCommon.Debugging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SkylerCommon.Memory
{
    public unsafe class MemoryBlock
    {
        int[] Buffer    { get; set; }

        GCHandle handle { get; set; }

        public MemoryBlock(ulong Size)
        {
            if (Size % 4 != 0)
                Debug.LogError("Invalid Memory Allocation Size.");

            Buffer = new int[Size / 4];

            handle = GCHandle.Alloc(Buffer,GCHandleType.Pinned);
        }

        public static implicit operator byte*(MemoryBlock block) => (byte*)MemoryTools.GetPointer(block.Buffer);

        ~MemoryBlock()
        {
            handle.Free();
        }
    }
}
