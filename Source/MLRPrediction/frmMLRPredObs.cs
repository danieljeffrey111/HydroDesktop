using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VBTools;

namespace MLRPrediction
{
    public partial class frmMLRPredObs : Form
    {

        private double _plotThresholdHoriz = double.NaN;
        private double _plotThresholdVert = double.NaN;
        private double _plotProbThreshold = double.NaN;

        private DataTable _dtObs = null;
        private DataTable _dtStats = null;
        
        public frmMLRPredObs( DataTable dtObs, DataTable dtStats)
        {
            InitializeComponent();
            _dtObs = dtObs;
            _dtStats = dtStats;

            mlrPredObs1.UpdateResults(GetObsPredData());
        }


        public void ConfigureDisplay(double decisionThreshold, double regulatoryThreshold, Globals.DependentVariableTransforms transform, double exponent)
        {
            mlrPredObs1.SetThresholds(decisionThreshold, regulatoryThreshold);
            mlrPredObs1.PowerExponent = exponent;
            mlrPredObs1.Transform = transform;
            mlrPredObs1.UpdateResults(GetObsPredData());
        }


        private List<double[]> GetObsPredData()
        {
            if ((_dtObs == null) || (_dtStats == null))
                return null;

            int minRecs = Math.Min(_dtObs.Rows.Count, _dtStats.Rows.Count);
            if (minRecs < 1)
                return null;

            List<double[]> data = new List<double[]>();
            double[] record = null;
            for (int i = 0; i < minRecs; i++)
            {
                record = new double[2];
                record[0] = Convert.ToDouble(_dtObs.Rows[i]["Observation"]);
                record[1] = Convert.ToDouble(_dtStats.Rows[i]["Model_Prediction"]);
                data.Add(record);
            }
            return data;
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void frmMLRPredObs_Load(object sender, EventArgs e)
        {
            VBTools.VBProjectManager _projMgr = VBProjectManager.GetProjectManager();            
            mlrPredObs1.Transform = _projMgr.ModelingInfo.DependentVariableTransform;

            if (_projMgr.ModelingInfo.DependentVariableTransform == Globals.DependentVariableTransforms.Power)
            {
                mlrPredObs1.PowerExponent = _projMgr.ModelingInfo.PowerTransformExponent;
            }
            mlrPredObs1.UpdateResults(GetObsPredData());
        }


        private void frmMLRPredObs_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            string apppath = Application.StartupPath.ToString();
            VBCSHelp help = new VBCSHelp(apppath, sender);
            if (!help.Status)
            {
                MessageBox.Show(
                "User documentation is found in the Documentation folder where you installed Virtual Beach"
                + "\nIf your web browser is PDF-enabled, open the User Guide with your browser.",
                "Neither Adobe Acrobat nor Adobe Reader found.",
                MessageBoxButtons.OK);
            }
        }
    }
}
