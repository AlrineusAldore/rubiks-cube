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
    [Table("Users")]
    class User
    {
        [PrimaryKey]
        public string username { get; set; }
        public string password { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)] //One to many relationship with states (1 user has many states)
        public List<State> states { get; set; }

        public User()
        {
        }
        public User(string username, string password)
        {
            this.username = username;
            this.password = password;
        }
        public void setUser(string username, string password)
        {
            this.username = username;
            this.password = password;
        }
    }
}