using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//This was taken from.
//https://github.com/Thealexbarney/LibHac/blob/master/src/LibHac/Lz4.cs

namespace SkylerCommon.Utilities
{
    public class LZ4
    {
        public static byte[] Decompress(byte[] cmp, int decLength)
        {
            var dec = new byte[decLength];

            int cmpPos = 0;
            int decPos = 0;

            int GetLength(int length)
            {
                byte sum;

                if (length == 0xf)
                {
                    do
                    {
                        length += sum = cmp[cmpPos++];
                    }
                    while (sum == 0xff);
                }

                return length;
            }

            do
            {
                byte token = cmp[cmpPos++];

                int encCount = (token >> 0) & 0xf;
                int litCount = (token >> 4) & 0xf;

                //Copy literal chunk
                litCount = GetLength(litCount);

                Buffer.BlockCopy(cmp, cmpPos, dec, decPos, litCount);

                cmpPos += litCount;
                decPos += litCount;

                if (cmpPos >= cmp.Length)
                {
                    break;
                }

                //Copy compressed chunk
                int back = cmp[cmpPos++] << 0 |
                           cmp[cmpPos++] << 8;

                encCount = GetLength(encCount) + 4;

                int encPos = decPos - back;

                if (encCount <= back)
                {
                    Buffer.BlockCopy(dec, encPos, dec, decPos, encCount);

                    decPos += encCount;
                }
                else
                {
                    while (encCount-- > 0)
                    {
                        dec[decPos++] = dec[encPos++];
                    }
                }
            }
            while (cmpPos < cmp.Length &&
                   decPos < dec.Length);

            return dec;
        }
    }
}
