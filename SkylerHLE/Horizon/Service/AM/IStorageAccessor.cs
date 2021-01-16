using SkylerCommon.Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Service.AM
{
    public class IStorageAccessor : ICommandObject
    {
        public Dictionary<ulong, ServiceCall> Calls { get; set; }

        IStorage storage;

        public IStorageAccessor(IStorage storage)
        {
            Calls = new Dictionary<ulong, ServiceCall>()
            {
                {0, GetSize },
                {10,Write },
                {11,Read }
            };

            this.storage = storage;
        }

        public ulong GetSize(CallContext context)
        {
            context.Writer.Write((long)storage.Buffer.Length);

            return 0;
        }

        public ulong Read(CallContext context)
        {
            ulong ReadPosition = context.Reader.ReadStruct<ulong>();

            (ulong position, ulong size) = context.request.GetBufferType0x22();

            byte[] Data;

            if (storage.Buffer.Length > (long)size)
            {
                Data = new byte[size];

                Buffer.BlockCopy(storage.Buffer,0,Data,0,(int)size);
            }
            else
            {
                Data = storage.Buffer;
            }

            GlobalMemory.GetWriter(position).WriteStruct(Data);

            return 0;
        }

        public ulong Write(CallContext context)
        {
            context.PrintStubbed();

            return 0;
        }
    }
}
