using SkylerCPU;
using SkylerHLE.Horizon.Service.Sessions;
using System.Collections.Generic;
using System.Threading;
using static SkylerHLE.Switch;

namespace SkylerHLE.Horizon.Execution
{
    public class KThread
    {
        public CpuContext Cpu       { get; set; }
        public ulong ID             { get; set; }
        public Process HostProcess  { get; set; }
        public bool Running         { get; set; }
        public ulong ThreadPriority { get; set; }
        public Thread HostThread    { get; set; }

        public int WaitHandle     { get; set; }
        public ulong MutexAddress   { get; set; }
        public ulong CondVarAddress { get; set; }
        public bool CondVarSignaled { get; set; }
        public KEvent SyncHandler   { get; set; }
        public List<KThread> MutexWaiters { get; set; }
        public KThread MutexOwner { get; set; }

        public KThread(CpuContext context,Process process,ulong Priority)
        {
            ID = MainOS.Handles.AddObject(this);
            Cpu = context;

            Cpu.ThreadInformation = this;
            ThreadPriority = Priority;
            MutexWaiters = new List<KThread>();
            SyncHandler = new KEvent();
        }

        void Execute() => Cpu.Execute();

        public void StartThread()
        {
            HostThread = new Thread(Execute);

            HostThread.Start();
        }
    }
}
