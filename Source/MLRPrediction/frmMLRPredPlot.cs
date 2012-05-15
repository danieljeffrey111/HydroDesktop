using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZedGraph;


namespace MLRPrediction
{
    public partial class frmMLRPredPlot : Form
    {

        private double _plotThresholdHoriz = double.NaN;
        private double _plotThresholdVert = double.NaN;
        private double _plotProbThreshold = double.NaN;


        private DataTable _dtObs = null;
        private DataTable _dtStats = null;

        public frmMLRPredPlot( DataTable dtObs, DataTable dtStats)
        {
            InitializeComponent();

            if (dtObs == null || dtStats == null) return;

            if (dtObs.Rows.Count < 1 || dtStats.Rows.Count < 1)
            {
                MessageBox.Show("Must have observations and predictions to display plots.", "Insufficient data for plotting.", MessageBoxButtons.OK);
                return;
            }

            _dtObs = dtObs;
            _dtStats = dtStats;
            tbThresholdDec.Text = dtStats.Rows[0]["Decision_Criterion"].ToString();
            tbThresholdReg.Text = dtStats.Rows[0]["Regulatory_Standard"].ToString();
            _plotThresholdHoriz = Convert.ToDouble(tbThresholdDec.Text);
            _plotThresholdVert = Convert.ToDouble(tbThresholdReg.Text);
            //rbValue.Checked = true;
        }


        private void frmMLRPredPlot_Load(object sender, EventArgs e)
        {
            string[] rptlist = new string[]{
                "XY ScatterPlot (Observed vs Predicted)",
                "Time Series Plot (Observed and Predicted)",
                "XY ScatterPlot (ProbExceed(Est) vs Observed)"};

            cboPlotList.DataSource = rptlist;

            //change UI for initial assumptions
            cbPlotThreshold.Checked = true;

            cboPlotList.SelectedIndex = 0;
            CreateGraph(ctlModelEstPlot);
        }

        private void btnMakePlot_Click(object sender, EventArgs e)
        {
            CreateGraph(ctlModelEstPlot);
        }

        private void CreateGraph(ZedGraphControl zgc)
        {

            GraphPane myPane = zgc.GraphPane;
            myPane.CurveList.Clear();

            string[] tags = getValues<string>(_dtObs.Columns.IndexOf("ID"), _dtObs);
            double[] obs = getValues<double>(_dtObs.Columns.IndexOf("Observation"), _dtObs);
            double[] est = getValues<double>(_dtStats.Columns.IndexOf("Model_Prediction"), _dtStats);
            double[] pexceed = getValues<double>(_dtStats.Columns.IndexOf("Exceedance_Probability"), _dtStats);
            //double[] unbiased = getValues<double>(_dtStats.Columns.IndexOf("Unbiased_Estimate"), _dtStats);
            

            string plot = string.Empty;
            if (cboPlotList.SelectedIndex == 0)
            {
                plot = "SampleValues_and_PredictedValues_ScatterPlot";
                myPane = addPlotXY(obs, est, tags, plot /*, unbiased */);
                if (cbPlotThreshold.Checked)
                    myPane = addThresholdCurve(myPane, plot);
                
            }
            else if (cboPlotList.SelectedIndex == 2)
            {
                plot = "XY ScatterPlot (ProbExceed(Est) vs Observed)";
                myPane = addPlotPROB(obs, pexceed, tags, plot /*, unbiased */);
                if (cbPlotThreshold.Checked)
                    myPane = addProbThresholdCurve(myPane);
            }
            else
            {
                plot = "SampleValues_and_PredictedValues_TimeSeries";
                myPane = addPlotTS(obs, est, tags, plot);
                if (cbPlotThreshold.Checked)
                    myPane = addThresholdCurve(myPane, plot);
            }

            myPane.XAxis.Cross = 0.0;
            zgc.AxisChange();
            zgc.Refresh();
            zgc.RestoreScale(myPane);

            }

        private GraphPane addProbThresholdCurve(GraphPane myPane)
        {
            double xMin, xMax, yMin, yMax;
            double xPlotMin, xPlotMax, yPlotMin, yPlotMax;
            myPane.CurveList[0].GetRange(out xMin, out xMax, out yMin, out yMax, false, false, myPane);

            if (xMax.GetType() == typeof(ZedGraph.XDate))
            {
                xPlotMin = xMin < 0.0 ? xMin : 0;
                xPlotMax = xMax > _plotProbThreshold / 100.0 ? xMax : _plotProbThreshold / 100.0;
            }
            else
            {
                xPlotMin = xMin;
                xPlotMax = xMax;
            }
            yPlotMin = yMin < 0.0 ? yMin : 0;

            //mikec wants max yscale to be this so display is max 1.2
            yPlotMax = 1.0;

            //probability threshold
            PointPair pp1 = new PointPair(xPlotMin, _plotProbThreshold/100.0d);
            PointPair pp2 = new PointPair(xPlotMax, _plotProbThreshold/100.0d);
            PointPairList ppl1 = new PointPairList();
            ppl1.Add(pp1);
            ppl1.Add(pp2);

            //regulatory threshold
            pp1 = new PointPair(_plotThresholdVert, yPlotMin);
            pp2 = new PointPair(_plotThresholdVert, yPlotMax);
            PointPairList ppl2 = new PointPairList();
            ppl2.Add(pp1);
            ppl2.Add(pp2);

            LineItem curve1 = myPane.AddCurve("Exceedance Probability Threshold", ppl1, Color.Red);
            LineItem curve2 = myPane.AddCurve("Regulatory Threshold", ppl2, Color.Green);
            curve1.Line.IsVisible = true;

            return myPane;

        }

        private GraphPane addThresholdCurve(GraphPane myPane, string plot)
        {
            double xMin, xMax, yMin, yMax;
            double xPlotMin, xPlotMax, yPlotMin, yPlotMax;
            myPane.CurveList[0].GetRange(out xMin, out xMax, out yMin, out yMax, false, false, myPane);

            //xPlotMin = xMin < 0.0 ? xMin : 0;
            //xPlotMax = xMax > _plotThresholdHoriz ? xMax : _plotThresholdHoriz / 100.0;
            if (xMax.GetType() == typeof(ZedGraph.XDate))
            {
                xPlotMin = xMin < 0.0 ? xMin : 0;
                xPlotMax = xMax > _plotProbThreshold / 100.0 ? xMax : _plotProbThreshold / 100.0;
            }
            else
            {
                xPlotMin = xMin;
                xPlotMax = xMax;
            }
            yPlotMin = yMin < 0.0 ? yMin : 0;
            yPlotMax = yMax > _plotThresholdVert ? yMax : _plotThresholdVert;


            //decision threshold
            PointPair pp1 = new PointPair(xPlotMin, _plotThresholdHoriz);
            PointPair pp2 = new PointPair(xPlotMax, _plotThresholdHoriz);
            PointPairList ppl1 = new PointPairList();
            ppl1.Add(pp1);
            ppl1.Add(pp2);

            //regulatory threshold
            pp1 = new PointPair(_plotThresholdVert, yPlotMin);
            pp2 = new PointPair(_plotThresholdVert, yPlotMax);
            PointPairList ppl2 = new PointPairList();
            ppl2.Add(pp1);
            ppl2.Add(pp2);


            if (plot == "SampleValues_and_PredictedValues_TimeSeries")
            {
                LineItem curve1 = myPane.AddCurve("Decision Threshold", ppl1, Color.Red);
                curve1.Line.IsVisible = true;
            }
            else if (plot == "SampleValues_and_PredictedValues_ScatterPlot")
            {

                LineItem curve1 = myPane.AddCurve("Decision Threshold", ppl1, Color.Red);
                LineItem curve2 = myPane.AddCurve("Regulatory Threshold", ppl2, Color.Green);
                curve1.Line.IsVisible = true;
                curve2.Line.IsVisible = true;
            }

            return myPane;
        }

        private GraphPane addPlotTS(double[] iv, double[] est, string[] tags, string plot)
        {
            DateTime date;
            bool dateAxis = false;
            if (IsDate(tags[0], out date)) dateAxis = true;

            PointPairList ppl1 = new PointPairList();
            PointPairList ppl2 = new PointPairList();
            string tag = string.Empty;

            int npts = iv.Length > est.Length ? est.Length : iv.Length;

            for (int i = 0; i < npts; i++)
            {
                //tag = "(" + iv[i].ToString("n2") + " , " + tags[i] + ") ";
                tag = tags[i];
                if (dateAxis)
                {

                    DateTime d = Convert.ToDateTime(tags[i]);
                    ppl1.Add((XDate)d, iv[i], tag);
                    ppl2.Add((XDate)d, est[i], tag);
                }
                else
                {
                    double n = Convert.ToDouble(tags[i]);
                    ppl1.Add(n, iv[i], tag);
                    ppl2.Add(n, est[i], tag);
                }
            }
         
            GraphPane gp = ctlModelEstPlot.GraphPane;
            LineItem curve1 = gp.AddCurve("Observations", ppl1, Color.Blue);
            LineItem curve2 = gp.AddCurve("Predictions", ppl2, Color.Green);
            curve1.Line.IsVisible = true;
            curve2.Line.IsVisible = true;

            if (dateAxis) gp.XAxis.Type = AxisType.Date;
            else gp.XAxis.Type = AxisType.Linear;

            gp.XAxis.Title.Text = "ID";
            gp.YAxis.Title.Text = "Observations & Predictions";

            gp.Tag = "TSPlot";
            gp.Title.Text = plot;

            tbFN.Text = "";
            tbFP.Text = "";

            return gp;
        }

        private GraphPane addPlotXY(double[] obs, double[] pred, string[] tags, string plot /*, double[] unbiased */)
        {
            int FPCount = 0, FNCount = 0;
            PointPairList ppl1 = new PointPairList();
  
            string tag = string.Empty;

            int npts = obs.Length > pred.Length ? pred.Length : obs.Length;

            for (int i = 0; i < npts; i++)
            {
                //tag = "(" + iv[i].ToString("n2") + " , " + response[i].ToString("n2") + ") " + tags[i];
                tag = tags[i];
                ppl1.Add(obs[i], pred[i], tag);
                //ppl2.Add(iv[i], unbiased[i], tag);

                if (pred[i] > _plotThresholdHoriz && obs[i] < _plotThresholdVert) FPCount++;
                if (pred[i] < _plotThresholdHoriz && obs[i] > _plotThresholdVert) FNCount++;
            }
    
            GraphPane gp = ctlModelEstPlot.GraphPane;
            LineItem curve1 = gp.AddCurve(null, ppl1, Color.Black);
            //LineItem curve2 = gp.AddCurve("Unbiased Estimates", ppl2, Color.Blue);
            curve1.Line.IsVisible = false;
  
            gp.XAxis.Title.Text = "Observations";
            gp.YAxis.Title.Text = "Predictions";

            gp.Tag = "XYPlot";
            gp.Title.Text = "Observations vs Predictions";
            gp.XAxis.Type = AxisType.Linear;

            tbFN.Text = FNCount.ToString();
            tbFP.Text = FPCount.ToString();

            return gp;
        }

        private GraphPane addPlotPROB(double[] obs, double[] pexceed, string[] tags, string plot)
        {
            int FPCount = 0, FNCount = 0;
            PointPairList ppl1 = new PointPairList();

            string tag = string.Empty;

            int npts = obs.Length > pexceed.Length ? pexceed.Length : obs.Length;

            for (int i = 0; i < npts; i++)
            {
                //tag = "(" + iv[i].ToString("n2") + " , " + response[i].ToString("n2") + ") " + tags[i];
                tag = tags[i];
                ppl1.Add(obs[i], pexceed[i], tag);
                //ppl2.Add(iv[i], unbiased[i], tag);
                if (pexceed[i] > _plotProbThreshold/100.0 && obs[i] < _plotThresholdVert) FPCount++;
                if (pexceed[i] < _plotProbThreshold/100.0 && obs[i] > _plotThresholdVert) FNCount++;
            }
    
            GraphPane gp = ctlModelEstPlot.GraphPane;
            LineItem curve1 = gp.AddCurve(null, ppl1, Color.Black);
            curve1.Line.IsVisible = false;

            gp.XAxis.Title.Text = "Observations";
            gp.YAxis.Title.Text = "Probability of Estimate Exceedance";

            gp.Tag = "PROBPlot";
            gp.Title.Text = "Observations vs Exceedance Probability ";
            gp.XAxis.Type = AxisType.Linear;

            tbFN.Text = FNCount.ToString();
            tbFP.Text = FPCount.ToString();

            return gp;
        }

        /// <summary>
        /// used for extracting columns of data from the datatable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="colndx"></param>
        /// <returns></returns>
        private T[] getValues<T>(int colndx, DataTable dt)
        {
            string t = typeof(T).ToString();
            switch (t)
            {
                case "System.Double":
                    var dvalues = (from row in dt.Select()
                                   select row.Field<T>(colndx)).ToArray<T>();
                    return dvalues;
                case "System.String":
                    //var svalues = (from row in _dt.Select()
                    //          select row[colndx]).Cast<string>().ToArray();
                    //return svalues.Cast<T>().ToArray();
                    string[] svalues = new string[dt.Rows.Count];
                    for (int r = 0; r < dt.Rows.Count; r++)
                    {
                        svalues[r] = dt.Rows[r][colndx].ToString();
                    }
                    return svalues.Cast<T>().ToArray();
                default:
                    return null;
            }

        }

        /// <summary>
        /// determines if the passed string can be parsed into a date
        /// used by the ts plot to set appropriate x axis type
        /// </summary>
        /// <param name="anyString"></param>
        /// <param name="resultDate"></param>
        /// <returns></returns>
        private bool IsDate(string anyString, out DateTime resultDate)
        {
            bool isDate = true;

            if (anyString == null)
            {
                anyString = "";
            }
            try
            {
                resultDate = DateTime.Parse(anyString);
            }
            catch
            {
                resultDate = DateTime.MinValue;
                isDate = false;
            }

            return isDate;
        }

        private void rbValue_CheckedChanged(object sender, EventArgs e)
        {
            if (rbValue.Checked)
            {
                _plotThresholdHoriz = Convert.ToDouble(tbThresholdDec.Text);
                _plotThresholdVert = Convert.ToDouble(tbThresholdReg.Text);
            }
        }

        private void rbLog10Value_CheckedChanged(object sender, EventArgs e)
        {
            if (rbLog10Value.Checked) 
            {
                _plotThresholdHoriz = Math.Log10(Convert.ToDouble(tbThresholdDec.Text));
                _plotThresholdVert = Math.Log10(Convert.ToDouble(tbThresholdReg.Text));
            }
        }

        private void rbLogeValue_CheckedChanged(object sender, EventArgs e)
        {
            if (rbLogeValue.Checked) 
            {
                _plotThresholdHoriz = Math.Log(Convert.ToDouble(tbThresholdDec.Text));
                _plotThresholdVert = Math.Log(Convert.ToDouble(tbThresholdReg.Text));
            }
        }


        private void cboPlotList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboPlotList.SelectedItem.ToString() == "XY ScatterPlot (ProbExceed(Est) vs Observed)")
            {
                //change UI for this plot
                label5.Text = "Percent Probability (0-100)";
                tbThresholdDec.Text = "50";
                _plotProbThreshold = Convert.ToDouble(tbThresholdDec.Text);
            }
            else
            {
                label5.Text = "Decision Criterion (Horizontal)";
                tbThresholdDec.Text = _plotThresholdHoriz.ToString();
            }
        }

        private void tbThresholdDec_Validating(object sender, CancelEventArgs e)
        {
            double threshold;
            if (!double.TryParse(tbThresholdDec.Text, out threshold))
            {
                e.Cancel = true;
                tbThresholdDec.Select(0, tbThresholdDec.Text.Length);
                this.errorProvider1.SetError(tbThresholdDec, "Text must convert to a number.");
            }
            if (cboPlotList.SelectedItem.ToString() == "XY ScatterPlot (ProbExceed(Est) vs Observed)")
                _plotProbThreshold = threshold;
            else _plotThresholdHoriz = threshold;
        }

        private void tbThresholdDec_Validated(object sender, EventArgs e)
        {
            errorProvider1.SetError(tbThresholdDec, "");
        }

        private void tbThresholdReg_Validating(object sender, CancelEventArgs e)
        {
            double threshold;
            if (!double.TryParse(tbThresholdReg.Text, out threshold))
            {
                e.Cancel = true;
                tbThresholdReg.Select(0, tbThresholdReg.Text.Length);
                this.errorProvider1.SetError(tbThresholdReg, "Text must convert to a number.");
            }
            _plotThresholdVert = threshold;
        }

        private void tbThresholdReg_Validated(object sender, EventArgs e)
        {
            errorProvider1.SetError(tbThresholdReg, "");
        }


    }
}
