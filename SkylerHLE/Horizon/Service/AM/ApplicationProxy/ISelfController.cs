using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Service.AM.ApplicationProxy
{
    public class ISelfController : ICommandObject
    {
        public Dictionary<ulong, ServiceCall> Calls { get; set; } 

        public ISelfController() 
        {
            Calls = new Dictionary<ulong, ServiceCall>()
            {
                { 1,  Helper.Stubbed },
                { 9,  Helper.Stubbed },
                { 10, Helper.Stubbed },
                { 11, Helper.Stubbed },
                { 12, Helper.Stubbed },
                { 13, Helper.Stubbed },
                { 14, Helper.Stubbed },
                { 16, Helper.Stubbed },
                { 50, Helper.Stubbed }
            };
        }
    }
}
