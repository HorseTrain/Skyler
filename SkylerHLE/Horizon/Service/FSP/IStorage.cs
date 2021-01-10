using SkylerCommon.Globals;
using SkylerCommon.Memory;
using SkylerHLE.Horizon.IPC;
using SkylerHLE.Horizon.IPC.Descriptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Service.FSP
{
    public class IStorage : ICommandObject
    {
        public Dictionary<ulong, ServiceCall> Calls { get; set; }

        public IStorage()
        {
            Calls = new Dictionary<ulong, ServiceCall>()
            {
                {0,Read }
            };
        }

        public ulong Read(CallContext context)
        {
            ulong Offset = context.Reader.ReadStruct<ulong>();
            ulong Size = context.Reader.ReadStruct<ulong>();

            if (context.request.ReceiveDescriptors.Count > 0)
            {
                GlobalMemory.GetWriter(context.request.ReceiveDescriptors[0].Address).WriteStruct(Switch.romFS.ReadData(Offset, Size));
            }

            return 0;
        }
    }
}
