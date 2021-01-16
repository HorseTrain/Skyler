using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Service.AM.ApplicationProxy
{
    public class IApplicationFunctions : ICommandObject
    {
        public Dictionary<ulong, ServiceCall> Calls { get; set; } 

        public IApplicationFunctions()
        {
            Calls = new Dictionary<ulong, ServiceCall>()
            {
                {1,   PopLaunchParameter},
                {21,  GetDesiredLanguage},
                {20,  EnsureSaveData },
                {22,  SetTerminateResult },
                {23,  GetDisplayVersion },
                {40,  NotifyRunning},
                {50,  GetPseudoDeviceId },
                {66,  InitializeGamePlayRecording },
                {67,  SetGamePlayRecordingState }
            };
        }

        public ulong NotifyRunning(CallContext context)
        {
            context.Writer.Write(1);

            return 0;
        }

        public ulong GetPseudoDeviceId(CallContext context)
        {
            context.PrintStubbed();

            context.Writer.Write(0L);
            context.Writer.Write(0L);

            return 0;
        }

        public ulong InitializeGamePlayRecording(CallContext context)
        {
            context.PrintStubbed();

            return 0;
        }

        public ulong SetGamePlayRecordingState(CallContext context)
        {
            context.PrintStubbed();

            return 0;
        }

        public ulong PopLaunchParameter(CallContext context)
        {
            Helper.Make(context,new IStorage(MakeLaunchParams()));

            return 0;
        }

        public ulong GetDesiredLanguage(CallContext context)
        {
            context.Writer.Write(357911326309L); //American English

            return 0;
        }

        public ulong EnsureSaveData(CallContext context)
        {
            context.PrintStubbed();

            context.Writer.Write(0L);

            return 0;
        }

        public ulong SetTerminateResult(CallContext context)
        {
            context.PrintStubbed();

            return 0;
        }

        public ulong GetDisplayVersion(CallContext context)
        {
            context.Writer.Write(1L);
            context.Writer.Write(0L);

            return 0;
        }

        static byte[] MakeLaunchParams()
        {
            using (MemoryStream MS = new MemoryStream())
            {
                BinaryWriter Writer = new BinaryWriter(MS);

                MS.SetLength(0x88);

                Writer.Write((uint)0xc79497ca);
                Writer.Write(1); 
                Writer.Write(1L);
                Writer.Write(0L);

                return MS.ToArray();
            }
        }
    }
}
