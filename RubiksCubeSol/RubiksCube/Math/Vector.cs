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

namespace RubiksCube.Math
{
    class Vector
    {
        int x;
        int y;
        int z;

        public Vector(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public Vector(int[] coords)
        {
            x = coords[0];
            y = coords[1];
            z = coords[2];
        }

        //Returns dot product of vectors
        public int GetDotProduct(Vector other)
        {
            return x * other.x + y * other.y + z * other.z;
        }

        public override string ToString()
        {
            return "[" + x +", " + y + ", " + z + "]";
        }
    }
}