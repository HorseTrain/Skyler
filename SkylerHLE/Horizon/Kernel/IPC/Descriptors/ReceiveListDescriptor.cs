﻿using SkylerCommon.Memory;
using SkylerHLE.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkylerHLE.Horizon.IPC.Descriptors
{
    public class ReceiveListDescriptor
    {
        public ulong Address { get; set; }
        public ulong Size { get; set; }

        public bool IsNotZero => Address != 0 && Size != 0;

        public ReceiveListDescriptor(MemoryReader reader)
        {
            ulong double0 = reader.ReadStruct<ulong>();

            Address = double0 & 0xffffffffffff;

            Size = (ushort)(double0 >> 48);
        }
    }
}
