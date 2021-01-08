using SkylerHLE.Horizon.Service.HID;
using SkylerHLE.Horizon.Service.NV;
using SkylerHLE.Horizon.Service.ServiceGenerator;
using SkylerHLE.Horizon.Service.TIME;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Service
{

    //TODO: Orginize services better
    public static class ServiceCollection
    {
        public static Dictionary<(string, ulong), ServiceCall> Calls = new Dictionary<(string, ulong), ServiceCall>()
        {
            {("hid",0),         hid.HidCreateAppletResource},

            {("sm:",0),         sm.InitService },
            {("sm:",1),         sm.GetService },

            {("nvdrv:a",0),     nvdrv.Open },
            {("nvdrv:a",1),     Ioctl.DrvIoctl },
            {("nvdrv:a",3),     nvdrv.Initialize },

            {("time:s",0),      time.TimeGetStandardUserSystemClock},

            {("vi:m",2),        vi.GetDisplayService}
        };

        public static ServiceCall GetService(string Name, ulong ID)
        {
            if (Calls.ContainsKey((Name,ID)))
            {
                return Calls[(Name, ID)];
            }

            return null;
        }
    }
}
