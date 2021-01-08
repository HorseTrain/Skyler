using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Service
{
    public interface ICommandObject //I really need to get better at naming things.
    {
        public Dictionary<ulong, ServiceCall> Calls { get; set; }
    }
}
