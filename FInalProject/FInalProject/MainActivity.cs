using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;
using Xamarin.Essentials;

namespace FInalProject
{
    [Activity(Label = "welcome page", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        BroadcastBattery broadcastBattery;
        TextViewTwo tv = new TextViewTwo();
        Button btnToVIdeo;
        TextView textView1, textView2, time;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            btnToVIdeo = FindViewById<Button>(Resource.Id.btnToVIdeo);
            btnToVIdeo.Text = tv.button.btnText;
            btnToVIdeo.Click += btnToVIdeo_click;

            textView1=FindViewById<TextView>(Resource.Id.textView1);
            textView1.Text = tv.tv1;
            textView2=FindViewById<TextView>(Resource.Id.textView2);
            textView2.Text = tv.tv2;

            time = FindViewById<TextView>(Resource.Id.time);
            broadcastBattery = new BroadcastBattery(time);
        }



        /// <summary>
        /// when player return to the game
        /// </summary>
        protected override void OnResume()
        {
            base.OnResume();
            RegisterReceiver(broadcastBattery, new IntentFilter(Intent.ActionBatteryChanged));
        }


        /// <summary>
        /// when app is paused or in the background
        /// </summary>
        protected override void OnPause()
        {
            UnregisterReceiver(broadcastBattery);
            base.OnPause();
        }



        /// <summary>
        /// function to move to video
        /// </summary>
        private void btnToVIdeo_click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(ExplanationVIdeoActivity));
            StartActivity(intent);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}