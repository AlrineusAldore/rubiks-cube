using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System;
using Android.Content;
using Android.Media;

namespace srevice2019
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        Button btnStop; 
        Button btnStart, btnbip;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
         
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            btnStart = FindViewById<Button>(Resource.Id.btnStart);
            btnStop = FindViewById<Button>(Resource.Id.btnStop);
            btnStart.Click += BtnStart_Click;
            btnStop.Click += BtnStop_Click;
            btnbip = FindViewById<Button>(Resource.Id.btnbip);
            btnbip.Click += Btnbtnbip_Click;
        }

        private void Btnbtnbip_Click(object sender, EventArgs e)
        {
            MediaPlayer mp;
              mp = MediaPlayer.Create(this, Resource.Raw.bip);// מיצר נגן
            // mp = Android.Media.MediaPlayer.Create(this, Resource.Raw.click);// מיצר נגן
            mp.Start(); // מפעיל נגן
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(MusicService));
            StopService(intent);
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(MusicService));
            StartService(intent);
        }

        
    }
}