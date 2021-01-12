using SkylerCommon.Debugging;
using SkylerCommon.Globals;
using SkylerCommon.Memory;
using SkylerHLE.Horizon.IPC;
using SkylerHLE.Horizon.Service.Sessions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Service.VI
{
    public class IApplicationDisplayService : ICommandObject
    {
        public Dictionary<ulong, ServiceCall> Calls { get; set; } = new Dictionary<ulong, ServiceCall>()
        {
            {100,   GetRelayService},
            {101,   GetSystemDisplayService},
            {102,   GetManagerDisplayService},
            {1010,  OpenDisplay },
            {2101,  SetLayerScalingMode },
            {2020,  OpenLayer},
        };


        //I feel like these should all be in ServiceGenerator
        public static ulong GetRelayService(CallContext context)
        {
            Helper.Make(context,new IHOSBinderDriver());

            return 0;
        }

        public static ulong GetSystemDisplayService(CallContext context)
        {
            Helper.Make(context, new ISystemDisplayService());

            return 0;
        }

        public static ulong GetManagerDisplayService(CallContext context)
        {
            Helper.Make(context,new IManagerDisplayService());

            return 0;
        }

        public static ulong OpenDisplay(CallContext context)
        {
            DisplayContext Display = new DisplayContext(context.Reader.ReadString());

            context.Writer.Write(Display.ID);

            return 0;
        }

        public static ulong SetLayerScalingMode(CallContext context)
        {
            //TODO:

            return 0;
        }

        public static ulong OpenLayer(CallContext context)
        {
            ulong LayerID = context.Reader.ReadStruct<ulong>();
            ulong UserId = context.Reader.ReadStruct<ulong>();

            ulong ParcelPtr = context.request.ReceiveDescriptors[0].Address;

            byte[] Parcel = MakeGraphicsBufferProcedure(ParcelPtr);

            MemoryWriter writer = GlobalMemory.GetWriter(ParcelPtr);
            writer.WriteStruct(Parcel);

            context.Writer.Write((ulong)Parcel.Length);

            return 0;
        }

        public static byte[] MakeGraphicsBufferProcedure(ulong BasePointer)
        {
            ulong Id = 0x20;
            ulong CookiePtr = 0L;

            using (MemoryStream ms = new MemoryStream())
            {
                BinaryWriter Writer = new BinaryWriter(ms);

                Writer.Write(2);
                Writer.Write(0);
                Writer.Write((int)(Id >> 0));
                Writer.Write((int)(Id >> 32));
                Writer.Write((int)(CookiePtr >> 0));
                Writer.Write((int)(CookiePtr >> 32));
                Writer.Write((byte)'d');
                Writer.Write((byte)'i');
                Writer.Write((byte)'s');
                Writer.Write((byte)'p');
                Writer.Write((byte)'d');
                Writer.Write((byte)'r');
                Writer.Write((byte)'v');
                Writer.Write((byte)'\0');
                Writer.Write(0L);

                return MakeParcel(ms.ToArray(), new byte[] { 0, 0, 0, 0 });
            }
        }

        public static byte[] MakeParcel(byte[] Data, byte[] Objs)
        {
            if (Data == null)
            {
                throw new ArgumentNullException(nameof(Data));
            }

            if (Objs == null)
            {
                throw new ArgumentNullException(nameof(Objs));
            }

            using (MemoryStream MS = new MemoryStream())
            {
                BinaryWriter Writer = new BinaryWriter(MS);

                Writer.Write(Data.Length);
                Writer.Write(0x10);
                Writer.Write(Objs.Length);
                Writer.Write(Data.Length + 0x10);

                Writer.Write(Data);
                Writer.Write(Objs);

                return MS.ToArray();
            }
        }
    }
}
