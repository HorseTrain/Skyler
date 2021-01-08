using System;
using System.Collections.Generic;
using System.Text;
using static SkylerUnicorn.uc;
using static SkylerUnicorn.uc_arch;
using static SkylerUnicorn.uc_mode;
using static SkylerUnicorn.uc_arm64_reg;
using static SkylerUnicorn.uc_err;
using static SkylerUnicorn.uc_prot;
using static SkylerUnicorn.uc_hook_type;
using System.Runtime.InteropServices;

namespace SkylerUnicorn
{
    public struct Hook
    {
        public ulong hook;
        public GCHandle alloc;
    }

    public unsafe class Arm64Engine
    {
        public void* context { get; private set; }

        ulong RamSize;

        public List<Hook> Hooks = new List<Hook>();

        public Arm64Engine()
        {
            void* tmp;

            if (uc_open(UC_ARCH_ARM64, UC_MODE_LITTLE_ENDIAN,&tmp) != 0)
            {
                throw new Exception();
            }

            context = tmp;

            SetRegRaw((int)UC_ARM64_REG_CPACR_EL1, 0x00300000);
        }

        public void MapMemory(ulong size, void* Base)
        {
            RamSize = size;

            uc_err err = uc_mem_map_ptr(context, 0, size, UC_PROT_ALL, Base);

            if (err != 0)
            {
                throw new Exception(err.ToString());
            }
        }

        public void Step(ulong count)
        {
            uc_err error = uc_emu_start(context, PC,RamSize,0,count);

            if (error != 0)
            {
                throw new Exception(error.ToString());
            }
        }

        public ulong GetRegRaw(int index)
        {
            ulong Out;

            uc_reg_read(context, index, &Out);

            return Out;
        }

        public void SetRegRaw(int index, ulong value)
        {
            uc_reg_write(context, index, &value);
        }

        /*
        public void SetSVC(HookFun call)
        {
            Hook hook = new Hook();

            hook.alloc = GCHandle.Alloc(call);

            uc_hook_add(context,&hook.hook, (int)UC_HOOK_INTR,Marshal.GetFunctionPointerForDelegate(call).ToPointer(),(void*)new IntPtr(),0,RamSize);

            Hooks.Add(hook);
        }
        */

        public void SetX(int index, ulong value) => SetRegRaw((int)UC_ARM64_REG_X0 + index,value);

        public ulong GetX(int index) => GetRegRaw((int)UC_ARM64_REG_X0 + index);

        void SetW(int index, uint value) => SetX(index,value);
        uint GetW(int index) => (uint)GetX(index);

        public ulong PC
        {
            get => GetRegRaw((int)UC_ARM64_REG_PC);
            set => SetRegRaw((int)UC_ARM64_REG_PC,value);
        }

        public ulong SP
        {
            get => GetRegRaw((int)UC_ARM64_REG_SP);
            set => SetRegRaw((int)UC_ARM64_REG_SP, value);
        }

        ~Arm64Engine()
        {
            foreach (Hook hook in Hooks)
            {
                uc_hook_del(context,hook.hook);
                hook.alloc.Free();
            }

            uc_close(context);
        }
    }
}
