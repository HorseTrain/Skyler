using SkylerCommon.Debugging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.VirtualFS
{
    public class RomFS
    {
        FileStream Stream { get; set; }

        public RomFS(string Path)
        {
            Stream = new FileStream(Path,FileMode.Open,FileAccess.Read);
        }

        public byte[] ReadData(ulong offset, ulong size)
        {
            Stream.Seek((long)offset,SeekOrigin.Begin);

            byte[] Out = new byte[size];

            if (size > int.MaxValue)
            {
                Debug.ThrowNotImplementedException();
            }

            Stream.Read(Out,0,(int)size);

            return Out;
        }

        ~RomFS()
        {
            Stream.Close();
        }
    }
}
