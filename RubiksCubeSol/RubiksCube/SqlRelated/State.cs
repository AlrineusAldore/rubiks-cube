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
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace RubiksCube
{
    [Table("States")]
    public class State
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int id { get; set; }
        public string cubeStr { get; set; }

        [ForeignKey(typeof(User))]
        public string username { get; set; }

        [ManyToOne]      // Many to one relationship with User (many states have 1 shared user)
        public User user { get; set; }


        public State()
        {
        }
        public State(string cubeStr, string username)
        {
            this.cubeStr = cubeStr;
            this.username = username;
        }
        public void setState(string cubeStr, string username)
        {
            this.cubeStr = cubeStr;
            this.username = username;
        }
    }
}