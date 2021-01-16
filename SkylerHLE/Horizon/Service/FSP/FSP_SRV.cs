using SkylerCommon.Debugging;
using SkylerHLE.VirtualFS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Service.FSP
{
    //TODO: Create a custom fs "Standard"
    //Is this not a kobject ?
    public static class FSP_SRV
    {
        public static ulong Call(CallContext context)
        {
            switch (context.CommandID)
            {
                case 1: return SetCurrentProcess(context);
                case 18: return OpenSdCardFileSystem(context);
                case 51: return OpenSaveDataFileSystem(context);
                case 200: return OpenDataStorageByCurrentProcess(context);
                case 1005: return GetGlobalAccessLogMode(context);
                default: Debug.LogError($"fsp-srv does not contain: {context.CommandID}"); return 0;
            }
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
