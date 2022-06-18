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
        ImageView[] arrows;
        ImageView[] faces;

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
            //If we get a string from intent, it's from saved states
            string intentCube = Intent.GetStringExtra("cubeStr");
            if (intentCube != null)
                cubeStr = intentCube;

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
            for (int i = 0; i < 12; i++)
            {
                arrows[i].SetOnClickListener(this);
            }

            UpdateColors();
        }

        private void CreateCubeBoard()
        {
            arrows = new ImageView[12];
            faces = new ImageView[9];

            arrows[0] = FindViewById<ImageView>(Resource.Id.up_left);
            arrows[1] = FindViewById<ImageView>(Resource.Id.up_mid);
            arrows[2] = FindViewById<ImageView>(Resource.Id.up_right);
            arrows[3] = FindViewById<ImageView>(Resource.Id.right_up);
            arrows[4] = FindViewById<ImageView>(Resource.Id.right_mid);
            arrows[5] = FindViewById<ImageView>(Resource.Id.right_down);
            arrows[6] = FindViewById<ImageView>(Resource.Id.down_right);
            arrows[7] = FindViewById<ImageView>(Resource.Id.down_mid);
            arrows[8] = FindViewById<ImageView>(Resource.Id.down_left);
            arrows[9] = FindViewById<ImageView>(Resource.Id.left_down);
            arrows[10] = FindViewById<ImageView>(Resource.Id.left_mid);
            arrows[11] = FindViewById<ImageView>(Resource.Id.left_up);

            faces[0] = FindViewById<ImageView>(Resource.Id.face1);
            faces[1] = FindViewById<ImageView>(Resource.Id.face2);
            faces[2] = FindViewById<ImageView>(Resource.Id.face3);
            faces[3] = FindViewById<ImageView>(Resource.Id.face4);
            faces[4] = FindViewById<ImageView>(Resource.Id.face5);
            faces[5] = FindViewById<ImageView>(Resource.Id.face6);
            faces[6] = FindViewById<ImageView>(Resource.Id.face7);
            faces[7] = FindViewById<ImageView>(Resource.Id.face8);
            faces[8] = FindViewById<ImageView>(Resource.Id.face9);


            for (int i = 0; i < 9; i++)
                faces[i].SetOnClickListener(this);
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
            else
            {
                for (int i = 0; i < 12; i++)
                {
                    if (v == arrows[i])
                    {
                        Rotate(i);
                        break;
                    }
                }
            }
        }

        private void Rotate(int x)
        {
            switch (x)
            {
                case 0:
                    rubiksCube.Li();
                    break;
                case 1:
                    rubiksCube.Mi();
                    break;
                case 2:
                    rubiksCube.R();
                    break;
                case 3:
                    rubiksCube.Ui();
                    break;
                case 4:
                    rubiksCube.E();
                    break;
                case 5:
                    rubiksCube.D();
                    break;
                case 6:
                    rubiksCube.Ri();
                    break;
                case 7:
                    rubiksCube.M();
                    break;
                case 8:
                    rubiksCube.L();
                    break;
                case 9:
                    rubiksCube.Di();
                    break;
                case 10:
                    rubiksCube.Ei();
                    break;
                case 11:
                    rubiksCube.U();
                    break;
            }
            UpdateColors();
        }

        private void UpdateColors()
        {
            string front = rubiksCube.GetFront();
            for (int i = 0; i < 9; i++)
            {
                //Char to cube color to xamarin color to set the background
                faces[i].SetBackgroundColor(CubeColor2XmlColor(rubiksCube.CharToColor(front[i])));
            }

        }

        private Android.Graphics.Color CubeColor2XmlColor(Color color)
        {
            Android.Graphics.Color graphicsColor = Android.Graphics.Color.Gray;

            switch (color)
            {
                case Color.yellow:
                    graphicsColor = Android.Graphics.Color.Yellow;
                    break;
                case Color.orange:
                    graphicsColor = Android.Graphics.Color.Orange;
                    break;
                case Color.blue:
                    graphicsColor = Android.Graphics.Color.Blue;
                    break;
                case Color.red:
                    graphicsColor = Android.Graphics.Color.Red;
                    break;
                case Color.green:
                    graphicsColor = Android.Graphics.Color.Green;
                    break;
                case Color.white:
                    graphicsColor = Android.Graphics.Color.White;
                    break;
                default:
                    graphicsColor = Android.Graphics.Color.Gray;
                    break;
            }

            return graphicsColor;

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