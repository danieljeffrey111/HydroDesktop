using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace VBStatistics
{
    public class Utility
    {
        /// <summary>
        /// Returns entire data table as a 2-d double array
        /// </summary>
        /// <param name="dt">Datatable to convert</param>
        /// <returns></returns>
        public static double[][] ArrayFromTable(DataTable dt)
        {
            int numCols = dt.Columns.Count;

            int[] columnIdx = new int[numCols];
            for (int i = 0; i < numCols; i++)
                columnIdx[i] = i;

            return ArrayFromTable(dt, columnIdx);
        }


        public static double[][] ArrayFromTable(DataTable dt, string[] columns)
        {
            int[] columnIdx = new int[columns.Length];
            for (int i = 0; i < columns.Length; i++)
            {
                columnIdx[i] = dt.Columns[""].Ordinal;
            }

            return ArrayFromTable(dt, columnIdx);

        }

        public static double[][] ArrayFromTable(DataTable dt, int[] columns)
        {
            if (dt == null)
                return null;

            int numRows = dt.Rows.Count;
            int[] rows = new int[numRows];

            double[][] data = new double[numRows][];

            for (int i = 0; i < numRows; i++)
                rows[i] = i;

            return ArrayFromTable(dt, columns, rows);

        }

        /// <summary>
        /// Returns a 2-d double array of specified rows and columns from datatable
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="columns"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        public static double[][] ArrayFromTable(DataTable dt, int[] columns, int[] rows)
        {
            if (dt == null)
                return null;

            int numRows = rows.Length;
            double[][] data = new double[numRows][];

            for (int i = 0; i < numRows; i++)
            {
                data[i] = new double[columns.Length];
                for (int j = 0; j < columns.Length; j++)
                {
                    data[i][j] = Convert.ToDouble(dt.Rows[rows[i]][columns[j]]);
                }
            }

            return data;
        }



        public static double[] GetColumnFromTable(DataTable dt, int column)
        {
            return GetColumnFromTable(dt, column, null);
        }

        public static double[] GetColumnFromTable(DataTable dt, int column, int[] rows)
        {
            int numRows;
            if (rows != null)
                numRows = rows.Length;
            else
                numRows = dt.Rows.Count;

            double[] data = new double[numRows];

            for (int i = 0; i < numRows; i++)
            {
                if (rows != null)
                    data[i] = Convert.ToDouble(dt.Rows[rows[i]][column]);
                else
                    data[i] = Convert.ToDouble(dt.Rows[i][column]);
            }

            return data;
        }


        public static long Choose(long n, long k)
        {
            long result = 1;

            for (long i = Math.Max(k, n - k) + 1; i <= n; ++i)
                result *= i;

            for (long i = 2; i <= Math.Min(k, n - k); ++i)
                result /= i;

            return result;
        }
    }
}
