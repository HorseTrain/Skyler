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
            {("/dev/nvmap",         270), MapIocGetID},

            {("/dev/nvhost-ctrl-gpu", 18181), NVGPU_GPU_IOCTL_GET_CHARACTERISTICS},
            {("/dev/nvhost-ctrl-gpu", 18182), NVGPU_GPU_IOCTL_GET_TPC_MASKS},
            {("/dev/nvhost-ctrl-gpu", 18177), NVGPU_GPU_IOCTL_ZBC_GET_ACTIVE_SLOT_MASK},
            {("/dev/nvhost-ctrl-gpu", 18178), NVGPU_GPU_IOCTL_ZCULL_GET_INFO},

            {("/dev/nvhost-as-gpu",0x4102), Helper.Stubbed},
            {("/dev/nvhost-as-gpu",0x4109), Helper.Stubbed},
            {("/dev/nvhost-as-gpu",0x4108), Helper.Stubbed},
        };

        public static ulong DrvIoctl(CallContext context)
        {
            uint fd = context.Reader.ReadStruct<uint>();
            uint cmd = context.Reader.ReadStruct<uint>() & 0xffff;

            FileDescriptor descriptor = (FileDescriptor)Switch.MainOS.Handles.GetObject(fd);

            ulong position = context.request.PointerDescriptors[0].Address;

            context.Writer.Write(0);

            Console.WriteLine(cmd.ToString("X"));

            return IoctlCommands[(descriptor.Name, cmd)](context);
        }

        public static ulong MapIocCreate(CallContext context)
        {
            ulong Position = context.request.GetSendBuffPtr();

            context.Reader.Seek(Position);
            uint Size = context.Reader.ReadStruct<uint>();

            GlobalMemory.GetWriter(Position + 4).WriteStruct(new NvMap(Size).ID);

            return 0;
        }

        public static ulong MapIocFromID(CallContext context)
        {
            ulong Position = context.request.GetSendBuffPtr();

            uint ID = GlobalMemory.GetReader().ReadStructAtOffset<uint>(Position);

            GlobalMemory.GetWriter().WriteStruct<uint>(Position + 4, ((NvMap)Switch.MainOS.Handles[ID]).ID); //Might be redudant?



            return 0;
        }

        public static ulong MapIocAlloc(CallContext context)
        {
            ulong Position = context.request.GetSendBuffPtr();

            MemoryReader Reader = GlobalMemory.GetReader(Position);

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

            MemoryReader Reader = GlobalMemory.GetReader(Position);

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

            GlobalMemory.GetWriter().WriteStruct(Position + 8, Response);

            return 0;
        }

        public static ulong MapIocGetID(CallContext context)
        {
            ulong Position = context.request.GetSendBuffPtr();

            MemoryReader Reader = GlobalMemory.GetReader();

            Reader.Seek(Position + 4);

            uint Handle =   Reader.ReadStruct<uint>();

            NvMap map = (NvMap)Switch.MainOS.Handles[Handle];

            GlobalMemory.GetWriter().WriteStruct(Position,Handle);

            return 0;
        }

        public static ulong NVGPU_GPU_IOCTL_GET_CHARACTERISTICS(CallContext context)
        {
            ulong InputPosition = context.request.GetBufferType0x21().Position;
            ulong OutputPosition = context.request.GetBufferType0x22().Position;

            gpu_characteristics args = GlobalMemory.GetReader(InputPosition).ReadStruct<gpu_characteristics>();

            args.BufferSize = 0xa0;
            
            args.Arch = 0x120;
            args.Impl = 0xb;
            args.Rev = 0xa1;
            args.NumGpc = 0x1;
            args.L2CacheSize = 0x40000;
            args.OnBoardVideoMemorySize = 0x0;
            args.NumTpcPerGpc = 0x2;
            args.BusType = 0x20;
            args.BigPageSize = 0x20000;
            args.CompressionPageSize = 0x20000;
            args.PdeCoverageBitCount = 0x1b;
            args.AvailableBigPageSizes = 0x30000;
            args.GpcMask = 0x1;
            args.SmArchSmVersion = 0x503;
            args.SmArchSpaVersion = 0x503;
            args.SmArchWarpCount = 0x80;
            args.GpuVaBitCount = 0x28;
            args.Reserved = 0x0;
            args.Flags = 0x55;
            args.TwodClass = 0x902d;
            args.ThreedClass = 0xb197;
            args.ComputeClass = 0xb1c0;
            args.GpfifoClass = 0xb06f;
            args.InlineToMemoryClass = 0xa140;
            args.DmaCopyClass = 0xb0b5;
            args.MaxFbpsCount = 0x1;
            args.FbpEnMask = 0x0;
            args.MaxLtcPerFbp = 0x2;
            args.MaxLtsPerLtc = 0x1;
            args.MaxTexPerTpc = 0x0;
            args.MaxGpcCount = 0x1;
            args.RopL2EnMask0 = 0x21d70;
            args.RopL2EnMask1 = 0x0;
            args.ChipName = 0x6230326d67;
            args.GrCompbitStoreBaseHw = 0x0;

            GlobalMemory.GetWriter(OutputPosition).WriteStruct(args) ;

            return 0;
        }

        public static ulong NVGPU_GPU_IOCTL_GET_TPC_MASKS(CallContext context)
        {
            ulong InputPosition = context.request.GetBufferType0x21().Position;
            ulong OutputPosition = context.request.GetBufferType0x22().Position;

            gpu_get_tcp_mask args = GlobalMemory.GetReader(InputPosition).ReadStruct<gpu_get_tcp_mask>();

            if (args.MaskBufferAddress != 0)
            {
                args.TpcMask = 3;
            }

            GlobalMemory.GetWriter(OutputPosition).WriteStruct(args);

            return 0;
        }

        public static ulong NVGPU_GPU_IOCTL_ZBC_GET_ACTIVE_SLOT_MASK(CallContext context)
        {
            ulong OutPosition = context.request.GetBufferType0x22().Position;

            gpu_get_active_slot_mask args = new gpu_get_active_slot_mask();

            args.Slot = 0x07;
            args.Mask = 0x01;

            GlobalMemory.GetWriter(OutPosition).WriteStruct(args);

            context.PrintStubbed();

            return 0;
        }

        public static ulong NVGPU_GPU_IOCTL_ZCULL_GET_INFO(CallContext context)
        {
            ulong OutputPosition = context.request.GetBufferType0x22().Position;

            gpu_z_cull_get_info args = new gpu_z_cull_get_info();

            args.WidthAlignPixels = 0x20;
            args.HeightAlignPixels = 0x20;
            args.PixelSquaresByAliquots = 0x400;
            args.AliquotTotal = 0x800;
            args.RegionByteMultiplier = 0x20;
            args.RegionHeaderSize = 0x20;
            args.SubregionHeaderSize = 0xc0;
            args.SubregionWidthAlignPixels = 0x20;
            args.SubregionHeightAlignPixels = 0x40;
            args.SubregionCount = 0x10;

            GlobalMemory.GetWriter(OutputPosition).WriteStruct(args);

            context.PrintStubbed();

            return 0;
        }
    }
}
