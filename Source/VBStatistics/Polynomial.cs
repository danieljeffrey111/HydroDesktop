using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VBTools;

namespace VBStatistics
{
    public class Polynomial
    {
        //run a regression on y, x and x**2 and compute x(new) = intercept + c1*x + c2*x*x and adjustedR**2 fit
        //save x var name, intercept, c1, c2 and adjustedR**2 in memory (_polyDT) (for prediction) and 
        //return new []x (_polytransform) and fit (_rsqrd)


        private DataTable _modelDT = null;
        private double[] _polytransform = null;
        private double _adjrsqrd, _intercept, _c1, _c2, _rsqrd;
        private string _colname = string.Empty;
        public static DataTable _polyDT = null;

        //constructor for procedural processing in datasheet
        public Polynomial(DataTable dt, int colndx)
        {
            createResultsTable();
            _colname = dt.Columns[colndx].ColumnName;
            _modelDT = createModelTable(dt, colndx);
            MultipleRegression model = new MultipleRegression(_modelDT, "Y", new [] { "X", "X**2"} );
            model.Compute();
            DataTable result = model.Parameters;
            computePoly(result);
            _adjrsqrd = model.AdjustedR2;
            _rsqrd = model.R2;
            savePolyInfo();
        }

        //constructor for manual processing (plotting)
        public Polynomial(double[] y, double[] x, string colname)
        {
            createResultsTable();
            _colname = colname;
            _modelDT = createModelTable(y, x);
            MultipleRegression model = new MultipleRegression(_modelDT, "Y", new[] { "X", "X**2" });
            model.Compute();
            DataTable result = model.Parameters;
            computePoly(result);
            _adjrsqrd = model.AdjustedR2;
            _rsqrd = model.R2;
            savePolyInfo();
        }


        private void createResultsTable()
        {
            //throw new NotImplementedException();
            if (_polyDT == null)
            {
                _polyDT = new DataTable();
                _polyDT.Columns.Add("colname", typeof(string));
                _polyDT.Columns.Add("intercept", typeof(double));
                _polyDT.Columns.Add("coeffX", typeof(double));
                _polyDT.Columns.Add("coeffXX", typeof(double));
                _polyDT.Columns.Add("adjR**2", typeof(double));
            }
        }


        private void savePolyInfo()
        {
            //throw new NotImplementedException();
            _polyDT.Rows.Add(new Object [] {_colname, _intercept, _c1, _c2, _adjrsqrd});
        }

        public double[] getPolyT
        {
            get { return _polytransform; }
        }

        public double getAdjRsqrd
        {
            get { return _adjrsqrd; }
        }

        public double getRsqrd
        {
            get { return _rsqrd; }
        }

        public DataTable getPolyInfo
        {
            get { return _polyDT; }
        }

        private void computePoly(DataTable result)
        {
            //throw new NotImplementedException();
            _polytransform = new double [_modelDT.Rows.Count];
            _intercept = (double)result.Rows[0]["Value"];
            _c1 = (double)result.Rows[1]["Value"];
            _c2 = (double)result.Rows[2]["Value"];
            int n = 0;
            foreach (DataRow r in _modelDT.Rows)
            { 
                _polytransform[n] = _intercept + _c1*(double)r.ItemArray[1] + _c2*(double)r.ItemArray[2];
                n++;
            }
        }


        private DataTable createModelTable(DataTable dt, int colndx)
        {
            //throw new NotImplementedException();
            addModelTableCols();
            double [] y = Utility.GetColumnFromTable(dt, 1);
            double [] x = Utility.GetColumnFromTable(dt, colndx);
            Transform t = new Transform(dt, colndx);
            double[] x2 = t.SQUARE;
            //DataRow r;
            for (int i = 0; i < y.Length; i++)
                _modelDT.Rows.Add(new Object[] {y[i], x[i], x2[i]});
            return _modelDT;
        }

        private DataTable createModelTable(double[] y, double[] x)
        {
            addModelTableCols();
            Transform t = new Transform(x);
            double[] x2 = t.SQUARE;
            for (int i = 0; i < y.Length; i++)
                _modelDT.Rows.Add(new Object[] { y[i], x[i], x2[i] });
            return _modelDT;
        }


        private void addModelTableCols()
        {
            //throw new NotImplementedException();
            _modelDT = new DataTable("PolynomialData");
            _modelDT.Columns.Add("Y", typeof(double));
            _modelDT.Columns.Add("X", typeof(double));
            _modelDT.Columns.Add("X**2", typeof(double));
        }
    }
}
