using SkylerCommon.Globals;
using SkylerCommon.Memory;
using SkylerHLE.Horizon.Handles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Service.NV
{
    public static class Ioctl
    {
        public static Dictionary<(string, ulong), ServiceCall> IoctlCommands = new Dictionary<(string, ulong), ServiceCall>()
        {
            {("/dev/nvmap",         257), MapIocCreate},
            {("/dev/nvmap",         259), MapIocFromID},
            {("/dev/nvmap",         260), MapIocAlloc },
            {("/dev/nvmap",         265), MapIocParam },
            {("/dev/nvmap",         270), MapIocGetID}

        };

        public static ulong DrvIoctl(CallContext context)
        {
            uint fd = context.Reader.ReadStruct<uint>();
            uint cmd = context.Reader.ReadStruct<uint>() & 0xffff;

            FileDescriptor descriptor = (FileDescriptor)Switch.MainOS.Handles.GetObject(fd);

            ulong position = context.request.PointerDescriptors[0].Address;

            context.Writer.Write(0);

            return IoctlCommands[(descriptor.Name, cmd)](context);
        }

        public static ulong MapIocCreate(CallContext context)
        {
            ulong Position = context.request.GetSendBuffPtr();

            context.Reader.Seek(Position);
            uint Size = context.Reader.ReadStruct<uint>();

            GlobalMemory.RamWriter.Seek(Position + 4);
            GlobalMemory.RamWriter.WriteStruct(new NvMap(Size).ID);

            return 0;
        }

        public static ulong MapIocFromID(CallContext context)
        {
            ulong Position = context.request.GetSendBuffPtr();

            uint ID = GlobalMemory.RamReader.ReadStructAtOffset<uint>(Position);

            GlobalMemory.RamWriter.WriteStruct<uint>(Position + 4, ((NvMap)Switch.MainOS.Handles[ID]).ID); //Might be redudant?



            return 0;
        }

        public static ulong MapIocAlloc(CallContext context)
        {
            ulong Position = context.request.GetSendBuffPtr();

            GlobalMemory.RamReader.Seek(Position);

            MemoryReader Reader = GlobalMemory.RamReader;

            uint Handle = Reader.ReadStruct<uint>();
            uint HeapMask = Reader.ReadStruct<uint>();
            uint Flags = Reader.ReadStruct<uint>();
            uint Align = Reader.ReadStruct<uint>();
            byte Kind = (byte)Reader.ReadStruct<ulong>();
            ulong Addr = Reader.ReadStruct<ulong>();

            NvMap map = (NvMap)Switch.MainOS.Handles.GetObject(Handle);

            map.Address = Addr;
            map.Align = Align;
            map.Kind = Kind;

            return 0;
        }

        public static ulong MapIocParam(CallContext context)
        {
            ulong Position = context.request.GetSendBuffPtr();

            GlobalMemory.RamReader.Seek(Position);

            MemoryReader Reader = GlobalMemory.RamReader;

            uint Handle = Reader.ReadStruct<uint>();
            uint Peram = Reader.ReadStruct<uint>();

            NvMap map = (NvMap)Switch.MainOS.Handles[Handle];

            uint Response = 0;

            switch (Peram)
            {
                case 1: Response = map.Size; break;
                case 2: Response = map.Align; break;
                case 4: Response = 0x40000000; break;
                case 5: Response = map.Kind; break;
            }

            GlobalMemory.RamWriter.WriteStruct(Position + 8, Response);

            return 0;
        }

        public static ulong MapIocGetID(CallContext context)
        {
            ulong Position = context.request.GetSendBuffPtr();

            MemoryReader Reader = GlobalMemory.RamReader;

            Reader.Seek(Position + 4);

            uint Handle =   Reader.ReadStruct<uint>();

            NvMap map = (NvMap)Switch.MainOS.Handles[Handle];

            GlobalMemory.RamWriter.WriteStruct(Position,Handle);

            return 0;
        }
    }
}
