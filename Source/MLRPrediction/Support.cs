using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VBTools;

namespace MLRPrediction
{
    public class Support
    {                

        /// <summary>
        /// create a model expression string for display
        /// </summary>
        /// <param name="model"></param>
        /// <param name="depVarname"></param>
        /// <returns>model expression as a string (depvar = i + c(j)x(j)...)</returns>
        static public string BuildModelExpression(Dictionary<string, double> model, string depVarname, string format)
        {
            string numFormat = "";
            if (String.IsNullOrWhiteSpace(format))
                numFormat = "g8";
            else
                numFormat = format;

            //generates the model expression
            string opsign = string.Empty;
            string variable = string.Empty;
            double coeff = double.NaN;
            string line = "";
            
            if (!String.IsNullOrWhiteSpace(depVarname))
                line = depVarname + " = ";

            if (model.Keys.Count < 0) return line;

            foreach (KeyValuePair<string, double> mterm in model)
            {
                variable = mterm.Key;
                coeff = mterm.Value;
                if (coeff < 0) opsign = " - ";
                else opsign = " + ";
                if (variable.Contains("(Intercept)"))
                    line = line + coeff.ToString(numFormat);
                else
                {
                    coeff = Math.Abs(coeff);
                    line = line + opsign + coeff.ToString(numFormat) +
                        "*[" + variable + "]";
                }
            }

            line = line.Replace("[", "(");
            line = line.Replace("]", ")");

            return line;
        }


        static public bool IsMainEffect(string columnName, DataTable dt)
        {
            DataColumn dc = null;
            if (dt.Columns.Contains(columnName) == false)
                return false;

            dc = dt.Columns[columnName];

            if (dc.ExtendedProperties.ContainsKey(VBTools.Globals.MAINEFFECT) == false)
                return false;

            string sval = dc.ExtendedProperties[VBTools.Globals.MAINEFFECT].ToString();
            bool retVal = false;
            if (String.Compare(sval, "true", true) == 0)
                retVal = true;

            return retVal;
            
        }

        static public bool IsNearZero(double val)
        {
            double nearzero = 1.0e-21;
            double negnearzero = -1.0e-21;

            if ((val < nearzero) && (val > negnearzero))
                return true;
            else
                return false;

        }
    }
}
