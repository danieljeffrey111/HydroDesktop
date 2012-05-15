using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VBTools;
using VBStatistics;
using System.Data;


namespace MLRPrediction
{
    public class mlr
    {
        private double[] _obs = null;
        private double[] _pred = null;
        private VBProjectManager _proj = null;

        public double[] obs
        {
            get { return _obs; }
        }
        public double[] pred
        {
            get { return _pred; }
        }
        public mlr()
        {
            _proj = VBProjectManager.GetProjectManager();
            DataTable dt = _proj.CorrelationDataTable;
            MultipleRegression model = computeModel(dt);
            _obs = model.ObservedValues;
            _pred = model.PredictedValues;
        }

        private MultipleRegression computeModel(DataTable dt)
        {
            //given a datatable, build a model
            MultipleRegression model = null;
            string[] idvars = _proj.ModelIndependentVariables.ToArray();
            string dvar = _proj.ModelDependentVariable;
            if (dt != null)
            {
                model = new MultipleRegression(dt, dvar, idvars);
                model.Compute();
            }
            return model;
        }
    }
}
