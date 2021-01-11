using SkylerCommon.Globals;
using SkylerCommon.Memory;
using SkylerCPU;
using SkylerHLE;
using SkylerHLE.Horizon.Execution;
using SkylerHLE.Horizon.Kernel.SVC;
using SkylerUnicorn;
using System;
using System.Runtime.InteropServices;

namespace SkylerCore.CPU
{
    public unsafe class UnicornCpuContext : CpuContext
    {
        Arm64Engine engine { get; set; }

        public UnicornCpuContext()
        {
            engine = new Arm64Engine();

            engine.MapMemory(GlobalMemory.RamSize,GlobalMemory.BaseMemoryPointer);

            AddHook(CallSVC);
        }

        protected override ulong GetX(ulong index) =>               engine.GetX((int)index);
        protected override void SetX(ulong index, ulong Value) =>   engine.SetX((int)index,Value);

        public override ulong tpidrro_el0 { get => engine.GetRegRaw((int)uc_arm64_reg.UC_ARM64_REG_TPIDRRO_EL0); set => engine.SetRegRaw((int)uc_arm64_reg.UC_ARM64_REG_TPIDRRO_EL0, value); }

        public override ulong PC { get => engine.GetRegRaw((int)uc_arm64_reg.UC_ARM64_REG_PC); set => engine.SetRegRaw((int)uc_arm64_reg.UC_ARM64_REG_PC, value); }
        public override ulong SP { get => engine.GetRegRaw((int)uc_arm64_reg.UC_ARM64_REG_SP); set => engine.SetRegRaw((int)uc_arm64_reg.UC_ARM64_REG_SP, value); }

        public override void AddHook(CpuHook Hook)
        {
            Hook hook = new Hook();

            hook.alloc = GCHandle.Alloc(Hook);

            uc.uc_hook_add(engine.context,&hook.hook, (int)uc_hook_type.UC_HOOK_INTR, Marshal.GetFunctionPointerForDelegate(Hook).ToPointer(), (void*)new IntPtr(),0,GlobalMemory.RamSize);

            engine.Hooks.Add(hook);            
        }

        public override void CallSVC()
        {
            MemoryReader reader = new MemoryReader(GlobalMemory.BaseMemoryPointer);

            SupervisorCallCollection.Call((reader.ReadStructAtOffset<int>(PC - 4) >> 5) & 0x7FFF, X);
        }

        public override void Execute()
        {
            engine.Step(0);
        }

        public static UnicornCpuContext CreateContext() => new UnicornCpuContext();
    }
}
