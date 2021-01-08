using SkylerCommon.Debugging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Service.Sessions
{
    public class KObject : KSession
    {
        public object Obj { get; set; }

        public KObject(KSession session, object obj) : base(session,true)
        {
            Obj = obj;
        }
    }
}
