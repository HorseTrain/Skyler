using SkylerCommon.Debugging;
using SkylerCommon.Utilities.Tools;
using SkylerHLE.Horizon.IPC;
using SkylerHLE.Horizon.Kernel.SVC;
using SkylerHLE.Horizon.Service;
using SkylerHLE.Horizon.Service.Sessions;
using System;
using System.IO;

namespace SkylerHLE.Horizon.Kernel.IPC.Handlers
{
    public static class Request
    {
        public static void HandleIPCRequest(CallContext context)
        {
            ulong Magic = context.Reader.ReadStruct<ulong>();
            ulong CommandID = context.Reader.ReadStruct<ulong>();

            context.CommandID = CommandID;

            if (context.session is KDomain)
            {
                HandleIPCDomain(context,CommandID);
            }
            else
            {
                HandleIPCSession(context,CommandID);
            }

            if (!context.Ignore)
            {
                if (context.Call == null)
                {
                    Debug.LogError($"Unknown Service: {context.session.Name}, {CommandID}", true);
                }

                using (MemoryStream stream = new MemoryStream()) //TODO: Use ram writer instead.
                {
                    context.Writer = new BinaryWriter(stream);

                    ulong result = context.Call(context);

                    context.response = ResponseHandler.FillResponse(context.response, result, stream.ToArray());

                    SupervisorCallCollection.SvcLog($"Called Service: {StringTools.FillStringBack(context.session.Name, ' ', 20)} {StringTools.FillStringBack(CommandID, ' ', 5)}");
                }
            }
        }

        public static void HandleIPCDomain(CallContext context,ulong CommandID)
        {
            switch (context.request.DCommand)
            {
                case DomainCommand.SendMsg: HandleIPCDomainSendMessage(context, CommandID); break;
                case DomainCommand.DeleteObj: HandleIPCDomainDeleteObject(context,CommandID); break;
                default: Debug.ThrowNotImplementedException(); break;
            }
        }

        public static void HandleIPCDomainSendMessage(CallContext context, ulong CommandID)
        {
            KDomain domain = (KDomain)context.session;

            object obj = domain.GetObject(context.request.DID);

            if (obj is KDomain)
            {
                context.Call = ServiceCollection.GetService(domain.Name,CommandID);

                context.Data = domain.Name;
                context.CommandID = CommandID;
            }
            else
            {
                if (((ICommandObject)obj).Calls.ContainsKey(CommandID))
                    context.Call = ((ICommandObject)obj).Calls[CommandID];
                else
                {
                    Debug.LogWarning($"{obj.GetType()}: Does not contain call {CommandID}");
                }
            }
        }

        public static void HandleIPCDomainDeleteObject(CallContext context, ulong CommandID)
        {
            //TODO:

            context.Ignore = true;
        }

        public static void HandleIPCSession(CallContext context,ulong CommandID)
        {
            if (context.session is KObject)
            {
                HandleIPCObject(context,CommandID);
            }
            else
            {
                context.Call = ServiceCollection.GetService(context.session.Name,CommandID);

                context.Data = context.session.Name;
                context.CommandID = CommandID;
            }
        }

        public static void HandleIPCObject(CallContext context, ulong CommandID)
        {
            ICommandObject obj = (ICommandObject)((KObject)context.session).Obj;

            if (obj.Calls.ContainsKey(CommandID))
            {
                context.Call = obj.Calls[CommandID];
            }
            else
            {
                Debug.LogWarning($"{obj.GetType()}: Does not contain call {CommandID}");
            }
        }
    }
}
