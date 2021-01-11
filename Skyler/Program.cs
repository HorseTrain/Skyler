using SkylerCommon.Debugging;
using SkylerCommon.Utilities.Tools;
using SkylerCore.CPU;
using SkylerGraphics.ContextHandler;
using SkylerHLE;
using SkylerHLE.Horizon;
using SkylerHLE.Horizon.Execution;
using SkylerHLE.Memory;
using System;
using static SkylerHLE.Switch;

namespace Skyler
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            InitSwitch();

            Process process = MainOS.OpenProcess();

#if (DEBUG)
            //string path = @"C:\Users\Raymond\Desktop\application\application.nro";
            //string path = @"C:\Users\Raymond\Desktop\application\Pong.nso";
            //string path = @"D:\Games\Roms\Super Mario Odyssey";
            string path = @"D:\Games\Roms\Sonic Mania";
            //string path = @"C:\Users\Raymond\Desktop\application\spacenx.nso";
            //string path = @"C:\Users\Raymond\Desktop\application\oxidgb.10-print.nso";
#else
            string path = @"C:\Users\Raymond\Desktop\application\oxidgb.10-print.nso";

            Debug.ProgramInDebugMode = true;
#endif

            if (path.Length == 0)
            {
                Debug.LogError("Please load Valid nintendo switch executable.");
            }

            if (path.Contains("."))
            {
                Debug.Log("Loading as Homebrew");

                process.LoadExecutable(path);
            }
            else
            {
                Debug.Log("Loading as Cart");
                Debug.LogWarning("No RomFS support.");

                process.LoadGame(path);

                if (!path.EndsWith("\\"))
                {
                    path += "\\";
                }

                MainSwitch.LoadRomFS(path + "main.romfs");
            }

            FrameBuffers.MainFrameBuffer = MemoryMetaData.AddressSpaceBegin;

            Scheduler.ThreadGenerator = UnicornCpuContext.CreateContext;

            process.BeginProgram();
        }
    }
}
