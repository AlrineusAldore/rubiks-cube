using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace srevice2019
{
    [Service(Label = "FirstService")]  //write service to menifest file  חשוב
    class MusicService : Service
    {
        IBinder binder;   //null not in bagrut 
        MediaPlayer mp;
        public override StartCommandResult OnStartCommand(Android.Content.Intent intent, StartCommandFlags flags, int startId)
        {
            // start your service logic here

            mp = MediaPlayer.Create(this, Resource.Raw.remix);// מיצר נגן
            mp.Start(); // מפעיל נגן
            // Return the correct StartCommandResult for the type of service you are building
            return StartCommandResult.NotSticky;
        }

        public override IBinder OnBind(Intent intent)
        {
            binder = null;
            return binder;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (mp != null)
            {
                mp.Stop();
                mp.Release();
                mp = null;
            }

        }
    }
}
   