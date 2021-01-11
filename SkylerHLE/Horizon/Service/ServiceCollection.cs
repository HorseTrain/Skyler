using SkylerCommon.Debugging;
using SkylerHLE.Horizon.Handles;
using SkylerHLE.Horizon.Service.ACC;
using SkylerHLE.Horizon.Service.AppletAE.ApplicationProxy;
using SkylerHLE.Horizon.Service.FSP;
using SkylerHLE.Horizon.Service.HID;
using SkylerHLE.Horizon.Service.LM;
using SkylerHLE.Horizon.Service.NV;
using SkylerHLE.Horizon.Service.Pctl;
using SkylerHLE.Horizon.Service.PPC;
using SkylerHLE.Horizon.Service.ServiceGenerator;
using SkylerHLE.Horizon.Service.SET;
using SkylerHLE.Horizon.Service.TIME;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Service
{

    //TODO: Orginize services better
    public static class ServiceCollection
    {
        public static ServiceCall GetService(string Name, ulong ID)
        {
            return CreateService;
        }

        public static ulong CreateService(CallContext context)
        {
            string name = (string)context.Data;

            object collection = null;

            switch (name)
            {
                case "acc:u0": return Acc.Call(context);

                case "apm": 
                    
                    switch (context.CommandID)
                    {
                        case 0: collection = new IManager(); break;
                    }
                    
                    break;

                case "apm:p": 
                    
                    switch (context.CommandID)
                    {
                        case 0: collection = new IManager(); break;
                    }
                    
                    break;

                case "appletOE": 
                    
                    switch (context.CommandID)
                    {
                        case 0: collection = new IApplicationProxy(); break;
                    }
                    
                     break;

                case "hid":

                    switch (context.CommandID)
                    {
                        case 0: collection = new IAppletResource((SharedMemory)Switch.MainOS.Handles[(uint)Switch.MainOS.HidHandle.ID]);break;
                    }

                    break;

                case "lm":

                    switch (context.CommandID)
                    {
                        case 0: collection = new Logger(); break;
                    }

                    break;

                case "sm:":

                    switch (context.CommandID)
                    {
                        case 0: return sm.InitService(context);
                        case 1: return sm.GetService(context);
                    }

                    break;

                case "set": return set.Call(context);

                case "nvdrv:a": return nvdrv.Call(context);
                case "nvdrv": return nvdrv.Call(context);

                case "vi:m":

                    switch (context.CommandID)
                    {
                        case 2: return vi.GetDisplayService(context);
                    }

                    break;

                case "pctl:a":

                    switch (context.CommandID)
                    {
                        case 0: collection = new IParentalControlService(); break;
                    }

                    break;

                case "time:s":

                    switch (context.CommandID)
                    {
                        case 0: collection = new ISystemClock(0); break;
                        case 1: collection = new ISystemClock(0); break;
                        case 3: collection = new ITimeZoneService(); break;
                        case 4: collection = new ISystemClock(0); break;
                    }

                    break;

                case "fsp-srv": return FSP_SRV.Call(context);
            }

            if (collection == null)
            {
                Debug.LogError($"Unknown Service. {name}, {context.CommandID}", true);
            }

            Helper.Make(context, collection);

            return 0;
        }
    }
}
