using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RubiksCube
{
    [BroadcastReceiver(Enabled = true)]
    [IntentFilter(new[] { Intent.ActionTimeTick })]
    public class TimeBroadCast : BroadcastReceiver
    {
        TextView tv;

        public TimeBroadCast() { }
        public TimeBroadCast(TextView tv)
        {
            this.tv = tv;
        }
        public override void OnReceive(Context context, Intent intent)
        {
            var time24 = DateTime.Now.ToString("HH:mm:ss");
            tv.Text = time24;
        }
    }
}