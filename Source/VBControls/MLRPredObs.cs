using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VBTools;
using VBStatistics;
using ZedGraph;

namespace VBControls
{
    public partial class MLRPredObs : UserControl
    {
        private List<double[]> _XYPlotdata;
        //Threshold value used for sensitiviy, specificity, accuracy
        double _decisionThreshold;
        double _mandateThreshold;
        double _powerExp = double.NaN;

        public MLRPredObs()
        {
            InitializeComponent();

            _decisionThreshold = Convert.ToDouble(tbThresholdDec.Text);
            _mandateThreshold = Convert.ToDouble(tbThresholdDec.Text);

            InitResultsGraph();
        }

        public double ThresholdHoriz
        {
            get { return _decisionThreshold; }
        }
        public double ThresholdVert
        {
            get { return _mandateThreshold; }
        }

        public void SetThresholds(double decisionThreshold, double mandateThreshold)
        {
            _decisionThreshold = decisionThreshold;
            _mandateThreshold = mandateThreshold;
            tbThresholdDec.Text = _decisionThreshold.ToString();
            tbThresholdReg.Text = _mandateThreshold.ToString();
        }

        public void SetThresholds(string decisionThreshold, string mandateThreshold)
        {
            _decisionThreshold = Convert.ToDouble(decisionThreshold);
            _mandateThreshold = Convert.ToDouble(mandateThreshold);
            tbThresholdDec.Text = decisionThreshold;
            tbThresholdReg.Text = mandateThreshold;
        }

        public Globals.DependentVariableTransforms Transform
        {
            set 
            {
                EventArgs args = new EventArgs();

                if (value == Globals.DependentVariableTransforms.none)
                {
                    rbValue.Checked = true;
                    _decisionThreshold = Convert.ToDouble(tbThresholdDec.Text);
                    _mandateThreshold = Convert.ToDouble(tbThresholdReg.Text);
                    rbValue_CheckedChanged(this, args);
                }
                else if (value == Globals.DependentVariableTransforms.Ln)
                {
                    rbLogeValue.Checked = true;
                    _decisionThreshold = Math.Log(Convert.ToDouble(tbThresholdDec.Text));
                    _mandateThreshold = Math.Log(Convert.ToDouble(tbThresholdReg.Text));
                    rbLogeValue_CheckedChanged(this, args);
                }
                else if (value == Globals.DependentVariableTransforms.Log10)
                {
                    rbLog10Value.Checked = true;
                    _decisionThreshold = Math.Log10(Convert.ToDouble(tbThresholdDec.Text));
                    _mandateThreshold = Math.Log10(Convert.ToDouble(tbThresholdReg.Text));
                    rbLog10Value_CheckedChanged(this, args);
                }
                else if (value == Globals.DependentVariableTransforms.Power)
                {
                    rbPwrValue.Checked = true;
                    double pwr = Convert.ToDouble(txtPwrValue.Text);
                    _decisionThreshold = Math.Pow(Convert.ToDouble(tbThresholdDec.Text),pwr);
                    _mandateThreshold = Math.Pow(Convert.ToDouble(tbThresholdReg.Text),pwr);
                    rbPwrValue_CheckedChanged(this, args);
                }

                this.Refresh();
            }

            get
            {               
                if (rbLogeValue.Checked)
                    return Globals.DependentVariableTransforms.Ln;
                else if (rbLog10Value.Checked)
                    return Globals.DependentVariableTransforms.Log10;
                else if (rbPwrValue.Checked)
                    return Globals.DependentVariableTransforms.Power;
                else //(rbValue.Checked)
                    return Globals.DependentVariableTransforms.none;
            }                        
        }

        public double PowerExponent
        {
            get { return _powerExp; }
            set 
            {
                    _powerExp = value; 
                    txtPwrValue.Text = _powerExp.ToString(); 
            }
        }

        private void InitResultsGraph()
        {
            _XYPlotdata = new List<double[]>();

            GraphPane myPane = zedGraphControl1.GraphPane;
            if (myPane.CurveList.Count > 0)
                myPane.CurveList.RemoveRange(0, myPane.CurveList.Count - 1);

            myPane.Title.Text = "Results";
            myPane.XAxis.Title.Text = "Observed";
            myPane.YAxis.Title.Text = "Predicted";

            myPane.XAxis.MajorGrid.IsVisible = true;
            myPane.XAxis.MajorGrid.Color = Color.Gray;

            myPane.YAxis.MajorGrid.IsVisible = true;
            myPane.YAxis.MajorGrid.Color = Color.Gray;

            PointPairList list = new PointPairList();

            LineItem curve = myPane.AddCurve("Y", list, Color.Red, SymbolType.Circle);
            curve.Line.IsVisible = false;
            // Hide the symbol outline
            curve.Symbol.Border.IsVisible = true;
            // Fill the symbol interior with color
            curve.Symbol.Fill = new Fill(Color.Firebrick);

            //Vertical and horizontal threshold lines
            PointPairList list2 = new PointPairList();
            LineItem curve2 = myPane.AddCurve("Decision Threshold", list2, Color.Blue, SymbolType.None);
            curve2.Line.IsVisible = false;

            PointPairList list3 = new PointPairList();
            LineItem curve3 = myPane.AddCurve("Regulatory Threshold", list3, Color.Green, SymbolType.None);
            curve3.Line.IsVisible = false;

            // Scale the axes
            zedGraphControl1.AxisChange();
        }

        public void UpdateResults(List<double[]> data)
        {
            _XYPlotdata = data;

            if ((_XYPlotdata == null) || (_XYPlotdata.Count < 1))
            {
                //zedGraphControl1.GraphPane.CurveList.Clear();
                zedGraphControl1.GraphPane.CurveList[0].Clear();
                zedGraphControl1.Refresh();
                return;
            }
                        
            // Make sure that the curvelist has at least one curve
            if (zedGraphControl1.GraphPane.CurveList.Count <= 0)
                return;

            // Get the first CurveItem in the graph
            LineItem curve = zedGraphControl1.GraphPane.CurveList[0] as LineItem;
            if (curve == null)
                return;

            // Get the PointPairList
            IPointListEdit list = curve.Points as IPointListEdit;

            // If this is null, it means the reference at curve.Points does not
            // support IPointListEdit, so we won't be able to modify it
            if (list == null)
                return;

            list.Clear();

            double maxX = Double.NegativeInfinity;
            double maxY = Double.NegativeInfinity;
            double minX = 0;
            double minY = 0;
            for (int i = 0; i < data.Count; i++)
            {
                list.Add(data[i][0], data[i][1]);
                if (data[i][0] > maxX)
                    maxX = data[i][0];
                if (data[i][1] > maxY)
                    maxY = data[i][1];
                if (data[i][0] < minX)
                    minX = data[i][0];
                if (data[i][1] < minY)
                    minY = data[i][1];
            }

            //if data out of range of thresholds, make the threshold plot lines bigger
            if (_decisionThreshold > maxX) maxX = _mandateThreshold; //_decisionThreshold;
            if (_mandateThreshold > maxY) maxY = _decisionThreshold; //_mandateThreshold;

            //find the model error counts for the XYplot display
            //comment out because not using 
            ModelErrorCounts mec = new ModelErrorCounts();
            mec.getCounts(_decisionThreshold, _mandateThreshold, data);
            if (mec.Status)
            {
                int fpc = mec.FPCount;
                int fnc = mec.FNCount;
                tbFN.Text = fnc.ToString();
                tbFP.Text = fpc.ToString();
                txbSpecificity.Text = mec.Specificity.ToString();
                txbSensitivity.Text = mec.Sensitivity.ToString();
                txbAccuracy.Text = mec.Accuracy.ToString();
            }
            else
            {
                string msg = "Data Error: " + mec.Message.ToString();
                MessageBox.Show(msg);
                return;
            }


            LineItem curve2 = zedGraphControl1.GraphPane.CurveList[1] as LineItem;
            LineItem curve3 = zedGraphControl1.GraphPane.CurveList[2] as LineItem;
            if ((curve2 != null) && (curve3 != null))
            {
                curve2.Line.IsVisible = false;
                curve3.Line.IsVisible = false;
                //if (chkThreshold.Checked)
                //{
                // Get the PointPairList
                IPointListEdit list2 = curve2.Points as IPointListEdit;
                list2.Clear();
                //mikec want the thresholds crossing, thus the "-1", "+1"
                list2.Add(minX - 1, _decisionThreshold);
                list2.Add(maxX + 1, _decisionThreshold);
                curve2.Line.IsVisible = true;

                // Get the PointPairList
                //mikec want the thresholds crossing, thus the "-1", "+1"
                IPointListEdit list3 = curve3.Points as IPointListEdit;
                list3.Clear();
                list3.Add(_mandateThreshold, minY - 1);
                list3.Add(_mandateThreshold, maxY + 1);
                curve3.Line.IsVisible = true;
                // }
            }



            // Keep the X scale at a rolling 30 second interval, with one
            // major step between the max X value and the end of the axis
            Scale xScale = zedGraphControl1.GraphPane.XAxis.Scale;
            if (data[data.Count - 1][0] > xScale.Max - xScale.MajorStep)
            {
                //xScale.Max = data[data.Count - 1][0] + xScale.MajorStep;
                //xScale.Min = xScale.Max - 30.0;
            }

            //mikec - get rid of the line at y=0...?
            GraphPane zgc1pane = zedGraphControl1.GraphPane;
            //...best I can do for now
            zgc1pane.XAxis.Cross = 0;
            
            // Make sure the Y axis is rescaled to accommodate actual data
            zedGraphControl1.AxisChange();
            // Force a redraw
            zedGraphControl1.Invalidate();
            //listBox1.Refresh();
            zedGraphControl1.Refresh();
            Application.DoEvents();
        }

        private void tbThresholdReg_TextChanged(object sender, EventArgs e)
        {

            if (Double.TryParse(tbThresholdReg.Text, out _mandateThreshold) == false)
            {
                //string msg = @"Mandate threshold must be a numeric value.";
                string msg = @"Regulatory standard must be a numeric value.";
                MessageBox.Show(msg);
                return;
            }
        }

        private void tbThresholdDec_TextChanged(object sender, EventArgs e)
        {
            if (Double.TryParse(tbThresholdDec.Text, out _decisionThreshold) == false)
            {
                string msg = @"Decision criterion must be a numeric value.";
                MessageBox.Show(msg);
                return;
            }
        }

        private void txtPwr_Leave(object sender, EventArgs e)
        {
            double power;
            TextBox txtBox = (TextBox)sender;

            if (!Double.TryParse(txtBox.Text, out power))
            {
                MessageBox.Show("Invalid number.");
                txtBox.Focus();
            }
        }

        private void rbPwrValue_CheckedChanged(object sender, EventArgs e)
        {
            if (rbPwrValue.Checked)
                txtPwrValue.Enabled = true;
            else
                txtPwrValue.Enabled = false;
        }

        private void btnXYPlot_Click(object sender, EventArgs e)
        {
            this.Refresh();
        }

        public void Refresh()
        {
            //just validate and get the thresholds, and then transform
            tbThresholdDec_TextChanged(null, EventArgs.Empty);
            tbThresholdReg_TextChanged(null, EventArgs.Empty);
            if (rbValue.Checked)
                rbValue_CheckedChanged(null, EventArgs.Empty);
            else if (rbLog10Value.Checked)
                rbLog10Value_CheckedChanged(null, EventArgs.Empty);
            else if (rbLogeValue.Checked)
                rbLogeValue_CheckedChanged(null, EventArgs.Empty);
            else if (rbPwrValue.Checked)
                rbPwrValue_Changed(null, EventArgs.Empty);
            //now plot it
            //if (_XYPlotdata.Count > 0)
            //{
            UpdateResults(_XYPlotdata);
            //}
        }

        private void rbValue_CheckedChanged(object sender, EventArgs e)
        {
            if (rbValue.Checked)
            {
                double tv = double.NaN;
                double th = double.NaN;
                try
                {
                    tv = Convert.ToDouble(tbThresholdReg.Text.ToString());
                    th = Convert.ToDouble(tbThresholdDec.Text.ToString());
                }
                catch
                {
                    string msg = @"Cannot convert thresholds. (" + tbThresholdDec.Text + ", " + tbThresholdReg.Text + ") ";
                    MessageBox.Show(msg);
                    return;
                }

                _mandateThreshold = tv;
                _decisionThreshold = th;
                //UpdateResults2(_XYPlotdata);
            }
        }

        private void rbLog10Value_CheckedChanged(object sender, EventArgs e)
        {
            if (rbLog10Value.Checked)
            {
                double tv = double.NaN;
                double th = double.NaN;
                //ms has no fp error checking... check for all conditions.
                //log10(x) when x == 0 results in NaN and when x < 0 results in -Infinity
                try
                {
                    tv = Math.Log10(Convert.ToDouble(tbThresholdReg.Text.ToString()));
                    th = Math.Log10(Convert.ToDouble(tbThresholdDec.Text.ToString()));
                }
                catch
                {
                    string msg = @"Cannot Log 10 transform thresholds. (" + tbThresholdDec.Text + ", " + tbThresholdReg.Text + ") ";
                    MessageBox.Show(msg);
                    return;
                }
                if (tv.Equals(double.NaN) || th.Equals(double.NaN))
                {
                    string msg = @"Entered values must be greater than 0. (" + tbThresholdDec.Text + ", " + tbThresholdReg.Text + ") ";
                    MessageBox.Show(msg);
                    return;
                }
                if (tv < 0 || th < 0)
                {
                    string msg = @"Entered values must be greater than 0. (" + tbThresholdDec.Text + ", " + tbThresholdReg.Text + ") ";
                    MessageBox.Show(msg);
                    return;
                }

                _mandateThreshold = tv;
                _decisionThreshold = th;
                //UpdateResults2(_XYPlotdata);
            }
        }

        private void rbLogeValue_CheckedChanged(object sender, EventArgs e)
        {
            if (rbLogeValue.Checked)
            {
                double tv = double.NaN;
                double th = double.NaN;
                //ms has no fp error checking... check for all conditions.
                //loge(x) when x == 0 results in NaN and when x < 0 results in -Infinity
                try
                {
                    tv = Math.Log(Convert.ToDouble(tbThresholdReg.Text.ToString()));
                    th = Math.Log(Convert.ToDouble(tbThresholdDec.Text.ToString()));
                }
                catch
                {
                    string msg = @"Cannot Log e transform thresholds. (" + tbThresholdDec.Text + ", " + tbThresholdReg.Text + ") ";
                    MessageBox.Show(msg);
                    return;
                }
                if (tv.Equals(double.NaN) || th.Equals(double.NaN))
                {
                    string msg = @"Entered values must be greater than 0. (" + tbThresholdDec.Text + ", " + tbThresholdReg.Text + ") ";
                    MessageBox.Show(msg);
                    return;
                }
                if (tv < 0 || th < 0)
                {
                    string msg = @"Entered values must be greater than 0. (" + tbThresholdDec.Text + ", " + tbThresholdReg.Text + ") ";
                    MessageBox.Show(msg);
                    return;
                }

                _mandateThreshold = tv;
                _decisionThreshold = th;
                //UpdateResults2(_XYPlotdata);
            }

        }


        private void rbPwrValue_Changed(object sender, EventArgs e)
        {
            if (rbPwrValue.Checked)
            {
                double tv = double.NaN;
                double th = double.NaN;
                //ms has no fp error checking... check for all conditions.
                //loge(x) when x == 0 results in NaN and when x < 0 results in -Infinity
                try
                {
                    double power = Convert.ToDouble(txtPwrValue.Text);
                    tv = Math.Pow(Convert.ToDouble(tbThresholdReg.Text), power);
                    th = Math.Pow(Convert.ToDouble(tbThresholdDec.Text), power);
                }
                catch
                {
                    string msg = @"Cannot power transform thresholds. (" + tbThresholdDec.Text + ", " + tbThresholdReg.Text + ") ";
                    MessageBox.Show(msg);
                    return;
                }
                if (tv.Equals(double.NaN) || th.Equals(double.NaN))
                {
                    string msg = @"Entered values must be greater than 0. (" + tbThresholdDec.Text + ", " + tbThresholdReg.Text + ") ";
                    MessageBox.Show(msg);
                    return;
                }
                if (tv < 0 || th < 0)
                {
                    string msg = @"Entered values must be greater than 0. (" + tbThresholdDec.Text + ", " + tbThresholdReg.Text + ") ";
                    MessageBox.Show(msg);
                    return;
                }

                _mandateThreshold = tv;
                _decisionThreshold = th;
                //UpdateResults2(_XYPlotdata);
            }
        }
    }
}
