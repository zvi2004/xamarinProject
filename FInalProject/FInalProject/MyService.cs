using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace FInalProject
{
    [Service]
    public class MyService : Service
    {
        string mod;
        MediaPlayer mp;
        AudioManager am;
        MyHandler myHandler;



        /// <summary>
        /// 
        /// </summary>
        public override void OnCreate()
        {
            base.OnCreate();
            myHandler = new MyHandler(this);
        }



        /// <summary>
        /// 
        /// </summary>
        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            ThreadStart threadStart = new ThreadStart(Run);
            Thread thread = new Thread(threadStart);
            thread.Start();

            return base.OnStartCommand(intent, flags, startId);
        }



        /// <summary>
        /// 
        /// </summary>
        public override void OnDestroy()
        {
            base.OnDestroy();
        }



        /// <summary>
        /// 
        /// </summary>
        private void Run()
        {
            mp = MediaPlayer.Create(this, Resource.Raw.song);
            mp.Start();

            am = (AudioManager)GetSystemService(Context.AudioService);
            int max = am.GetStreamMaxVolume(Stream.Music);
            am.SetStreamVolume(Stream.Music, max , 0);
        }



        /// <summary>
        /// 
        /// </summary>
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }
    }
}