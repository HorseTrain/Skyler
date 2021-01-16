using SkylerCommon.Debugging;
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

        public static ulong Call(CallContext context)
        {
            switch (context.CommandID)
            {
                case 0: return InitService(context); 
                case 1: return GetService(context);
                default: Debug.ThrowNotImplementedException(); return 0;
            }
        }
    }
}
