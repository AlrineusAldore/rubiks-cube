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
        Button btnReset;
        bool isEditOn;
        Color selectedColor;

        FrameLayout main;
        ImageView[] arrows;
        ImageView[] frontFaces;
        ImageButton[] colorBtns;
        ImageView[,] faces;

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
            isEditOn = false;
            selectedColor = Color.white; //default value

            InitButtons();
            //Let user save state if he's logged in 
            if (username == "")
                btnSaveState.Visibility = ViewStates.Gone;
            else
                btnSaveState.Visibility = ViewStates.Visible;

            CreateGraphicBoard();
            foreach (ImageView face in faces)
            {
                face.SetOnClickListener(this);
                face.Visibility = ViewStates.Visible;
            }
            UpdateColors();
        }

        //Creates all the interactive graphic faces of the whole cube
        private void CreateGraphicBoard()
        {
            main = FindViewById<FrameLayout>(Resource.Id.main);

            //Create new table layout
            TableLayout tableLayout = new TableLayout(this);
            TableLayout.LayoutParams tableParams = new TableLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
            tableParams.Gravity = GravityFlags.Center;
            tableParams.LeftMargin = 150;
            tableParams.TopMargin = 200;
            tableLayout.LayoutParameters = tableParams;

            TableRow[] tableRows = new TableRow[9];
            int rowIndex = -1;
            int line = 0;
            faces = new ImageView[6,9]; //Create 6x9 (big faces x smol faces) 2d array

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (i == 0)
                    {
                        line = j / 3;
                    }
                    else if (i < 5)
                    {
                        line = 3 + j / 3;
                    }
                    else if (i == 5)
                    {
                        line = 6 + j / 3;
                    }
                    else
                        line = 99; //error
                    int index = i * 9 + j; //Flattened index
                    faces[i,j] = new ImageView(this); //Init the view

                    //Create buttons with attributes
                    TableRow.LayoutParams facesParams = new TableRow.LayoutParams(75, 75);
                    //facesParams.Gravity = GravityFlags.Center;
                    facesParams.LeftMargin = 3; // small margin for space between faces
                    facesParams.TopMargin = 3;

                    //For the special row structure
                    if (i == 0 || i == 1 || i == 5)
                    {
                        //top and bottom faces are positioned at the middle left columns
                        if (i == 0 || i == 5)
                            facesParams.Column = 3 + j % 3;

                        //top, first middle, and bottom, all create a row every 3 faces 
                        if (j % 3 == 0)
                        {
                            rowIndex++;
                            //Create new table row
                            tableRows[line] = new TableRow(this);
                            TableRow.LayoutParams rowParams = new TableRow.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
                            rowParams.Gravity = GravityFlags.Center;
                            tableRows[line].LayoutParameters = rowParams;
                        }
                    }

                    faces[i,j].LayoutParameters = facesParams;
                    faces[i,j].SetBackgroundColor(CubeColor2XmlColor((Color)i));
                    if (i == 0 || i == 5)
                        tableRows[line].AddView(faces[i, j], faces[i, j].LayoutParameters); //add curr face to curr row when each row is 3
                    else
                        tableRows[line].AddView(faces[i, j], faces[i, j].LayoutParameters); //do the same when each row is 9
                }
            }


            //Add rows to table
            for (int i = 0; i < 9; i++)
                tableLayout.AddView(tableRows[i], i);

            main.AddView(tableLayout);
        }

        //Initializes all buttons
        private void InitButtons()
        {
            btnSaveState = FindViewById<Button>(Resource.Id.btnSaveState);
            btnBackSave = FindViewById<Button>(Resource.Id.btnBackSave);
            btnEditCube = FindViewById<Button>(Resource.Id.btnEdit);
            btnReset = FindViewById<Button>(Resource.Id.btnReset);
            btnSaveState.SetOnClickListener(this);
            btnBackSave.SetOnClickListener(this);
            btnEditCube.SetOnClickListener(this);
            btnReset.SetOnClickListener(this);

            arrows = new ImageView[12];
            frontFaces = new ImageView[9];
            colorBtns = new ImageButton[6];

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

            frontFaces[0] = FindViewById<ImageView>(Resource.Id.face1);
            frontFaces[1] = FindViewById<ImageView>(Resource.Id.face2);
            frontFaces[2] = FindViewById<ImageView>(Resource.Id.face3);
            frontFaces[3] = FindViewById<ImageView>(Resource.Id.face4);
            frontFaces[4] = FindViewById<ImageView>(Resource.Id.face5);
            frontFaces[5] = FindViewById<ImageView>(Resource.Id.face6);
            frontFaces[6] = FindViewById<ImageView>(Resource.Id.face7);
            frontFaces[7] = FindViewById<ImageView>(Resource.Id.face8);
            frontFaces[8] = FindViewById<ImageView>(Resource.Id.face9);

            colorBtns[0] = FindViewById<ImageButton>(Resource.Id.btnYellow);
            colorBtns[1] = FindViewById<ImageButton>(Resource.Id.btnOrange);
            colorBtns[2] = FindViewById<ImageButton>(Resource.Id.btnBlue);
            colorBtns[3] = FindViewById<ImageButton>(Resource.Id.btnRed);
            colorBtns[4] = FindViewById<ImageButton>(Resource.Id.btnGreen);
            colorBtns[5] = FindViewById<ImageButton>(Resource.Id.btnWhite);

            for (int i = 0; i < 12; i++)
                arrows[i].SetOnClickListener(this);

            for (int i = 0; i < 9; i++)
                frontFaces[i].SetOnClickListener(this);

            for (int i = 0; i < 6; i++)
                colorBtns[i].SetOnClickListener(this);
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
            else if (v == btnEditCube)
            {
                isEditOn = !isEditOn;

                //Make color buttons visible/invisible depends if edit is on
                ViewStates vState;
                if (isEditOn)
                    vState = ViewStates.Visible;
                else
                    vState = ViewStates.Gone;

                for (int i = 0; i < 6; i++)
                    colorBtns[i].Visibility = vState;
            }
            else if (v == btnReset)
            {
                rubiksCube = new RubiksCube(Constants.NEW_CUBE_STR);
                UpdateColors();
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

                //only edit stuff if edit is on
                if (isEditOn)
                {
                    //If it wasn't color buttons, check for faces
                    if (!CheckColorButtons(v))
                    {
                        for (int i = 0; i < 9; i++)
                        {
                            if (v == frontFaces[i])
                                UpdateFace(i);
                        }
                    }
                }
            }
        }

        private bool CheckColorButtons(View v)
        {
            if (v == colorBtns[0])
                selectedColor = Color.yellow;
            else if (v == colorBtns[1])
                selectedColor = Color.orange;
            else if (v == colorBtns[2])
                selectedColor = Color.blue;
            else if (v == colorBtns[3])
                selectedColor = Color.red;
            else if (v == colorBtns[4])
                selectedColor = Color.green;
            else if (v == colorBtns[5])
                selectedColor = Color.white;
            else
                return false;

            return true; //meaning it was one of those buttons
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
            string cubeStr = rubiksCube.ToString();
            string front = rubiksCube.GetFront();
            for (int i = 0; i < 9; i++)
            {
                //Char to cube color to xamarin color to set the background
                frontFaces[i].SetBackgroundColor(CubeColor2XmlColor(rubiksCube.CharToColor(front[i])));
            }

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    faces[i,j].SetBackgroundColor(CubeColor2XmlColor(rubiksCube.CharToColor(cubeStr[(i * 9) + j])));
                }
            }
        }
        

        private void UpdateFace(int n)
        {
            //n is from 0 to 8 representing all front faces' indexes
            rubiksCube.cubies[n % 3, n / 3, 2].colors[2] = selectedColor; //front faces from left to right then top to bottom
            frontFaces[n].SetBackgroundColor(CubeColor2XmlColor(selectedColor));
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