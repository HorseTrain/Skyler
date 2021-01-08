using SkylerCPU;
using static SkylerHLE.Switch;

namespace SkylerHLE.Horizon.Execution
{
    public class KThread
    {
        public CpuContext Cpu       { get; set; }
        public ulong ID             { get; set; }
        public Process HostProcess  { get; set; }
        public bool Running         { get; set; }

        public KThread(CpuContext context,Process process)
        {
            ID = MainOS.Handles.AddObject(this);
            Cpu = context;

            Cpu.ThreadInformation = this;
        }

        public void Execute() => Cpu.Execute();
    }
}
