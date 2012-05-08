using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Collections;
//using Extreme.Statistics;
//using Extreme.Statistics.Distributions;
//using Extreme.Statistics.Tests;


namespace VBStatistics
{
    public class AndersonDarlingNormality
    {
        //private double _adtest = double.NaN;
        private double _adStat = double.NaN;
        private double _adStatPval = double.NaN;
        private bool _reject;

        public void getADstat(double[] data)
        {
            //try
            //{
            //    NumericalVariable nv = new NumericalVariable(data);
            //    AndersonDarlingTest adtest = new AndersonDarlingTest(nv, nv.Mean, nv.StandardDeviation);
            //    _adStat = adtest.Statistic;
            //    _adStatPval = adtest.PValue;
            //    _reject = adtest.Reject();
            //}
            //catch
            //{
            //    _adStat = double.NaN;
            //    _adStatPval = double.NaN;
            //    _reject = false;
            //}

             
        }

        public double ADStat
        {
            get { return _adStat; }
        }

        public double ADStatPval
        {
            get { return _adStatPval; }
        }

        public bool RejectNHyp
        {
            get { return _reject; }
        }

    }
}
