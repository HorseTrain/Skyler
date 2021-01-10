using SkylerCPU;
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

        public KThread(CpuContext context,Process process,ulong Priority)
        {
            ID = MainOS.Handles.AddObject(this);
            Cpu = context;

            Cpu.ThreadInformation = this;
            ThreadPriority = Priority;
        }

        void Execute() => Cpu.Execute();

        public void StartThread()
        {
            HostThread = new Thread(Execute);

            HostThread.Start();
        }
    }
}
