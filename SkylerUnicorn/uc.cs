using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace SkylerUnicorn
{
    public unsafe class uc
    {
        [DllImport("unicorn.dll")]
        public static extern uc_err uc_open(uc_arch arch,uc_mode mode,void** engine);

        [DllImport("unicorn.dll")]
        public static extern uc_err uc_close(void* engine);

        [DllImport("unicorn.dll")]
        public static extern uc_err uc_mem_map_ptr(void* engine, ulong address,ulong size, uc_prot perms,void* ptr);

        [DllImport("unicorn.dll")]
        public static extern uc_err uc_emu_start(void* engine, ulong begin, ulong until, ulong timeout, ulong size);

        [DllImport("unicorn.dll")]
        public static extern ulong uc_reg_read(void* engine, int index, void* result);

        [DllImport("unicorn.dll")]
        public static extern ulong uc_reg_write(void* engine, int index, void* value);

        [DllImport("unicorn.dll")]
        public static extern uc_err uc_hook_add(void* engine, void* hh, int type, void* callback, void* user_data, ulong begin, ulong end);
        [DllImport("unicorn.dll")]
        public static extern uc_err uc_hook_del(void* engine, ulong hh);
    }
}
