using OpenTK.Windowing.GraphicsLibraryFramework;
using SkylerCommon.Globals;
using SkylerGraphics.Display;
using System;
using System.Collections.Generic;

namespace SkylerHLE.Input
{
    public class InputManager
    {
        public static ulong UpdateSpeed = 8; //Update at about 120 fps

        public List<Controller> Controllers { get; set; } //TODO

        public void Update(TKWindow window)
        {
            int State = 0;

            if (window.KeyboardState.IsKeyDown(Keys.Up))
            {
                State |= 0x2000;
            }

            if (window.KeyboardState.IsKeyDown(Keys.Down))
            {
                State |= 0x8000;
            }

            if (window.KeyboardState.IsKeyDown(Keys.Left))
            {
                State |= 0x1000;
            }

            if (window.KeyboardState.IsKeyDown(Keys.Right))
            {
                State |= 0x4000;
            }

            if (window.KeyboardState.IsKeyDown(Keys.Z))
            {
                State |= 1;
            }

            if (window.KeyboardState.IsKeyDown(Keys.X))
            {
                State |= 0x8;
            }

            if (window.KeyboardState.IsKeyDown(Keys.Enter))
            {
                State |= 0x400;
            }

            if (window.KeyboardState.IsKeyDown(Keys.Tab))
            {
                State |= 0x800;
            }

            GlobalMemory.GetWriter().WriteStruct(Switch.MainOS.HidHandle.VirtualPosition + 0xae38,State);
        }
    }
}
