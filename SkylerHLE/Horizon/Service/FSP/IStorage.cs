using SkylerCommon.Debugging;
using SkylerCommon.Globals;
using SkylerCommon.Memory;
using SkylerCommon.Utilities.Tools;
using SkylerHLE.Horizon.IPC;
using SkylerHLE.Horizon.IPC.Descriptors;
using SkylerHLE.VirtualFS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Service.FSP
{
    public class IStorage : ICommandObject
    {
        public Dictionary<ulong, ServiceCall> Calls { get; set; }

        RomFS romFS;

        public IStorage(RomFS romFS)
        {
            Calls = new Dictionary<ulong, ServiceCall>()
            {
                {0,Read }
            };

            this.romFS = romFS;
        }

        public ulong Read(CallContext context)
        {
            ulong Offset = context.Reader.ReadStruct<ulong>();
            ulong Size = context.Reader.ReadStruct<ulong>();

            if (context.request.ReceiveDescriptors.Count > 0)
            {
                SREDescriptor descriptor = context.request.ReceiveDescriptors[0];

                if (Size > descriptor.Size)
                {
                    Size = descriptor.Size;
                }

                GlobalMemory.GetWriter(descriptor.Address).WriteStruct(romFS.ReadData(Offset, Size));
            }

            Debug.Log($"Read Address {StringTools.FillStringBack(Offset,' ',20)}, with size {StringTools.FillStringBack(Size,' ',20)}",LogLevel.Low);

            return 0;
        }
    }
}
