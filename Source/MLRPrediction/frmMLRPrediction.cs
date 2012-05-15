using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using VBTools;
using VBTools.IO;
using VBStatistics;
using VBControls;
using Ciloci.Flee;

namespace MLRPrediction
{
    public partial class frmMLRPrediction : DockContent, IFormState
    {
        private ContextMenu cmforResponseVar = new ContextMenu();
        private VBProjectManager _projMgr = null;
        private Dictionary<string, double> _model;
        private Dictionary<string, string> _mainEffects;
        private string _modelExpression = "";
        //These are the main effects variables actually include in the model
        //It will also have ID as first element
        private string[] _referencedVariables = null;

        private DataTable _corrDT = null;
        private DataTable _dtVariables = null;
        private DataTable _dtObs = null;
        private DataTable _dtStats = null;

        private string _respVarTransform = "none";

        private DataTable _dtObsOrig = null;
        private bool _obsTransformed = false;

        private bool _projectOpened;

        private Dictionary<string, double> _currentModel = null;

        public frmMLRPrediction()
        {
            InitializeComponent();
            _projMgr = VBProjectManager.GetProjectManager();

            //_projMgr.UnpackRequest += new VBProjectManager.EventHandler<UnpackEventArgs>(UnpackState);
            _projMgr.ProjectOpened += new VBProjectManager.ProjectOpenedHandler(ProjectOpenedListener);
            _projMgr.ProjectSaved += new VBProjectManager.ProjectSavedHandler<PackEventArgs>(ProjectSavedListener);

        }

        public void UnpackState(object objPackedStates)
        {
            PredInfo formPackedState = (PredInfo)objPackedStates;

            if (_model == null) return;

            _currentModel = _model;

            //Console.WriteLine("\n*** Prediction: project opened.***\n");

            DataSet ds = null;

            //PredInfo predInfo = _projMgr.PredictionInfo;
            if (formPackedState == null)
            {
                //this.Hide();
                return;
            }

            if (!String.IsNullOrWhiteSpace(formPackedState.IVData))
            {
                ds = new DataSet();
                ds.ReadXml(new StringReader(formPackedState.IVData), XmlReadMode.ReadSchema);
                _dtVariables = ds.Tables[0];
                dgvVariables.DataSource = _dtVariables;

                setViewOnGrid(dgvVariables);
            }

            if (!String.IsNullOrWhiteSpace(formPackedState.ObsData))
            {
                ds = new DataSet();
                ds.ReadXml(new StringReader(formPackedState.ObsData), XmlReadMode.ReadSchema);
                _dtObs = ds.Tables[0];
                dgvObs.DataSource = _dtObs;
                setViewOnGrid(dgvObs);
            }

            if (!String.IsNullOrWhiteSpace(formPackedState.StatData))
            {
                ds = new DataSet();
                ds.ReadXml(new StringReader(formPackedState.StatData), XmlReadMode.ReadSchema);
                _dtStats = ds.Tables[0];
                dgvStats.DataSource = _dtStats;
                setViewOnGrid(dgvStats);
            }

            ds = null;

            if (double.IsNaN(formPackedState.RegulatoryStandard))
                txtRegStd.Text = "255";
            else
                txtRegStd.Text = formPackedState.RegulatoryStandard.ToString();

            if (double.IsNaN(formPackedState.DecisionCriteria))
                txtDecCrit.Text = "255";
            else
                txtDecCrit.Text = formPackedState.DecisionCriteria.ToString();

            if (double.IsNaN(formPackedState.PowerTransform))
                txtPower.Text = "255";
            else
                txtPower.Text = formPackedState.PowerTransform.ToString();

            if (formPackedState.DependentVariableTransform == Globals.DependentVariableTransforms.none)
                rbNone.Checked = true;
            else if (formPackedState.DependentVariableTransform == Globals.DependentVariableTransforms.Log10)
                rbLog10.Checked = true;
            else if (formPackedState.DependentVariableTransform == Globals.DependentVariableTransforms.Ln)
                rbLn.Checked = true;
            else if (formPackedState.DependentVariableTransform == Globals.DependentVariableTransforms.Power)
                rbPower.Checked = true;

            _projectOpened = true;

            txtModel.Text = Support.BuildModelExpression(_model, _projMgr.ModelingInfo.DependentVariable, "");
            _modelExpression = Support.BuildModelExpression(_model, null, "");

            _projMgr._comms.sendMessage("Show the MLR Prediction tab.", this);
        }

        private void ProjectOpenedListener()
        {
            if (_model == null) return;

            _currentModel = _model;

            //Console.WriteLine("\n*** Prediction: project opened.***\n");

            DataSet ds = null;

            PredInfo predInfo = _projMgr.PredictionInfo;
            if (predInfo == null)
            {
                //this.Hide();
                return; 
            }
            
            if (!String.IsNullOrWhiteSpace(predInfo.IVData))
            {
                ds = new DataSet();
                ds.ReadXml(new StringReader(predInfo.IVData), XmlReadMode.ReadSchema);
                _dtVariables = ds.Tables[0];
                dgvVariables.DataSource = _dtVariables;

                setViewOnGrid(dgvVariables);
            }

            if (!String.IsNullOrWhiteSpace(predInfo.ObsData))
            {
                ds = new DataSet();
                ds.ReadXml(new StringReader(predInfo.ObsData), XmlReadMode.ReadSchema);
                _dtObs = ds.Tables[0];
                dgvObs.DataSource = _dtObs;
                setViewOnGrid(dgvObs);
            }

            if (!String.IsNullOrWhiteSpace(predInfo.StatData))
            {
                ds = new DataSet();
                ds.ReadXml(new StringReader(predInfo.StatData), XmlReadMode.ReadSchema);
                _dtStats = ds.Tables[0];
                dgvStats.DataSource = _dtStats;
                setViewOnGrid(dgvStats);
            }

            ds = null;

            if (double.IsNaN(predInfo.RegulatoryStandard))
                txtRegStd.Text = "255";
            else
                txtRegStd.Text = predInfo.RegulatoryStandard.ToString();

            if (double.IsNaN(predInfo.DecisionCriteria))
                txtDecCrit.Text = "255";
            else
                txtDecCrit.Text = predInfo.DecisionCriteria.ToString();

            if (double.IsNaN(predInfo.PowerTransform))
                txtPower.Text = "255";
            else
                txtPower.Text = predInfo.PowerTransform.ToString();

            if (predInfo.DependentVariableTransform == Globals.DependentVariableTransforms.none)
                rbNone.Checked = true;
            else if (predInfo.DependentVariableTransform == Globals.DependentVariableTransforms.Log10)
                rbLog10.Checked = true;
            else if (predInfo.DependentVariableTransform == Globals.DependentVariableTransforms.Ln)
                rbLn.Checked = true;
            else if (predInfo.DependentVariableTransform == Globals.DependentVariableTransforms.Power)
                rbPower.Checked = true;

            _projectOpened = true;

            txtModel.Text = Support.BuildModelExpression(_model, _projMgr.ModelingInfo.DependentVariable, "");
            _modelExpression = Support.BuildModelExpression(_model, null, "");

            _projMgr._comms.sendMessage("Show the MLR Prediction tab.", this);
        }

        
        private void ProjectSavedListener(object sender, PackEventArgs e)
        {
            PredInfo localMLRPrediction = new PredInfo();

            //First, check whether there's anything to save.
            if ((_projMgr.Model == null) || (_projMgr.Model.Count <= 0))
            {
                _projMgr.PredictionInfo = null;
                return;
            }

           // PredInfo predInfo = new PredInfo();
            StringWriter sw = null;

            dgvVariables.EndEdit();
            _dtVariables = (DataTable)dgvVariables.DataSource;
            if (_dtVariables != null)
            {
                _dtVariables.AcceptChanges();
                _dtVariables.TableName = "Variables";
                sw = new StringWriter();
                _dtVariables.WriteXml(sw, XmlWriteMode.WriteSchema, false);
                localMLRPrediction.IVData = sw.ToString();
                sw.Close();
            }

            dgvObs.EndEdit();
            _dtObs = (DataTable)dgvObs.DataSource;
            if (_dtObs != null)
            {
                _dtObs.AcceptChanges();
                _dtObs.TableName = "Observations";
                sw = new StringWriter();
                _dtObs.WriteXml(sw, XmlWriteMode.WriteSchema, false);
                localMLRPrediction.ObsData = sw.ToString();
                sw.Close();
            }

            dgvStats.EndEdit();
            _dtStats = (DataTable)dgvStats.DataSource;
            if (_dtStats != null)
            {
                _dtStats.AcceptChanges();
                _dtStats.TableName = "Stats";
                sw = new StringWriter();
                _dtStats.WriteXml(sw, XmlWriteMode.WriteSchema, false);
                localMLRPrediction.StatData = sw.ToString();
                sw.Close();
            }

            localMLRPrediction.RegulatoryStandard = Convert.ToDouble(txtRegStd.Text);
            localMLRPrediction.DecisionCriteria = Convert.ToDouble(txtDecCrit.Text);

            string pwrTrans = txtPower.Text;
            if (String.IsNullOrWhiteSpace(pwrTrans))
                localMLRPrediction.PowerTransform = 1.0;

            if (rbNone.Checked)
                localMLRPrediction.DependentVariableTransform = Globals.DependentVariableTransforms.none;
            else if (rbLog10.Checked)
                localMLRPrediction.DependentVariableTransform = Globals.DependentVariableTransforms.Log10;
            else if (rbLn.Checked)
                localMLRPrediction.DependentVariableTransform = Globals.DependentVariableTransforms.Ln;
            else if (rbPower.Checked)
                localMLRPrediction.DependentVariableTransform = Globals.DependentVariableTransforms.Power;

            e.DictPacked.Add("frmMLRPrediction", localMLRPrediction);
        }

        private void frmMLRPrediction_Enter(object sender, EventArgs e)
        {
            //if we have a model selected, go.  otherwise go away
            txtModel.Text = "";

            if ((_projMgr.Model == null) || (_projMgr.Model.Count <= 0))
                return;

            if (_projMgr.ModelingInfo == null)
                return;

            _corrDT = _projMgr.CorrelationDataTable;

            //Lets get all the main effect variables
            _mainEffects = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
            //Add ID first so it shows up first in datatable.
            //_mainEffects.Add("ID", "ID");
            for (int i = 2; i < _corrDT.Columns.Count; i++ )
            {
                bool bMainEffect = Support.IsMainEffect(_corrDT.Columns[i].ColumnName, _corrDT);
                if (bMainEffect)
                {                    
                    string colName = _corrDT.Columns[i].ColumnName;
                    _mainEffects.Add(colName, colName);
                }
            }            

            _model = _projMgr.Model;

            if (_corrDT.Columns[1].ExtendedProperties.ContainsKey(VBTools.Globals.DEPENDENTVARIBLEDEFINEDTRANSFORM))
            {
                _respVarTransform = _corrDT.Columns[1].ExtendedProperties[VBTools.Globals.DEPENDENTVARIBLEDEFINEDTRANSFORM].ToString();
                if (String.Compare(_respVarTransform, Globals.DependentVariableTransforms.none.ToString(), 0) == 0)
                    rbNone.Checked = true;
                else if (String.Compare(_respVarTransform, Globals.DependentVariableTransforms.Log10.ToString(), 0) == 0)
                    rbLog10.Checked = true;
                else if (String.Compare(_respVarTransform, Globals.DependentVariableTransforms.Ln.ToString(), 0) == 0)
                    rbLn.Checked = true;
                else if (_respVarTransform.Contains(Globals.DependentVariableTransforms.Power.ToString()))
                {
                    rbPower.Checked = true;
                    double power = getTransformPower(_respVarTransform);
                    txtPower.Text = power.ToString();
                }
            }
            else
                rbNone.Checked = true;
            
            txtModel.Text = Support.BuildModelExpression(_model, _projMgr.ModelingInfo.DependentVariable, "");
            _modelExpression = Support.BuildModelExpression(_model, null, "");

            //Lets get only the main effects in the model
            string[] refvars = ExpressionEvaluator.GetReferencedVariables(_modelExpression, _mainEffects.Keys.ToArray());
            List<string> refVarList = new List<string>();
            refVarList.Add("ID");
            refVarList.AddRange(refvars);
            _referencedVariables = refVarList.ToArray();


            if (!_projectOpened)
            {
                if (newModelSelected(_model))
                {
                    dgvVariables.DataSource = CreateEmptyIVsDataTable();
                    dgvObs.DataSource = CreateEmptyObservationsDataTable();
                }
                //_projectOpened = false;
            }
            _projectOpened = false;
            _currentModel = _model;
        }

        private bool newModelSelected(Dictionary<string, double> modeldic)
        {
 
            if (_currentModel == null)
            {
                _currentModel = modeldic;
                return true;
            }

            if (_projectOpened)
            {
                _projectOpened = false;
                return true;
            }

            if (_currentModel.Keys.Count == modeldic.Keys.Count)
            {
                foreach (KeyValuePair<string, double> kv in modeldic)
                {
                    if (!_currentModel.ContainsKey(kv.Key) || kv.Value != _currentModel[kv.Key])
                    {
                        _currentModel = modeldic;
                        return true;
                    }

                }
                return false;
            }
            else
            {
                _currentModel = modeldic;
                return true; ;
            }

        }

        private void dgvVariables_Scroll(object sender, ScrollEventArgs e)
        {
            int first = dgvVariables.FirstDisplayedScrollingRowIndex;

            if (dgvObs.Rows.Count > 0)
            {                
                if (first >= dgvObs.Rows.Count)
                    dgvObs.FirstDisplayedScrollingRowIndex = dgvObs.Rows.Count - 1;
                else
                    dgvObs.FirstDisplayedScrollingRowIndex = dgvVariables.FirstDisplayedScrollingRowIndex;
            }
            if (dgvStats.Rows.Count > 0)
            {
                if (first >= dgvStats.Rows.Count)
                    dgvStats.FirstDisplayedScrollingRowIndex = dgvStats.Rows.Count - 1;
                else
                    dgvStats.FirstDisplayedScrollingRowIndex = dgvVariables.FirstDisplayedScrollingRowIndex;
            }
        }

        private void dgvObs_Scroll(object sender, ScrollEventArgs e)
        {
            int first = dgvObs.FirstDisplayedScrollingRowIndex;

            if (dgvVariables.Rows.Count > 0)
            {
                if (first >= dgvVariables.Rows.Count)
                    dgvVariables.FirstDisplayedScrollingRowIndex = dgvVariables.Rows.Count - 1;
                else
                    dgvVariables.FirstDisplayedScrollingRowIndex = dgvObs.FirstDisplayedScrollingRowIndex;            
            }

            if (dgvStats.Rows.Count > 0)
            {
                if (first >= dgvStats.Rows.Count)
                    dgvStats.FirstDisplayedScrollingRowIndex = dgvStats.Rows.Count - 1;
                else
                    dgvStats.FirstDisplayedScrollingRowIndex = dgvObs.FirstDisplayedScrollingRowIndex;
            }
        }

        private void dgvStats_Scroll(object sender, ScrollEventArgs e)
        {
            int first = dgvStats.FirstDisplayedScrollingRowIndex;

            if (dgvVariables.Rows.Count > 0)
            {
                if (first >= dgvVariables.Rows.Count)
                    dgvVariables.FirstDisplayedScrollingRowIndex = dgvVariables.Rows.Count - 1;
                else
                    dgvVariables.FirstDisplayedScrollingRowIndex = dgvStats.FirstDisplayedScrollingRowIndex;
            }

            if (dgvObs.Rows.Count > 0)
            {
                if (first >= dgvObs.Rows.Count)
                    dgvObs.FirstDisplayedScrollingRowIndex = dgvObs.Rows.Count - 1;
                else
                    dgvObs.FirstDisplayedScrollingRowIndex = dgvStats.FirstDisplayedScrollingRowIndex;
            }
        }

        //private void btnChange_Click(object sender, EventArgs e)
        //{
        //    if (btnChange.Text == "Change")
        //    {
        //        txtDecCrit.ReadOnly = false;
        //        txtRegStd.ReadOnly = false;
        //        btnChange.Text = "Save";
        //    }
        //    else if (btnChange.Text == "Save")
        //    {
        //        txtDecCrit.ReadOnly = true;
        //        txtRegStd.ReadOnly = true;
        //        btnChange.Text = "Change";
        //    }
        //}

        public void btnImportIVs_Click(object sender, EventArgs e)
        {
            VBTools.ImportExport import = new ImportExport();
            DataTable dt = import.Input;            
            if (dt == null)
                return;

            string[] headerCaptions = { "Model Variables", "Imported Variables" };

           
            Dictionary<string, string> fields = new Dictionary<string, string>(_mainEffects);
            //if (_mainEffects.ContainsKey("ID") == false)                          
            //    fields.Add("ID", "ID");
            
            //frmColumnMapper colMapper =  new frmColumnMapper(_mainEffects, dt, headerCaptions, true);
            frmColumnMapper colMapper = new frmColumnMapper(_referencedVariables, dt, headerCaptions, true);
                
            DialogResult dr = colMapper.ShowDialog();

            if (dr == DialogResult.OK)
            {
                dt = colMapper.MappedTable;

                int errndx = 0;
                if (!recordIndexUnique(dt, out errndx))
                {
                    MessageBox.Show("Unable to import datasets with non-unique record identifiers.\n" +
                                    "Fix your datatable by assuring unique record identifier values\n" +
                                    "in the ID column and try importing again.\n\n" +
                                    "Record Identifier values cannot be blank or duplicated;\nencountered " +
                                    "error near row " + errndx.ToString(), "Import Data Error - Cannot Import This Dataset", MessageBoxButtons.OK);
                    return;
                }

                dgvVariables.DataSource = dt;
            }
            else
                return;

            
            //DataColumn dc = new DataColumn("IVRowIdx", typeof(int));
            //dt.Columns.Add(dc);
            //dc.SetOrdinal(0);
            foreach (DataGridViewColumn dvgCol in dgvVariables.Columns)
            {
                dvgCol.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    dt.Rows[i]["IVRowIdx"] = i;
            //}
            //dgvVariables.Columns[0].Visible = false;

            setViewOnGrid(dgvVariables);            
            btnMakePredictions.Enabled = false;
        }

        /// <summary>
        /// test all cells in the daetime column for uniqueness
        /// could do this with linq but then how does one find where?
        /// Code copied from Mike's VBDatasheet.frmDatasheet.
        /// </summary>
        /// <param name="dt">table to search</param>
        /// <param name="where">record number of offending timestamp</param>
        /// <returns>true iff all unique, false otherwise</returns>
        private bool recordIndexUnique(DataTable dt, out int where)
        {
            Dictionary<string, int> temp = new Dictionary<string, int>();
            int ndx = -1;
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    string tempval = dr["ID"].ToString();
                    temp.Add(dr["ID"].ToString(), ++ndx);
                    if (string.IsNullOrWhiteSpace(dr["ID"].ToString()))
                    {
                        where = ndx++;
                        //MessageBox.Show("Record Identifier values cannot be blank - encountered blank in row " + ndx++.ToString() + ".\n",
                        //    "Import data error", MessageBoxButtons.OK);
                        return false;
                    }
                }
            }
            catch (ArgumentException)
            {
                where = ndx++;
                //MessageBox.Show("Record Identifier values cannot be duplicated - encountered existing record in row " + ndx++.ToString() + ".\n",
                //    "Import data error", MessageBoxButtons.OK);
                return false;
            }
            where = ndx;
            return true;
        }

        public void btnImportObs_Click(object sender, EventArgs e)
        {
            VBTools.ImportExport import = new ImportExport();
            DataTable dt = import.Input;
            if (dt == null)
                return;

            string[] headerCaptions = { "Obs IDs", "Obs" };
            //Dictionary<string, string> obsColumns = new Dictionary<string, string>();
           // obsColumns.Add("ID", "ID");
            //obsColumns.Add("Observation", "Observation");

            string[] obsColumns = { "ID", "Observation" };

            frmColumnMapper colMapper = new frmColumnMapper(obsColumns, dt, headerCaptions, true);
            DialogResult dr = colMapper.ShowDialog();

            if (dr == DialogResult.OK)
            {
                dt = colMapper.MappedTable;
                dgvObs.DataSource = dt;
            }
            else
                return;

            foreach (DataGridViewColumn dvgCol in dgvObs.Columns)
                dvgCol.SortMode = DataGridViewColumnSortMode.NotSortable;

            setViewOnGrid(dgvObs);
                        
        }

        public void btnMakePredictions_Click(object sender, EventArgs e)
        {
            string[] vars = _model.Keys.ToArray();
            
            _dtVariables = (DataTable)dgvVariables.DataSource;
            if (_dtVariables == null)
                return;

            if (_dtVariables.Rows.Count < 1)
                return;

            dgvVariables.EndEdit();
           
            _dtVariables.AcceptChanges();

            dgvObs.EndEdit();            
            _dtObs = (DataTable)dgvObs.DataSource;
            if (_dtObs != null)
                _dtObs.AcceptChanges();

            ExpressionEvaluator expEval = new ExpressionEvaluator();
            DataTable dtPredictions = expEval.Evaluate(_modelExpression, _dtVariables);
            _dtStats = GeneratePredStats(dtPredictions, _dtObs);
             if (_dtStats == null)
                 return;

            dgvStats.DataSource = _dtStats;

            foreach (DataGridViewColumn dvgCol in dgvStats.Columns)
                dvgCol.SortMode = DataGridViewColumnSortMode.NotSortable;

            setViewOnGrid(dgvStats);           
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


        private DataTable GeneratePredStats(DataTable dtPredictions, DataTable dtObs)
        {
            Globals.DependentVariableTransforms dvt =  GetTransformType();
            if (dvt == Globals.DependentVariableTransforms.Power)
            {
                if (ValidateNumericTextBox(txtPower) == false)
                    return null;
            }
            double decCrit = Convert.ToDouble(txtDecCrit.Text);
            decCrit = GetTransformedValue(decCrit);

            double regStd = Convert.ToDouble(txtRegStd.Text);
            regStd = GetTransformedValue(regStd);

            DataTable dt = new DataTable();

            dt.Columns.Add("ID", typeof(string));
            dt.Columns.Add("Model_Prediction", typeof(double));
            dt.Columns.Add("Decision_Criterion", typeof(double));
            dt.Columns.Add("Exceedance_Probability", typeof(double));
            dt.Columns.Add("Regulatory_Standard", typeof(double));
            dt.Columns.Add("Error_Type", typeof(string));
            dt.Columns.Add("Untransformed", typeof(double));

            double predValue = 0.0;
            string id = "";
            for (int i = 0; i < dtPredictions.Rows.Count; i++)
            {
                predValue = (double)dtPredictions.Rows[i]["CalcValue"];
                DataRow dr = dt.NewRow();
                id = (string)dtPredictions.Rows[i]["ID"];
                dr["ID"] = id;
                dr["Model_Prediction"] = predValue;
                dr["Decision_Criterion"] = decCrit;
                dr["Exceedance_Probability"] = Statistics.PExceed(predValue, decCrit, _projMgr.ModelingInfo.RMSE);
                dr["Regulatory_Standard"] = regStd;
               
                if (dvt ==  Globals.DependentVariableTransforms.Log10)
                    dr["Untransformed"] = Math.Pow(10, predValue);
                else if (dvt ==  Globals.DependentVariableTransforms.Ln)
                    dr["Untransformed"] = Math.Pow(Math.E, predValue);
                else if (dvt ==  Globals.DependentVariableTransforms.Power)
                {
                    double power = getTransformPower(_respVarTransform);
                    if (power == double.NaN)
                        power = 1.0;

                    dr["Untransformed"] = Math.Sign(predValue) * Math.Pow(Math.Abs(predValue), (1.0 / power));
                }
               else //No transform
                    dr["Untransformed"] = predValue;

                //determine if we have an error and its type
                //No guarentee we have same num of obs as we do predictions or that we have any obs at all
                //if ((dtObs != null ) && (i <= dtObs.Rows.Count - 1))
                if ((dtObs != null) && (dtObs.Rows.Count > 0))
                {
                    string errType = "";
                    DataRow[] rows = dtObs.Select("ID = '" + id + "'");

                    if ((rows != null) && (rows.Length > 0))
                    {
                        //double obs = (double)dtObs.Rows[i]["Observation"];
                        double obs = (double)rows[0][1];
                        if ((predValue > decCrit) && (obs < regStd))
                            errType = "False Positive";
                        else if ((obs > regStd) && (predValue < decCrit))
                            errType = "False Negative";
                    }
                    dr["Error_Type"] = errType;
                }
                dt.Rows.Add(dr);
            }
            return dt;            
        }


        private double getTransformPower(string pwrTransform)
        {
            if (String.IsNullOrWhiteSpace(pwrTransform))
                return double.NaN;

            char[] delim = ",".ToCharArray();
            string[] svals = pwrTransform.Split(delim);

            double power = 1.0;
            if (svals.Length != 2)
                 return double.NaN;

            if (!Double.TryParse(svals[1], out power))
                return double.NaN;

            return power;
        }


        private VBTools.Globals.DependentVariableTransforms GetTransformType()
        {
            Globals.DependentVariableTransforms dvt = Globals.DependentVariableTransforms.none;

            if (String.Compare(_respVarTransform, Globals.DependentVariableTransforms.Log10.ToString(), 0) == 0)
                dvt = Globals.DependentVariableTransforms.Log10;
            else if (String.Compare(_respVarTransform, Globals.DependentVariableTransforms.Ln.ToString(), 0) == 0)
                dvt = Globals.DependentVariableTransforms.Ln;
            else if (_respVarTransform.Contains(Globals.DependentVariableTransforms.Power.ToString()))
                dvt = Globals.DependentVariableTransforms.Power;

            return dvt;
        }


        //Keeps the Import Obs button lined up with the Obs grid
        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            //btnImportObs.Left =  e.X + 10;
        }


        private void splitContainer2_SplitterMoved(object sender, SplitterEventArgs e)
        {
            int left1 = splitContainer1.Panel2.Left;
            int left2 = splitContainer2.Panel2.Left;
            //btnPlot.Left = left1 + left2 + 10;
        }

        //keeps the Import Obs button level with the Import IVs button
        private void btnImportIVs_Move(object sender, EventArgs e)
        {
           // btnImportObs.Top = btnImportIVs.Top;
           // btnPlot.Top = btnImportIVs.Top;
        }

        private void dgvVariables_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            //int rowIdx = e.RowIndex;
            //if (rowIdx < dgvObs.Rows.Count)
            //    dgvObs.Rows[rowIdx].Selected = true;

            //if (rowIdx < dgvStats.Rows.Count)
            //    dgvStats.Rows[rowIdx].Selected = true;
            
        }

        private void dgvObs_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            //int rowIdx = e.RowIndex;
            //if (rowIdx < dgvVariables.Rows.Count)
            //    dgvVariables.Rows[rowIdx].Selected = true;

            //if (rowIdx < dgvStats.Rows.Count)
            //    dgvStats.Rows[rowIdx].Selected = true;
        }

        private void dgvStats_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            //int rowIdx = e.RowIndex;
            //if (rowIdx < dgvVariables.Rows.Count)
            //    dgvVariables.Rows[rowIdx].Selected = true;

            //if (rowIdx < dgvObs.Rows.Count)
            //    dgvObs.Rows[rowIdx].Selected = true;
        }


        private void frmMLRPrediction_Load(object sender, EventArgs e)
        {
            cmforResponseVar.MenuItems.Add("Transform");
            cmforResponseVar.MenuItems[0].MenuItems.Add("Log10", new EventHandler(log10T));
            cmforResponseVar.MenuItems[0].MenuItems.Add("Ln", new EventHandler(lnT));
            cmforResponseVar.MenuItems[0].MenuItems.Add("Power", new EventHandler(powerT));
            cmforResponseVar.MenuItems.Add("Untransform", new EventHandler(untransform));
        }


        private void untransform(object o, EventArgs e)
        {
            if (_dtObsOrig != null)
            {
                DataColumn dc = _dtObsOrig.Columns[1];
                dc.ColumnName = "Observation";
                dgvObs.DataSource = _dtObsOrig;
                _obsTransformed = false;
            }
        }


        /// <summary>
        /// response variable transform log10
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void log10T(object o, EventArgs e)
        {

            dgvObs.EndEdit();
            DataTable _dt = (DataTable)dgvObs.DataSource;
            DataTable _dtCopy = _dt.Copy();
            if (_dt == null) return;
            _dtObsOrig = _dt;
            //can only transform the response variable and the response variable can not be a transformed ME or and interacted ME
            Transform t = new Transform(_dt, 1);
            double[] newvals = new double[_dt.Rows.Count];
            newvals = t.LOG10;
            if (t.Message != "")
            {
                MessageBox.Show("Cannot Log10 transform variable. " + t.Message, "VB Transform Rule", MessageBoxButtons.OK);
                return;
            }

            _obsTransformed = true;

            for (int i=0;i<_dtCopy.Rows.Count;i++)
            {
                _dtCopy.Rows[i][1] = newvals[i];                
            }

            DataColumn dc = _dtCopy.Columns["Observation"];
            dc.ColumnName = "LOG10[Observation]";

            dgvObs.DataSource = _dtCopy;
        }

        private void lnT(object o, EventArgs e)
        {
            dgvObs.EndEdit();
            DataTable _dt = (DataTable)dgvObs.DataSource;
            DataTable _dtCopy = _dt.Copy();
            if (_dt == null) return;
            _dtObsOrig = _dt;
            Transform t = new Transform(_dt, 1);
            double[] newvals = new double[_dt.Rows.Count];
            newvals = t.LOGE;
            if (t.Message != "")
            {
                MessageBox.Show("Cannot Ln transform variable. " + t.Message, "VB Transform Rule", MessageBoxButtons.OK);
                return;
            }

            _obsTransformed = true;

            for (int i = 0; i < _dtCopy.Rows.Count; i++)
            {
                _dtCopy.Rows[i][1] = newvals[i];
            }

            DataColumn dc = _dtCopy.Columns["Observation"];
            dc.ColumnName = "LN[Observation]";

            dgvObs.DataSource = _dtCopy;
        }

        private void powerT(object o, EventArgs e)
        {
            dgvObs.EndEdit();
            DataTable _dt = (DataTable)dgvObs.DataSource;
            DataTable _dtCopy = _dt.Copy();
            if (_dt == null) return;
            _dtObsOrig = _dt;
            
            frmPowerExponent frmExp = new frmPowerExponent(_dt, 1);
            DialogResult dlgr = frmExp.ShowDialog();
            if (dlgr != DialogResult.Cancel)
            {
                double[] newvals = new double[_dt.Rows.Count];
                newvals = frmExp.TransformedValues;
                if (frmExp.TransformMessage != "")
                {
                    MessageBox.Show("Cannot Power transform variable. " + frmExp.TransformMessage, "VB Transform Rule", MessageBoxButtons.OK);
                    return;
                }

                _obsTransformed = true;
                string sexp = frmExp.Exponent.ToString("n2");
                for (int i = 0; i < _dtCopy.Rows.Count; i++)
                {
                    _dtCopy.Rows[i][1] = newvals[i];
                }

                DataColumn dc = _dtCopy.Columns["Observation"];
                dc.ColumnName = "POWER[" + sexp+ ",Observation]";
                dgvObs.DataSource = _dtCopy;
            }

        }

 

        private void dgvVariables_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            //int rowIdx = e.RowIndex;
            //if (rowIdx < dgvObs.Rows.Count)
            //    dgvObs.Rows[rowIdx].Selected = false;

            //if (rowIdx < dgvStats.Rows.Count)
            //    dgvStats.Rows[rowIdx].Selected = false;
        }

        private void dgvObs_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            //int rowIdx = e.RowIndex;
            //if (rowIdx < dgvVariables.Rows.Count)
            //    dgvVariables.Rows[rowIdx].Selected = false;

            //if (rowIdx < dgvStats.Rows.Count)
            //    dgvStats.Rows[rowIdx].Selected = false;
        }

        private void dgvStats_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            //int rowIdx = e.RowIndex;
            //if (rowIdx < dgvVariables.Rows.Count)
            //    dgvVariables.Rows[rowIdx].Selected = false;

            //if (rowIdx < dgvObs.Rows.Count)
            //    dgvObs.Rows[rowIdx].Selected = false;
        }

        public void btnExportTable_Click(object sender, EventArgs e)
        {
            dgvVariables.EndEdit();
            _dtVariables = (DataTable)dgvVariables.DataSource;
            if (_dtVariables != null)
                _dtVariables.AcceptChanges();
            else
                return;

            dgvObs.EndEdit();
            _dtObs = (DataTable)dgvObs.DataSource;
            if (_dtObs != null)
                _dtObs.AcceptChanges();

            dgvStats.EndEdit();
            _dtStats = (DataTable)dgvStats.DataSource;
            if (_dtStats != null)
                _dtStats.AcceptChanges();

            if ((_dtVariables == null) && (_dtObs == null) && (_dtStats == null))
                return;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Export Prediction Data";
            sfd.Filter = @"CSV Files|*.csv|All Files|*.*";

            DialogResult dr = sfd.ShowDialog();
            if (dr == DialogResult.Cancel)
                return;

            int maxRowsVars = _dtVariables.Rows.Count;
            int maxRowsObs = 0;
            int maxRowsStats = 0;
            
            if (_dtObs != null)
                maxRowsObs = _dtObs.Rows.Count;
            if (_dtStats != null)
                maxRowsStats = _dtStats.Rows.Count;

            int maxRows = Math.Max(maxRowsVars, Math.Max(maxRowsObs, maxRowsStats));

            StringBuilder sb = new StringBuilder();

            //Write out the column headers
            if (_dtVariables != null)
            {
                for (int i = 0; i < _dtVariables.Columns.Count; i++)
                {
                    if (i > 0)
                        sb.Append(",");

                    sb.Append(_dtVariables.Columns[i].ColumnName);
                }
            }

            if (_dtObs != null)
            {
                for (int i = 0; i < _dtObs.Columns.Count; i++)
                {
                    sb.Append(",");
                    sb.Append(_dtObs.Columns[i].ColumnName);
                }
            }

            if (_dtStats != null)
            {
                for (int i = 0; i < _dtStats.Columns.Count; i++)
                {
                    sb.Append(",");
                    sb.Append(_dtStats.Columns[i].ColumnName);
                }
            } //Finished writing out column headers
            

            sb.Append(Environment.NewLine);

            //write out the data
            for (int i = 0; i < maxRows; i++)
            {
                for (int j = 0; j < _dtVariables.Columns.Count; j++)
                {
                    if (j > 0)
                        sb.Append(",");

                    if (i < _dtVariables.Rows.Count)
                        sb.Append(_dtVariables.Rows[i][j].ToString());
                    else
                        sb.Append("");
                }
               

                if (_dtObs != null)
                {
                    for (int j = 0; j < _dtObs.Columns.Count; j++)
                    {                        
                        sb.Append(",");

                        if (i < _dtObs.Rows.Count)
                            sb.Append(_dtObs.Rows[i][j].ToString());
                        else
                            sb.Append("");
                    }    
                }

                if (_dtStats != null)
                {
                    for (int j = 0; j < _dtStats.Columns.Count; j++)
                    {
                        sb.Append(",");

                        if (i < _dtStats.Rows.Count)
                            sb.Append(_dtStats.Rows[i][j].ToString());
                        else
                            sb.Append("");
                    }
                }

                sb.Append(Environment.NewLine);
            } //End writing out data
            

            string fileName = sfd.FileName;

            StreamWriter sw = new StreamWriter(fileName);
            sw.Write(sb.ToString());
            sw.Close();

            
        }

        private StringBuilder AddCommanSerparatedColumns(DataTable dt, StringBuilder sb)
        {
            if ((dt == null) || (dt.Columns.Count < 1))
                return sb;

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (i > 0)
                    sb.Append(",");

                sb.Append(dt.Columns[i].ColumnName);
            }

            return sb;
        }

        private StringBuilder AddRow(DataTable dt, StringBuilder sb)
        {
            if ((dt == null) || (dt.Columns.Count < 1))
                return sb;

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (i > 0)
                    sb.Append(",");

                sb.Append(dt.Columns[i].ColumnName);
            }

            return sb;
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

        public void btnClearTable_Click(object sender, EventArgs e)
        {
            dgvVariables.DataSource = null;
            dgvObs.DataSource = null;
            dgvStats.DataSource = null;

            if (_dtVariables != null)
                _dtVariables.Clear();            
            _dtVariables = null;

            if (_dtObs != null)
                _dtObs.Clear();            
            _dtObs = null;

            if (_dtStats != null)
                _dtStats.Clear();            
            _dtStats = null;

            dgvVariables.DataSource = CreateEmptyIVsDataTable();
            dgvObs.DataSource = CreateEmptyObservationsDataTable();
        }

        public void btnPlot_Click(object sender, EventArgs e)
        {
            //frmMLRPredPlot frmPlot = new frmMLRPredPlot(_dtObs, _dtStats);
            dgvObs.EndEdit();
            dgvStats.EndEdit();
            
            DataTable dtObs = dgvObs.DataSource as DataTable;
            DataTable dtStats = dgvStats.DataSource as DataTable;

            if ((dtObs == null) || (dtObs.Rows.Count < 1))
            {
                MessageBox.Show("Plotting requires Observation data.");
                return;
            }

            if ((dtStats == null) || (dtStats.Rows.Count < 1))
            {
                MessageBox.Show("Plotting requires Prediction data.");
                return;
            }

            frmMLRPredObs frmPlot = new frmMLRPredObs(dtObs, dtStats);
            frmPlot.Show();
        }

        private DataTable CreateEmptyIVsDataTable()
        {
            //We are going to put an ID column first.
            //ID is used to link IV and Obs records.

            DataTable dt = new DataTable("Variables");

            dt.Columns.Add("ID", typeof(string));

            for (int i = 1; i < _referencedVariables.Length;i++)
                dt.Columns.Add(_referencedVariables[i], typeof(double));
                       
            return dt;            
        }

        private DataTable CreateEmptyObservationsDataTable()
        {
            DataTable dt = new DataTable("Observations");
            dt.Columns.Add("ID", typeof(string));
            dt.Columns.Add("Observation", typeof(double));

            return dt;
        }

        private void dgvVariables_SelectionChanged(object sender, EventArgs e)
        {

            dgvObs.SelectionChanged -= new EventHandler(dgvObs_SelectionChanged);
            dgvStats.SelectionChanged -= new EventHandler(dgvStats_SelectionChanged);

            DataGridViewSelectedRowCollection selRowCol = dgvVariables.SelectedRows;
            if (dgvObs.DataSource != null)
                dgvObs.ClearSelection();

            if (dgvStats.DataSource != null)
                dgvStats.ClearSelection();

            

            for (int i=0;i<selRowCol.Count;i++)
            {
                if (dgvObs.DataSource != null)
                {
                    if (selRowCol[i].Index < dgvObs.Rows.Count)
                        dgvObs.Rows[selRowCol[i].Index].Selected = true;
                }

                if (dgvStats.DataSource != null)
                {
                    if (selRowCol[i].Index < dgvStats.Rows.Count)
                        dgvStats.Rows[selRowCol[i].Index].Selected = true;
                }
            }

            dgvObs.SelectionChanged += new EventHandler(dgvObs_SelectionChanged);
            dgvStats.SelectionChanged += new EventHandler(dgvStats_SelectionChanged);
        }

        private void dgvObs_SelectionChanged(object sender, EventArgs e)
        {
            dgvVariables.SelectionChanged -= new EventHandler(dgvVariables_SelectionChanged);
            dgvStats.SelectionChanged -= new EventHandler(dgvStats_SelectionChanged);

            DataGridViewSelectedRowCollection selRowCol = dgvObs.SelectedRows;

           
            if (dgvVariables.DataSource != null)
                dgvVariables.ClearSelection();

            if (dgvStats.DataSource != null)
                dgvStats.ClearSelection();

            for (int i = 0; i < selRowCol.Count; i++)
            {
                if (dgvVariables.DataSource != null)
                {
                    if (selRowCol[i].Index < dgvVariables.Rows.Count)
                        dgvVariables.Rows[selRowCol[i].Index].Selected = true;
                }

                if (dgvStats.DataSource != null)
                {
                    if (selRowCol[i].Index < dgvStats.Rows.Count)
                        dgvStats.Rows[selRowCol[i].Index].Selected = true;
                }
            }

            dgvVariables.SelectionChanged += new EventHandler(dgvVariables_SelectionChanged);
            dgvStats.SelectionChanged += new EventHandler(dgvStats_SelectionChanged);
        }

        private void dgvStats_SelectionChanged(object sender, EventArgs e)
        {
            dgvVariables.SelectionChanged -= new EventHandler(dgvVariables_SelectionChanged);
            dgvObs.SelectionChanged -= new EventHandler(dgvObs_SelectionChanged);

            DataGridViewSelectedRowCollection selRowCol = dgvStats.SelectedRows;
            if (dgvVariables.DataSource != null)
                dgvVariables.ClearSelection();

            if (dgvObs.DataSource != null)
                dgvObs.ClearSelection();

            for (int i = 0; i < selRowCol.Count; i++)
            {
                if (dgvVariables.DataSource != null)
                {
                    if (selRowCol[i].Index < dgvVariables.Rows.Count)
                        dgvVariables.Rows[selRowCol[i].Index].Selected = true;
                }

                if (dgvObs.DataSource != null)
                {
                    if (selRowCol[i].Index < dgvObs.Rows.Count)
                        dgvObs.Rows[selRowCol[i].Index].Selected = true;
                }
            }

            dgvVariables.SelectionChanged += new EventHandler(dgvVariables_SelectionChanged);
            dgvObs.SelectionChanged += new EventHandler(dgvObs_SelectionChanged);
        }


        public void setViewOnGrid(DataGridView dgv)
        {
            //utility method used to set numerical precision displayed in grid

            //seems to be the only way I can figure to get a string in col 1 that may
            //(or may not) be a date and numbers in all other columns.
            //in design mode set "no format" for the dgv defaultcellstyle
            if (dgv.Rows.Count <= 1) return;

            string testcellval = string.Empty;
            //double numval = double.NaN;
            for (int col = 0; col < dgv.Columns.Count; col++)
            {
                //skips col 0 (date/time/string/whatever)
                testcellval = dgv[col, 0].Value.ToString();
                //double.TryParse(testcellval, out numval); //dumb: "0", a number, returns 0 as well as nonsense does
                double result;
                bool isNum = Double.TryParse(testcellval, out result); //try a little visualbasic magic

                //if (numval != 0)
                if (isNum)
                {
                    dgv.Columns[col].ValueType = typeof(System.Double);
                    dgv.Columns[col].DefaultCellStyle.Format = "g4";
                    //shit! only works if no data in table
                    // _rawdataDT.Columns[col].DataType = typeof(System.Double);
                }
                else
                {
                    dgv.Columns[col].ValueType = typeof(System.String);
                    // _rawdataDT.Columns[col].DataType = typeof(System.String);
                    //dgv.Columns[col].DefaultCellStyle.Format = "none";
                }

            }
        }

        private List<string> getBadCells(DataTable dt, bool skipFirstColumn)
        {
            double result;
            if (dt == null)
                return null;

            List<string> cells = new List<string>();
            foreach (DataColumn dc in dt.Columns)
            {
                if (skipFirstColumn)
                {
                    if (dt.Columns.IndexOf(dc) == 0)
                        continue;
                }
                foreach (DataRow dr in dt.Rows)
                {
                    if (string.IsNullOrEmpty(dr[dc].ToString()))
                        cells.Add("Row " + dr[0].ToString() + " Column " + dc.Caption + " has blank cell.");
                    else if (!Double.TryParse(dr[dc].ToString(), out result) && dt.Columns.IndexOf(dc) != 0)
                        cells.Add("Row " + dr[0].ToString() + " Column " + dc.Caption + " has non-numeric cell value: '" + dr[dc].ToString() + "'");
                }
            }


            return cells;
        }

        private void dgvVariables_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            string err = "Data value must be numeric.";
            dgvVariables.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
            MessageBox.Show(err);
        }

        private void dgvObs_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            string err = "Data value must be numeric.";
            dgvObs.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
            MessageBox.Show(err);
        }

        private void rbPower_CheckedChanged(object sender, EventArgs e)
        {
            if (rbPower.Checked)
                txtPower.Enabled = true;
            else
                txtPower.Enabled = false;
        }



        private bool ValidateNumericTextBox(TextBox txtBox)
        {
            double val = 1.0;
            if (!Double.TryParse(txtBox.Text, out val))
            {
                MessageBox.Show(txtPower.Text + "Invalid number.");
                txtPower.Focus();
                return false;
            }

            return true;

            


        }

        private void txtDecCrit_Leave(object sender, EventArgs e)
        {
            double result;
            if (!Double.TryParse(txtDecCrit.Text, out result))
            {
                MessageBox.Show("Invalid number.");
                txtDecCrit.Focus();
            }

        }

        private void txtRegStd_Leave(object sender, EventArgs e)
        {
            double result;
            if (!Double.TryParse(txtRegStd.Text, out result))
            {
                MessageBox.Show("Invalid number.");
                txtRegStd.Focus();
            }
        }

        private double GetTransformedValue(double value)
        {
            double retValue = 0.0;
           
           if (rbLog10.Checked)
                retValue = Math.Log10(value);
            else if (rbLn.Checked)
                retValue = Math.Log(value);
            else if (rbPower.Checked)
            {
                double power = Convert.ToDouble(txtPower.Text);
                retValue = Math.Pow(value, power);
            }
           else
               retValue = value;

            return retValue;

        }

        public void btnIVDataValidation_Click(object sender, EventArgs e)
        {
            DataTable dt = dgvVariables.DataSource as DataTable;
            if ((dt == null) ||(dt.Rows.Count < 1))
                return;

            DataTable dtCopy = dt.Copy();
            DataTable dtSaved = dt.Copy();
            frmMissingValues frmMissVal = new frmMissingValues(dgvVariables, dtCopy);
            frmMissVal.ShowDialog();
            if (frmMissVal.Status)
            {
                int errndx;
                if (!recordIndexUnique(frmMissVal.ValidatedDT, out errndx))
                {
                    MessageBox.Show("Unable to process datasets with non-unique record identifiers.\n" +
                                    "Fix your datatable by assuring unique record identifier values\n" +
                                    "in the ID column and try validating again.\n\n" +
                                    "Record Identifier values cannot be blank or duplicated;\nencountered " +
                                    "error near row " + errndx.ToString(), "Data Validation Error - Cannot Process This Dataset", MessageBoxButtons.OK);
                    return;
                }
                dgvVariables.DataSource = frmMissVal.ValidatedDT;
                btnMakePredictions.Enabled = true;
            }
            else
            {
                dgvVariables.DataSource = dtSaved;
            }

        }

        private void dgvObs_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            showContextMenus((DataGridView)sender, e);
        }

        private void showContextMenus(DataGridView dgv, MouseEventArgs me)
        {
            DataGridView.HitTestInfo ht = dgv.HitTest(me.X, me.Y);
            int colndx = ht.ColumnIndex;
            int rowndx = ht.RowIndex;

            DataTable _dt = (DataTable)dgvObs.DataSource;
            if (rowndx > 0 && colndx > 0) return; //cell hit, go away

            if (rowndx < 0 && colndx >= 0)
            {
                if (colndx == 1)
                {
                    if (!_obsTransformed)
                    {
                        cmforResponseVar.MenuItems[0].Enabled = true; //we can transform a response variable
                        cmforResponseVar.MenuItems[1].Enabled = false; //but we cannot untransform an untransformed variable
                    }
                    else
                    {
                        cmforResponseVar.MenuItems[0].Enabled = false; //but we cannot transform a transformed response
                        cmforResponseVar.MenuItems[1].Enabled = true; //but we can untransform a transformed response
                    }
                    cmforResponseVar.Show(dgv, new Point(me.X, me.Y));

                }
            }

        }



        private void dgvVariables_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //If user has edited the ID column, make sure the IDs are still unique.
            //if (String.Compare(dgvVariables.Columns[e.ColumnIndex].Name, "ID", true) == 0)
            //{
            //    string id = dgvVariables.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();



            //}
           
            btnMakePredictions.Enabled = false;
        }

        private void frmMLRPrediction_HelpRequested(object sender, HelpEventArgs hlpevent)
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

        private void dgvVariables_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            StringFormat sf = new StringFormat();
            int count = dgvVariables.RowCount;
            sf.Alignment = StringAlignment.Center;
            if(( e.ColumnIndex < 0) && (e.RowIndex >= 0) && (e.RowIndex < count) )
            {
                e.PaintBackground(e.ClipBounds, true);
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), this.Font, Brushes.Black, e.CellBounds, sf);
                e.Handled = true;
            }
        }



    }
}
