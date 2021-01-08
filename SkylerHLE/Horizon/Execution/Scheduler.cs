using SkylerCPU;
using System.Collections.Generic;
using System.Threading;
using static SkylerHLE.Switch;

namespace SkylerHLE.Horizon.Execution
{
    public class Scheduler
    {
        public ulong ID         { get; set; }

        public KThread GetThread(ulong ID) => (KThread)MainOS.Handles.GetObject(ID);

        public Scheduler()
        {
            ID = MainOS.Handles.AddObject(this);
            HostThreads = new Dictionary<ulong, Thread>();
        }

        public Dictionary<ulong, Thread> HostThreads { get; set; }

        public KThread CreateThread<T>(T cpu,Process process,ulong PC, ulong SP, ulong TLS) where T : CpuContext
        {
            KThread Guest = new KThread(cpu, process);

            Guest.Cpu.PC = PC;
            Guest.Cpu.SP = SP;
            Guest.Cpu.tpidrro_el0 = TLS;

            Guest.Cpu.X[0] = 0;
            Guest.Cpu.X[1] = Guest.ID;

            return Guest;
        }

        public void RemoveThread(ulong ID) => MainOS.Handles.RemoveObject(ID);

        public void DetatchAndStartThread(ulong id)
        {
            KThread GuestThread = GetThread(id);

            GuestThread.Running = true;

            Thread HostThread = new Thread(GuestThread.Execute);

            HostThreads.Add(GuestThread.ID,HostThread);

            HostThread.Start();
        }
    }
}
