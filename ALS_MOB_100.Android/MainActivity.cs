
using Android;
using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.Content.PM;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;

namespace ALS_MOB_100.Droid
{
    [Activity(Label = "ALS_MOB_100", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        BluetoothManager _manager;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            _manager = (BluetoothManager)Android.App.Application.Context.GetSystemService(Android.Content.Context.BluetoothService);
            _manager.Adapter.Enable();


            base.OnCreate(savedInstanceState);

            //if (this.CheckSelfPermission(Manifest.Permission.Bluetooth) != Permission.Granted)
            //{
            //    RequestPermissions(new String[] { Manifest.Permission.Bluetooth }, 1);
            //}
            //if (this.CheckSelfPermission(Manifest.Permission.BluetoothAdmin) != Permission.Granted)
            //{
            //    RequestPermissions(new String[] { Manifest.Permission.BluetoothAdmin }, 1);
            //}
            //if (this.CheckSelfPermission(Manifest.Permission.BluetoothPrivileged) != Permission.Granted)
            //{
            //    RequestPermissions(new String[] { Manifest.Permission.BluetoothPrivileged }, 1);
            //}

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

            int add = 0;
            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessCoarseLocation) != (int)Permission.Granted)
            {
                add++;
            }
            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) != (int)Permission.Granted)
            {
                add++;
            }
            if (add != 0)
            {
                Android.Support.V4.App.ActivityCompat.RequestPermissions(this,
                          new string[] {
                    Android.Manifest.Permission.AccessCoarseLocation,
                    Android.Manifest.Permission.AccessFineLocation,
                    Android.Manifest.Permission.Bluetooth,
                  }, 4);
            }

            OpenLocationSettings();
            OpenBLESettings();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public void OpenLocationSettings()
        {


            LocationManager LM = (LocationManager)Android.App.Application.Context.GetSystemService(Android.Content.Context.LocationService);
            if (LM.IsProviderEnabled(LocationManager.GpsProvider) == false)
            {
                AlertDialog ad = new AlertDialog.Builder(this).Create();

                ad.SetMessage("Please open location");
                ad.SetCancelable(false);
                ad.SetCanceledOnTouchOutside(false);
                ad.SetButton("ok", delegate
                {
                    Android.Content.Context ctx = Android.App.Application.Context;
                    ctx.StartActivity(new Intent(Android.Provider.Settings.ActionLocationSourceSettings));
                });

                ad.SetButton2("cancle", delegate
                {

                });
                ad.Show();

            }
        }
        public void OpenBLESettings()
        {
            //BluetoothManager BL = (BluetoothManager)Android.App.Application.Context.GetSystemService(Android.Content.Context.BluetoothService);
            //if (BL.Adapter.IsEnabled == true)
            //{
            //    AlertDialog ad = new AlertDialog.Builder(this).Create();

            //    ad.SetMessage("Please open BLE");
            //    ad.SetCancelable(false);
            //    ad.SetCanceledOnTouchOutside(false);
            //    ad.SetButton("ok", delegate
            //    {
            //        Android.Content.Context ctx = Android.App.Application.Context;
            //        ctx.StartActivity(new Intent(Android.Provider.Settings.ActionLocationSourceSettings));
            //    });

            //    ad.SetButton2("cancle", delegate
            //    {

            //    });
            //    ad.Show();

            //}
        }
    }
}