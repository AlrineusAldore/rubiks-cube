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
    //Not literally constants as they are assigned a value in the constructor, but they are not meant to be changed.
    public static class Constants
    {
        //CW is clockwise, CC is counter-clockwise

        //90 degree rotations in the XY plane
        public static Matrix ROT_XY_CW { get; set; }
        public static Matrix ROT_XY_CC { get; set; }

        //90 degree rotations in the XZ plane
        public static Matrix ROT_XZ_CW { get; set; }
        public static Matrix ROT_XZ_CC { get; set; }

        //90 degree rotations in the YZ plane
        public static Matrix ROT_YZ_CW { get; set; }
        public static Matrix ROT_YZ_CC { get; set; }

        //Vectors representing sides
        public static Vector RIGHT { get; set; }
        public static Vector LEFT { get; set; }
        public static Vector UP { get; set; }
        public static Vector DOWN { get; set; }
        public static Vector FRONT { get; set; }
        public static Vector BACK { get; set; }

        public static string NEW_CUBE_STR { get; set; }
        public static string EMPTY_CUBE_STR { get; set; }


        static Constants()
        {
            //XY plane maintains base column vector for Z axis (0,0,1) 
            ROT_XY_CW =  new Matrix(new int[] { 0, 1, 0, 
                                               -1, 0, 0, 
                                                0, 0, 1 });

            ROT_XY_CC =  new Matrix(new int[] { 0,-1, 0, 
                                                1, 0, 0, 
                                                0, 0, 1 });

            //XZ plane maintains base column vector for y axis (0,1,0)
            ROT_XZ_CW =  new Matrix(new int[] { 0, 0,-1, 
                                                0, 1, 0, 
                                                1, 0, 0 });

            ROT_XZ_CC =  new Matrix(new int[] { 0, 0, 1, 
                                                0, 1, 0, 
                                               -1, 0, 0 });

            //YZ plane maintains base column vector for X axis (1,0,0)
            ROT_YZ_CW =  new Matrix(new int[] { 1, 0, 0, 
                                                0, 0, 1, 
                                                0,-1, 0 });

            ROT_YZ_CC =  new Matrix(new int[] { 1, 0, 0, 
                                                0, 0,-1, 
                                                0, 1, 0 });

            RIGHT = new Vector(1, 0, 0);
            LEFT =  new Vector(-1, 0, 0);
            UP =    new Vector(0, 1, 0);
            DOWN =  new Vector(0,-1, 0);
            FRONT = new Vector(0, 0, 1);
            BACK =  new Vector(0, 0,-1);

            NEW_CUBE_STR = "yyyyyyyyyooooooooobbbbbbbbbrrrrrrrrrgggggggggwwwwwwwww";
            EMPTY_CUBE_STR = "nnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnn";
        }
    }
}