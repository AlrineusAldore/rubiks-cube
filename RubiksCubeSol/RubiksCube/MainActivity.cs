using Android.App;
using Android.OS;
using Android.Runtime;
using AndroidX.AppCompat.App;
using Android.Widget;
using Android.Views;
using Android.Content;
using Android.Text;
using Android.Text.Style;
using System.Collections.Generic;
using System;

namespace RubiksCube
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, View.IOnClickListener
    {
        TextView tvHello, tvExplanation;
        Button btnPlay, btnNotations, btnSaves;
        EditText etUsername, etEmail, etPass, etConfirmPass;
        Button btnLoginRegister;
        Button btnEndNots;
        Dialog d;
        Tuple<string, string> user;
        bool isLogin;
        bool isLoggedIn;
        ISharedPreferences sp;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            sp = GetSharedPreferences("details", FileCreationMode.Private);

            tvHello = FindViewById<TextView>(Resource.Id.tvHello);
            btnPlay = FindViewById<Button>(Resource.Id.btnPlay);
            btnNotations = FindViewById<Button>(Resource.Id.btnNotations);
            btnSaves = FindViewById<Button>(Resource.Id.btnSaves);

            btnPlay.SetOnClickListener(this);
            btnNotations.SetOnClickListener(this);
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
            {
                Intent intent = new Intent(this, typeof(RubiksCubeActivity));
                StartActivity(intent);
            }
            else if (v == btnNotations)
            {
                //Create dialog
                CreateNotationsDialog();
            }
            else if (v == btnEndNots)
            {
                d.Cancel();
            }
            else if (v == btnLoginRegister)
            {
                string str = "";
                d.Cancel();

                if (isLogin)
                {
                    //log in if username exists and if the username matches the password
                    if (true)//users.ContainsKey(etUsername.Text) && users[etUsername.Text] == etPass.Text)
                    {
                        user = new Tuple<string, string>(etUsername.Text, etPass.Text);
                        str = "Logged in successfully";
                        isLoggedIn = true;
                        UpdateLoggedInStatus();
                    }
                    else
                        str = "Username or password are wrong";
                }
                else
                {
                    //register if username doesn't exist
                    if (true) //!users.ContainsKey(etUsername.Text))
                    {
                        user = new Tuple<string, string>(etUsername.Text, etPass.Text);
                        str = "Registered user successfully";
                        isLoggedIn = true;
                        UpdateLoggedInStatus();
                        //users[etUsername.Text] = etPass.Text;
                    }
                    else
                        str = "User already exists";
                }

                //make toast bigger using span
                SpannableString ss = new SpannableString(str);
                ss.SetSpan(new RelativeSizeSpan(1.8f), 0, str.Length, 0);

                Toast.MakeText(this, ss, ToastLength.Long).Show();
            }
        }

        //Creates options menu
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.main_menu, menu);
            return true;
        }

        public override bool OnPrepareOptionsMenu(IMenu menu)
        {
            menu.FindItem(Resource.Id.Login).SetVisible(!isLoggedIn);
            menu.FindItem(Resource.Id.Register).SetVisible(!isLoggedIn);
            menu.FindItem(Resource.Id.Signout).SetVisible(isLoggedIn);
            return base.OnPrepareOptionsMenu(menu);
        }

        //Commands for items in menu
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.Login)
            {
                isLogin = true;
                isLoggedIn = true;
                CreateLoginRegisterDialog();
                return true;
            }
            else if (id == Resource.Id.Register)
            {
                isLogin = false;
                isLoggedIn = true;
                CreateLoginRegisterDialog();
                return true;
            }
            else if (id == Resource.Id.Signout)
            {
                isLoggedIn = false;
                UpdateLoggedInStatus();
            }
            else if (id == Resource.Id.Notations)
            {
                //Intent intent = new Intent(this, typeof(RulesActivity));
                //StartActivityForResult(intent, 0);
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }


        private void CreateLoginRegisterDialog()
        {
            //Create dialog
            d = new Dialog(this);
            d.SetContentView(Resource.Layout.login_register_layout);
            d.SetCancelable(true);

            //Connect variables to objects
            etUsername = d.FindViewById<EditText>(Resource.Id.etUsername);
            etEmail = d.FindViewById<EditText>(Resource.Id.etEmail);
            etPass = d.FindViewById<EditText>(Resource.Id.etPass);
            etConfirmPass = d.FindViewById<EditText>(Resource.Id.etConfirmPass);
            btnLoginRegister = d.FindViewById<Button>(Resource.Id.btnLoginRegister);
            btnLoginRegister.SetOnClickListener(this);

            //change how dialog looks based on whether you want to login or register
            if (isLogin)
            {
                //enable login, disable register
                etEmail.Visibility = ViewStates.Gone;
                etConfirmPass.Visibility = ViewStates.Gone;
                d.SetTitle("login");
                btnLoginRegister.Text = "Log In";
            }
            else
            {
                //enable register, disable login
                etEmail.Visibility = ViewStates.Visible;
                etConfirmPass.Visibility = ViewStates.Visible;
                d.SetTitle("register");
                btnLoginRegister.Text = "Register";
            }

            d.Show(); //show dialog
        }

        //Changes main screen appearance based on whether you're logged in or not
        private void UpdateLoggedInStatus()
        {
            tvHello.Text = "How ya doin' ";

            if (!isLoggedIn)
            {
                btnSaves.Visibility = ViewStates.Invisible;
                tvHello.Text += "Guesty?";
            }
            else
            {
                btnSaves.Visibility = ViewStates.Visible;
                tvHello.Text += user.Item1 + "?";
            }
        }
        
        private void CreateNotationsDialog()
        {
            //Create dialog
            d = new Dialog(this);
            d.SetContentView(Resource.Layout.notations_layout);
            d.SetCancelable(true);
            d.SetTitle("Notations");

            tvExplanation = d.FindViewById<TextView>(Resource.Id.tvExplanation);
            btnEndNots = d.FindViewById<Button>(Resource.Id.btnEndNots);
            btnEndNots.SetOnClickListener(this);

            tvExplanation.Text = "A single letter by itself refers to a clockwise face rotation in 90 degrees (quarter turn).\n";
            tvExplanation.Text += "A letter followed by an apostrophe means to turn that face counterclockwise 90 degrees.\n";
            tvExplanation.Text += "A letter with the number 2 after it marks a double turn (180 degrees).\n\n";

            tvExplanation.Text += "R - Right Clockwise\n";
            tvExplanation.Text += "L - Left Clockwise\n";
            tvExplanation.Text += "U - Up Clockwise\n";
            tvExplanation.Text += "D - Down Clockwise\n";
            tvExplanation.Text += "F - Front Clockwise\n";
            tvExplanation.Text += "B - Back Clockwise\n\n";

            tvExplanation.Text += "R' - Right Counter-Clockwise\n";
            tvExplanation.Text += "L' - Left Counter-Clockwise\n";
            tvExplanation.Text += "U' - Up Counter-Clockwise\n";
            tvExplanation.Text += "D' - Down Counter-Clockwise\n";
            tvExplanation.Text += "F' - Front Counter-Clockwise\n";
            tvExplanation.Text += "B' - Back Counter-Clockwise\n\n";

            tvExplanation.Text += "M - Middle: The layer between R & L, turns in L direction\n";
            tvExplanation.Text += "E - Equator: The layer between U & D, turns in D direction\n";
            tvExplanation.Text += "S - Standing: The layer between F & B, turns in F direction\n";
            tvExplanation.Text += "M' - Middle: The layer between R & L, turns in R direction\n";
            tvExplanation.Text += "E' - Equator: The layer between U & D, turns in U direction\n";
            tvExplanation.Text += "S' - Standing: The layer between F & B, turns in B direction\n\n";

            tvExplanation.Text += "X - Rotate the entire cube in R direction\n";
            tvExplanation.Text += "Y - Rotate the entire cube in U direction\n";
            tvExplanation.Text += "Z - Rotate the entire cube in F direction\n";
            tvExplanation.Text += "X' - Rotate the entire cube in L direction\n";
            tvExplanation.Text += "Y' - Rotate the entire cube in D direction\n";
            tvExplanation.Text += "Z' - Rotate the entire cube in B direction\n";

            //TODO: connect "go back" button

            d.Show();
            d.Window.SetLayout(1100, 1700);
        }


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

    }
}