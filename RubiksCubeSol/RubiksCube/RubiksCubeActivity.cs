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
        Button btnSaveState;
        Button btnEditCube;
        Button btnBackSave;

        FrameLayout main;
        ImageView[,] arrows;
        Button[,] faces;

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

            main = FindViewById<FrameLayout>(Resource.Id.main);
            CreateCubeBoard();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    arrows[i, j].SetOnClickListener(this);
                }
            }
        }

        private void CreateCubeBoard()
        {
            arrows = new ImageView[4, 3];
            faces = new Button[3, 3];

            arrows[0,0] = FindViewById<ImageView>(Resource.Id.up_left);
            arrows[0,1] = FindViewById<ImageView>(Resource.Id.up_mid);
            arrows[0,2] = FindViewById<ImageView>(Resource.Id.up_right);
            arrows[1,0] = FindViewById<ImageView>(Resource.Id.right_up);
            arrows[1,1] = FindViewById<ImageView>(Resource.Id.right_mid);
            arrows[1,2] = FindViewById<ImageView>(Resource.Id.right_down);
            arrows[2,0] = FindViewById<ImageView>(Resource.Id.down_right);
            arrows[2,1] = FindViewById<ImageView>(Resource.Id.down_mid);
            arrows[2,2] = FindViewById<ImageView>(Resource.Id.down_left);
            arrows[3,0] = FindViewById<ImageView>(Resource.Id.left_down);
            arrows[3,1] = FindViewById<ImageView>(Resource.Id.left_mid);
            arrows[3,2] = FindViewById<ImageView>(Resource.Id.left_up);

            faces[0,0] = FindViewById<Button>(Resource.Id.btn1);
            faces[0,1] = FindViewById<Button>(Resource.Id.btn2);
            faces[0,2] = FindViewById<Button>(Resource.Id.btn3);
            faces[1,0] = FindViewById<Button>(Resource.Id.btn4);
            faces[1,1] = FindViewById<Button>(Resource.Id.btn5);
            faces[1,2] = FindViewById<Button>(Resource.Id.btn6);
            faces[2,0] = FindViewById<Button>(Resource.Id.btn7);
            faces[2,1] = FindViewById<Button>(Resource.Id.btn8);
            faces[2,2] = FindViewById<Button>(Resource.Id.btn9);


            int n = 1;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    faces[i, j].SetOnClickListener(this);
                    faces[i, j].SetBackgroundColor(Android.Graphics.Color.Green);
                }
            }
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