using SkylerHLE.Horizon.Service.VI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Service.ServiceGenerator
{
    public static class vi
    {
        public static ulong GetDisplayService(CallContext context)
        {
            context.Reader.ReadStruct<int>();

            Helper.Make(context,new IApplicationDisplayService());

            return 0;
        }
    }
}
