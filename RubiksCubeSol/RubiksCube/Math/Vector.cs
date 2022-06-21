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
    public class Vector
    {
        public int x { get; set; }
        public int y { get; set; }
        public int z { get; set; }

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

        public Vector GetCopy()
        {
            return new Vector(x, y, z);
        }

        //Counts how many times number appears in vector coords
        public int Count(int num)
        {
            int count = 0;
            if (x == num)
                count++;
            if (y == num)
                count++;
            if (z == num)
                count++;

            return count;
        }

        //Overloading addition operator
        public static Vector operator +(Vector v1, Vector v2)
        {
            return new Vector(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        }

        //Overloading subtraction operator
        public static Vector operator -(Vector v1, Vector v2)
        {
            return new Vector(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        }

        //Overloading [index] operator
        public int this[int key]
        {
            get => GetValue(key);
            //set => SetValue(key, value);
        }

        private int GetValue(int n)
        {
            int[] arr = new int[] { x, y, z };
            return arr[n];
        }

        public override string ToString()
        {
            return "[" + x +", " + y + ", " + z + "]";
        }
    }
}