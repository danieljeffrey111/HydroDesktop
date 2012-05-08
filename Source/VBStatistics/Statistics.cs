using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
//using Extreme.Statistics;
//using Extreme.Statistics.Distributions;

namespace VBStatistics
{
    public class Statistics
    {
        //public static DataTable getPearsonCorrCoeffs(DataTable data)
        //{
        //    DataTable dtCoeff = new DataTable();
        //    dtCoeff.Columns.Add("Variable1");
        //    dtCoeff.Columns.Add("Variable2");
        //    dtCoeff.Columns.Add("PearsonCorrelation", Type.GetType("System.Decimal"));

        //    if ((data == null) || (data.Rows.Count == 0) || (data.Columns.Count < 3))
        //    {
        //        return dtCoeff; 
        //    }
        //    Hashtable htVarValues = new Hashtable();
        //    double[] arColumn;

        //    for (int intColumnIndex = 2; intColumnIndex < data.Columns.Count; intColumnIndex++)
        //    {
        //        arColumn = new double[data.Rows.Count];
        //        int count = 0;
        //        foreach (DataRow dr in data.Rows)
        //        {
        //            arColumn.SetValue(Convert.ToDouble(dr[intColumnIndex].ToString()), count);
        //            count++;
        //        }
        //        htVarValues.Add(intColumnIndex, arColumn);
        //    }
        //    string strCoeff = "";
        //    int intLength = 0;
        //    for (int intFirstColumnIndex = 2; intFirstColumnIndex < (data.Columns.Count - 1); intFirstColumnIndex++)
        //    {
        //        double[] var1Values = (double[])htVarValues[intFirstColumnIndex];
        //        for (int intSecondColumnIndex = (intFirstColumnIndex + 1); intSecondColumnIndex < data.Columns.Count; intSecondColumnIndex++)
        //        {
        //            double[] var2Values = (double[])htVarValues[intSecondColumnIndex];
        //            double coeff = Stats.Correlation(var1Values, var2Values);
        //            coeff = Math.Abs(coeff);
        //            if (coeff < 0.001)
        //            {
        //                coeff = 0;
        //            }
        //            strCoeff = coeff.ToString();
        //            intLength = strCoeff.Length;
        //            if (intLength > 8)
        //            {
        //                intLength = 8;
        //            }
        //            strCoeff = strCoeff.Substring(0, intLength);
        //            coeff = Convert.ToDouble(strCoeff);
        //            DataRow dr = dtCoeff.NewRow();
        //            dr["Variable1"] = data.Columns[intFirstColumnIndex].ColumnName;
        //            dr["Variable2"] = data.Columns[intSecondColumnIndex].ColumnName;
        //            dr["PearsonCorrelation"] = coeff;
        //            dtCoeff.Rows.Add(dr);
        //        }
        //    }
        //    dtCoeff = sortTable(dtCoeff, "PearsonCorrelation", "DESC");
        //    return dtCoeff;
    //    }
    //    public static DataTable sortTable(DataTable dtUnsorted, string sortColumn, string sortDirection)
    //    {
    //        string sortFormat = "{0} {1}";
    //        dtUnsorted.DefaultView.Sort = string.Format(sortFormat, sortColumn, sortDirection);
    //        return dtUnsorted.DefaultView.Table;
    //    }

    //    public static double Correlation(double[] deparray, double[] vararray)
    //    {
    //        double correlation = Stats.Correlation(deparray, vararray);
            
    //        return correlation;
    //    }

        

    //    //public static object descriptiveStats(double[] data)
    //    //{
    //    //    NumericalVariable descstats = new NumericalVariable(data);
    //    //    return (object) descstats;
    //    //}

    //    public static double PExceed(double prediction, double threshold, double se)
    //    {
    //        return Extreme.Statistics.Distributions.NormalDistribution.DistributionFunction
    //            (prediction, threshold, se) * 100.0d;
    //    }



    //    /// <summary>
    //    /// return a 2-tailed p-value for the t distribution of the pearson correlation score
    //    /// <param name="pcoeff">pearson score for the variable relative to the dependent variable</param>
    //    /// <param name="n">number of observations minus 2</param>
    //    /// <returns></returns>
    //    public static double Pvalue4Correlation(double pcoeff, int n)
    //    {
    //        double Pval = double.NaN;

    //        int degreeF = n - 2;
    //        double tscore = pcoeff / Math.Sqrt((1 - Math.Pow(pcoeff, 2.0d)) / degreeF);
    //        StudentTDistribution tDist = new StudentTDistribution(degreeF);

    //        if (tscore < 0)
    //        {
    //            Pval = 2 * tDist.DistributionFunction(tscore);
    //        }
    //        else
    //        {
    //            Pval = 2 * (1 - tDist.DistributionFunction(tscore));
    //        }

    //        return Pval;
    //    }



    }
       
}
