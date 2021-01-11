using SkylerHLE.Horizon.Service.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Execution
{
    public class MutexLock
    {
        public ulong Address    { get; set; }
        public KThread Owner    { get; set; }
        public KThread Wait     { get; set; }
        public KEvent Halter    { get; set; }

        public MutexLock(ulong Address,KThread Owner,KThread Wait)
        {
            this.Owner = Owner;
            this.Wait = Wait;
            this.Address = Address;

            Halter = new KEvent();
        }
    }
}
