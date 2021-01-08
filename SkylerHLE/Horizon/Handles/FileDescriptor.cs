using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Handles
{
    public class FileDescriptor
    {
        public ulong Handle     { get; set; }
        public string Name      { get; set; }

        public FileDescriptor(string Name)
        {
            this.Name = Name;

            Handle = Switch.MainOS.Handles.AddObject(this);
        }
    }
}
