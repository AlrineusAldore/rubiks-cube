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
    public class StatesActivity : Activity, ListView.IOnItemClickListener
    {
        public static List<State> statesList { get; set; }
        StateAdapter stateAdapter;
        ListView lvStates;
        SQLiteHandler sqlHandler;
        string username;

        Dialog d;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your application here
            SetContentView(Resource.Layout.states_layout);

            //Get username from intent and make sure its not empty
            username = Intent.GetStringExtra("username") ?? "";
            System.Diagnostics.Debug.Assert(username != "", "username empty for saved states activity");

            //Get all states from database and create the states adapter with them
            sqlHandler = new SQLiteHandler();
            statesList = sqlHandler.GetAllStates(username);
            stateAdapter = new StateAdapter(this, statesList);

            lvStates = FindViewById<ListView>(Resource.Id.lvStates);
            lvStates.Adapter = stateAdapter; //adds the adapter to the listview
            lvStates.OnItemClickListener = this;
        }

        public void OnItemClick(AdapterView parent, View view, int position, long id)
        {
            throw new NotImplementedException();
        }
    }
}