using SkylerCommon.Utilities;
using SkylerHLE.Horizon.Execution;
using SkylerHLE.Horizon.Handles;
using SkylerHLE.Horizon.Service.AppletAE;
using SkylerHLE.Horizon.Service.Sessions;

namespace SkylerHLE.Horizon
{
    public class HorizonOS
    {
        public ObjectCollection Handles     { get; set; } 

        public Scheduler scheduler          { get; set; }

        public AppletManager AppletManager  { get; set; }

        public SharedMemory HidHandle       { get; set; }

        public HorizonOS()
        {
            Switch.MainOS = this;

            Handles = new ObjectCollection();
            scheduler = new Scheduler();
            AppletManager = new AppletManager();

            HidHandle = new SharedMemory(0);
        }

        public Process OpenProcess()
        {
            AppletManager.SetFocus(true);

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
