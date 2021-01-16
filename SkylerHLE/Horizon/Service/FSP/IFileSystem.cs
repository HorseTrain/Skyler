using SkylerCommon.Debugging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Service.FSP
{
    public class IFileSystem : ICommandObject
    {
        public Dictionary<ulong, ServiceCall> Calls { get; set; }

        public string Path { get; set; }

        public IFileSystem(string path)
        {
            Calls = new Dictionary<ulong, ServiceCall>()
            {

            };

            path = Path;
        }
    }
}
