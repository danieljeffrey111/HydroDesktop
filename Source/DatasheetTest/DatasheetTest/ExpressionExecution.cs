using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using VBTools;

namespace VBDatasheet
{
    public class ExpressionExecution
    {
        protected Dictionary<string , Globals.Operations> _operations;
        protected Expression[] _expressions = null;
        protected DataTable _dt = null;

        public Expression[] Expressions
        {
            get { return _expressions; }
            set { _expressions = value; }
        }

        
        //public ExpressionExecution()
        //{

        //}

        public ExpressionExecution(DataTable dt, Expression[] expressions)
        {
            _dt = dt.Copy();
            _expressions = expressions;
            Init();
        }

        /// <summary>
        /// Generate the dictionary of operations 
        /// </summary>
        private void Init()
        {
            _operations = new Dictionary<string, Globals.Operations>();
            Array enumValArray = Enum.GetValues( typeof(Globals.Operations));

            for (int i=0;i<enumValArray.Length;i++)
            {
                string sEnum = enumValArray.GetValue(i).ToString();
                Globals.Operations op = (Globals.Operations)enumValArray.GetValue(i);
                _operations.Add(sEnum, op);
            }
        }

        public DataTable Execute()
        {
            for (int i = 0; i < _expressions.Length; i++)
            {
                if (VerifyExpression(_expressions[i]) == false)
                    continue;

                ExecuteExpression(_expressions[i]);
            }
            return _dt;
        }

        private bool VerifyExpression(Expression expression)
        {
            if (_dt == null)
                return false;

            foreach (string var in expression.Variables)
            {
                if (_dt.Columns.Contains(var) == false)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// This function verifies that the expression contains a valid Operation and that the field(s) are included in the datatable
        /// </summary>
        /// <param name="expression">Expression to be verified</param>
        private bool ExecuteExpression(Expression expression)
        {
            switch (expression.Operation)
            {
                case Globals.Operations.MAX :
                    MAXIMUM(expression);
                    break;
                case Globals.Operations.MEAN:
                    MEAN(expression);
                    break;
                case Globals.Operations.PROD:
                    PRODUCT(expression);
                    break;
                case Globals.Operations.SUM:
                    SUM(expression);
                    break;
                case Globals.Operations.MIN:
                    MINIMUM(expression);
                    break;
            }
            return true;
        }

        /// <summary>
        /// Select the maximum values of all the fields in the expression
        /// </summary>
        /// <param name="expression"></param>
        private void MAXIMUM(Expression expression)
        {
            string newColName = expression.ToString();
            DataColumn dc = AddColumnToDataTable(expression);

            if (dc != null)
            {
                for (int i = 0; i < _dt.Rows.Count; i++)
                {
                    double max = double.NegativeInfinity;
                    foreach (string var in expression.Variables)
                    {
                        double current = Convert.ToDouble(_dt.Rows[i][var]);
                        if (current > max)
                            max = current;
                    }

                    _dt.Rows[i][newColName] = max;
                }
            }
        }

        /// <summary>
        /// Select the mininum values of all the fields in the expression
        /// </summary>
        /// <param name="expression"></param>
        private void MINIMUM(Expression expression)
        {
            string newColName = expression.ToString();
            DataColumn dc = AddColumnToDataTable(expression);

            if (dc != null)
            {
                for (int i = 0; i < _dt.Rows.Count; i++)
                {
                    double min = double.PositiveInfinity;
                    foreach (string var in expression.Variables)
                    {
                        double current = Convert.ToDouble(_dt.Rows[i][var]);
                        if (current < min)
                            min = current;
                    }

                    _dt.Rows[i][newColName] = min;
                }
            }
        }

        /// <summary>
        /// Take the average of the values of all the fields in the expression
        /// </summary>
        /// <param name="expression"></param>
        private void MEAN(Expression expression)
        {
            string newColName = expression.ToString();
            DataColumn dc = AddColumnToDataTable(expression);

            if (dc != null)
            {
                for (int i = 0; i < _dt.Rows.Count; i++)
                {
                    double mean = 0.0;
                    foreach (string var in expression.Variables)
                    {
                        mean += Convert.ToDouble(_dt.Rows[i][var]);
                    }

                    _dt.Rows[i][newColName] = mean / expression.Variables.Count;
                }
            }
        }

        /// <summary>
        /// Sum the values of all the fields in the expression
        /// </summary>
        /// <param name="expression"></param>
        private void SUM(Expression expression)
        {
            string newColName = expression.ToString();
            DataColumn dc = AddColumnToDataTable(expression);

            if (dc != null)
            {
                for (int i = 0; i < _dt.Rows.Count; i++)
                {
                    double sum = 0.0;
                    foreach (string var in expression.Variables)
                    {
                        sum += Convert.ToDouble(_dt.Rows[i][var]);
                    }

                    _dt.Rows[i][newColName] = sum;
                }
            }
        }


        /// <summary>
        /// Multiply the values of all the fields in the expression
        /// </summary>
        /// <param name="expression"></param>
        private void PRODUCT(Expression expression)
        {
            string newColName = expression.ToString();
            DataColumn dc = AddColumnToDataTable(expression);

            if (dc != null)
            {
                for (int i = 0; i < _dt.Rows.Count; i++)
                {
                    double product = 1.0;
                    foreach (string var in expression.Variables)
                    {
                        product = product * Convert.ToDouble(_dt.Rows[i][var]);
                    }

                    _dt.Rows[i][newColName] = product;
                }
            }
        }

        private DataColumn AddColumnToDataTable(Expression expression)
        {
            DataColumn dc = null;
            string newColName = expression.ToString();
            if (!_dt.Columns.Contains(newColName))
            {
                dc = _dt.Columns.Add(newColName, typeof(double));
                dc.ExtendedProperties[VBTools.Globals.OPERATION] = true;

                foreach (string var in expression.Variables)
                {
                    DataColumn dc2 = _dt.Columns[var];
                    if (dc2.ExtendedProperties.ContainsKey(VBTools.Globals.TRANSFORM))
                    {
                        dc.ExtendedProperties.Add(VBTools.Globals.TRANSFORM, true);
                        break;
                    }
                }
            }
            return dc;
        }

    }
}
