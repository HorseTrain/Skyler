using SkylerCommon.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Service.Sessions
{
    public class KDomain : KSession
    {
        public ObjectCollection Objects     { get; set; }

        public KDomain(KSession session) : base(session,true) //Maybe make explicit "Add to handles ?"
        {
            Objects = new ObjectCollection();
        }

        public object GetObject(ulong id) => Objects.GetObject(id);
        public uint CreateObject(object obj) => (uint)Objects.AddObject(obj);
    }
}
