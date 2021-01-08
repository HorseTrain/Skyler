using System;
using System.Collections.Generic;
using System.Text;

namespace SkylerUnicorn
{
    public enum uc_prot
    {
        UC_PROT_NONE = 0,
        UC_PROT_READ = 1,
        UC_PROT_WRITE = 2,
        UC_PROT_EXEC = 4,
        UC_PROT_ALL = 7,
    }
}
