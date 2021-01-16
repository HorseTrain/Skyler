using SkylerCommon.Debugging;
using SkylerCommon.Globals;
using SkylerCommon.Utilities.Tools;
using SkylerCPU;
using SkylerHLE.Horizon.Execution;
using SkylerHLE.Horizon.IPC;
using SkylerHLE.Horizon.Kernel.IPC;
using SkylerHLE.Horizon.Service.Sessions;
using System;

namespace SkylerHLE.Horizon.Kernel.SVC
{
    public class SvcIPC
    {
        public static void ConnectToNamedPort(ObjectIndexer<ulong> X)
        {
            string Name = GlobalMemory.GetReader().ReadStringAtAddress(X[1], 8);

            KSession session = new KSession(Name, true);

            Debug.Log($"Connected to NamePort: {Name}",LogLevel.Low);

            X[0] = 0;
            X[1] = session.ID;
        }

        public static void SendSyncRequest(ObjectIndexer<ulong> X)
        {
            ulong SessionHandle = X[0];
            ulong Address = ((CpuContext)X.parent).tpidrro_el0;

            KSession session = (KSession)Switch.MainOS.Handles.GetObject(SessionHandle);

            IPCCommand command = new IPCCommand(GlobalMemory.GetReader(Address),session is KDomain);

            IPCHandler.CallIPC(command, session);

            X[0] = 0;
            //TODO: Add Error Handling.
        }
    }
}
