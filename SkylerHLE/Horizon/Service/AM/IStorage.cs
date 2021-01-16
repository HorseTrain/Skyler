using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Service.AM
{
    //I wish this had a better name.
    public class IStorage : ICommandObject
    {
        public Dictionary<ulong, ServiceCall> Calls { get; set; }

        public byte[] Buffer;

        public IStorage(byte[] Buffer)
        {
            Calls = new Dictionary<ulong, ServiceCall>()
            {
                {0, Open }
            };

            this.Buffer = Buffer;
        }

        public ulong Open(CallContext context)
        {
            Helper.Make(context,new IStorageAccessor(this));

            return 0;
        }
    }
}
