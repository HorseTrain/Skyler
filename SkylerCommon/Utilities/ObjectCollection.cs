using System;
using System.Collections.Generic;
using System.Text;

namespace SkylerCommon.Utilities
{
    //Maybe use uints instead of ulongs ?
    public class ObjectCollection
    {
        Dictionary<ulong, object> Objects { get; set; }

        public ulong MinValue { get; set; } = 0;
        public ulong MaxValue { get; set; } = ulong.MaxValue;

        public ObjectCollection(ulong Min = 0, ulong Max = ulong.MaxValue)
        {
            MinValue = Min;
            MaxValue = Max;

            Objects = new Dictionary<ulong, object>();
        }

        ulong CurrentID { get; set; }

        public ulong GetID()
        {
            CurrentID++;

            return CurrentID - 1;
        }

        public ulong AddObject(object obj)
        {
            lock (Objects)
            {
                ulong ID = GetID();

                Objects.Add(ID, obj);

                return ID;
            }
        }

        public void RemoveObject(ulong ID)
        {
            lock (Objects)
            {
                Objects.Remove(ID);
            }
        }
        public object GetObject(ulong ID) => Objects[ID];

        public void SwapObject(ulong Handle, object obj)
        {
            lock (Objects)
            {
                Objects[Handle] = obj;
            }
        }

        public object this[ulong index] => GetObject(index);

        public void DeleteObject(ulong index)
        {
            lock (Objects)
            {
                Objects.Remove(index);
            }
        }
    }
}
