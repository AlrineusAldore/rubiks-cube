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
    class Matrix
    {
        int[] elems;

        public Matrix(int[] elems)
        {
            this.elems = elems;
            if (elems.Length != 9)
                //dont accept matrix
        }
    }
}