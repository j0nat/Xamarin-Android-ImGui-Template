using System;
using Microsoft.Xna.Framework;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Client_Desktop
{
    /// <summary>
    /// Main class for client desktop.
    /// </summary>
    static class Program
    {
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                IntPtr h = Process.GetCurrentProcess().MainWindowHandle;
                ShowWindow(h, 0);
            }
            catch
            {
                // Only works on Windows...
            }

            using (Game game = new GraphicsEngine.GUIWindow(GraphicsEngine.Platform.Desktop, new DesktopFunctions()))
            {
                game.Run();
            }
        }
    }
}
