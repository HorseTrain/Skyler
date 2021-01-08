using SkylerHLE.Horizon.Handles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Service.ServiceGenerator
{
    public static class nvdrv
    {
        public static ulong Open(CallContext context)
        {
            context.Reader.Seek(context.request.SendDescriptors[0].Address);

            string Name = context.Reader.ReadString();

            FileDescriptor descriptor = new FileDescriptor(Name);

            context.Writer.Write((uint)descriptor.Handle);
            context.Writer.Write(0);

            return 0;
        }

        public static ulong Initialize(CallContext context)
        {
            ulong TransferMemSize = context.Reader.ReadStruct<ulong>();
            uint TransferMemHandle = context.request.HandleDescriptor.ToCopy[0];

            context.Writer.Write(0);

            return 0;
        }
    }
}
