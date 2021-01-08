﻿using SkylerCommon.Memory;
using SkylerHLE.Memory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SkylerHLE.Horizon.IPC
{
    public class HandleDescriptor
    {
        public bool     SendCurrentPID      { get; set; }
        public uint[]   ToCopy              { get; set; }
        public uint[]   ToMove              { get; set; }
        public ulong    PID                 { get; set; }

        public HandleDescriptor(MemoryReader reader)
        {
            uint word0 = reader.ReadStruct<uint>();

            SendCurrentPID = (word0 & 1) != 0;

            ToCopy = new uint[(word0 >> 1) & 0xf];
            ToMove = new uint[(word0 >> 5) & 0xf];

            PID = SendCurrentPID ? reader.ReadStruct<ulong>() : 0;

            for (int Index = 0; Index < ToCopy.Length; Index++)
            {
                ToCopy[Index] = reader.ReadStruct<uint>();
            }

            for (int Index = 0; Index < ToMove.Length; Index++)
            {
                ToMove[Index] = reader.ReadStruct<uint>();
            }
        }

        public HandleDescriptor(uint[] Copy, uint[] Move)
        {
            ToCopy = Copy ?? throw new ArgumentNullException(nameof(Copy));
            ToMove = Move ?? throw new ArgumentNullException(nameof(Move));
        }

        public HandleDescriptor(uint[] Copy, uint[] Move, ulong PId) : this(Copy, Move)
        {
            this.PID = PId;

            SendCurrentPID = true;
        }

        public static HandleDescriptor MakeCopy(uint Handle) => new HandleDescriptor(
                new uint[] { Handle },
                new uint[0]);

        public static HandleDescriptor MakeMove(uint Handle) => new HandleDescriptor(
                new uint[0],
                new uint[] { Handle });

        public byte[] GenerateResponse()
        {
            using (MemoryStream MS = new MemoryStream())
            {
                BinaryWriter Writer = new BinaryWriter(MS);

                int Word = SendCurrentPID ? 1 : 0;

                Word |= (ToCopy.Length & 0xf) << 1;
                Word |= (ToMove.Length & 0xf) << 5;

                Writer.Write(Word);

                if (SendCurrentPID)
                {
                    Writer.Write((long)PID);
                }

                foreach (int Handle in ToCopy)
                {
                    Writer.Write(Handle);
                }

                foreach (int Handle in ToMove)
                {
                    Writer.Write(Handle);
                }

                return MS.ToArray();
            }
        }
    }
}
