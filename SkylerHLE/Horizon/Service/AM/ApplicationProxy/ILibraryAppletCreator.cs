using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Service.AM.ApplicationProxy
{
    public class ILibraryAppletCreator : ICommandObject
    {
        public Dictionary<ulong, ServiceCall> Calls { get; set; } 

        public ILibraryAppletCreator()
        {
            Calls = new Dictionary<ulong, ServiceCall>()
            {

            };
        }
    }
}
