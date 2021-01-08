using SkylerHLE.Horizon.Handles;
using SkylerHLE.Horizon.IPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Service.HID
{
    public class IAppletResource : ICommandObject
    {
        public Dictionary<ulong, ServiceCall> Calls { get; set; }

        SharedMemory shared { get; set; }

        public IAppletResource(SharedMemory shared)
        {
            Calls = new Dictionary<ulong, ServiceCall>()
            {
                {0, GetSharedMemoryHandle}
            };

            this.shared = shared;
        }

        public ulong GetSharedMemoryHandle(CallContext context)
        {
            context.response.HandleDescriptor = HandleDescriptor.MakeCopy((uint)Switch.MainOS.HidHandle);

            return 0;
        }
    }
}
