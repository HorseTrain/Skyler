using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Service.AM.ApplicationProxy
{
    public class IDisplayController : ICommandObject
    {
        public Dictionary<ulong, ServiceCall> Calls { get; set; }

        public IDisplayController()
        {
            Calls = new Dictionary<ulong, ServiceCall>()
            {

            };
        }
    }
}
