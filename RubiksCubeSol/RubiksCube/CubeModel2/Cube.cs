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
    class Cube
    {
        public Face[] faces { get; private set; }

        public Cube(bool shouldFill)
        {
            faces = new Face[6];
            for (int i = 0; i < 6; i++)
                faces[i] = new Face((Color)i, shouldFill);
        }

        public void Rotate(bool isClockwise, Color color)
        {
            //Get the face with the right center color
            for (int i = 0; i < 6; i++)
            {
                if (faces[i].GetCenterColor() == color)
                {
                    for (int j = 0; j < 2; j++)
                    {

                        // Swap elements of each cycle
                        // in clockwise direction
                        Square temp = faces[i].squares[0, j];
                        faces[i].squares[0, j] = faces[i].squares[2 - j, 0];
                        faces[i].squares[2 - j, 0] = faces[i].squares[2, 2 - j];
                        faces[i].squares[2, 2 - j] = faces[i].squares[j, 2];
                        faces[i].squares[j, 2] = temp;
                    }
                }
            }
        }
    }
}