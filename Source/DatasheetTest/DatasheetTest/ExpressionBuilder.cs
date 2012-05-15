using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using VBTools;

namespace VBDatasheet
{   
    public class Expression
    {
        private List<string> _variables;
        private Globals.Operations _operation;
        private string _expressionString;

        public string ExpressionString
        {
            get { return _expressionString; }
        }

        public Expression(Globals.Operations operation)
        {
            _operation = operation;
            _variables = new List<string>();
        }

        public Expression(Expression exp)
        {
            _operation = exp.Operation;
            _variables = exp.Variables.ToList();
            _expressionString = exp.ExpressionString;
        }

        public List<string> Variables
        {
            get { return _variables; }
        }
        
        public Globals.Operations Operation
        {
            get { return _operation; }
        }
        
        public string SetOperation(Globals.Operations operation)
        {
            _operation = operation;
            UpdateString();
            return _expressionString;
        }

        public string AddVariable(string variable)
        {
            _variables.Add(variable);
            UpdateString();
            return _expressionString;
        }

        public string RemoveVariable(string variable)
        {
            _variables.Remove(variable);
            UpdateString();
            return _expressionString;
        }

        //Update the expression string with current operation and variables.
        private void UpdateString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GetEnumString(_operation));
            sb.Append("[");

            for (int i = 0; i < _variables.Count; i++)
            {
                if (i != 0)
                    sb.Append(",");
                sb.Append(_variables[i]);                
            }

            sb.Append("]");
            _expressionString = sb.ToString();
        }

        private string GetEnumString(Enum enumVal)
        {
            return Convert.ToString(enumVal);
        }

        public override string ToString()
        {
            return _expressionString;
        }

    }

   
}
