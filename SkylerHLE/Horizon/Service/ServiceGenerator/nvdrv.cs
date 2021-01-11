using SkylerCommon.Debugging;
using SkylerHLE.Horizon.Handles;
using SkylerHLE.Horizon.IPC;
using SkylerHLE.Horizon.Service.NV;
using SkylerHLE.Horizon.Service.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Service.ServiceGenerator
{
    public static class nvdrv
    {
        static KEvent Event = new KEvent();

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

        public static ulong SetAruid(CallContext context)
        {
            context.Writer.Write(0);

            return 0;
        }

        public static ulong QueryEvent(CallContext context)
        {
            context.response.HandleDescriptor = HandleDescriptor.MakeCopy((uint)Event.ID);

            context.Writer.Write(0);

            return 0;
        }

        public static ulong Call(CallContext context)
        {
            switch (context.CommandID)
            {
                case 0: return nvdrv.Open(context);
                case 1: return Ioctl.DrvIoctl(context);
                case 3: return nvdrv.Initialize(context);
                case 4: return QueryEvent(context);
                case 8: return SetAruid(context);
                default: Debug.LogError(context.CommandID,true); return 0;
            }
        }
    }
}
