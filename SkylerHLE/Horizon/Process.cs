using SkylerCommon.Debugging;
using SkylerCommon.Globals;
using SkylerCommon.Memory;
using SkylerCPU;
using SkylerHLE.Horizon.Execution;
using SkylerHLE.Horizon.Loaders;
using SkylerHLE.Memory;
using System;
using System.Collections.Generic;
using System.IO;
using static SkylerHLE.Switch;

namespace SkylerHLE.Horizon
{
    public class Process 
    {
        public ulong ProcessID          { get; set; }

        public bool InMemory            { get; set; } 

        List<IExecutable> Executables   { get; set; }

        public ulong ImageBase          { get; set; }
        public ulong ProgramStart       { get; set; }

        internal Process()
        {
            ProcessID = MainOS.Handles.AddObject(this);

            Executables = new List<IExecutable>();

            ImageBase = MemoryMetaData.AddressSpaceBegin;

            ProgramStart = ImageBase;
        }

        public void AddExecutable(IExecutable Executable) => Executables.Add(Executable);

        public void UploadToMemory(IExecutable executable)
        {
            InMemory = true;

            MemoryWriter writer = GlobalMemory.GetWriter();

            writer.WriteStruct(ImageBase + executable.Text.Offset, executable.Text.Data);
            writer.WriteStruct(ImageBase + executable.RoData.Offset, executable.RoData.Data);
            writer.WriteStruct(ImageBase + executable.Data.Offset, executable.Data.Data);

            Switch.Memory.MapMemory(ImageBase + executable.Text.Offset, (ulong)executable.Text.Length, MemoryPermission.ReadAndExecute,MemoryType.CodeStatic);
            Switch.Memory.MapMemory(ImageBase + executable.RoData.Offset, (ulong)executable.RoData.Length, MemoryPermission.Read,MemoryType.CodeMutable);
            Switch.Memory.MapMemory(ImageBase + executable.Data.Offset, (ulong)executable.Data.Length, MemoryPermission.ReadAndWrite,MemoryType.CodeMutable);

            if (executable.Mod0Offset == 0)
            {
                ulong BssOffset = executable.Data.Offset + executable.Data.Length;
                ulong BssSize = executable.BssSize;

                Switch.Memory.MapMemory(ImageBase + BssOffset,BssSize, MemoryPermission.ReadAndWrite, MemoryType.Normal);

                ImageBase = ImageBase + BssOffset + BssSize;

                return;
            }

            //<-- https://switchbrew.org/wiki/NSO#MOD --> 860
            MemoryReader reader = GlobalMemory.GetReader();

            ulong Mod0Offset = ImageBase + executable.Mod0Offset;

            ulong DynamicOffset = Mod0Offset + reader.ReadStructAtOffset<uint>(Mod0Offset + 4);
            ulong BssStartOffset = Mod0Offset + reader.ReadStructAtOffset<uint>(Mod0Offset + 8);
            ulong BssEnd = Mod0Offset + reader.ReadStructAtOffset<uint>(Mod0Offset + 12);

            Switch.Memory.MapMemory(BssStartOffset,BssEnd - BssStartOffset,MemoryPermission.ReadAndWrite,MemoryType.Normal);

            ImageBase = MemoryMetaData.PageRoundUp(BssEnd);
        }

        public IExecutable LoadExecutable(string path, bool ForceNSO = false)
        {
            IExecutable Out;

            byte[] Source = File.ReadAllBytes(path);

            if (path.EndsWith(".nro"))
            {
                Out = new NroExecutable(Source);
            }
            else if (path.EndsWith(".nso") || ForceNSO)
            {
                Out = new NsoExecutable(Source);
            }
            else
            {
                Debug.LogError($"Unkown Rom At {path}");

                return null;
            }

            AddExecutable(Out);

            UploadToMemory(Out);

            return Out;
        }

        public void LoadGame(string path)
        {
            string[] Files = Directory.GetFiles(path);

            if (!path.EndsWith("\\"))
                path += "\\";

            List<string> Skip = new List<string>();

            void LoadNSO(string name)
            {
                string cp = path + name;

                if (File.Exists(cp))
                {
                    IExecutable executable = LoadExecutable(cp, true);

                    Debug.Log($"Uploaded NSO: {name}");

                    Executables.Add(executable);
                }
            }

            LoadNSO("rtld");

            ImageBase += MemoryMetaData.PageSize;

            LoadNSO("main");

            for (int i = 0; i < 10; i++)
            LoadNSO($"subsdk{i}");

            LoadNSO("sdk");
        }

        public void BeginProgram<T>() where T : CpuContext, new()
        {
            KThread thread = MainOS.scheduler.CreateThread(new T(),this,ProgramStart,MemoryMetaData.StackTop,MemoryMetaData.TlsCollectionAddress,0,44);

            MainOS.scheduler.DetatchAndStartThread(thread.ID);
        }
    }
}
