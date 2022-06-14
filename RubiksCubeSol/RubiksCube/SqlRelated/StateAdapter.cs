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
    class StateAdapter : BaseAdapter<State>
    {
        public Context context { get; set; }
        public List<State> states { get; set; }

        public StateAdapter(Context context, List<State> states)
        {
            this.context = context;
            this.states = states;
        }


        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            //Inflate state_row (from StatesActivity) to turn it into a view object (turn xml file to view object)
            LayoutInflater layoutInflater = ((StatesActivity)context).LayoutInflater;
            View view = layoutInflater.Inflate(Resource.Layout.state_row, parent, false);

            TextView tvStateCubeStr = view.FindViewById<TextView>(Resource.Id.tvStateCubeStr);
            TextView tvStateId = view.FindViewById<TextView>(Resource.Id.tvStateId);

            State temp = states[position];
            if (temp != null)
            {
                tvStateCubeStr.Text = temp.cubeStr;
                tvStateId.Text = "" + temp.id;
            }

            /* some stuff that generated automatically for some reason and i dont wanna delete just yet
            var view = convertView;
            StateAdapterViewHolder holder = null;

            if (view != null)
                holder = view.Tag as StateAdapterViewHolder;

            if (holder == null)
            {
                holder = new StateAdapterViewHolder();
                var inflater = context.GetSystemService(Context.LayoutInflaterService).JavaCast<LayoutInflater>();
                //replace with your item and your holder items
                //comment back in
                //view = inflater.Inflate(Resource.Layout.item, parent, false);
                //holder.Title = view.FindViewById<TextView>(Resource.Id.text);
                view.Tag = holder;
            }


            //fill in your items
            //holder.Title.Text = "new text here"; */

            return view;
        }

        //Implemented
        public override int Count
        {
            get
            {
                return states.Count;
            }
        }

        public override State this[int position]
        {
            get { return states[position]; }
        }
    }

    class StateAdapterViewHolder : Java.Lang.Object
    {
        //Your adapter views to re-use
        //public TextView Title { get; set; }
    }
}