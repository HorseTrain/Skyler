using SkylerHLE.Horizon.IPC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Kernel.IPC.Handlers
{
    public static class ResponseHandler
    {
        private const long SfciMagic = 'S' << 0 | 'F' << 8 | 'C' << 16 | 'I' << 24;
        private const long SfcoMagic = 'S' << 0 | 'F' << 8 | 'C' << 16 | 'O' << 24;

        public static IPCCommand FillResponse(IPCCommand Response, ulong Result, byte[] Data = null)
        {
            Response.Type = CommandType.Response;

            using (MemoryStream MS = new MemoryStream())
            {
                BinaryWriter Writer = new BinaryWriter(MS);

                Writer.Write(SfcoMagic);
                Writer.Write(Result);

                if (Data != null)
                {
                    Writer.Write(Data);
                }

                Response.RawData = MS.ToArray();
            }

            return Response;
        }

        public static IPCCommand FillResponse(IPCCommand Response, ulong Result, params int[] Values)
        {
            using (MemoryStream MS = new MemoryStream())
            {
                BinaryWriter Writer = new BinaryWriter(MS);

                foreach (int Value in Values)
                {
                    Writer.Write(Value);
                }

                return FillResponse(Response, Result, MS.ToArray());
            }
        }
    }
}
