using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Service.Sessions
{
    public class KEvent
    {
        public ulong ID                     { get; set; }

        public ManualResetEvent WaitEvent   { get; set; }

        public KEvent()
        {
            ID = Switch.MainOS.Handles.AddObject(this);

            WaitEvent = new ManualResetEvent(false);
        }

        public void Send() => WaitEvent.Set();
    }
}
