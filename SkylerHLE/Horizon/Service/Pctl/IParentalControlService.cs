using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Service.Pctl
{
    public class IParentalControlService : ICommandObject
    {
        public Dictionary<ulong, ServiceCall> Calls { get; set; }

        public IParentalControlService()
        {
            Calls = new Dictionary<ulong, ServiceCall>()
            {

            };
        }
    }
}
