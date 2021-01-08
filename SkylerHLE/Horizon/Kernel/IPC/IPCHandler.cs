using SkylerCommon.Globals;
using SkylerCommon.Memory;
using SkylerHLE.Horizon.IPC;
using SkylerHLE.Horizon.Kernel.IPC.Handlers;
using SkylerHLE.Horizon.Service;
using SkylerHLE.Horizon.Service.Sessions;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Kernel.IPC
{
    public static class IPCHandler
    {
        public static void CallIPC(IPCCommand command,KSession session)
        {
            CallContext context = new CallContext()
            {
                session = session,
                request = command,
                response = new IPCCommand(command.IsDomain),
                Reader = GlobalMemory.GetReader()
            };

            context.Reader.Seek(command.RawDataPointer);

            switch (command.Type)
            {
                case CommandType.Request: Request.HandleIPCRequest(context); break;
                case CommandType.Control: Control.HandleControl(context); break;
                case CommandType.CloseSession:
                    
                    //TODO:

                    break;
                default: throw new NotImplementedException();
            }

            GlobalMemory.GetWriter(command.Address).WriteStruct(context.response.BuildResponse(command.Address));
        }
    }
}
