using SkylerCommon.Debugging;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Service.ACC
{
    public class Acc
    {
        public static ulong Call(CallContext context)
        {
            switch (context.CommandID)
            {
                //case 0: return GetUserCount(context);
                case 100: return InitializeApplicationInfo(context);
                case 101: return GetBaasAccountManagerForApplication(context);
                default: Debug.ThrowNotImplementedException(context.CommandID.ToString()); return 0; 
            }
        }

        public static ulong GetUserCount(CallContext context)
        {
            context.Writer.Write(0);

            context.PrintStubbed();

            return 0;
        }

        public static ulong InitializeApplicationInfo(CallContext context)
        {
            context.Writer.Write(0);

            context.PrintStubbed();

            return 0;
        }

        public static ulong GetBaasAccountManagerForApplication(CallContext context)
        {
            Helper.Make(context,new IManagerForApplication());

            return 0;
        }
    }
}
