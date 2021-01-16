using SkylerCommon.Debugging;
using SkylerCommon.Timing;
using SkylerGraphics.Display;
using SkylerHLE.Horizon;
using SkylerHLE.Horizon.Execution;
using SkylerHLE.Horizon.Handles;
using SkylerHLE.Horizon.Service.Sessions;
using SkylerHLE.Input;
using SkylerHLE.Memory;
using SkylerHLE.VirtualFS;
using System;
using System.IO;
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
        public static RomFS romFS           { get; set; }

        public bool ContextReady            { get; set; }

        //I want to move these somewhere else
        public KEvent VsyncEvent            { get; set; }
        public TKWindow MainDisplay         { get; set; }
        public InputManager Input           { get; set; }

        public bool InDock                  { get; set; }

        private Switch()
        {
            if (MainSwitch != null)
                Debug.LogError("A Nintendo Switch Instance Already Exists.",true);

            GlobalCounter.Init();
            MainSwitch = this;

            horizonOS = new HorizonOS();
            memory = new MemoryManager();

            OperationFileData.Setup();

            new Thread(CreateDisplay).Start();
        }

        public void CreateDisplay()
        {
            MainDisplay = new TKWindow();

            MainDisplay.End = HandleEmulationEnd;
            MainDisplay.ScreenUpdate = ScreenUpdate;
            Input = new InputManager();
            VsyncEvent = new KEvent();

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

        public void LoadRomFS(string path)
        {
            romFS = new RomFS(path);
        }

        public void ScreenUpdate()
        {
            ContextReady = true;

            Input.Update(MainDisplay);

            VsyncEvent.WaitEvent.Set();
        }
    }
}
