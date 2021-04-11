using System;
using GraphicsEngine.Interfaces;
using Microsoft.Xna.Framework;

namespace Client_Desktop
{
    /// <summary>
    /// This class implements <see cref="IPlatformFunctions"/>.
    /// </summary>
    class DesktopFunctions : IPlatformFunctions
    {
        public event EventHandler KeyPressEvent;

        public void CloseKeyboard()
        {
            throw new NotImplementedException("Not supported on desktop.");
        }

        public void Exit()
        {
            Environment.Exit(0);
        }

        public void OpenCamera()
        {
            throw new NotImplementedException("Not supported on desktop.");
        }

        public void OpenKeyboard()
        {
            throw new NotImplementedException("Not supported on desktop.");
        }

        public void RegisterDesktopKeyboardInput(object window)
        {
            Game game = (Game)window;

            game.Window.TextInput += (s, a) =>
            {
                if (a.Character == '\t') return;

                KeyPressEvent.Invoke(a.Character, EventArgs.Empty);
            };
        }

        public float ScreenDensity()
        {
            return 1;
        }
    }
}
