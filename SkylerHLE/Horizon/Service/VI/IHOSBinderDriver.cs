using SkylerCommon.Debugging;
using SkylerCommon.Globals;
using SkylerCommon.Memory;
using SkylerGraphics.ContextHandler;
using SkylerHLE.Horizon.Service.NV;
using SkylerHLE.Horizon.Service.Sessions;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Service.VI
{
    public class IHOSBinderDriver : ICommandObject
    {
        public Dictionary<ulong, ServiceCall> Calls { get; set; }

        public Dictionary<(string, ulong), ParcelService> Methods;

        public IHOSBinderDriver()
        {
            Calls = new Dictionary<ulong, ServiceCall>()
            {
                {0, TransactParcel },
                {1, AdjustRefcount}
            }; 

            Methods = new Dictionary<(string, ulong), ParcelService>()
            {
                { ("android.gui.IGraphicBufferProducer", 0x1), GraphicBufferProducerRequestBuffer  },
                { ("android.gui.IGraphicBufferProducer", 0x7), GraphicBufferProducerQueueBuffer  },
                { ("android.gui.IGraphicBufferProducer", 0xa), GraphicBufferProducerConnect        },
                { ("android.gui.IGraphicBufferProducer", 0x3), GraphicBufferProducerDequeueBuffer  },
                { ("android.gui.IGraphicBufferProducer", 0xe), GraphicBufferPreallocateBuffer      },
            };
        }

        public ulong TransactParcel(CallContext context)
        {
            uint id = context.Reader.ReadStruct<uint>();
            uint code = context.Reader.ReadStruct<uint>();

            ulong DataPosition = context.request.SendDescriptors[0].Address;
            ulong DataSize = context.request.SendDescriptors[0].Size;

            MemoryReader reader = GlobalMemory.GetReader(DataPosition);

            byte[] Data = reader.ReadStruct<byte>(DataSize);

            Data = Parcel.GetParcelData(Data);

            using (MemoryStream MS = new MemoryStream(Data))
            {
                System.IO.BinaryReader Reader = new System.IO.BinaryReader(MS);

                MS.Seek(4, SeekOrigin.Current);

                int StrSize = Reader.ReadInt32();

                string InterfaceName = Encoding.Unicode.GetString(Data, 8, StrSize * 2);

                if (Methods.ContainsKey((InterfaceName, code)))
                {
                    return Methods[(InterfaceName, code)](context, Data);
                }
                else
                {
                    Debug.LogError($"{InterfaceName},{code.ToString("X")} Unknown Method.");

                    return 0;
                }
            }

            return 0;
        }

        public ulong AdjustRefcount(CallContext context)
        {
            //TODO:

            return 0;
        }

        public static byte[] Gbfr { get; set; }

        public static ulong GraphicBufferProducerRequestBuffer(CallContext Context, byte[] ParcelData)
        {
            int GbfrSize = Gbfr?.Length ?? 0;

            byte[] Data = new byte[GbfrSize + 4];

            if (Gbfr != null)
            {
                Buffer.BlockCopy(Gbfr, 0, Data, 0, GbfrSize);
            }

            return MakeReplyParcel(Context, Data);
        }

        public static ulong GraphicBufferProducerQueueBuffer(CallContext Context, byte[] ParcelData)
        {
            return MakeReplyParcel(Context, 1280, 720, 0, 0, 0);
        }

        public static ulong GraphicBufferProducerConnect(CallContext Context, byte[] ParcelData)
        {
            return MakeReplyParcel(Context, 1280, 720, 0, 0, 0);
        }

        private class BufferObj 
        {
            public BufferObj() 
            {
                Switch.MainOS.Handles.AddObject(this);
            }
        }

        public static ulong GraphicBufferProducerDequeueBuffer(CallContext Context, byte[] ParcelData)
        {
            new BufferObj();

            return MakeReplyParcel(Context, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        }

        public static ulong GraphicBufferPreallocateBuffer(CallContext Context, byte[] ParcelData)
        {
            uint GbfrSize = (uint)ParcelData.Length - 0x54;

            Gbfr = new byte[GbfrSize];

            Buffer.BlockCopy(ParcelData, 0x54, Gbfr, 0, (int)GbfrSize);

            using (MemoryStream MS = new MemoryStream(ParcelData))
            {
                System.IO. BinaryReader Reader = new System.IO.BinaryReader(MS);

                MS.Seek(0xd4, SeekOrigin.Begin);

                int Handle = Reader.ReadInt32();

                NvMap map = (NvMap)Switch.MainOS.Handles.GetObject((uint)Handle);

                //Switch.MainSwitch.gpu.Renderer.FrameBufferLocation = map.Address;
                FrameBuffers.MainFrameBuffer = map.Address;
            }

            return MakeReplyParcel(Context, 0);
        }

        public static ulong MakeReplyParcel(CallContext Context, byte[] Data)
        {
            ulong ReplyPos = Context.request.ReceiveDescriptors[0].Address;

            byte[] Reply = Parcel.MakeParcel(Data, new byte[0]);

            GlobalMemory.GetWriter().WriteStruct(ReplyPos,Reply);

            return 0;
        }

        public static ulong MakeReplyParcel(CallContext Context, params int[] Ints)
        {
            using (MemoryStream MS = new MemoryStream())
            {
                BinaryWriter Writer = new BinaryWriter(MS);

                foreach (int Int in Ints)
                {
                    Writer.Write(Int);
                }

                return MakeReplyParcel(Context, MS.ToArray());
            }
        }
    }
}
