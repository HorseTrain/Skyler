using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Service.AM.ApplicationProxy
{
    public class IAudioController : ICommandObject
    {
        public Dictionary<ulong, ServiceCall> Calls { get; set; }

        public IAudioController()
        {
            Calls = new Dictionary<ulong, ServiceCall>()
            {

            };
        }
    }
}
