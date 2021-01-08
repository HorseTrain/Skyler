using SkylerCommon.Debugging;
using SkylerCommon.Timing;
using SkylerGraphics.Display;
using SkylerHLE.Horizon;
using SkylerHLE.Horizon.Execution;
using SkylerHLE.Horizon.Handles;
using SkylerHLE.Input;
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
        public InputManager Input           { get; set; }

        private Switch()
        {
            if (MainSwitch != null)
                Debug.LogError("A Nintendo Switch Instance Already Exists.",true);

            GlobalCounter.Init();
            MainSwitch = this;

            horizonOS = new HorizonOS();
            memory = new MemoryManager();

            new Thread(CreateDisplay).Start();
            new Thread(CreateInputManager).Start();
        }

        public void CreateDisplay()
        {
            MainDisplay = new TKWindow();

            MainDisplay.End = HandleEmulationEnd;

            MainDisplay.Start();
        }

        public void CreateInputManager()
        {
            while (MainDisplay == null)
            {
                Thread.Yield();
            }

            Input = new InputManager();

            while (true)
            {
                if (MainOS.HidHandle.Mapped)
                    Input.Update(MainDisplay);

                Thread.Sleep((int)InputManager.UpdateSpeed);
            }
        }

        public void HandleEmulationEnd()
        {
            //TODO:
        }

        public static void InitSwitch()
        {
            new Switch();
        }
    }
}
