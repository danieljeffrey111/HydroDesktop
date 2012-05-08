using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
//using Extreme.Statistics;

namespace VBStatistics
{
    public class DescriptiveStats
    {
        //private NumericalVariable _stats = null;
        private double _max;
        private double _min;
        private double _mean;
        private double _stddev;
        private double _kurtosis;
        private double _skewness;
        private double _range;
        private double _variance;
        private double _sum;
        private int _count;
        private double _median;

        //public void getStats(double[] data) 
        //{
        //    NumericalVariable _stats = new NumericalVariable(data);
        //    _max = _stats.Maximum;
        //    _min = _stats.Minimum;
        //    _mean = _stats.Mean;
        //    _stddev = _stats.StandardDeviation;
        //    _kurtosis = _stats.Kurtosis;
        //    _skewness = _stats.Skewness;
        //    _range = _stats.Range;
        //    _variance = _stats.Variance;
        //    _sum = _stats.Sum;
        //    _count = _stats.CountValid;
        //    _median = _stats.Median;

        //}


        public double Max
        {
            get { return _max; }
        }
        public double Min
        {
            get { return _min; }
        }
        public double Mean
        {
            get { return _mean; }
        }
        public double StdDev
        {
            get { return _stddev; }
        }
        public double Kurtosis
        {
            get { return _kurtosis; }
        }
        public double Range
        {
            get { return _range; }
        }
        public double Skewness
        {
            get { return _skewness; }
        }
        public double Variance
        {
            get { return _variance; }
        }
        public double Sum
        {
            get { return _sum; }
        }
        public int Count
        {
            get { return _count; }
        }
        public double Median
        {
            get { return _median; }
        }
    }
}
