using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Service.LM
{
    public class Logger : ICommandObject
    {
        public Dictionary<ulong, ServiceCall> Calls { get; set; } = new Dictionary<ulong, ServiceCall>()
        {

        };

        public Logger()
        {

        }
    }
}
