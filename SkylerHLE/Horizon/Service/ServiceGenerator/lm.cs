using SkylerHLE.Horizon.Service.LM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Service.ServiceGenerator
{
    public static class lm
    {
        public static ulong OpenLogger(CallContext context)
        {
            Helper.Make(context,new Logger());

            return 0;
        }
    }
}
