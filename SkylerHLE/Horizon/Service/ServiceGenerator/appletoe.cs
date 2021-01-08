using SkylerHLE.Horizon.Service.AppletAE.ApplicationProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Service.ServiceGenerator
{
    public static class appletoe
    {
        public static ulong OpenApplicationProxy(CallContext context)
        {
            Helper.Make(context,new IApplicationProxy());

            return 0;
        }
    }
}
