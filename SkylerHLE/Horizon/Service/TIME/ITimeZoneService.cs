using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Service.TIME
{
    public class ITimeZoneService : ICommandObject
    {
        public Dictionary<ulong, ServiceCall> Calls { get; set; }

        public ITimeZoneService()
        {
            Calls = new Dictionary<ulong, ServiceCall>()
            {

            };
        }
    }
}
