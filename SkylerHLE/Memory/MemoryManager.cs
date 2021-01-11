using SkylerCommon.Debugging;
using SkylerCommon.Globals;
using SkylerCommon.Memory;
using SkylerCommon.Utilities.Tools;
using System;

namespace SkylerHLE.Memory
{
    public unsafe class MemoryManager
    {
        public static void* BaseAddress             { get; set; }
        public MemoryAllocator allocator            { get; set; }

        MemoryBlock Ram                             { get; set; }

        public PageTable[] PageTables               { get; set; }

        const int PagePartitionBit = 10;

        public const ulong PagesInMemory = GlobalMemory.RamSize / MemoryMetaData.PageSize;
        public const ulong PageTableCount = PagesInMemory >> PagePartitionBit;
        public const ulong PageTableLength = PagesInMemory / PageTableCount;

        public void MapMemory(ulong Address, ulong Size, MemoryPermission Permission,MemoryType Type)
        {
            if (false)
            {
                if (Address != MemoryMetaData.PageRoundDown(Address))
                    Debug.LogWarning("Invalid Page Size, Automatically Fixing.");

                if (Size != MemoryMetaData.PageRoundUp(Size))
                    Debug.LogWarning("Invalid Page Size, Automatically Fixing.");
            }

            Address = MemoryMetaData.PageRoundDown(Address);
            Size = MemoryMetaData.PageRoundUp(Size);
            ulong Top = Address + Size;

            for (ulong i = Address; i < Top; i += MemoryMetaData.PageSize)
            {
                RequestPage(i).permission = Permission;
                RequestPage(i).memorytype = Type;
                RequestPage(i).mapped = true;
            }

            //Debug.Log($"Mapped Memory: {StringTools.FillStringBack(Permission,' ',20)} {StringTools.FillStringBack(Type, ' ', 15)},{StringTools.FillStringBack(Address, ' ', 15)}, With Size: {StringTools.FillStringBack(Size, ' ', 20)}");
        }

        public MemoryMapInfo GetMemoryInfo(ulong Address)
        {
            if (!IsValidPosition(Address))
                return null;

            Address = MemoryMetaData.PageRoundDown(Address);

            PageEntry CheckRef = RequestPage(Address);

            ulong Bottom = Address;
            ulong Top = Address;

            while (true)
            {
                if (Bottom <= MemoryMetaData.PageSize || !PageEntry.Compare(CheckRef, RequestPage(Bottom - MemoryMetaData.PageSize)))
                {
                    break;
                }

                Bottom -= MemoryMetaData.PageSize;
            }

            while (true)
            {
                Top += MemoryMetaData.PageSize;

                if (Top >= MemoryMetaData.RamSize)
                {
                    break;
                }

                if (!PageEntry.Compare(CheckRef,RequestPage(Top)))
                {
                    break;
                }    
            }

            return new MemoryMapInfo(Bottom,Top - Bottom,CheckRef.Attr,CheckRef.permission,CheckRef.memorytype);
        }

        public static bool IsValidPosition(ulong Position) => Position >> 32 == 0;

        public MemoryManager()
        {
            Switch.Memory = this;

            allocator = new MemoryAllocator();

            AllocateSwitchBlock();

            InitSwitchOperationMemory();
        }

        public (ulong,ulong) RequestPageWithIndex(ulong Index) => (Index >> PagePartitionBit, Index & ((1 << PagePartitionBit) - 1));

        public ref PageEntry RequestPage(ulong Address) => ref RequestPage(RequestPageWithIndex(Address >> MemoryMetaData.PageBit));

        public ref PageEntry RequestPage((ulong,ulong) Pointer)
        {
            lock (PageTables)
            {
                if (PageTables[Pointer.Item1] == null)
                {
                    PageTables[Pointer.Item1] = new PageTable(Pointer.Item1);
                }

                return ref PageTables[Pointer.Item1].Entries[Pointer.Item2];
            }
        }

        void AllocateSwitchBlock()
        {
            Ram = new MemoryBlock(GlobalMemory.RamSize);
            BaseAddress = Ram;

            GlobalMemory.BaseMemory = (IntPtr)BaseAddress;

            GlobalMemory.SetBaseAddress(BaseAddress);
        }

        //Maybe not the best place.
        public void InitSwitchOperationMemory()
        {
            PageTables = new PageTable[PageTableCount];

            //Map tls space
            MapMemory(MemoryMetaData.TlsCollectionAddress,MemoryMetaData.TlsSize,MemoryPermission.ReadAndWrite,MemoryType.ThreadLocal);

            //map stack space
            MapMemory(MemoryMetaData.MainStackAddress, MemoryMetaData.MainStackSize, MemoryPermission.ReadAndWrite, MemoryType.Normal);
        }
    }
}
