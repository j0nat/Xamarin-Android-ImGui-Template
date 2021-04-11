using System;
using Android.Views;
using Android.Views.InputMethods;
using GraphicsEngine.Interfaces;

namespace Client_Mobile.Android
{
    class AndroidFunctions : IPlatformFunctions
    {
        private readonly InputMethodManager inputMethodManager;
        public View View;
        private readonly Activity1 mainActivity;

        public event EventHandler KeyPressEvent;
        private bool capitalizedLetters;

        /// <summary>
        /// Create a new implementation of <see cref="IPlatformFunctions"/> for Android.
        /// </summary>
        /// <param name="inputMethodManager"></param>
        /// <param name="mainActivity"></param>
        public AndroidFunctions(InputMethodManager inputMethodManager, Activity1 mainActivity)
        {
            this.inputMethodManager = inputMethodManager;
            this.mainActivity = mainActivity;
            this.capitalizedLetters = false;
        }

        public void CloseKeyboard()
        {
            View.KeyPress -= View_KeyPress;

            inputMethodManager.HideSoftInputFromWindow((View).WindowToken, HideSoftInputFlags.None);
        }

        public void OpenKeyboard()
        {
            View.KeyPress -= View_KeyPress;

            inputMethodManager.ShowSoftInput(View, ShowFlags.Forced);
            inputMethodManager.ToggleSoftInput(ShowFlags.Forced, HideSoftInputFlags.ImplicitOnly);

            View.KeyPress += View_KeyPress;
        }

        private void View_KeyPress(object sender, View.KeyEventArgs e)
        {
            if (e.KeyCode == Keycode.Back && e.Event.Action == KeyEventActions.Up)
            {
                View.KeyPress -= View_KeyPress;
            }
            
            if (e.Event.IsCapsLockOn || e.Event.IsShiftPressed)
            {
                capitalizedLetters = true;
            }
            else
            {
                capitalizedLetters = false;
            }

            string input = GetInput(e.KeyCode, capitalizedLetters);
            input = SpecialCharacters(input, e.Event.UnicodeChar);

            if (e.KeyCode == Keycode.Del && e.Event.Action == KeyEventActions.Up)
            {
                KeyPressEvent.Invoke("DELETE", EventArgs.Empty);
            }
            else
            {
                if (e.KeyCode == Keycode.Space && e.Event.Action == KeyEventActions.Up)
                {
                    KeyPressEvent.Invoke(' ', EventArgs.Empty);
                }
                else
                {
                    if (input != "-1" && e.Event.Action == KeyEventActions.Up)
                    {
                        KeyPressEvent.Invoke(Convert.ToChar(input), EventArgs.Empty);
                    }
                    else
                    {
                        if (e.Event.Characters != null)
                        {
                            KeyPressEvent.Invoke(Convert.ToChar(e.Event.Characters), EventArgs.Empty);
                        }
                    }
                }
            }

        }

        private string SpecialCharacters(string input, int unicode)
        {
            switch (unicode)
            {
                case 33: return "!";
                case 63: return "?";
                case 64: return "@";
                case 34: return "\"";
                case 35: return "#";
                case 37: return "%";
                case 38: return "&";
                case 41: return ")";
                case 40: return "(";
                case 61: return "=";
                case 92: return "\\";
                case 47: return "/";
                case 58: return ":";
                case 59: return ";";
                case 42: return "*";
                default: return input;
            }
        }

        private string GetInput(Keycode key, bool capitalized)
        {
            switch (key)
            {
                case Keycode.A: return (capitalized ? "A" : "a");
                case Keycode.B: return (capitalized ? "B" : "b");
                case Keycode.C: return (capitalized ? "C" : "c");
                case Keycode.D: return (capitalized ? "D" : "d");
                case Keycode.E: return (capitalized ? "E" : "e");
                case Keycode.F: return (capitalized ? "F" : "f");
                case Keycode.G: return (capitalized ? "G" : "g");
                case Keycode.H: return (capitalized ? "H" : "h");
                case Keycode.I: return (capitalized ? "I" : "i");
                case Keycode.J: return (capitalized ? "J" : "j");
                case Keycode.K: return (capitalized ? "K" : "k");
                case Keycode.L: return (capitalized ? "L" : "l");
                case Keycode.M: return (capitalized ? "M" : "m");
                case Keycode.N: return (capitalized ? "N" : "n");
                case Keycode.O: return (capitalized ? "O" : "o");
                case Keycode.P: return (capitalized ? "P" : "p");
                case Keycode.Q: return (capitalized ? "Q" : "q");
                case Keycode.R: return (capitalized ? "R" : "r");
                case Keycode.S: return (capitalized ? "S" : "s");
                case Keycode.T: return (capitalized ? "T" : "t");
                case Keycode.U: return (capitalized ? "U" : "u");
                case Keycode.V: return (capitalized ? "V" : "v");
                case Keycode.W: return (capitalized ? "W" : "w");
                case Keycode.X: return (capitalized ? "X" : "x");
                case Keycode.Y: return (capitalized ? "Y" : "y");
                case Keycode.Z: return (capitalized ? "Z" : "z");
                case Keycode.Num0: return "0";
                case Keycode.Num1: return "1";
                case Keycode.Num2: return "2";
                case Keycode.Num3: return "3";
                case Keycode.Num4: return "4";
                case Keycode.Num5: return "5"; 
                case Keycode.Num6: return "6";
                case Keycode.Num7: return "7";
                case Keycode.Num8: return "8";
                case Keycode.Num9: return "9";
                case Keycode.Period: return ".";
                case Keycode.Comma: return ",";
                case Keycode.NumpadDot: return ".";
                default: return "-1";
            }
        }

        public float ScreenDensity()
        {
            return mainActivity.ApplicationContext.Resources.DisplayMetrics.Density;
        }

        public void RegisterDesktopKeyboardInput(object window)
        {
            throw new NotImplementedException("Not supported on Android.");
        }

        public void Exit()
        {
            mainActivity.FinishAndRemoveTask();
        }
    }
}