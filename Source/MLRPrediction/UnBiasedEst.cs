using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VBStatistics;
using System.Data;

namespace MLRPrediction
{
    public class UnBiasedEst
    {
        private double _slope;
        private double _intercept;
        private double[] _obs = null;
        private double[] _est = null;
        private DataTable _data = null;
        private double[] _unbiasedEst = null;

        public double[] UnBiasedEST
        {
            get { return _unbiasedEst; }
        }
        public double Slope
        {
            get { return _slope; }
        }

        public double Intercept
        {
            get { return _intercept; }
        }

        public UnBiasedEst(double[] obs, double[] est)
        {
            _obs = obs;
            _est = est;
            _data = getTable();
            MultipleRegression model = new MultipleRegression(_data, "EST", new[] { "OBS" });
            model.Compute();
            _intercept = (double)model.Parameters.Rows[0][1];
            _slope = (double)model.Parameters.Rows[1][1];
            _unbiasedEst = new double[_obs.Length];
            for (int i = 0; i < _obs.Length; i++)
            {
                _unbiasedEst[i] = (est[i] - _intercept) / _slope;
            }
        }

        private DataTable getTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("OBS", typeof(System.Double));
            dt.Columns.Add("EST", typeof(System.Double));
            for (int i = 0; i < _obs.Length; i++)
            {
                dt.Rows.Add (new object[] {_obs[i], _est[i]} );
            }
            return dt;
        }

    }
}
