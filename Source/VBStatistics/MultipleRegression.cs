using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
//using Extreme.Statistics;
//using Extreme.Mathematics.LinearAlgebra;
//using Extreme.Statistics.Tests;

namespace VBStatistics
{
    public class MultipleRegression
    {
        //private LinearRegressionModel _model;

        //private VariableCollection _data = null;
        private DataTable _dataTable = null;
        private string _dependentVar = "";
        private string[] _independentVars = null;
        private double _adjR2;
        private double _R2;
        private double _AIC;
        private double _AICC;
        private double _BIC;
        private double _Press;
        private double _RMSE;

        //private Dictionary<string, double> _parameters = null;
        private double[] _studentizedResiduals = null;
        private double[] _dffits = null;
        private double[] _cooks = null;
        private DataTable _parameters = null;
        private double[] _predictedValues = null;
        private double[] _observedValues = null;

        private Dictionary<string, double> _VIF = null;
        private double _maxVIF = 0;
        private string _maxVIFParameter = "";

        private double _ADresidPvalue = double.NaN;
        private double _ADresidNormStatVal = double.NaN;

        private double _WSresidPvalue = double.NaN;
        private double _WSresidNormStatVal = double.NaN;

        //public MultipleRegression(DataTable dataTable, string dependentVariable, string[] independendVariables )
        //{
        //    _dataTable = dataTable;
        //    _dependentVar = dependentVariable;
        //    _independentVars = independendVariables;

        //    _data = new VariableCollection(dataTable);
        //}

        public double R2
        {
            get { return _R2; }
        }

        public double AdjustedR2
        {
            get { return _adjR2; }
        }

        public double AIC
        {
            get { return _AIC; }
        }

        public double AICC
        {
            get { return _AICC; }
        }

        public double BIC
        {
            get { return _BIC; }
        }

        public double Press
        {
            get { return _Press; }
        }

        public double RMSE
        {
            get { return _RMSE; }
        }

        public double[] DFFITS
        {
            get { return _dffits; }
        }

        public double[] Cooks
        {
            get { return _cooks; }
        }

        public double[] StudentizedResiduals
        {
            get { return _studentizedResiduals; }
        }

        public DataTable Parameters
        {
            get { return _parameters; }
        }

        public double[] PredictedValues
        {
            get { return _predictedValues; }
        }

        public double[] ObservedValues
        {
            get { return _observedValues; }
        }

        public double ADResidPvalue 
        {
            get { return _ADresidPvalue; }
        }

        public double ADResidNormStatVal
        {
            get { return _ADresidNormStatVal; }
        }

        public double WSResidPvalue
        {
            get { return _WSresidPvalue; }
        }

        public double WSResidNormStatVal
        {
            get { return _WSresidNormStatVal; }
        }



        //public Dictionary<string, double> VIFs
        //{
        //    get { return _VIF; }
        //}

        public double MaxVIF
        {
            get { return _maxVIF; }
        }

        //public void Compute()
        //{
        //    // Next, create a VariableCollection from the data table:
            

        //    // Now create the regression model. Parameters are the name 
        //    // of the dependent variable, a string array containing 
        //    // the names of the independent variables, and the VariableCollection
        //    // containing all variables.
        //    //LinearRegressionModel model = new LinearRegressionModel(_dataTable, _dependentVar, _independentVars);
        //    _model = new LinearRegressionModel(_dataTable, _dependentVar, _independentVars);

        //    //LogisticRegressionModel model = new LogisticRegressionModel(_dataTable, _dependentVar, _independentVars);            

        //    // We can set model options now, such as whether to include a constant:
        //    _model.NoIntercept = false;

        //    // The Compute method performs the actual regression analysis.
        //    _model.Compute();            
        //    _adjR2 = _model.AdjustedRSquared;
        //    _R2 = _model.RSquared;
        //    _RMSE = _model.StandardError;
            

        //    //_AIC = model.GetAkaikeInformationCriterion();
        //    //_BIC = model.GetBayesianInformationCriterion();            

            
        //    //Calculate the Corrected AIC
        //    double sse = _model.AnovaTable.ErrorRow.SumOfSquares;            
        //    double n = _dataTable.Rows.Count;
        //    double p = _independentVars.Length + 1;

        //    _AIC = n * Math.Log(sse / n) + (2 * p) + n + 2;
        //    //_AICC = 1 + (Math.Log(sse / n)) + (2)*(p + 1)/ (n - p - 1);
        //    _AICC = _AIC + (2 * (p + 1) * (p + 2)) / (n - p - 2);
        //    _BIC = n * (Math.Log(sse / n)) + (p * Math.Log(n));


        //    _Press = 0.0;
        //    GeneralVector vecLeverage = _model.GetLeverage();
        //    GeneralVector vecResiduals = _model.Residuals;
        //    double leverage = 0.0;
        //    for (int i = 0; i < _dataTable.Rows.Count; i++)
        //    {
        //        leverage = Math.Min(vecLeverage[i], 0.99);
        //        //_Press += ((vecResiduals[i])* (vecResiduals[i])) / ((1 - vecLeverage[i]) * (1 - vecLeverage[i]));
        //        _Press += Math.Pow((vecResiduals[i]) / (1 - leverage),2);
        //    }


        //    _parameters = createParametersDataTable();
        //    DataRow dr = null;
        //    foreach (Parameter param in _model.Parameters)
        //    {
        //        dr = _parameters.NewRow();
        //        dr["Name"] = param.Name;
        //        dr["Value"] = param.Value;
        //        dr["StandardError"] = param.StandardError;
        //        dr["TStatistic"] = param.Statistic;
        //        dr["PValue"] = param.PValue;
        //        dr["StandardizedCoefficient"] = getStandardCoeff(param.Name, param.Value);
        //        _parameters.Rows.Add(dr);
        //    }

        //    _predictedValues = _model.PredictedValues.ToArray();            
        //    _observedValues = _model.DependentVariable.ToArray() as double[];
        //    _dffits = _model.GetDffits().ToArray<double>();
        //    _cooks = _model.GetCooksDistance().ToArray<double>();
        //    _studentizedResiduals = _model.GetStudentizedResiduals().ToArray<double>();


        //    //AndersonDarlingTest adtest = new AndersonDarlingTest((NumericalVariable)vecResiduals);
            
        //    Extreme.Statistics.Tests.OneSampleTest ADtest = _model.GetNormalityOfResidualsTest(TestOfNormality.AndersonDarling);
        //    Extreme.Statistics.Tests.OneSampleTest WStest = _model.GetNormalityOfResidualsTest(TestOfNormality.ShapiroWilk);
        //    _ADresidPvalue = ADtest.PValue;
        //    _ADresidNormStatVal = ADtest.Statistic;
        //    //_WSresidPvalue = WStest.PValue;
        //    //_WSresidNormStatVal = WStest.Statistic;

        //    Extreme.Mathematics.LinearAlgebra.SymmetricMatrix matrix = _model.GetCorrelationMatrix();
        //    Extreme.Mathematics.LinearAlgebra.SymmetricMatrix corrMatrix = new SymmetricMatrix(matrix.ColumnCount -1);

        //    //Extreme Opt returns a Correlation matrix that contains an extra row and column
        //    //Looks like these are related to the intercept
        //    //We are carving out the std correlation matrix
        //    for (int row=1;row < matrix.ColumnCount; row++)
        //    {
        //        for (int col=1;col < matrix.ColumnCount; col++)
        //        {
        //            corrMatrix[row-1,col-1] = matrix[row,col];
        //        }
        //    }           
            
        //    Extreme.Mathematics.Matrix InvCorrMatrix = corrMatrix.GetInverse();
        //    Extreme.Mathematics.Vector VIFVector = InvCorrMatrix.GetDiagonal();
        //    Extreme.Mathematics.Vector vifs = InvCorrMatrix.GetDiagonal().ToArray();

        //    _VIF = new Dictionary<string, double>();
        //    for (int i = 0; i < vifs.Count(); i++)
        //        _VIF.Add(_independentVars[i].ToString(), vifs.GetValue(i));

        //    _maxVIF = VIFVector.AbsoluteMax();
        //    _maxVIFParameter = _independentVars[VIFVector.AbsoluteMaxIndex()]; 

           
                        
        //}

        //private double getStandardCoeff(string paramName, double coeff)
        //{
        //    //throw new NotImplementedException();
        //    if (paramName == "(Intercept)") return double.NaN;
        //    NumericalVariable nv = new NumericalVariable(_dataTable.Columns[_dependentVar]);
        //    double stdevY = nv.StandardDeviation;
        //    nv = new NumericalVariable(_dataTable.Columns[paramName]);
        //    double stdevX = nv.StandardDeviation;
        //    return coeff * stdevX / stdevY;
        //}

        //public double Predict(DataRow independentValues)
        //{
        //    double[] indVals = new double[_independentVars.Length];
        //    for (int i = 0; i < _independentVars.Length; i++)
        //    {
        //        indVals[i] = Convert.ToDouble(independentValues[_independentVars[i]]);
        //    }

        //    return Predict(indVals);
        //}

        //public double Predict(double[] independentValues)
        //{
        //    return _model.Predict(independentValues);
        //}

        private DataTable createParametersDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Name",typeof(string));
            dt.Columns.Add("Value", typeof(double));
            dt.Columns.Add("StandardError", typeof(double));
            dt.Columns.Add("TStatistic", typeof(double));
            dt.Columns.Add("PValue", typeof(double));
            dt.Columns.Add("StandardizedCoefficient", typeof(double));

            return dt;
        }

        public Dictionary<string, double> Model
        {
            get
            {

                Dictionary<string, double> parameters = new Dictionary<string, double>();
                for (int i = 0; i < _parameters.Rows.Count; i++)
                {
                    parameters.Add(_parameters.Rows[i][0].ToString(), Convert.ToDouble(_parameters.Rows[i][1]));
                }

                return parameters;
            }
        }
        
    }
}
