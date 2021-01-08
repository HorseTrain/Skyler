using SkylerCommon.Utilities;
using SkylerHLE.Horizon.Execution;
using SkylerHLE.Horizon.Handles;

namespace SkylerHLE.Horizon
{
    public class HorizonOS
    {
        public ObjectCollection Handles { get; set; } 

        public Scheduler scheduler      { get; set; }

        public SharedMemory HidHandle   { get; set; }

        public HorizonOS()
        {
            Switch.MainOS = this;

            Handles = new ObjectCollection();
            scheduler = new Scheduler();

            HidHandle = new SharedMemory(0);
        }

        public Process OpenProcess()
        {
            return new Process();
        }

        public Process LoadSingle(string path)
        {
            Process temp = OpenProcess();

            temp.LoadExecutable(path);

            return temp;
        }

        public Process LoadCart(string path)
        {
            Process temp = OpenProcess();

            temp.LoadGame(path);

            return temp;
        }
    }
}
