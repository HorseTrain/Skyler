using SkylerCommon.Debugging;
using SkylerCommon.Utilities.Tools;
using SkylerCore.CPU;
using SkylerGraphics.ContextHandler;
using SkylerHLE;
using SkylerHLE.Horizon;
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
            string path = args[0];
#else
            string path = args[0];
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
            }

            FrameBuffers.MainFrameBuffer = MemoryMetaData.AddressSpaceBegin;

            process.BeginProgram<UnicornCpuContext>();
        }
    }
}
