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
    [Activity(Label = "RubiksCubeActivity")]
    public class RubiksCubeActivity : Activity, View.IOnClickListener
    {
        ISharedPreferences sp;
        string username;
        RubiksCube rubiksCube;
        Dialog d;

        FrameLayout frameLayout;
        Button btnSaveState;
        Button btnEditCube;
        Button btnBackSave;

        Button[] arrows;

        Button[] colors;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.rubiks_cube_layout);
            // Create your application here
            
            //Get username from intent
            username = Intent.GetStringExtra("username") ?? "";
            
            //Continue cube from where you last left off by using sp
            sp = GetSharedPreferences("details", FileCreationMode.Private);
            string cubeStr = sp.GetString("cubeStr", Constants.NEW_CUBE_STR);
            rubiksCube = new RubiksCube(cubeStr);

            btnSaveState = FindViewById<Button>(Resource.Id.btnSaveState);
            btnBackSave = FindViewById<Button>(Resource.Id.btnBackSave);
            btnSaveState.SetOnClickListener(this);
            btnBackSave.SetOnClickListener(this);

            //Let user save state if he's logged in
            if (username == "")
                btnSaveState.Visibility = ViewStates.Gone;
            else
                btnSaveState.Visibility = ViewStates.Visible;
        }

        public void OnClick(View v)
        {
            if (v == btnBackSave)
            {
                //Save state in sp and go back to previous activity
                var editor = sp.Edit();
                editor.PutString("cubeStr", rubiksCube.ToString());
                editor.Commit();

                SetResult(Result.Canceled);
                Finish();
            }
            else if (v == btnSaveState)
            {
                CreateAlertDialog();
            }
        }

        private void CreateAlertDialog()
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetTitle("Save state?");
            builder.SetMessage("Sure you wanna save state?");
            builder.SetCancelable(true);

            builder.SetPositiveButton("yea mate", yesAction);
            builder.SetNegativeButton("nah fam", noAction);

            AlertDialog dialog = builder.Create();
            dialog.Show();
        }

        //Must functions for AlertDialog
        public void yesAction(object sender, DialogClickEventArgs e)
        {
            //Save the current state to database
            State state = new State(rubiksCube.ToString(), username);
            SQLiteHandler.Instance.InsertState(state);
            Toast.MakeText(this, "Current state has been saved", ToastLength.Long).Show();
        }
        public void noAction(object sender, DialogClickEventArgs e)
        {
        }
    }
}