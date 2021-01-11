using SkylerCommon.Debugging;
using SkylerHLE.VirtualFS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Service.FSP
{
    //Is this not a kobject ?
    public static class FSP_SRV
    {
        public static Dictionary<ulong, ServiceCall> Calls { get; set; } = new Dictionary<ulong, ServiceCall>()
        {
            {1,     SetCurrentProcess},
            {18,    OpenSdCardFileSystem },
            {51,    OpenSaveDataFileSystem },
            {200,   OpenDataStorageByCurrentProcess },
            {1005,  GetGlobalAccessLogMode}
        };

        public static ulong Call(CallContext context)
        {
            if (!Calls.ContainsKey(context.CommandID))
            {
                Debug.LogError($"fsp-srv does not contain: {context.CommandID}");
            }

            return Calls[context.CommandID](context);
        }

        public static ulong SetCurrentProcess(CallContext context)
        {
            context.PrintStubbed();

            return 0;
        }

        public static ulong OpenSdCardFileSystem(CallContext context)
        {
            Helper.Make(context, new IFileSystem(OperationFileData.SDPath));

            return 0;
        }

        public static ulong OpenSaveDataFileSystem(CallContext context)
        {
            Helper.Make(context,new IFileSystem(OperationFileData.SavePath));

            return 0;
        }

        public static ulong OpenDataStorageByCurrentProcess(CallContext context)
        {
            Helper.Make(context,new IStorage(Switch.romFS));

            return 0;
        }

        public static ulong GetGlobalAccessLogMode(CallContext context)
        {
            context.Writer.Write(0);

            return 0;
        }
    }
}
