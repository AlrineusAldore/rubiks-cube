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
        Cubie[,,] cubies { get; set; }

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

                        //-1 to convert from 0->2 to -1->1
                        cubies[x, y, z] = new Cubie(x-1, y-1, z-1, cx, cy, cz);
                    }
                }
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

        //maybe not needed since im not using console (?)
        
        public string ToPrettyString()
        {
            /* end result:
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
    }
}