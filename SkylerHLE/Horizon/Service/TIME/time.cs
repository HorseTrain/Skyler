using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Service.TIME
{
    public class time
    {
        public static ulong TimeGetStandardUserSystemClock(CallContext context)
        {
            //TODO: Add args.

            Helper.Make(context,new ISystemClock(0));

            return 0;
        }
    }
}
