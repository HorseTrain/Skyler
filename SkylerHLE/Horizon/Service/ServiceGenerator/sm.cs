using SkylerHLE.Horizon.IPC;
using SkylerHLE.Horizon.Service.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Service.ServiceGenerator
{
    public static class sm
    {
        public static ulong InitService(CallContext context)
        {
            context.session.Open();

            return 0;
        }

        public static ulong GetService(CallContext context)
        {
            context.response.HandleDescriptor = HandleDescriptor.MakeMove((uint)new KSession(context.Reader.ReadString(),true).ID);

            return 0;
        }
    }
}
