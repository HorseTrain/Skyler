using SkylerCommon.Debugging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Service.NV
{
    public class NvMap
    {
        public uint ID          { get; set; }
        public uint Size        { get; set; }
        public uint Align       { get; set; }
        public uint Kind        { get; set; }
        public ulong Address    { get; set; }

        public NvMap(uint size)
        {
            Size = size;

            ID = (uint)Switch.MainOS.Handles.AddObject(this);

            Debug.Log($"Created NV Map With Size {size}",LogLevel.High);
        }

        //TODO: Make accessing map handles in here.
    }
}
