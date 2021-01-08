using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Memory
{
    public enum MemoryPermission : byte
    {
        None = 0,
        Read = 1 << 0,
        Write = 1 << 1,
        Execute = 1 << 2,

        ReadAndWrite = Read | Write,
        ReadAndExecute = Read | Execute
    }
}
