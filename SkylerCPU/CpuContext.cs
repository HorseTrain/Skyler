using SkylerCommon.Debugging;
using SkylerCommon.Utilities.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkylerCPU
{
    public delegate CpuContext CreateThread();

    public class CpuContext
    {
        public object ThreadInformation { get; set; }
        public ObjectIndexer<ulong> X => new ObjectIndexer<ulong>(GetX,SetX,this);
        public ObjectIndexer<uint> W => new ObjectIndexer<uint>(GetW,SetW,this);

        protected virtual ulong GetX(ulong index)
        {
            Debug.ThrowNotImplementedException("");

            return 0;
        }

        protected virtual void SetX(ulong index, ulong Value)
        {
            Debug.ThrowNotImplementedException("");
        }

        uint GetW(ulong index) => (uint)GetX(index);
        void SetW(ulong index, uint value) => SetX(index,value);

        public virtual ulong tpidrro_el0 { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public virtual ulong dczid_el0 { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public virtual ulong PC { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public virtual ulong SP { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public List<CpuHook> Hooks { get; set; }

        public virtual void AddHook(CpuHook Hook)
        {
            throw new NotImplementedException();
        }

        public virtual void CallSVC()
        {
            throw new NotImplementedException();
        }

        public virtual void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
