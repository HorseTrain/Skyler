using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Service.PPC
{
    public class ISession : ICommandObject
    {
        public Dictionary<ulong, ServiceCall> Calls { get; set; }

        public ISession()
        {
            Calls = new Dictionary<ulong, ServiceCall>()
            {

            };
        }
    }
}
