
using Android.App;
using Android.OS;

namespace ALS_MOB_100.Droid
{
    [Activity(Theme = "@style/Theme.Splash",
       MainLauncher = true,
       NoHistory = true)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            System.Threading.Thread.Sleep(3000);
            StartActivity(typeof(MainActivity));
        }
    }
}