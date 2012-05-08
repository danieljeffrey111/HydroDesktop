using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VBStatistics
{
    public class ModelErrorCounts
    {
        private int _FPCount = 0;
        private int _FNCount = 0;
        private bool _status = true;
        private string _message = string.Empty;

        private int _TPCount = 0;
        private int _TNCount = 0;
        private double _specificity = double.MinValue;
        private double _sensitivity = double.MinValue;
        private double _accuracy = double.MinValue;

        public void getCounts(double thresholdHoriz, double thresholdVert, List<double[]> data)
        {
            double obs = double.NaN;
            double pred = double.NaN;
            for (int i = 0; i < data.Count; i++)
            {
                obs = data[i][0];
                pred = data[i][1];

                if ((pred >= thresholdHoriz) && (obs >= thresholdVert))
                    _TPCount++;
                else if ((pred >= thresholdHoriz) && (obs < thresholdVert))
                    _FPCount++;
                else if ((pred < thresholdHoriz) && (obs >= thresholdVert))
                    _FNCount++;
                else if ((pred < thresholdHoriz) && (obs < thresholdVert))
                    _TNCount++;
                else
                {
                    _status = false;
                    _message = "Point is not in any quadrant. Model error count halted.";
                    return;
                }
            }
            _specificity = (double)_TNCount / (double)(_TNCount + _FPCount);
            _sensitivity = (double)_TPCount / (double)(_TPCount + _FNCount);
            _accuracy = (double)(_TPCount + _TNCount) / (double)(_TPCount + _TNCount + _FPCount + _FNCount);
        }

        public void getCounts(double thresholdHoriz, double thresholdVert, double[] predictions, double[] observations)
        {
            double obs = double.NaN;
            double pred = double.NaN;
            //for (int i = 0; i < data.Count; i++)
            for (int i = 0; i < predictions.Length; i++)
            {
                obs = observations[i];
                pred = predictions[i];

                if ((pred >= thresholdHoriz) && (obs >= thresholdVert))
                    _TPCount++;
                else if ((pred >= thresholdHoriz) && (obs < thresholdVert))
                    _FPCount++;
                else if ((pred < thresholdHoriz) && (obs >= thresholdVert))
                    _FNCount++;
                else if ((pred < thresholdHoriz) && (obs < thresholdVert))
                    _TNCount++;
                else
                {
                    _status = false;
                    _message = "Point is not in any quadrant. Model error count halted.";
                    return;
                }
            }
            _specificity = (double)_TNCount / (double)(_TNCount + _FPCount);
            _sensitivity = (double)_TPCount / (double)(_TPCount + _FNCount);
            _accuracy = (double)(_TPCount + _TNCount) / (double)(_TPCount + _TNCount + _FPCount + _FNCount);
        }

        public void getCount(double thresholdHoriz, double thresholdVert, double prediction, double observation)
        {
            double obs = observation;
            double pred = prediction;

            if ((pred >= thresholdHoriz) && (obs >= thresholdVert))
                _TPCount++;
            else if ((pred >= thresholdHoriz) && (obs < thresholdVert))
                _FPCount++;
            else if ((pred < thresholdHoriz) && (obs >= thresholdVert))
                _FNCount++;
            else if ((pred < thresholdHoriz) && (obs < thresholdVert))
                _TNCount++;
            else
            {
                _status = false;
                _message = "Point is not in any quadrant. Model error count halted.";
                return;
            }
        }

        public int FPCount
        {
            set { _FPCount = value; }
            get { return _FPCount; }
        }

        public int FNCount
        {
            set { _FNCount = value; }
            get { return _FNCount; }
        }

        public bool Status
        {
            set { _status = value; }
            get { return _status; }
        }

        public string Message
        {
            set { _message = value; }
            get { return _message; }
        }

        public double Sensitivity
        {
            set{ _sensitivity = value; }
            get { return _sensitivity; }
        }

        public double Specificity
        {
            set{ _specificity = value; }
            get { return _specificity; }
        }

        public double Accuracy
        {
            set { _accuracy = value; }
            get { return _accuracy; }
        }
    }
}
