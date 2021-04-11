using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Microsoft.Xna.Framework;

using System.IO;
using System.Reflection;
using System;

using Android.Views.InputMethods;
using Android.Content;
using Android.Runtime;
using Android;
using Android.Support.V13.App;

namespace Client_Mobile.Android
{
    /// <summary>
    /// This is the main activity for the android implementation.
    /// </summary>
    [Activity(
        Label = "@string/app_name",
        MainLauncher = true,
        Icon = "@mipmap/ic_launcher",
        AlwaysRetainTaskState = true,
        LaunchMode = LaunchMode.SingleInstance,
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize
    )]
    public class Activity1 : AndroidGameActivity
    {
        private Game game;
        private View view;
        private AndroidFunctions androidFunctions;

        /// <summary>
        /// Create MonoGame instance and set the content view.
        /// </summary>
        /// <param name="bundle"></param>
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            
            var requiredPermissions = new String[] { 
                Manifest.Permission.ReadExternalStorage,
                Manifest.Permission.WriteExternalStorage};

            ActivityCompat.RequestPermissions(this, requiredPermissions, 123);

            InputMethodManager inputMethodManager = GetSystemService(InputMethodService) as InputMethodManager;
            androidFunctions = new AndroidFunctions(inputMethodManager, this);

            game = new GraphicsEngine.GUIWindow(GraphicsEngine.Platform.Android, androidFunctions);

            view = game.Services.GetService(typeof(View)) as View;
            androidFunctions.View = view;
            
            SetContentView(view);

            game.Run();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            if (requestCode == 123)
            {
                bool permissionsNotGiven = false;

                if (permissions.Length != 2)
                {
                    permissionsNotGiven = true;
                }

                if (permissionsNotGiven)
                {
                    Finish();
                }
            }

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
        }

        /// <summary>
        /// Convert DP to PX to retrieve screen pixel density.
        /// </summary>
        /// <param name="context">Activity context</param>
        /// <param name="dp">DP</param>
        /// <returns>Pixels.</returns>
        public static int dpToPx(Context context, int dp)
        {
            float density = context.Resources.DisplayMetrics.Density;

            return (int)Math.Round((float)dp * density);
        }

        protected override void OnResume()
        {
            androidFunctions.CloseKeyboard();

            base.OnResume();
        }

        protected override void OnDestroy()
        {
            androidFunctions.CloseKeyboard();

            base.OnDestroy();
        }
    }
}
