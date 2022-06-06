using Android.App;
using Android.OS;
using Android.Runtime;
using AndroidX.AppCompat.App;
using Android.Widget;
using Android.Views;

namespace RubiksCube
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, View.IOnClickListener
    {
        TextView tvHello;
        Button btnPlay, btnRules, btnSaves;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            tvHello = FindViewById<TextView>(Resource.Id.tvHello);
            btnPlay = FindViewById<Button>(Resource.Id.btnPlay);
            btnRules = FindViewById<Button>(Resource.Id.btnRules);
            btnSaves = FindViewById<Button>(Resource.Id.btnSaves);

            btnPlay.SetOnClickListener(this);
            btnRules.SetOnClickListener(this);
            btnSaves.SetOnClickListener(this);
        }


        public void HandleCube()
        {
            RubiksCube cube = new RubiksCube();

            string cubeStr = cube.ToPrettyString();

            tvHello.Text = cubeStr;
        }

        public void OnClick(View v)
        {
            if (v == btnPlay)
                HandleCube();
            else if (v == btnRules)
            {
                if (btnSaves.Visibility == ViewStates.Visible)
                    btnSaves.Visibility = ViewStates.Invisible;
                else
                    btnSaves.Visibility = ViewStates.Visible;
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}