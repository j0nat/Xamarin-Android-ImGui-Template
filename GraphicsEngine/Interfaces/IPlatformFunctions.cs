using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsEngine.Interfaces
{
    public interface IPlatformFunctions
    {

        /// <summary>
        /// Get the screen's pixel density.
        /// </summary>
        /// <returns>Screen pixel density</returns>
        float ScreenDensity();

        /// <summary>
        /// Open mobile keyboard.
        /// </summary>
        void OpenKeyboard();

        /// <summary>
        /// Close mobile keyboard.
        /// </summary>
        void CloseKeyboard();

        /// <summary>
        /// Register keyboard input to KeyPressEvent event. 
        /// </summary>
        /// <param name="window">The graphical window object.</param>
        void RegisterDesktopKeyboardInput(object window);

        /// <summary>
        /// Exit the program.
        /// </summary>
        void Exit();

        /// <summary>
        /// Raised when key is pressed.
        /// </summary>
        event EventHandler KeyPressEvent;
    }
}
