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
using System.IO;
using SQLite;

namespace RubiksCube
{
    public class SQLiteHandler
    {
        private static string DB_NAME = "Rubiks.db";
        private string path;
        private SQLiteConnection db;
        public bool isNewDb { get; set; }

        public SQLiteHandler()
        {
            //Here the database will reside
            path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), DB_NAME);
            isNewDb = !File.Exists(path); //get if it's a new db or not

            db = new SQLiteConnection(path); //Creates the db connection object
            //create tables if they dont exist
            db.CreateTable<User>(); 
            db.CreateTable<State>();
        }

        public void Open()
        {
            db = new SQLiteConnection(path); //Creates the db connection object
        }
        public void Close()
        {
            db.Close(); //Closes connection to db
        }

        public void InsertUser(string username, string password)
        {
            User user = new User(username, password);
            db.Insert(user, typeof(User));
        }

        public void ChangePassword(User user)
        {
            db.Update(user); //uses primary key to change user's pass (i think)
        }

        public List<State> GetAllStates(string username)
        {
            //Get all states of user
            string strSql = string.Format("SELECT * FROM states WHERE username='" + username + "'");
            var states = db.Query<State>(strSql);
            
            return states;
        }

        public void InsertState(string cubeStr, string username)
        {
            State state = new State(cubeStr, username);
            db.Insert(state, typeof(State));
        }

        public void DeleteState(State state)
        {
            db.Delete(state); //uses primary key to delete state
        }
    }
}