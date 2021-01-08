using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Service.TIME
{
    public class ISystemClock : ICommandObject
    {
        public Dictionary<ulong, ServiceCall> Calls { get; set; }

        ulong Type;
        DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public ISystemClock(ulong Type)
        {
            Calls = new Dictionary<ulong, ServiceCall>()
            {
                {0,GetCurrentTime }
            };

            this.Type = Type;
        }

        public ulong GetCurrentTime(CallContext context)
        {
            context.Writer.Write(0UL);

            DateTime CurrentTime = DateTime.Now;

            if (Type == 0 || Type == 1)
            {
                CurrentTime = CurrentTime.ToUniversalTime();
            }

            context.Writer.Write((long)(DateTime.Now - Epoch).TotalSeconds);

            return 0;
        }
    }
}
