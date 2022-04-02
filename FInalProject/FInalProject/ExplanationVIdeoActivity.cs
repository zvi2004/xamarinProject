using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FInalProject
{
    [Activity(Label = "Explanation VIdeo")]
    public class ExplanationVIdeoActivity : AppCompatActivity
    {
        private MediaController MediaController;
        VideoView videoView;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ExplanationVIdeo_layout);
            // Create your application here

            Button btnStart = FindViewById<Button>(Resource.Id.btnStart);
            Button btnStop = FindViewById<Button>(Resource.Id.btnStop);
            Button btnContinue = FindViewById<Button>(Resource.Id.btnContinue);

            videoView = FindViewById<VideoView>(Resource.Id.videoview);
            var uri = Android.Net.Uri.Parse("android.resource://" + Application.PackageName + "/" + Resource.Raw.light);
            videoView.SetVideoURI(uri);
            MediaController = new Android.Widget.MediaController(this);
            MediaController.SetMediaPlayer(videoView);
            videoView.SetMediaController(MediaController);
            videoView.RequestFocus();

            btnStart.Click += btnStart_click;
            btnStop.Click += btnStop_click;
            btnContinue.Click += btnContinue_click;
        
        }



        /// <summary>
        /// 
        /// </summary>
        private void btnContinue_click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(PickPuzzle));
            StartService((new Intent(this, typeof(MyService))));
            StartActivity(intent);
        }



        /// <summary>
        /// function to stop the video
        /// </summary>
        private void btnStop_click(object sender, EventArgs e)
        {
            videoView.Pause();
        }



        /// <summary>
        /// function to start or continu the video
        /// </summary>
        private void btnStart_click(object sender, EventArgs e)
        {
            videoView.Start();
        }


    }
}