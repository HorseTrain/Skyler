using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Execution
{
    public class KeyLock
    {
        public ulong KeyAddress         { get; set; }
        public ulong TagAddress         { get; set; }
        public int Tag                  { get; set; }
        public ulong TimeOut            { get; set; }
        public KThread GuestThread      { get; set; }
        public ManualResetEvent Event   { get; set; } //Pause threads with events.
    }
}
