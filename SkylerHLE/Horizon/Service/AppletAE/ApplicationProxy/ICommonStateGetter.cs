using SkylerCommon.Debugging;
using SkylerHLE.Horizon.IPC;
using SkylerHLE.Horizon.Service.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Service.AppletAE.ApplicationProxy
{
    public class ICommonStateGetter : ICommandObject
    {
        public Dictionary<ulong, ServiceCall> Calls { get; set; }

        public ICommonStateGetter()
        {
            Calls = new Dictionary<ulong, ServiceCall>()
            {
                {0, GetEventHandle },
                {1, ReceiveMessage },
                {5, GetOperationMode},
                {6, GetPerformanceMode },
                {9, GetCurrentFocusState}
            };
        }

        public ulong GetEventHandle(CallContext context)
        {
            KEvent Event = Switch.MainOS.AppletManager.MessegeSendEvent;

            context.response.HandleDescriptor = HandleDescriptor.MakeCopy((uint)Event.ID);

            return 0;
        }

        public static ulong ReceiveMessage(CallContext context)
        {
            context.Writer.Write((int)Switch.MainOS.AppletManager.PopMessage());

            return 0;
        }

        public ulong GetOperationMode(CallContext context)
        {
            if (Switch.MainSwitch.InDock)
            {
                context.Writer.Write((byte)1);
            }
            else
            {
                context.Writer.Write((byte)0);
            }

            return 0;
        }

        public ulong GetPerformanceMode(CallContext context) => GetOperationMode(context);

        public static ulong GetCurrentFocusState(CallContext context)
        {
            AppletManager manager = Switch.MainOS.AppletManager;

            if (manager.InFocus)
            {
                context.Writer.Write((byte)1);
            }
            else
            {
                context.Writer.Write((byte)0);
            }

            return 0;
        }
    }
}
