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
        StateAdapter stateAdapter;
        ListView lvStates;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your application here
            SetContentView(Resource.Layout.states_layout);


        }
        public void OnItemClick(AdapterView parent, View view, int position, long id)
        {
            throw new NotImplementedException();
        }
    }
}