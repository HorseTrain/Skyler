using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Service.AppletAE.ApplicationProxy
{
    public class IApplicationFunctions : ICommandObject
    {
        public Dictionary<ulong, ServiceCall> Calls { get; set; } 

        public IApplicationFunctions()
        {
            Calls = new Dictionary<ulong, ServiceCall>()
            {

            };
        }
    }
}
