using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SkylerHLE.Switch;

namespace SkylerHLE.Horizon.Service.Sessions
{
    public class KSession
    {
        public string Name      { get; set; }
        public ulong ID         { get; set; }

        public bool IsOpen      { get; set; }

        public KSession(string Name, bool AddToHandles = false)
        {
            this.Name = Name;

            if (AddToHandles)
            {
                ID = MainOS.Handles.AddObject(this);
            }
        }

        public KSession(KSession session, bool AddToHandles = false) : this(session.Name,AddToHandles)
        {
            Name = session.Name;
            IsOpen = session.IsOpen;

            if (AddToHandles)
            {
                ID = MainOS.Handles.AddObject(this);
            }
        }

        public void Close() => IsOpen = false;

        public void Open() => IsOpen = true;
    }
}
