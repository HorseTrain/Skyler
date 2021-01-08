using SkylerCommon.Utilities.Tools;
using SkylerHLE.Horizon.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Kernel.SVC
{
    public delegate void SupervisorCall(ObjectIndexer<ulong> Registers);
}
