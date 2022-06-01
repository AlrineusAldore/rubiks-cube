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
    public class Matrix
    {
        int[] elems;

        public Matrix(int[] elems)
        {
            this.elems = elems;

            //dont accept a non 3x3 matrix
            if (elems.Length != 9)
            {
                Console.WriteLine("Error! You entered " + elems.Length + "elements instead of 9");
            }
        }

        //Matrix-Vector multiplication
        public Vector MultiplyByVector(Vector other)
        {
            int[] coords = new int[3];
            int[][] rows = GetRows();

            //Each coordinate of new vector is dot product of given vector and current matrix row treated as vector
            for (int i = 0; i < 3; i++)
            {
                Vector rowVec = new Vector(rows[i]);
                coords[i] = other.GetDotProduct(rowVec);
            }

            return new Vector(coords);
        }

        //Matrix-Matrix multiplication
        public Matrix MultiplyByMatrix(Matrix other)
        {
            //Get elements as dot products of this matrix' rows and other matrix' cols
            int[] elements = new int[9];
            int[][] rows = GetRows();
            int[][] cols = other.GetColumns();

            for (int i = 0; i < 3; i++) //rows
            {
                Vector row = new Vector(rows[i]);

                for (int j = 0; j < 3; j++) //cols
                {
                    Vector col = new Vector(cols[j]);

                    elements[i * 3 + j] = row.GetDotProduct(col); //quick mafs
                }
            }

            return new Matrix(elements);
        }

        public int[][] GetRows()
        {
            int[][] rows = new int[3][];

            for (int i = 0; i < 3; i++)
            {
                rows[i] = new int[3];

                for (int j = 0; j < 3; j++)
                    rows[i][j] = elems[i*3 + j]; //store rows
            }

            return rows;
        }

        public int[][] GetColumns()
        {
            int[][] columns = new int[3][];

            for (int i = 0; i < 3; i++)
            {
                columns[i] = new int[3];

                for (int j = 0; j < 3; j++)
                    columns[j][i] = elems[i * 3 + j]; //store columns
            }

            return columns;
        }

        public override string ToString()
        {
            string str = "[{0}, {1}, {2},\n" +
                         " {3}, {4}, {5},\n" +
                         " {6}, {7}, {8}]";
            str = string.Format(str, elems);

            return str;
        }
    }
}