using SkylerCommon.Debugging;
using SkylerCommon.Timing;
using SkylerGraphics.Display;
using SkylerHLE.Horizon;
using SkylerHLE.Horizon.Handles;
using SkylerHLE.Memory;
using System;
using System.Threading;

namespace SkylerHLE
{
    public class Switch
    {
        public static Switch MainSwitch     { get; set; }
        public static HorizonOS MainOS      { get; set; }
        public static MemoryManager Memory  { get; set; }

        HorizonOS horizonOS                 { get; set; }
        MemoryManager memory                { get; set; }

        public TKWindow MainDisplay         { get; set; }

        private Switch()
        {
            if (MainSwitch != null)
                Debug.LogError("A Nintendo Switch Instance Already Exists.",true);


            GlobalCounter.Init();
            MainSwitch = this;

            horizonOS = new HorizonOS();
            memory = new MemoryManager();

            new Thread(CreateDisplay).Start();
        }

        public void CreateDisplay()
        {
            Thread.Sleep(1 * 1000);

            MainDisplay = new TKWindow();
        }

        public static void InitSwitch()
        {
            new Switch();
        }
    }
}
