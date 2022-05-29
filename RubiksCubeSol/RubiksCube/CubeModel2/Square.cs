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
    class Square
    {
        public Color color { get; private set; }
        public char c { get; private set; } //First char of color

        public Square(Color color)
        {
            this.color = color;
            c = color.ToString()[0];
        }
    }
}