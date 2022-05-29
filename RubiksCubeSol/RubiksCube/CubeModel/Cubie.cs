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
using System.Numerics;

namespace RubiksCube
{
    class Cubie
    {
        public int[] position { get; set; }
        public Color[] colors { get; set; }

        public Cubie(int x, int y, int z, Color cx, Color cy, Color cz)
        {
            position = new int[3] { x, y, z };
            colors = new Color[3] { cx, cy, cz };
        }
        public Cubie(int[] xyz, Color[] cxyz)
        {
            position = xyz;
            colors = cxyz;
        }

        public bool IsSame(Cubie cu)
        //Compares this cubie to another cubie by color
        // TODO: Might want to check diff colors positions
        {
            if (cu != null)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (colors[i] != cu.colors[i])
                        return false;
                }

                return true;
            }

            return false;
        }

        public void rotate()
        {
            Vector v;
        }
    }
}