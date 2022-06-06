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
using RubiksCube.Math;
using System.Diagnostics;

namespace RubiksCube
{
    class Cubie
    {
        public Vector position { get; set; }
        public Color[] colors { get; set; }

        public Cubie(int x, int y, int z, Color cx, Color cy, Color cz)
        {
            position = new Vector(x, y, z);
            colors = new Color[3] { cx, cy, cz };
        }
        public Cubie(Vector xyz, Color[] cxyz)
        {
            position = xyz;
            colors = cxyz;
        }

        public bool IsSame(Cubie other)
        //Compares this cubie to another cubie by color
        // TODO: Might want to check diff colors positions
        {
            if (other != null)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (colors[i] != other.colors[i])
                        return false;
                }

                return true;
            }

            return false;
        }

        public void rotate(Matrix matrix)
        {
            Vector prev = position.GetCopy();
            position = matrix.MultiplyByVector(position); // matrix vector multiplication for rotation of position

            //The difference is parallel to a plane or axis 
            Vector diff = position - prev;

            if (diff.Count(0) == 0) //color didn't change
                return;

            //count(0) == 1 -> It's parallel to an axis -> it represents the two other axes we wanna swap
            //count(0) == 2 -> It's parallel to a plane -> rotate it by the matrix, add the result + the original diff, and now it represents the two axes we wanna swap.
            if (diff.Count(0) == 2)
            {
                Vector res = matrix.MultiplyByVector(diff);
                diff = diff + res;
            }

            System.Diagnostics.Debug.Assert(diff.Count(0) == 1, "Problem in Cubie.Rotate(), diff.count(0) != 1");

            //Colors start as original
            Color cx = colors[0];
            Color cy = colors[1];
            Color cz = colors[2];

            //Colors swap pairs of non-zero values in axes of difference vector
            if (diff.x != 0 && diff.y != 0)
            {
                cx = colors[1];
                cy = colors[0];
            }
            else if (diff.x != 0 && diff.z != 0)
            {
                cx = colors[2];
                cz = colors[0];
            }
            else if (diff.y != 0 && diff.z != 0)
            {
                cy = colors[2];
                cz = colors[1];
            }

            colors = new Color[3] { cx, cy, cz }; //update colors
        }
    }
}