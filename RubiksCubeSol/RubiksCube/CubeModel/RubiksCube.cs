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
    class RubiksCube
    {
        //right +x red, left -x orange
        //up +y yellow, down -y white
        //front +z blue, back -z green
        public Cubie[,,] cubies { get; set; }

        public RubiksCube()
        {
            cubies = new Cubie[3, 3, 3];

            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    for (int z = 0; z < 3; z++)
                    {
                        Color cx = Color.none;
                        Color cy = Color.none;
                        Color cz = Color.none;
                        switch(x-1)
                        {
                            case 1:
                                cx = Color.red;
                                break;
                            case -1:
                                cx = Color.orange;
                                break;
                        }
                        switch(y-1)
                        {
                            case 1:
                                cy = Color.yellow;
                                break;
                            case -1:
                                cy = Color.white;
                                break;
                        }
                        switch(z-1)
                        {
                            case 1:
                                cz = Color.blue;
                                break;
                            case -1:
                                cz = Color.green;
                                break;
                        }

                        int[] arr = new int[3] { x, y, z};
                        switch(arr.Count(s => s == 1)) //counts the number of ones
                        {
                            //-1 to convert from 0->2 to -1->1
                            case 0:
                                cubies[x, y, z] = new Corner(x - 1, y - 1, z - 1, cx, cy, cz);
                                break;
                            case 1:
                                cubies[x, y, z] = new Edge(x - 1, y - 1, z - 1, cx, cy, cz);
                                break;
                            case 2:
                                cubies[x, y, z] = new Center(x - 1, y - 1, z - 1, cx, cy, cz);
                                break;
                            default:
                                cubies[x, y, z] = new Cubie(x - 1, y - 1, z - 1, cx, cy, cz);
                                break;
                        }
                    }
                }
            }
        }

        public RubiksCube(string cubeStr) : this()
        {
            for (int i = 0; i < 9; i++)
            {
                //Set all y colors of all top cubies (y=2)
                // x = 0,1,2,0,1,2,0,1,2
                // y = 2,2,2,2,2,2,2,2,2
                // z = 0,0,0,1,1,1,2,2,2
                cubies[i % 3, 2, i / 3].colors[1] = CharToColor(cubeStr[0]); //set to first char
                cubeStr = cubeStr.Remove(0, 1); //Remove first char
                //   y->y->y
                // ->y->y->y
                // ->y->y->y
            }
            for (int c = 0; c < 4; c++)
            {
                //x side z side (axes)
                int xs = 1, zs = 1;
                //get const 0 or 2 (-1 or 1, deciding direction)
                switch (c)
                {
                    case 0: //left side
                        xs = 0;
                        break;
                    case 1: //front side
                        zs = 2;
                        break;
                    case 2: //right side
                        xs = 2;
                        break;
                    case 3: //back side
                        zs = 0;
                        break;
                }

                if (c % 2 == 0) //true -> x axis
                {
                    for (int i = 0; i < 9; i++)
                    {
                        // x = (first loop 0, second loop 2)
                        // y = 0,0,0,1,1,1,2,2,2
                        // z = 0,1,2,0,1,2,0,1,2
                        cubies[xs, i / 3, i % 3].colors[0] = CharToColor(cubeStr[0]); //set to first char
                        cubeStr = cubeStr.Remove(0, 1); //Remove first char
                        //   (-+)x->(-+)x->(-+)x
                        // ->(-+)x->(-+)x->(-+)x
                        // ->(-+)x->(-+)x->(-+)x
                    }
                }
                else // false -> z axis
                {
                    for (int i = 0; i < 9; i++)
                    {
                        // x = 0,1,2,0,1,2,0,1,2
                        // y = 0,0,0,1,1,1,2,2,2
                        // z = (first loop 2, second loop 0)
                        cubies[i % 3, i / 3, zs].colors[2] = CharToColor(cubeStr[0]); //set to first char
                        cubeStr = cubeStr.Remove(0, 1); //Remove first char
                        //   (+-)z->(+-)z->(+-)z
                        // ->(+-)z->(+-)z->(+-)z
                        // ->(+-)z->(+-)z->(+-)z
                    }
                }
            }
            for (int i = 0; i < 9; i++)
            {
                //Get all y colors of all bottom cubies (y=0)
                // x = 0,1,2,0,1,2,0,1,2
                // y = 0,0,0,0,0,0,0,0,0
                // z = 0,0,0,1,1,1,2,2,2
                cubies[i % 3, 0, i / 3].colors[1] = CharToColor(cubeStr[0]); //set to first char
                cubeStr = cubeStr.Remove(0, 1); //Remove first char
                //   (-)y->(-)y->(-)y
                // ->(-)y->(-)y->(-)y
                // ->(-)y->(-)y->(-)y
            }
        }



        public override string ToString()
        {
            string str = "";

            for (int i = 0; i < 9; i++)
            {
                //Get all y colors of all top cubies (y=2)
                // x = 0,1,2,0,1,2,0,1,2
                // y = 2,2,2,2,2,2,2,2,2
                // z = 0,0,0,1,1,1,2,2,2
                str += cubies[i%3, 2, i/3].colors[1].ToString()[0];
                //   y->y->y
                // ->y->y->y
                // ->y->y->y
            }
            for (int c = 0; c < 4; c++)
            {
                //x side z side (axes)
                int xs = 1, zs = 1;
                //get const 0 or 2 (-1 or 1, deciding direction)
                switch(c)
                {
                    case 0: //left side
                        xs = 0;
                        break;
                    case 1: //front side
                        zs = 2;
                        break;
                    case 2: //right side
                        xs = 2;
                        break;
                    case 3: //back side
                        zs = 0;
                        break;
                }

                if (c % 2 == 0) //true -> x axis
                {
                    for (int i = 0; i < 9; i++)
                    {
                        // x = (first loop 0, second loop 2)
                        // y = 0,0,0,1,1,1,2,2,2
                        // z = 0,1,2,0,1,2,0,1,2
                        str += cubies[xs, i/3, i%3].colors[0].ToString()[0];
                        //   (-+)x->(-+)x->(-+)x
                        // ->(-+)x->(-+)x->(-+)x
                        // ->(-+)x->(-+)x->(-+)x
                    }
                }
                else // false -> z axis
                {
                    for (int i = 0; i < 9; i++)
                    {
                        // x = 0,1,2,0,1,2,0,1,2
                        // y = 0,0,0,1,1,1,2,2,2
                        // z = (first loop 2, second loop 0)
                        str += cubies[i%3, i/3, zs].colors[2].ToString()[0];
                        //   (+-)z->(+-)z->(+-)z
                        // ->(+-)z->(+-)z->(+-)z
                        // ->(+-)z->(+-)z->(+-)z
                    }
                }
            }
            for (int i = 0; i < 9; i++)
            {
                //Get all y colors of all bottom cubies (y=0)
                // x = 0,1,2,0,1,2,0,1,2
                // y = 0,0,0,0,0,0,0,0,0
                // z = 0,0,0,1,1,1,2,2,2
                str += cubies[i%3, 0, i/3].colors[1].ToString()[0];
                //   (-)y->(-)y->(-)y
                // ->(-)y->(-)y->(-)y
                // ->(-)y->(-)y->(-)y
            }

            

            return str;
        }

        public string GetFront(bool isFront)
        {
            int z = 0;
            if (isFront)
                z = 2;
            string front = "";
            for (int i = 0; i < 9; i++)
            {
                //Get all f colors of all front cubies (z=2)
                // x = 0,1,2,0,1,2,0,1,2
                // y = 2,2,2,1,1,1,0,0,0
                // z = 2,2,2,2,2,2,2,2,2
                front += cubies[i % 3, 2 - (i / 3), z].colors[2].ToString()[0];
                //   f->f->f
                // ->f->f->f
                // ->f->f->f
            }

            return front;
        }
        public string GetRight(bool isRight)
        {
            int x = 0;
            if (isRight)
                x = 2;
            string str = "";
            for (int i = 0; i < 9; i++)
            {
                str += cubies[x, i / 3, i % 3].colors[0].ToString()[0];
                //   (-+)x->(-+)x->(-+)x
                // ->(-+)x->(-+)x->(-+)x
                // ->(-+)x->(-+)x->(-+)x
            }

            return str;
        }
        public string GetUp(bool isUp)
        {
            int y = 0;
            if (isUp)
                y = 2;
            string str = "";
            for (int i = 0; i < 9; i++)
            {
                str += cubies[i % 3, y, i / 3].colors[1].ToString()[0];
                //   (-)y->(-)y->(-)y
                // ->(-)y->(-)y->(-)y
                // ->(-)y->(-)y->(-)y
            }

            return str;
        }


        public string ToPrettyString()
        {
            /* TODO end result:
                yyy
                yyy
                yyy
            ooo bbb rrr ggg
            ooo bbb rrr ggg
            ooo bbb rrr ggg
                www
                www
                www
            */
            string pretty = "";
            string original = ToString();

            /* currently it's:
                
            yyyyyyyyy
            ooooooooo
            bbbbbbbbb
            rrrrrrrrr
            ggggggggg
            wwwwwwwww
            */
            for (int i = 0; i < 6*9; i+=9)
            {
                pretty += original.Substring(i, 9) + "\n";
            }
            
            /*
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    for (int z = 0; z < 3; z++)
                    {
                        Color[] colors = new Color[3];

                        for (int i = 0; i < 3; i++)
                        {
                            colors[i] = cubies[x, y, z].colors[i];

                            if (colors[i] != Color.none)
                            {

                            }
                        }
                    }
                }
            }*/

            return pretty;
        }

        public Color CharToColor(char c)
        {
            Color col;

            switch(c)
            {
                case 'y':
                    col = Color.yellow;
                    break;
                case 'o':
                    col = Color.orange;
                    break;
                case 'b':
                    col = Color.blue;
                    break;
                case 'r':
                    col = Color.red;
                    break;
                case 'g':
                    col = Color.green;
                    break;
                case 'w':
                    col = Color.white;
                    break;
                default:
                    col = Color.none;
                    break;
            }

            return col;
        }
        //Flatten 3d array of cubies to 1d array
        private Cubie[] To1D()
        {
            List<Cubie> pieces = new List<Cubie>();
            foreach (Cubie c in cubies)
            {
                if (c == null)
                    break;
                pieces.Add(c);
            }
            return pieces.ToArray();
        }

        //After rotation via matrices, Cubies have correct position vectors but their index is incorrect so we make their new index the pos vector
        private void RearrangeCubies()
        {
            Cubie[,,] arranged = new Cubie[3, 3, 3];

            //Change cubies' index to pos vector
            foreach(Cubie c in cubies)
            {
                arranged[c.position.x + 1, c.position.y + 1, c.position.z + 1] = c;
            }

            cubies = arranged;
        }

        public Cubie[] GetFace(Vector face)
        {
            System.Diagnostics.Debug.Assert(face.Count(0) == 2); //make sure the vector represents a face
            List<Cubie> pieces = new List<Cubie>();

            //instead of triple loop
            foreach (Cubie c in cubies)
            {
                if (c == null)
                    break;
                if (face.GetDotProduct(c.position) > 0) //only true for cubies in the same face (simple mafs)
                    pieces.Add(c);
            }

            return pieces.ToArray();
        }

        public Cubie[] GetSlice(Vector plane)
        {
            System.Diagnostics.Debug.Assert(plane.Count(0) == 1); //make sure the vector represents a plane
            List<Cubie> pieces = new List<Cubie>();
            
            //Get index of the coord that is equal to 0
            int i = 0;
            for (i = 0; i < 3; i++)
            {
                if (plane[i] == 0)
                    break;
            }

            //instead of triple loop
            foreach (Cubie c in cubies)
            {
                if (c == null)
                    break;
                if (c.position[i] == 0) //If both cube and slice remain on the same plane
                    pieces.Add(c);
            }

            return pieces.ToArray();
        }

        //direction can be either an axis or a plane
        private void RotateSlice(Vector direction, Matrix matrix)
        {
            Cubie[] pieces;
            
            if (direction.Count(0) == 2) //its a face
                pieces = GetFace(direction);
            else //There is only 1 zero and its a plane
                pieces = GetSlice(direction);

            RotateCubies(pieces, matrix);
        }

        //Get the set of Cubies that need to rotate, and then rotate them
        private void RotateCubies(Cubie[] pieces, Matrix matrix)
        {
            foreach (var piece in pieces)
                piece.rotate(matrix);

            RearrangeCubies();
        }

        //Opposite to the axes means Clockwise is opposite too (so it turns to counter-clockwise and vice versa)

        //Rotate Faces
        public void R() { RotateSlice(Constants.RIGHT, Constants.ROT_YZ_CW); }
        public void Ri() { RotateSlice(Constants.RIGHT, Constants.ROT_YZ_CC); }
        public void L() { RotateSlice(Constants.LEFT, Constants.ROT_YZ_CC); }
        public void Li() { RotateSlice(Constants.LEFT, Constants.ROT_YZ_CW); }
        public void U() { RotateSlice(Constants.UP, Constants.ROT_XZ_CW); }
        public void Ui() { RotateSlice(Constants.UP, Constants.ROT_XZ_CC); }
        public void D() { RotateSlice(Constants.DOWN, Constants.ROT_XZ_CC); }
        public void Di() { RotateSlice(Constants.DOWN, Constants.ROT_XZ_CW); }
        public void F() { RotateSlice(Constants.FRONT, Constants.ROT_XY_CW); }
        public void Fi() { RotateSlice(Constants.FRONT, Constants.ROT_XY_CC); }
        public void B() { RotateSlice(Constants.BACK, Constants.ROT_XY_CC); }
        public void Bi() { RotateSlice(Constants.BACK, Constants.ROT_XY_CW); }

        //Rotate Middle slices
        public void M() { RotateSlice(Constants.UP+Constants.FRONT, Constants.ROT_YZ_CC); }
        public void Mi() { RotateSlice(Constants.UP+Constants.FRONT, Constants.ROT_YZ_CW); }
        public void E() { RotateSlice(Constants.RIGHT+Constants.FRONT, Constants.ROT_XZ_CC); }
        public void Ei() { RotateSlice(Constants.RIGHT+Constants.FRONT, Constants.ROT_XZ_CW); }
        public void S() { RotateSlice(Constants.RIGHT+Constants.UP, Constants.ROT_XY_CW); }
        public void Si() { RotateSlice(Constants.RIGHT+Constants.UP, Constants.ROT_XY_CC); }

        //Rotate Whole Cube
        public void X() { RotateCubies(To1D(), Constants.ROT_YZ_CW); }
        public void Xi() { RotateCubies(To1D(), Constants.ROT_YZ_CC); }
        public void Y() { RotateCubies(To1D(), Constants.ROT_XZ_CW); }
        public void Yi() { RotateCubies(To1D(), Constants.ROT_XZ_CC); }
        public void Z() { RotateCubies(To1D(), Constants.ROT_XY_CW); }
        public void Zi() { RotateCubies(To1D(), Constants.ROT_XY_CC); }
    }
}