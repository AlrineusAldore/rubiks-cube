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
    [Activity(Label = "StatesActivity")]
    public class StatesActivity : Activity, ListView.IOnItemClickListener, ListView.IOnItemLongClickListener
    {
        public static List<State> statesList { get; set; }
        StateAdapter stateAdapter;
        ListView lvStates;
        string username;

        int lastPos; //used for alert dialog

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your application here
            SetContentView(Resource.Layout.states_layout);

            //Get username from intent and make sure its not empty
            username = Intent.GetStringExtra("username") ?? "";
            System.Diagnostics.Debug.Assert(username != "", "username empty for saved states activity");

            //Get all states from database and create the states adapter with them
            statesList = SQLiteHandler.Instance.GetAllStates(username);
            stateAdapter = new StateAdapter(this, statesList);

            lvStates = FindViewById<ListView>(Resource.Id.lvStates);
            lvStates.Adapter = stateAdapter; //adds the adapter to the listview
            lvStates.OnItemClickListener = this;
            lvStates.OnItemLongClickListener = this;
        }

        public void OnItemClick(AdapterView parent, View view, int position, long id)
        {
            //On item click, get cube str of state and initialize rubiks cube with it
            Intent intent = new Intent(this, typeof(RubiksCubeActivity));
            intent.PutExtra("cubeStr", statesList[position].cubeStr);
            StartActivity(intent);
        }

        public bool OnItemLongClick(AdapterView parent, View view, int position, long id)
        {
            lastPos = position;
            CreateAlertDialog();
            return true;
        }

        private void CreateAlertDialog()
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetTitle("Wanna delete?");
            builder.SetMessage("Sure you wanna Delete State");
            builder.SetCancelable(true);

            builder.SetPositiveButton("Delete State", YesAction);
            builder.SetNegativeButton("Nevermind", NoAction);

            AlertDialog dialog = builder.Create();
            dialog.Show();
        }

        //Must functions for AlertDialog
        public void YesAction(object sender, DialogClickEventArgs e)
        {
            //Delete item and update adapter
            SQLiteHandler.Instance.DeleteState(statesList[lastPos]);
            statesList.RemoveAt(lastPos);
            stateAdapter.NotifyDataSetChanged();
            Toast.MakeText(this, "Deleted state", ToastLength.Short).Show();
        }
        public void NoAction(object sender, DialogClickEventArgs e)
        {
        }

        //update adapter after changes
        protected override void OnResume()
        {
            base.OnResume();
            if (stateAdapter != null)
            {
                stateAdapter.NotifyDataSetChanged();
            }
        }
    }
}