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
    //Singleton class (global class with 1 instance)
    public sealed class SQLiteHandler
    {
        private static readonly SQLiteHandler instance = new SQLiteHandler();

        private static string DB_NAME;
        private string path;
        private SQLiteConnection db;
        private bool isNewDb;

        public static SQLiteHandler Instance
        {
            get { return instance; }
        }
        static SQLiteHandler()
        { }
        private SQLiteHandler()
        {
            //Here the database will reside
            DB_NAME = "Rubiks.db";
            var folder = System.Environment.SpecialFolder.Personal;
            var folderPath = System.Environment.GetFolderPath(folder);
            path = Path.Combine(folderPath, DB_NAME);
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
        public void InsertUser(User user)
        {
            db.Insert(user, typeof(User));
        }

        public void ChangePassword(User user)
        {
            db.Update(user); //uses primary key to change user's pass (i think)
        }

        public User GetUser(string username)
        {
            User user = null;
            //Get user with given username
            string strSql = string.Format("SELECT * FROM users WHERE username='" + username + "'");
            var users = db.Query<User>(strSql);

            //Only options are 0 or 1 users
            if (users.Count > 0)
                user = users[0];

            return user;
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
        public void InsertState(State state)
        {
            db.Insert(state, typeof(State));
        }

        public void DeleteState(State state)
        {
            db.Delete(state); //uses primary key to delete state
        }
    }
}