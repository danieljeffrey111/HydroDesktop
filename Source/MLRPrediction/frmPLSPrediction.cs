using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using VBTools;
using VBTools.IO;
using VBStatistics;
using Ciloci.Flee;

namespace MLRPrediction
{
    public partial class frmPLSPrediction : DockContent
    {
        private VBProjectManager _proj = null;
        private string _modelExpression = "";

        private DataTable _corrDT = null;
        private DataTable _dtVariables = null;
        private DataTable _dtObs = null;
        private DataTable _dtStats = null;
        private Dictionary<string, double> _model;
        private Dictionary<string, string> _mainEffects;

        private dynamic ipyInterface;
        private dynamic pls_model;


        public frmPLSPrediction()
        {
            InitializeComponent();
            _proj = VBProjectManager.GetProjectManager();

            _proj.Project.ProjectOpened += new VBProject.ProjectOpenedHandler(ProjectOpenedListener);
            _proj.Project.ProjectSaved += new VBProject.ProjectSavedHandler(ProjectSavedListener);

        }

        public void SetModel(dynamic model)
        {
            this.pls_model = model;
            dynamic predictors = ipyInterface.Get_Predictors(model);
            List<string> Predictors = ((IList<object>)((IList<object>)predictors)).Cast<string>().ToList();
            //if we have a model selected, go.  otherwise go away
            txtModel.Text = "";

            //if ((_proj.Model == null) || (_proj.Model.Count <= 0))
            //    return;

            //_proj = VBProjectManager.GetProjectManager();
            //_mlrm = MLRModel.getMLRModel();

            //Not sure which datatable to use.
            //if (_proj.ModelDataTable != null)
            //    _corrDT = _proj.ModelDataTable;
            //else
            //    _corrDT = _proj.CorrelationDataTable;

            //Lets get all the main effect variables
            _mainEffects = new Dictionary<string, string>();
            for (int i=0; i<Predictors.Count; i++ )
            {
                _mainEffects.Add(Predictors[i], Predictors[i]);                
            }

            float reg_limit = (float)model.regulatory_threshold;
            float threshold = (float)model.threshold;

            txtRegStd.Text = Math.Round(reg_limit, 3).ToString();
            txtDecCrit.Text = Math.Round(threshold, 3).ToString();
        }


        public void SetIronPythonInterface(dynamic ipyInterface)
        {
            this.ipyInterface = ipyInterface;
        }


        private void ProjectOpenedListener(VBProject project)
        {

        }

        private void ProjectSavedListener(VBProject project)
        {

        }


        private void dgvVariables_Scroll(object sender, ScrollEventArgs e)
        {            
            if (dgvObs.Rows.Count > 0)
                dgvObs.FirstDisplayedScrollingRowIndex = dgvVariables.FirstDisplayedScrollingRowIndex;

            if (dgvStats.Rows.Count > 0)
                dgvStats.FirstDisplayedScrollingRowIndex = dgvVariables.FirstDisplayedScrollingRowIndex;

            //dgvObs.HorizontalScrollingOffset = dgvVariables.HorizontalScrollingOffset;
            //dgvStats.HorizontalScrollingOffset = dgvVariables.HorizontalScrollingOffset;
        }

        private void dgvObs_Scroll(object sender, ScrollEventArgs e)
        {
            if (dgvVariables.Rows.Count > 0)
                dgvVariables.FirstDisplayedScrollingRowIndex = dgvObs.FirstDisplayedScrollingRowIndex;
            if (dgvStats.Rows.Count > 0)
                dgvStats.FirstDisplayedScrollingRowIndex = dgvObs.FirstDisplayedScrollingRowIndex;
        }

        private void dgvStats_Scroll(object sender, ScrollEventArgs e)
        {
            if (dgvVariables.Rows.Count > 0)
                dgvVariables.FirstDisplayedScrollingRowIndex = dgvStats.FirstDisplayedScrollingRowIndex;
            if (dgvObs.Rows.Count > 0)
                dgvObs.FirstDisplayedScrollingRowIndex = dgvStats.FirstDisplayedScrollingRowIndex;
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            if (btnChange.Text == "Change")
            {
                txtDecCrit.ReadOnly = false;
                txtRegStd.ReadOnly = false;
                btnChange.Text = "Save";
            }
            else if (btnChange.Text == "Save")
            {
                txtDecCrit.ReadOnly = true;
                txtRegStd.ReadOnly = true;
                btnChange.Text = "Change";
            }
        }

        private void btnImportIVs_Click(object sender, EventArgs e)
        {
            VBTools.ImportExport import = new ImportExport();
            DataTable dt = import.Input;
            if (dt == null)
                return;

            string[] headerCaptions = { "Model Variables", "Imported Variables" };
            frmColumnMapper colMapper = new frmColumnMapper(_mainEffects, dt, headerCaptions, false);
            DialogResult dr = colMapper.ShowDialog();

            if (dr == DialogResult.OK)
            {
                dt = colMapper.MappedTable;
                dgvVariables.DataSource = dt;
            }

            foreach (DataGridViewColumn dvgCol in dgvVariables.Columns)
                dvgCol.SortMode = DataGridViewColumnSortMode.NotSortable;


        }

        private void btnImportObs_Click(object sender, EventArgs e)
        {
            VBTools.ImportExport import = new ImportExport();
            DataTable dt = import.Input;
            if (dt == null)
                return;

            string[] headerCaptions = { "Obs IDs", "Obs" };
            Dictionary<string, string> obsColumns = new Dictionary<string, string>();
            //obsColumns.Add("id", "id");
            obsColumns.Add(pls_model.target, pls_model.target);

            frmColumnMapper colMapper = new frmColumnMapper(obsColumns, dt, headerCaptions, false);
            DialogResult dr = colMapper.ShowDialog();

            if (dr == DialogResult.OK)
            {
                dt = colMapper.MappedTable;
                dgvObs.DataSource = dt;
            }

            string target = pls_model.target;
            for(int indx=0; indx<dgvObs.RowCount; indx++)
                if (Convert.ToDouble(dgvObs.Rows[indx].Cells[target].Value) > pls_model.regulatory_threshold )
                    dgvObs.Rows[indx].Cells[target].Style.ForeColor = Color.Red;

            foreach (DataGridViewColumn dvgCol in dgvObs.Columns)
                dvgCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                        
        }

        private void btnMakePredictions_Click(object sender, EventArgs e)
        {
            /*string[] vars = _model.Keys.ToArray();
            //VBStatistics.MultipleRegression mlr = new MultipleRegression(_corrDT, _proj.Project.ModelingInfo.DependentVariable, vars);
            //mlr.Compute();

            dgvVariables.EndEdit();
            _dtVariables = (DataTable)dgvVariables.DataSource;
            _dtVariables.AcceptChanges();

            dgvObs.EndEdit();
            _dtObs = (DataTable)dgvObs.DataSource;
            _dtObs.AcceptChanges();*/

            DataTable tblPrediction = (DataTable)dgvVariables.DataSource;
            dynamic predictions = ipyInterface.Predict(pls_model, tblPrediction);
            List<object> Predictions = ((IList<object>)predictions).ToList();

            /*List<double> lstPredictions = expEval.Evaluate(_modelExpression, _dtVariables);
             _dtStats = GeneratePredStats(lstPredictions, _dtObs);
            dgvStats.DataSource = _dtStats;

            foreach (DataGridViewColumn dvgCol in dgvStats.Columns)
                dvgCol.SortMode = DataGridViewColumnSortMode.NotSortable;*/
            System.Data.DataTable prediction_table = new System.Data.DataTable();

            //Set the headers
            prediction_table.Columns.Add("Prediction");
            List<bool> exceedances = new List<bool>();
            double threshold = Convert.ToDouble(txtDecCrit.Text);

            foreach (double result in Predictions)
            {
                double row = Math.Round(result, 3);
                prediction_table.Rows.Add(values: row);

                //Decide whether we are predicting an exceedance
                if (Convert.ToDouble(result) >= threshold) exceedances.Add(true);
                else exceedances.Add(false);
            }

            dgvStats.DataSource = prediction_table;
            dgvStats.AutoResizeColumns();

            for ( int i=0; i<exceedances.Count; i++ )
            {
                if (exceedances[i]) dgvStats.Rows[i].Cells[0].Style.ForeColor = Color.Red;
            }


        }

        private void dgvVariables_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            dgvVariables.EndEdit();
            _dtVariables = (DataTable)dgvVariables.DataSource;
            _dtVariables.AcceptChanges();
        }

        private void dgvObs_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            dgvObs.EndEdit();
            _dtObs = (DataTable)dgvVariables.DataSource;
            _dtObs.AcceptChanges();
        }


        private DataTable GeneratePredStats(List<double> lstPredictions, DataTable dtObs)
        {

            double decCrit = Convert.ToDouble(txtDecCrit.Text);
            double regStd = Convert.ToDouble(txtRegStd.Text);

            DataTable dt = new DataTable();

            dt.Columns.Add("Model_Prediction", typeof(double));
            dt.Columns.Add("Decision_Criterion", typeof(double));
            dt.Columns.Add("Exceedance_Probability", typeof(double));
            dt.Columns.Add("Regulatory_Standard", typeof(double));
            dt.Columns.Add("Error_Type", typeof(string));
            //dt.Columns.Add("Back Transformation", typeof(double));

            
            
            for (int i = 0; i < lstPredictions.Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr["Model_Prediction"] = lstPredictions[i];
                dr["Decision_Criterion"] = decCrit;
                dr["Exceedance_Probability"] = Statistics.PExceed(lstPredictions[i], decCrit, _proj.Project.ModelingInfo.RMSE);
                dr["Regulatory_Standard"] = regStd;


                //determine if we have an error and its type
                //No guarentee we have same num of obs as we do predictions or that we have any obs at all
                if ((dtObs != null ) && (i <= dtObs.Rows.Count - 1))
                {
                    string errType = "";
                    double obs = (double)dtObs.Rows[i]["Observation"];
                    if ((lstPredictions[i] > decCrit) && (obs < regStd))
                        errType = "False Positive";
                    else if ((obs > regStd) && (lstPredictions[i] < decCrit))
                        errType = "False Negative";

                    dr["Error_Type"] = errType;
                }
                

                dt.Rows.Add(dr);
            }

            return dt;            

        }

        //Keeps the Import Obs button lined up with the Obs grid
        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            btnImportObs.Left =  e.X + 10;
        }


        private void splitContainer2_SplitterMoved(object sender, SplitterEventArgs e)
        {
            int left1 = splitContainer1.Panel2.Left;
            int left2 = splitContainer2.Panel2.Left;
            btnPlot.Left = left1 + left2 + 10;
        }


        private void dgvVariables_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            int rowIdx = e.RowIndex;
            if (rowIdx < dgvObs.Rows.Count)
                dgvObs.Rows[rowIdx].Selected = true;

            if (rowIdx < dgvStats.Rows.Count)
                dgvStats.Rows[rowIdx].Selected = true;
            
        }

        private void dgvObs_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            int rowIdx = e.RowIndex;
            if (rowIdx < dgvVariables.Rows.Count)
                dgvVariables.Rows[rowIdx].Selected = true;

            if (rowIdx < dgvStats.Rows.Count)
                dgvStats.Rows[rowIdx].Selected = true;
        }

        private void dgvStats_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            int rowIdx = e.RowIndex;
            if (rowIdx < dgvVariables.Rows.Count)
                dgvVariables.Rows[rowIdx].Selected = true;

            if (rowIdx < dgvObs.Rows.Count)
                dgvObs.Rows[rowIdx].Selected = true;
        }

        private void frmMLRPrediction_Load(object sender, EventArgs e)
        {
            
        }

        private void dgvVariables_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            int rowIdx = e.RowIndex;
            if (rowIdx < dgvObs.Rows.Count)
                dgvObs.Rows[rowIdx].Selected = false;

            if (rowIdx < dgvStats.Rows.Count)
                dgvStats.Rows[rowIdx].Selected = false;
        }

        private void dgvObs_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            int rowIdx = e.RowIndex;
            if (rowIdx < dgvVariables.Rows.Count)
                dgvVariables.Rows[rowIdx].Selected = false;

            if (rowIdx < dgvStats.Rows.Count)
                dgvStats.Rows[rowIdx].Selected = false;
        }

        private void dgvStats_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            int rowIdx = e.RowIndex;
            if (rowIdx < dgvVariables.Rows.Count)
                dgvVariables.Rows[rowIdx].Selected = false;

            if (rowIdx < dgvObs.Rows.Count)
                dgvObs.Rows[rowIdx].Selected = false;
        }

        private void btnSaveTable_Click(object sender, EventArgs e)
        {
            dgvStats.EndEdit();
            _dtStats = (DataTable)dgvStats.DataSource;
            _dtStats.AcceptChanges();

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Save Prediction Data";
            sfd.Filter = @"VB2 Prediction Files|*.vbpred|All Files|*.*";

            DialogResult dr = sfd.ShowDialog();
            if (dr == DialogResult.Cancel)
                return;

            string fileName = sfd.FileName;

            DataSet ds = new DataSet();
            _dtVariables.TableName = "Variables";
            _dtObs.TableName = "Observations";
            _dtStats.TableName = "Stats";

            ds.Tables.Add(_dtVariables);
            ds.Tables.Add(_dtObs);
            ds.Tables.Add(_dtStats);

            //string dsXML = ds.GetXml();

            ds.WriteXml(fileName, XmlWriteMode.WriteSchema);
        }

        private void btnImportTable_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Open Prediction Data";
            ofd.Filter = @"VB2 Prediction Files|*.vbpred|All Files|*.*";
            DialogResult dr = ofd.ShowDialog();
            if (dr == DialogResult.Cancel)
                return;

            string fileName = ofd.FileName;

            DataSet ds = new DataSet();
            ds.ReadXml(fileName, XmlReadMode.ReadSchema);

            if (ds.Tables.Contains("Variables") == false)
            {
                MessageBox.Show("Invalid Prediction Dataset.  Does not contain variable information.");
                return;
            }


            _dtVariables = ds.Tables["Variables"];
            _dtObs = ds.Tables["Observations"];
            _dtStats = ds.Tables["Stats"];

            dgvVariables.DataSource = _dtVariables;
            dgvObs.DataSource = _dtObs;
            dgvStats.DataSource = _dtStats;

        }

        private void btnClearTable_Click(object sender, EventArgs e)
        {
            dgvVariables.DataSource = null;
            dgvObs.DataSource = null;
            dgvStats.DataSource = null;

            _dtVariables.Clear();
            _dtVariables = null;

            _dtObs.Clear();
            _dtObs = null;

            _dtStats.Clear();
            _dtStats = null;
        }







    }
}
