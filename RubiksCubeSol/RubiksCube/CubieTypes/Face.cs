using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using RubiksCube.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RubiksCube
{
    class Face : Cubie
    {
        public Face(Vector xyz, Color[] cxyz) : base(xyz, cxyz)
        {
        }

        public Face(int x, int y, int z, Color cx, Color cy, Color cz) : base(x, y, z, cx, cy, cz)
        {
        }
    }
}