using SkylerCommon.Debugging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Service.SET
{
    public static class set
    {
        public static ulong Call(CallContext context)
        {
            switch (context.CommandID)
            {
                case 1: return GetAvailableLanguageCodes(context); 
                default: Debug.ThrowNotImplementedException(); return 0;
            }
        }

        public static ulong GetLanguageCode(CallContext context)
        {


            return 0;
        }

        public static ulong GetAvailableLanguageCodes(CallContext context)
        {
            context.Writer.Write(15);

            return 0;
        }
    }
}
