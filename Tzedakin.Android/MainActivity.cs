using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using System;

namespace Tzedaka.Droid
{
    [Activity(Label = "Tzedaka", Icon = "@mipmap/icono_tzedaka", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        readonly string[] Permission =
        {
            Android.Manifest.Permission.Internet,
            Android.Manifest.Permission.ReadExternalStorage,
            Android.Manifest.Permission.Vibrate,
            Android.Manifest.Permission.ForegroundService
        };
        const int Requestapp = 0;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            RequestPermissions(Permission, Requestapp);

            Rg.Plugins.Popup.Popup.Init(this);
            LoadApplication(new App());

        }

        [Obsolete]
        public override void OnBackPressed()
        {
            Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}