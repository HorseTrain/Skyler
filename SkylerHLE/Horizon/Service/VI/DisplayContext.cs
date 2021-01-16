using SkylerHLE.Horizon.Service.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Service.VI
{
    public class DisplayContext : KSession
    {
        public DisplayContext(string Name) : base (Name,true)
        {

        }
    }
}
