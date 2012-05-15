using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using Ciloci.Flee;


namespace MLRPrediction
{
    public class ExpressionEvaluator
    {
        private static DataTable MyTable;
        private static DataRow MyCurrentRow;


        //public List<double> Evaluate(string expression, DataTable dt)
        public DataTable Evaluate(string expression, DataTable dt)
        {
            List<double> calcValues = new List<double>();
            DataTable dtCalcValues = new DataTable();
            dtCalcValues.Columns.Add("ID", typeof(string));
            dtCalcValues.Columns.Add("CalcValue", typeof(double));

            MyTable = dt;

            ExpressionContext context = new ExpressionContext();
            // Use string.format
            context.Imports.AddType(typeof(string));            
            context.Imports.AddType(typeof(CustomFunctions));

            // Use on demand variables to provide the values for the columns
            context.Variables.ResolveVariableType += new EventHandler<ResolveVariableTypeEventArgs>(Variables_ResolveVariableType);
            context.Variables.ResolveVariableValue += new EventHandler<ResolveVariableValueEventArgs>(Variables_ResolveVariableValue);

            // Create the expression; Flee will now query for the types of ItemName, Price, and Tax
            IDynamicExpression e = context.CompileDynamic(expression);
            
            Console.WriteLine("Computing value of '{0}' for all rows", e.Text);

            DataRow dr = null;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                MyCurrentRow = dt.Rows[i];
                // Evaluate the expression; Flee will query for the values of the columns
                double result = (double) e.Evaluate();
                calcValues.Add(result);

                dr = dtCalcValues.NewRow();
                dr[0] = MyCurrentRow[0] as string;
                dr[1] = result;
                dtCalcValues.Rows.Add(dr);
                Console.WriteLine("Row {0} = {1}", i, result);
            }

            //return calcValues;
            return dtCalcValues;
        }

        static void Variables_ResolveVariableType(object sender, ResolveVariableTypeEventArgs e)
        {
            // Simply supply the type of the column with the given name
            e.VariableType = MyTable.Columns[e.VariableName].DataType;
        }

        static void Variables_ResolveVariableValue(object sender, ResolveVariableValueEventArgs e)
        {
            // Supply the value of the column in the current row
            e.VariableValue = MyCurrentRow[e.VariableName];
        }

        static public string[] GetReferencedVariables(string expression, string[] variables)
        {
            if (String.IsNullOrWhiteSpace(expression))
                return null;

            if ((variables == null) ||(variables.Length < 1))
                return null;

            ExpressionContext context = new ExpressionContext();
            // Use string.format
            context.Imports.AddType(typeof(string));
            context.Imports.AddType(typeof(CustomFunctions));

            for (int i=0;i<variables.Length;i++)
                context.Variables[variables[i]] = 1.0;

            IDynamicExpression e = context.CompileDynamic(expression);
            return e.Info.GetReferencedVariables();

        }
    }
}
