using SkylerCommon.Debugging;
using SkylerHLE.Horizon.Service;
using SkylerHLE.Horizon.Service.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Kernel.IPC.Handlers
{
    public static class Control
    {
        public static void HandleControl(CallContext context)
        {
            ulong Magic = context.Reader.ReadStruct<ulong>();
            ulong CommandID = context.Reader.ReadStruct<ulong>();

            switch (CommandID)
            {
                case 0: ConvertKSession(context); break;
                case 3: ResponseHandler.FillResponse(context.response,0,0x500); break; //What is this?
                default: Debug.ThrowNotImplementedException(CommandID.ToString()); break;
            }
        }

        public static void ConvertKSession(CallContext context)
        {
            KDomain domain = new KDomain(context.session);

            Switch.MainOS.Handles.SwapObject(context.session.ID,domain);

            context.request = ResponseHandler.FillResponse(context.response,0,(int)domain.CreateObject(domain));
        }
    }
}
