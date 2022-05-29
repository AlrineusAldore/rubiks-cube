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
    class Face
    {
        public Square[,] squares { get; private set; }

        //shouldFill - if false, empty out the color of the side & corners squares (make them gray for user to fill out)
        public Face(Color color, bool shouldFill)
        {
            squares = new Square[3, 3];
            if (!shouldFill)
            {
                squares[1, 1] = new Square(color); //initialize middle square
                color = Color.empty; //for empty gray squares
            }

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    //if shouldn't fill, then initialize empty squares except middle one which is normal 
                    if (!shouldFill && i == 1 && j == 1)
                        continue;
                    squares[i, j] = new Square(color);
                }
            }
        }

        public Color GetCenterColor()
        {
            return squares[1, 1].color;
        }
    }
}