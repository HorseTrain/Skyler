using SkylerCommon.Debugging;
using SkylerHLE.Horizon.IPC;
using SkylerHLE.Horizon.Service.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Service
{
    public static class Helper
    {
        public static void Make(CallContext context, object Obj)
        {
            if (context.session is KDomain dom)
            {
                context.response.Responses.Add((int)dom.CreateObject(Obj));
            }
            else if (Obj is ICommandObject)
            {
                KObject data = new KObject(context.session,Obj);

                context.response.HandleDescriptor = HandleDescriptor.MakeMove((uint)data.ID);
            }
            else
            {
                Debug.LogError("Invalid Object!!");
            }
        }
    }
}
