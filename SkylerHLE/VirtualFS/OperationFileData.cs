using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.VirtualFS
{
    public static class OperationFileData
    {
        public static string SwitchOperation = Environment.GetEnvironmentVariable("USERPROFILE") + "\\Skyler";

        public static string SDPath = SwitchOperation + "\\sd";
        public static string SavePath = SwitchOperation + "\\save";

        public static void Setup()
        {
            EnsureFolderExists(SwitchOperation);

            EnsureFolderExists(SDPath);
            EnsureFolderExists(SavePath);
        }

        static void EnsureFolderExists(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
    }
}
