using SkylerHLE.Horizon.Handles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Service.HID
{
    public static class hid
    {
        public static ulong HidCreateAppletResource(CallContext context)
        {
            SharedMemory HidData = (SharedMemory)Switch.MainOS.Handles[(uint)Switch.MainOS.HidHandle];

            Helper.Make(context, new IAppletResource(HidData));

            return 0;
        }
    }
}
