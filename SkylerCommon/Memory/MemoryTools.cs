using System;
using System.Collections.Generic;
using System.Text;

namespace SkylerCommon.Memory
{
    public static class MemoryTools
    {
        public static unsafe T* GetPointer<T>(T[] Data) where T: unmanaged
        {
            fixed (T* data = Data)
            {
                return data;
            }
        }
    }
}
