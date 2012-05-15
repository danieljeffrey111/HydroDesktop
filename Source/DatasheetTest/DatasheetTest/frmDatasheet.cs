using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DotSpatial.Controls;
using HydroDesktop.Interfaces;
using WeifenLuo.WinFormsUI.Docking;
using VBTools;
using Microsoft.VisualBasic;
using Newtonsoft.Json;

namespace VBDatasheet
{

    public partial class frmDatasheet : UserControl
    {
        private ContextMenu cmforResponseVar = new ContextMenu();
        private ContextMenu cmforIVs = new ContextMenu();
        private ContextMenu cmforRows = new ContextMenu();

        
        private int _selectedColIndex = -1;
        private int _selectedRowIndex = -1;
        private int _responseVarColIndex = 1;
        private string _responseVarColName = string.Empty;
        private string _selectedColName = string.Empty;

        private enum AddReplace { Add, Replace };
        private AddReplace _addreplace;

        private static DataTable _dt = null;
        private dtRowInformation _dtRI = null;
        private dtColumnInformation _dtCI = null;

        private Utilities _utils = null;
        private Utilities.TableUtils _tableutils = null;
        private Utilities.GridUtils _gridutils = null;

        private VBProjectManager _projMgr = null;
        public event EventHandler DataImported;
       
        private enum _listvals {NCOLS, NROWS, DATECOLNAME, RVCOLNAME, BLANK, NDISABLEDROWS, NDISABLEDCOLS, NHIDDENCOLS, NIVS};
        private Type _ListVal = typeof(_listvals);
        private int _ndisabledcols = 0;
        private int _ndisabledrows = 0;
        private int _nhiddencols = 0;
        private int _nivs = 0;

        //data state relative to data used in modeling/residuals/prediction
        //state is dirty until the project manager's version of the datatable
        //matches the filtered "gotomodeling" datatable version
        public enum _dtState { clean, dirty };
        private _dtState _state = _dtState.dirty;

        private DataTable _savedDT = null;
        private DataGridView _savedDGV = null;

        private bool _initialPass = true;
        private bool _projectOpened = false;
        private bool _validated = false;
        public event EventHandler ResetProject;
        

        public DataTable DT
        {
            get { return _dt; }
            set { _dt = value; }
        }

        public frmDatasheet()
        {
            InitializeComponent();
            _projMgr = VBProjectManager.GetProjectManager();
            //_projMgr.UnpackRequest += new VBProjectManager.EventHandler<UnpackEventArgs>(UnpackState);
            //_projMgr.ProjectSaved += new VBProjectManager.ProjectSavedHandler(packState);
            _projMgr.ProjectOpened += new VBProjectManager.ProjectOpenedHandler(ProjectOpenedListener);
            _projMgr.ProjectSaved += new VBProjectManager.ProjectSavedHandler<PackEventArgs>(ProjectSavedListener);
                      

        }


        private void ProjectOpenedListener()
        {
            //Something must be seriously messed up if this happens.
            if (_projMgr == null)
                return;

            //Console.WriteLine("\n*** Datasheet: project opened.***\n");
            if (_projMgr._projectType == VBTools.Globals.ProjectType.MODEL) return;

            if (_projMgr.DataSheetInfo == null)
                return;
            if (_projMgr.ModelingInfo == null) _projMgr.ModelingInfo = new ModelingInfo();

            if (string.IsNullOrEmpty(_projMgr.DataSheetInfo.XmlDataSheetTable))
                return;

            _projectOpened = true;

            _dt = _projMgr.DataSheetDataTable.Copy();
            //_projMgr.CorrelationDataTable = _dt;

            dgv.DataSource = null;
            dgv.DataSource = _dt;

            _dtRI = dtRowInformation.getdtRI(_dt, true);
            _dtRI.DTRowInfo = _projMgr.DataSheetInfo.DtRowInfo;
            _dtCI = dtColumnInformation.getdtCI(_dt, true);
            _dtCI.DTColInfo = _projMgr.DataSheetInfo.DtColInfo;

            _selectedColIndex = _projMgr.DataSheetInfo.CurrentColIndex;
            _responseVarColName = _projMgr.DataSheetInfo.DepVarColName;
            _responseVarColIndex = _dt.Columns.IndexOf(_responseVarColName);

            _utils = new Utilities();
            _tableutils = new Utilities.TableUtils(_dt);
            _gridutils = new Utilities.GridUtils(dgv);

            _gridutils.maintainGrid(dgv, _dt, _selectedColIndex, _responseVarColName);


            FileInfo fi = new FileInfo(_projMgr.Name);
            string fn = fi.Name;
            showListInfo(fn);

            if (_projMgr.DataSheetInfo.Clean)
            {
                _state = _dtState.clean;
            }
            else
            {
                _state = _dtState.dirty;
            }

            //if clean, initial pass is false
            _initialPass = !_projMgr.DataSheetInfo.Clean;

            //btnValidate.Enabled = true;
            //dgv.Enabled = _projMgr.DataSheetInfo.Validated;
            //btnComputeUV.Enabled = _projMgr.DataSheetInfo.Validated;
            //btnManipulate.Enabled = _projMgr.DataSheetInfo.Validated;
            //btnTransform.Enabled = _projMgr.DataSheetInfo.Validated;
            //btnModeling.Enabled = _projMgr.DataSheetInfo.Validated;

            //_projMgr.TabStates.TabState["MLRModeling"] = true;

        }

        public void ProjectSavedListener(object sender, PackEventArgs e)
        {
            DataSheetInfo localDataSheetInfo = new DataSheetInfo();
            //Something must be seriously messed up if this happens.
            if (_projMgr == null)
                return;
            if (_dt != null)
            {
                //_projMgr.DataSheetInfo = new DataSheetInfo();
                _projMgr.DataSheetDataTable = _dt;
                localDataSheetInfo.CurrentColIndex = _selectedColIndex;
                localDataSheetInfo.DepVarColName = _responseVarColName;
                localDataSheetInfo.DtColInfo = _dtCI.DTColInfo;
                localDataSheetInfo.DtRowInfo = _dtRI.DTRowInfo;
                localDataSheetInfo.Validated = _validated;

                if (_state == _dtState.clean)
                {
                    localDataSheetInfo.Clean = true;
                    _projMgr.TabStates.TabState["MLRModeling"] = true;
                }
                else
                {
                    localDataSheetInfo.Clean = false;
                    _projMgr.TabStates.TabState["MLRModeling"] = false;
                    _projMgr.TabStates.TabState["Residuals"] = false;
                    _projMgr.TabStates.TabState["Prediction"] = false;
                }

            }
            e.DictPacked.Add("frmDatasheet", localDataSheetInfo);
            
        
        }

        /// <summary>
        /// laod the datasheet form, initialize then gridview's menu items/eventhandlers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmDatasheet_Load(object sender, EventArgs e)
        {

            //menu items for response variable column
            cmforResponseVar.MenuItems.Add("Transform");
            cmforResponseVar.MenuItems[0].MenuItems.Add("Log10", new EventHandler(log10T));
            cmforResponseVar.MenuItems[0].MenuItems.Add("Ln", new EventHandler(lnT));
            //cmforResponseVar.MenuItems[0].MenuItems.Add("10**X", new EventHandler(antilog10T));
            //cmforResponseVar.MenuItems[0].MenuItems.Add("e**X", new EventHandler(antilnT));
            cmforResponseVar.MenuItems[0].MenuItems.Add("Power", new EventHandler(powerT));
            cmforResponseVar.MenuItems.Add("View Plots", new EventHandler(seePlot));
            cmforResponseVar.MenuItems.Add("UnTransform", new EventHandler(UnTransform));
            cmforResponseVar.MenuItems.Add("Define Transform");
            cmforResponseVar.MenuItems[3].MenuItems.Add("none", new EventHandler(defineTransformForRV));
            cmforResponseVar.MenuItems[3].MenuItems.Add("Log10", new EventHandler(defineTransformForRV));
            cmforResponseVar.MenuItems[3].MenuItems.Add("Ln", new EventHandler(defineTransformForRV));
            cmforResponseVar.MenuItems[3].MenuItems.Add("Power", new EventHandler(defineTransformForRV));


            //menu items for iv columns
            cmforIVs.MenuItems.Add("Disable Column", new EventHandler(DisableCol));
            cmforIVs.MenuItems.Add("Enable Column", new EventHandler(EnableCol));
            cmforIVs.MenuItems.Add("Set Response Variable", new EventHandler(SetResponse));
            cmforIVs.MenuItems.Add("View Plots", new EventHandler(seePlot));
            cmforIVs.MenuItems.Add("Delete Column", new EventHandler(DeleteCol));
            cmforIVs.MenuItems.Add("Enable All Columns", new EventHandler(EnableAllCols));

            //menu items for rows 
            cmforRows.MenuItems.Add("Disable Row", new EventHandler(DisableRow));
            cmforRows.MenuItems.Add("Enable Row", new EventHandler(EnableRow));
            cmforRows.MenuItems.Add("Enable All Rows", new EventHandler(EnableAllRows));
        }

        /// <summary>
        /// for disabling main menu model save/saveas items
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnDataImported(EventArgs e)
        {
            EventHandler eh = DataImported;
            if (eh != null) eh(this, e);
            //VBLogger.getLogger().logEvent("", VBLogger.messageIntent.UserOnly, VBLogger.targetSStrip.StatusStrip1);
        }

        /// <summary>
        /// show the help document via F1 key event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="hlpevent"></param>
        private void frmDatasheet_HelpRequested(object sender, HelpEventArgs hlpevent)
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

        public void btnImportData_Click(object sender, EventArgs e)
        {
            
            //bring in file
            DataTable dataDT = new DataTable("Imported Data");
            ImportExport import = new ImportExport();
            if ((dataDT = import.Input) == null) return;

            string errcolname = string.Empty;
            int errndx = 0;
            if (!recordIndexUnique(dataDT, out errndx))
            {
                MessageBox.Show("Unable to import datasets with non-unique record identifiers.\n" +
                                "Fix your datatable by assuring unique record identifier values\n" +
                                "in the 1st column and try importing again.\n\n" +
                                "Record Identifier values cannot be blank or duplicated;\nencountered " +
                                "error near row " + errndx.ToString(), "Import Data Error - Cannot Import This Dataset", MessageBoxButtons.OK);
                return;
            }
            string offendingCol = string.Empty;
            if (datasetHasSpacesinColNmaes(dataDT, out offendingCol))
            {
                MessageBox.Show("Cannot import datasets with spaces in column names.\nEdit your dataset and re-import.\n" +
                    "First offending column encountered = " + offendingCol,
                    "Import Data Error - Column names have spaces.", MessageBoxButtons.OK);
                return;
            }

            _dt = dataDT.Copy();

            //tell main to disable the model save/saveas menu selections
            OnDataImported(null);

            //enable validation_click
            //init in case they've re-imported
            dgv.DataSource = null;
            dgv.DataSource = _dt;

            for (int c = 0; c < _dt.Columns.Count; c++)
            {
                _dt.Columns[c].ExtendedProperties[VBTools.Globals.MAINEFFECT] = true;
                if (c == 0)
                {
                    _dt.Columns[c].ExtendedProperties[VBTools.Globals.DATETIMESTAMP] = true;
                    dgv.Columns[c].ReadOnly = true; //cannot edit this puppy..... editable makes it breakable
                }
                if (c == 1) _dt.Columns[c].ExtendedProperties[VBTools.Globals.DEPENDENTVAR] = true;
                dgv.Columns[c].SortMode = DataGridViewColumnSortMode.NotSortable;
                dgv.Columns[c].DefaultCellStyle.ForeColor = Color.Black;
            }



            //initialize rows to all enabled for imported table
            //(builds dictionary of keys, <string>datetime and values <bool>enabled/disabled row)
            _dtRI = dtRowInformation.getdtRI(_dt, true);
            //initialize cols to all enabled for imported table
            //(builds dictionary of keys, <string>datetime and values <bool>enabled/disabled col)
            _dtCI = dtColumnInformation.getdtCI(_dt, true);

            //init the utilities
            _utils = new Utilities();
            _tableutils = new Utilities.TableUtils(_dt);
            _gridutils = new Utilities.GridUtils(dgv);

            //default col 1 as response
            _selectedColIndex = 1;
            _responseVarColIndex = 1;
            _responseVarColName = _dt.Columns[1].Caption;
            _gridutils.setResponseVarCol(dgv, _selectedColIndex, _selectedColIndex);
            _gridutils.setViewOnGrid(dgv);
            //SetResponse(null, null);

            //initial info for the list
            FileInfo fi = new FileInfo(import.getFileImportedName);
            string fn = fi.Name;
            showListInfo(fn);

            //don't allow anything 'til validation done.
            //btnValidate.Enabled = true;
            dgv.Enabled = false;
            //btnComputeUV.Enabled = false;
            //btnManipulate.Enabled = false;
            //btnTransform.Enabled = false;
            //btnModeling.Enabled = false;

            _projMgr.CorrelationDataTable = _dt;
            _initialPass = true;
            _validated = false;

            //make validation ribbon button enabled
            //_dsPlug.activateValidationClick();
           

        }

        private bool datasetHasSpacesinColNmaes(DataTable dataDT, out string name)
        {
            name = string.Empty;
            foreach (DataColumn dc in (dataDT.Columns))
            {
                name = dc.Caption;
                if (name.Contains(" ")) return true;
            }
            return false;
        }

        /// <summary>
        /// test all cells in the datetime column for uniqueness
        /// could do this with linq but then how does one find where?
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
                    string tempval = dr[0].ToString();
                    temp.Add(dr[0].ToString(), ++ndx);
                    if (string.IsNullOrWhiteSpace(dr[0].ToString()))
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

        /// <summary>
        /// populate the UI list with some file/data information
        /// </summary>
        /// <param name="fn">filename of data imported</param>
        private void showListInfo(string fn)
        {
            int ncols = _dt.Columns.Count;
            int nrows = _dt.Rows.Count;
            string dtname = _dt.Columns[0].ColumnName.ToString();
            string depvarname = _dt.Columns[1].ColumnName.ToString();
            _nivs = _dt.Columns.Count - 2;
            //double dmin = 0;
            //double avg = 0;
            //double unique = 0;

            listView1.Clear();
            listView1.View = View.Details;

            ListViewItem lvi;
            //if (listView1.Items.Count > 0)
            //{
            //    lvi = new ListViewItem("*****");
            //    lvi.SubItems.Add("");
            //    listView1.Items.Add(lvi);
            //}


            //lvi = new ListViewItem("File");
            //lvi.SubItems.Add(fn);
            //listView1.Items.Add(lvi);

            lvi = new ListViewItem("Column Count");
            lvi.SubItems.Add(ncols.ToString());
            listView1.Items.Add(lvi);


            lvi = new ListViewItem("Row Count");
            lvi.SubItems.Add(nrows.ToString());
            listView1.Items.Add(lvi);

            lvi = new ListViewItem("Date-Time Index");
            lvi.SubItems.Add(dtname);
            listView1.Items.Add(lvi);

            lvi = new ListViewItem("Response Variable");
            lvi.SubItems.Add(depvarname);
            listView1.Items.Add(lvi);

            lvi = new ListViewItem("");
            lvi.SubItems.Add("");
            listView1.Items.Add(lvi);

            lvi = new ListViewItem("Disabled Row Count");
            lvi.SubItems.Add(_ndisabledrows.ToString());
            //lvi.SubItems.Add(dmin.ToString("n2"));
            listView1.Items.Add(lvi);

            lvi = new ListViewItem("Disabled Column Count");
            //lvi.SubItems.Add(avg.ToString("n2"));
            lvi.SubItems.Add(_ndisabledcols.ToString());
            listView1.Items.Add(lvi);

            lvi = new ListViewItem("Hidden Column Count");
            //lvi.SubItems.Add(unique.ToString("n2"));
            lvi.SubItems.Add(_nhiddencols.ToString());
            listView1.Items.Add(lvi);

            lvi = new ListViewItem("Independent Variable Count");
            //lvi.SubItems.Add(unique.ToString("n2"));
            lvi.SubItems.Add(_nivs.ToString());
            listView1.Items.Add(lvi);

            ////magic numbers for widths: -1 set to max characters in subitems, -2 == autosize
            listView1.Columns.Add("File", -1, HorizontalAlignment.Right);
            listView1.Columns.Add(fn, -2, HorizontalAlignment.Left);
        }

        /// <summary>
        /// as user manipulates the dataset, track changes and update the UI listview
        /// </summary>
        /// <param name="listitem">listitem to update</param>
        /// <param name="value">value of item, number or text</param>
        private void updateListView(_listvals listitem, object value)
        {
            string name = string.Empty;
            int number;
            //Enum.GetName(_ListVal, listitem);

            switch (listitem)
            {
                case _listvals.NCOLS:
                    number = (int)value;
                    listView1.Items[0].SubItems[1].Text = number.ToString();
                    break;
                case _listvals.NROWS:
                    number = (int)value;
                    listView1.Items[1].SubItems[1].Text = number.ToString();
                    break;
                case _listvals.DATECOLNAME:
                    name = (string)value;
                    listView1.Items[2].SubItems[1].Text = name;
                    break;
                case _listvals.RVCOLNAME:
                    name = (string)value;
                    listView1.Items[3].SubItems[1].Text = name;
                    break;
                case _listvals.BLANK:
                    number = (int)value;
                    listView1.Items[4].SubItems[1].Text = "";
                    break;
                case _listvals.NDISABLEDROWS:
                    number = (int)value;
                    listView1.Items[5].SubItems[1].Text = number.ToString();
                    break;
                case _listvals.NDISABLEDCOLS:
                    number = (int)value;
                    listView1.Items[6].SubItems[1].Text = number.ToString();
                    break;
                case _listvals.NHIDDENCOLS:
                    number = (int)value;
                    listView1.Items[7].SubItems[1].Text = number.ToString();
                    break;
                case _listvals.NIVS:
                    number = (int)value;
                    listView1.Items[8].SubItems[1].Text = number.ToString();
                    break;


            }

        }

        #region control context menu views

        /// <summary>
        /// user clicked on the UI grid - decide what to do if anything
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgv_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            showContextMenus((DataGridView)sender, e);
        }

        /// <summary>
        /// user click captured - decide what menu items are appropriate and show them
        /// </summary>
        /// <param name="dgv">the datagridview</param>
        /// <param name="me">left mouse click event arg - tells which column/row/cell clicked</param>
        private void showContextMenus(DataGridView dgv, MouseEventArgs me)
        {
            DataGridView.HitTestInfo ht = dgv.HitTest(me.X, me.Y);
            int colndx = ht.ColumnIndex;
            int rowndx = ht.RowIndex;

            if (rowndx > 0 && colndx > 0) return; //cell hit, go away

            if (rowndx < 0 && colndx >= 0)
            {
                //col header hit, show proper menu
                _selectedColIndex = colndx;

                //do nothing if col 0 selected
                if (colndx >= 1)
                {
                    string colname = _dt.Columns[colndx].Caption;
                    if (colname == _responseVarColName)
                    //if (colndx == _responseVarColIndex)
                    {
                        //show context menu for response variable
                        //if (_dt.Columns[_responseVarColIndex].ExtendedProperties.ContainsKey(VBTools.Globals.DEPENDENTVAR))
                        if (_utils.testValueAttribute(_dt.Columns[_responseVarColIndex], VBTools.Globals.DEPENDENTVAR))
                        {
                            cmforResponseVar.MenuItems[0].Enabled = true; //we can transform a response variable
                        }
                        else
                        {
                            cmforResponseVar.MenuItems[0].Enabled = false; //but we cannot transform a transformed response
                        }

                        //if (_dt.Columns[_responseVarColIndex].ExtendedProperties.ContainsKey(VBTools.Globals.DEPENDENTVARIBLETRANSFORM))
                        if (_utils.testValueAttribute(_dt.Columns[_responseVarColIndex], VBTools.Globals.DEPENDENTVARIBLETRANSFORM))
                        {
                            cmforResponseVar.MenuItems[2].Enabled = true; //we can untransform the transformed response variable
                        }
                        else
                        {
                            cmforResponseVar.MenuItems[2].Enabled = false; //but cannot untransform a response variable
                        }

                        cmforResponseVar.Show(dgv, new Point(me.X, me.Y));
                    }

                    else
                    {
                        //show context menu for ivs
                        if (_dtCI.getColStatus(_dt.Columns[_selectedColIndex].ColumnName.ToString()))
                        {
                            //here if col enabled
                            cmforIVs.MenuItems[0].Enabled = true;
                            cmforIVs.MenuItems[1].Enabled = false; //cannot enable enabled col
                            cmforIVs.MenuItems[2].Enabled = true;

                            //response variable must be a ME, T(RV) or I(IV) not created by general transform
                            //if they do this then we're to remove all general operations performed,
                            if (canSetRV()) cmforIVs.MenuItems[2].Enabled = true;
                            else cmforIVs.MenuItems[2].Enabled = false;

                            if (_dt.Columns[_selectedColIndex].ExtendedProperties.ContainsKey(VBTools.Globals.MAINEFFECT))
                                cmforIVs.MenuItems[4].Enabled = false;  //cannot remove maineffect column
                            else cmforIVs.MenuItems[4].Enabled = true;
                        }
                        else
                        {
                            //here if col disabled
                            cmforIVs.MenuItems[0].Enabled = false; //cannot disable disabled col
                            cmforIVs.MenuItems[1].Enabled = true;
                            cmforIVs.MenuItems[2].Enabled = false; //cannot disabled the response variable
                        }
                        cmforIVs.Show(dgv, new Point(me.X, me.Y));
                    }
                }
            }

            else if (rowndx >= 0 && colndx < 0)
            {
                //row header hit, show menu
                _selectedRowIndex = rowndx;
                if (_dtRI.getRowStatus(_dt.Rows[_selectedRowIndex][0].ToString()))
                {
                    //here if row is enabled
                    cmforRows.MenuItems[0].Enabled = true;
                    cmforRows.MenuItems[1].Enabled = false; //cannot enable enabled row
                }
                else
                {
                    //here if row is disabled
                    cmforRows.MenuItems[0].Enabled = false; //cannot disable disabled row
                    cmforRows.MenuItems[1].Enabled = true;
                }
                cmforRows.Show(dgv, new Point(me.X, me.Y));
            }
        }

        #endregion

        #region context menu event handlers

        /// <summary>
        /// set table/grid/listview properties for the user-selected column menu item Enable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnableCol(object sender, EventArgs e)
        {
            string cn = _dt.Columns[_selectedColIndex].Caption;
            _dtCI.setColStatus(_dt.Columns[_selectedColIndex].ColumnName.ToString(), true);
            _dt.Columns[_selectedColIndex].ExtendedProperties[VBTools.Globals.ENABLED] = true;
            updateListView(_listvals.NDISABLEDCOLS, --_ndisabledcols);
            updateListView(_listvals.NIVS, ++_nivs);
            _gridutils.enableGridCol(dgv, _selectedColIndex, _dt);

            _state = _dtState.dirty;
        }

        private void EnableAllCols(object sender, EventArgs e)
        {
            //int ctr = 0;
            for (int c = 1; c < _dt.Columns.Count; c++)
            {
                _dtCI.setColStatus(_dt.Columns[c].Caption, true);
                _dt.Columns[c].ExtendedProperties[VBTools.Globals.ENABLED] = true;
                _gridutils.enableGridCol(dgv, c, _dt);
                //ctr++;
            }

            _ndisabledcols = 0;
            updateListView(_listvals.NDISABLEDCOLS, _ndisabledcols);

            int nonivs = _nhiddencols > 0 ? 3 : 2;
            _nivs = _dt.Columns.Count - nonivs;
            updateListView(_listvals.NIVS, _nivs);
            _state = _dtState.dirty;
        }

        /// <summary>
        /// set table/grid/listview properties for the user-selected column menu item Disable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DisableCol(object sender, EventArgs e)
        {
            string cn = _dt.Columns[_selectedColIndex].Caption;
            _dtCI.setColStatus(_dt.Columns[_selectedColIndex].ColumnName.ToString(), false);
            _dt.Columns[_selectedColIndex].ExtendedProperties[VBTools.Globals.ENABLED] = false;
            updateListView(_listvals.NDISABLEDCOLS, ++_ndisabledcols);
            updateListView(_listvals.NIVS, --_nivs);
            _gridutils.disableGridCol(dgv, _selectedColIndex);

            _state = _dtState.dirty;
        }

        /// <summary>
        /// set table/grid/listview properties for the user-selected column menu item SetResponse.
        /// pick this variable as the response variable, and if the previous response variable was a 
        /// transform, delete it from the table and unhide its original column.  Further, if
        /// independent variable transforms are present in the table remove them from the table (with 
        /// user permission)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetResponse(object sender, EventArgs e)
        {
            DialogResult dlgr = DialogResult.Yes;

            if (hasTCols())
            {
                dlgr = MessageBox.Show("Changing response variable results in removal of created columns; Proceed (Y?N)",
                "Are you sure you want to continue?", MessageBoxButtons.YesNo);
            }

            if (dlgr == DialogResult.Yes)
            {
                //save this 'cause we're maybe screwing with indicies by removing columns
                _selectedColName = _dt.Columns[_selectedColIndex].Caption;

                //maintain attributes
                _utils.setAttributeValue(_dt.Columns[_selectedColIndex], VBTools.Globals.DEPENDENTVAR, true);
                _utils.setAttributeValue(_dt.Columns[_responseVarColIndex], VBTools.Globals.DEPENDENTVAR, false);

                //filter transformed cols
                if (hasTCols()) _dt = _tableutils.filterTcols(_dt);


                //if (_utils.testValueAttribute(_dt.Columns[_responseVarColIndex],VBTools.Globals.DEPENDENTVARIBLETRANSFORM))
                if (_utils.testValueAttribute(_dt.Columns[_responseVarColName], VBTools.Globals.DEPENDENTVARIBLETRANSFORM))
                    {
                        _dt.Columns.Remove(_dt.Columns[_responseVarColName].Caption);
                        _dt.AcceptChanges();
                        _gridutils.unHideHiddenCols(dgv, _dt);

                        updateListView(_listvals.NHIDDENCOLS, --_nhiddencols);
                    }
                //}

                _responseVarColIndex = _dt.Columns.IndexOf(_selectedColName);
                _responseVarColName = _dt.Columns[_responseVarColIndex].Caption;

                _gridutils.maintainGrid(dgv, _dt, _selectedColIndex, _responseVarColName);

                //count iv columns and update list
                int nonivs = _nhiddencols > 0 ? 3 : 2;
                _nivs = _dt.Columns.Count - nonivs;
                updateListView(_listvals.NIVS, _nivs);
                //and rv name
                updateListView(_listvals.RVCOLNAME, _responseVarColName);

                _state = _dtState.dirty;

            }
        }

        /// <summary>
        /// set table/grid/listview properties for the user-selected column menu item Transform
        /// (the individual transform event handlers all call this method to add the transformed values
        /// to the table as a new column, hide the untransformed column.  if the previous variable
        /// was a transformed variable, 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="newcolname"></param>
        /// <param name="_selectedColIndex"></param>
        /// <param name="newvals"></param>
        private void transformRVValues(DataTable dt, string newcolname, int _selectedColIndex, double[] newvals)
        {
            //we're only here on transform of dependent variable
            DialogResult dlgr = DialogResult.Yes;
            if (hasTCols())
            {
                dlgr = MessageBox.Show("Datasheet contains independent variable transforms; transforming the dependent variable\n results in removal of created columns; Proceed (Y?N)",
                    "Are you sure you want to continue?", MessageBoxButtons.YesNo);
            }
            if (dlgr == DialogResult.Yes)
            {
                try
                {
                    DataTable dtCopy = dt.Copy();
                    _utils.setAttributeValue(dtCopy.Columns[_selectedColIndex], VBTools.Globals.DEPENDENTVAR, false);
                    string colname = dtCopy.Columns[_selectedColIndex].Caption;

                    if (hasTCols())
                    {
                        dtCopy = _tableutils.filterTcols(dtCopy);
                    }
                    dtCopy = _tableutils.setHiddenIVstoUnhidden(dtCopy);

                    int ordinal = dtCopy.Columns.IndexOf(colname);
                    

                    dtCopy.Columns.Add(newcolname, typeof(double));
                    for (int r = 0; r < dtCopy.Rows.Count; r++)
                        dtCopy.Rows[r][newcolname] = newvals[r];

                    //dtCopy.Columns[newcolname].SetOrdinal(_selectedColIndex + 1);
                    dtCopy.Columns[newcolname].SetOrdinal(ordinal + 1);
                    dtCopy.AcceptChanges();
                    _dtCI.addColumnNameToDic(newcolname);

  
                    //set properties of new rv
                    _responseVarColName = newcolname;
                    _responseVarColIndex = dtCopy.Columns.IndexOf(_responseVarColName);
                    _utils.setAttributeValue(dtCopy.Columns[newcolname], VBTools.Globals.DEPENDENTVARIBLETRANSFORM, true);

                    //set properties of old one
                    _utils.setAttributeValue(dtCopy.Columns[_selectedColIndex], VBTools.Globals.HIDDEN, true);


                    _dt = dtCopy;
                    _gridutils.maintainGrid(dgv, _dt, _selectedColIndex, _responseVarColName);

                    updateListView(_listvals.NHIDDENCOLS, ++_nhiddencols);
                    //count iv columns and update list
                    int nonivs = _nhiddencols > 0 ? 3 : 2;
                    _nivs = _dt.Columns.Count - nonivs;
                    updateListView(_listvals.NIVS, _nivs);
                    //and rv name
                    updateListView(_listvals.RVCOLNAME, _responseVarColName);

                    _state = _dtState.dirty;
                }
                catch (DuplicateNameException e)
                {
                    MessageBox.Show("Table already contains column: " + newcolname, "Cannot Add Column to Table", MessageBoxButtons.OK);
                }
            }
        }

        private void UnTransform(object o, EventArgs e)
        {
            //can only untransform response variable (only one transformable)
            //unhide the original response variable
            //remove the transformed column
            DialogResult dlgr = DialogResult.Yes;
            if (hasTCols())
            {
                dlgr = MessageBox.Show("Datasheet contains independent variable transforms; un-transforming the dependent variable\n results in removal of created columns; Proceed (Y?N)",
                    "Are you sure you want to continue?", MessageBoxButtons.YesNo);
            }
            if (dlgr == DialogResult.Yes)
            {
                DataTable dtCopy = _dt;
                dtCopy.Columns.Remove(_dt.Columns[_selectedColIndex].Caption);
                dtCopy.AcceptChanges();
                if (hasTCols())
                {
                    dtCopy = _tableutils.filterTcols(dtCopy);
                    //dtCopy = _tableutils.setHiddenIVstoUnhidden(dtCopy);
                }
                //dtCopy = _tableutils.setHiddenIVstoUnhidden(dtCopy);

                foreach (DataColumn c in dtCopy.Columns)
                {
                    //if (!_utils.testValueAttribute(c, VBTools.Globals.DEPENDENTVAR)) continue;
                    //bool hasattr = c.ExtendedProperties.ContainsKey(VBTools.Globals.DEPENDENTVAR);
                    //if (!hasattr) continue;
                    //bool isrv = (bool)c.ExtendedProperties[VBTools.Globals.DEPENDENTVAR];
                    //if (isrv == true)
                    //find the hidden col
                    if (!_utils.testValueAttribute(c, VBTools.Globals.HIDDEN)) continue;
                    {
                        _utils.setAttributeValue(c, VBTools.Globals.DEPENDENTVAR, true);
                        _utils.setAttributeValue(c, VBTools.Globals.HIDDEN, false);
                        _selectedColIndex = dtCopy.Columns.IndexOf(c);
                        _responseVarColName = dtCopy.Columns[_selectedColIndex].Caption;
                        _responseVarColIndex = _selectedColIndex;

                        updateListView(_listvals.NHIDDENCOLS, --_nhiddencols);
                        break;
                    }
                }
                _dt = dtCopy;
                _gridutils.maintainGrid(dgv, _dt, _selectedColIndex, _responseVarColName);
                //count iv columns and update list
                int nonivs = _nhiddencols > 0 ? 3 : 2;
                _nivs = _dt.Columns.Count - nonivs;
                updateListView(_listvals.NIVS, _nivs);
                //and rv name
                updateListView(_listvals.RVCOLNAME, _responseVarColName);

                _state = _dtState.dirty;
            }
        }

        /// <summary>
        /// determine if we can set the selected column as the response variable
        /// (cannot set a transformed independent variable as the response variable - or categorical variables....)
        /// </summary>
        /// <returns>false iff column is an IV transform, true otherwises</returns>
        private bool canSetRV()
        {
            bool colisT = _dt.Columns[_selectedColIndex].ExtendedProperties.ContainsKey(VBTools.Globals.TRANSFORM);
            bool colisI = _dt.Columns[_selectedColIndex].ExtendedProperties.ContainsKey(VBTools.Globals.OPERATION);
            bool colisCat = _utils.testValueAttribute(_dt.Columns[_selectedColIndex], VBTools.Globals.CATEGORICAL);
            //if (colisI == true || colisT == true)
            if (colisT == true || colisCat == true) return false;
            else return true;
        }

        /// <summary>
        /// determine if the table has manipulated (interacted) data columns
        /// </summary>
        /// <returns>true if table has manipulated columns, false otherwise</returns>
        private bool hasOCols()
        {
            //determine is the table has manipulated data
            foreach (DataColumn c in _dt.Columns)
            {
                bool colisI = c.ExtendedProperties.ContainsKey(VBTools.Globals.OPERATION);
                //if (colisI == true || colisT == true) return true;
                if (colisI == true) return true;
            }

            return false;
        }

        /// <summary>
        /// determine if the table has transformed IV columns
        /// </summary>
        /// <returns>true if present, false otherwise</returns>
        private bool hasTCols()
        {
            //determine is the table has transformed data
            foreach (DataColumn c in _dt.Columns)
            {
                bool colisT = c.ExtendedProperties.ContainsKey(VBTools.Globals.TRANSFORM);
                bool colisI = c.ExtendedProperties.ContainsKey(VBTools.Globals.OPERATION);
                //if (colisI == true || colisT == true) return true;
                if (colisT == true) return true;
            }

            return false;
        }

        /// <summary>
        /// determine if the table has either transformed IV columns OR manipulated columns
        /// </summary>
        /// <returns>true iff either present, false iff neither</returns>
        private bool hasOorTCols()
        {
            foreach (DataColumn c in _dt.Columns)
            {
                bool colisT = c.ExtendedProperties.ContainsKey(VBTools.Globals.TRANSFORM);
                bool colisI = c.ExtendedProperties.ContainsKey(VBTools.Globals.OPERATION);
                if (colisI == true || colisT == true) return true;
            }

            return false;
        }

        private void defineTransformForRV(object o, EventArgs e)
        {
            MenuItem mi = (MenuItem)o;
            string transform = mi.Text;
            if (transform == VBTools.Globals.DependentVariableTransforms.Power.ToString())
            {
                frmPowerExponent frmExp = new frmPowerExponent(_dt, _selectedColIndex);
                DialogResult dlgr = frmExp.ShowDialog();
                if (dlgr != DialogResult.Cancel)
                {
                    //if (frmExp.TransformMessage != "")
                    //{

                    //}
                    string sexp = frmExp.Exponent.ToString("n2");
                    transform += "," + sexp;
                    _projMgr.ModelingInfo.DependentVariableTransform = VBTools.Globals.DependentVariableTransforms.Power;
                    _projMgr.ModelingInfo.PowerTransformExponent = Convert.ToDouble(sexp);
                    _dt.Columns[_selectedColIndex].ExtendedProperties[VBTools.Globals.DEPENDENTVARIBLEDEFINEDTRANSFORM] = transform;
                    _state = _dtState.dirty;
                }
            }
            else
            {
                if (String.Compare(transform,"Log10",true) == 0)
                    _projMgr.ModelingInfo.DependentVariableTransform = VBTools.Globals.DependentVariableTransforms.Log10;
                else if (String.Compare(transform,"Ln",true) == 0)
                    _projMgr.ModelingInfo.DependentVariableTransform = VBTools.Globals.DependentVariableTransforms.Ln; 
                else if (String.Compare(transform,"none",true) == 0)
                    _projMgr.ModelingInfo.DependentVariableTransform = VBTools.Globals.DependentVariableTransforms.none;

                _dt.Columns[_selectedColIndex].ExtendedProperties[VBTools.Globals.DEPENDENTVARIBLEDEFINEDTRANSFORM] = transform;
                _state = _dtState.dirty;
            }
        }
        /// <summary>
        /// set grid/listview properties for the user-selected row menu item Enable
        /// note that there are no table row extended properties - row status is tracked
        /// in the dtRowInformation class
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnableRow(object sender, EventArgs e)
        {
            //dgv.Rows[_selectedRowIndex].DefaultCellStyle.ForeColor = Color.Black;
            _dtRI.setRowStatus(_dt.Rows[_selectedRowIndex][0].ToString(), true);
            for (int c = 0; c < dgv.Columns.Count; c++)
            {
                if (!_dtCI.getColStatus(dgv.Columns[c].Name.ToString())) continue;
                dgv[c, _selectedRowIndex].Style.ForeColor = Color.Black;
            }

            updateListView(_listvals.NDISABLEDROWS, --_ndisabledrows);
            _state = _dtState.dirty;
        }

        private void EnableAllRows(object sender, EventArgs e)
        {
            for (int r = 0; r < _dt.Rows.Count; r++)
            {
                _dtRI.setRowStatus(_dt.Rows[r][0].ToString(), true);
            }

            for (int r = 0; r < dgv.Rows.Count; r++)
            {
                for (int c = 0; c < dgv.Columns.Count; c++)
                {
                    if (!_dtCI.getColStatus(dgv.Columns[c].Name.ToString())) continue;
                    dgv[c, r].Style.ForeColor = Color.Black;
                }
            }

            _ndisabledrows = 0;
            updateListView(_listvals.NDISABLEDROWS, _ndisabledrows);
            _state = _dtState.dirty;

        }

        /// <summary>
        /// set grid/listview properties for the user-selected row menu item Disable
        /// note that there are no table row extended properties - row status is tracked
        /// in the dtRowInformation class
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DisableRow(object sender, EventArgs e)
        {
            //dgv.Rows[_selectedRowIndex].DefaultCellStyle.ForeColor = Color.Red;
            _dtRI.setRowStatus(_dt.Rows[_selectedRowIndex][0].ToString(), false);
            for (int c = 0; c < dgv.Columns.Count; c++)
            {
                dgv[c, _selectedRowIndex].Style.ForeColor = Color.Red;
            }

            updateListView(_listvals.NDISABLEDROWS, ++_ndisabledrows);
            _state = _dtState.dirty;
        }

        /// <summary>
        /// response variable transform log10
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void log10T(object o, EventArgs e)
        {
            //can only transform the response variable and the response variable can not be a transformed ME or and interacted ME
            Transform t = new Transform(_dt, _selectedColIndex);
            double[] newvals = new double[_dt.Rows.Count];
            newvals = t.LOG10;
            if (t.Message != "")
            {
                MessageBox.Show("Cannot Log10 transform variable. " + t.Message, "VB Transform Rule", MessageBoxButtons.OK);
                return;
            }

            string newcolname = "LOG10[" + _dt.Columns[_selectedColIndex].Caption + "]";
            performTOperation(_dt, newcolname, _selectedColIndex, newvals);
            _dt.Columns[newcolname].ExtendedProperties[VBTools.Globals.DEPENDENTVARIBLEDEFINEDTRANSFORM] = VBTools.Globals.DependentVariableTransforms.Log10.ToString();
            _projMgr.ModelingInfo.DependentVariableTransform = VBTools.Globals.DependentVariableTransforms.Log10;

            _responseVarColIndex = _dt.Columns.IndexOf(newcolname);
            _responseVarColName = _dt.Columns[_responseVarColIndex].Caption;

            _state = _dtState.dirty;
        }
        /// <summary>
        /// response variable transform natural log
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void lnT(object o, EventArgs e)
        {
            Transform t = new Transform(_dt, _selectedColIndex);
            double[] newvals = new double[_dt.Rows.Count];
            newvals = t.LOGE;
            if (t.Message != "")
            {
                MessageBox.Show("Cannot Ln transform variable. " + t.Message, "VB Transform Rule", MessageBoxButtons.OK);
                return;
            }

            string newcolname = "LN[" + _dt.Columns[_selectedColIndex].Caption + "]";
            performTOperation(_dt, newcolname, _selectedColIndex, newvals);
            _dt.Columns[newcolname].ExtendedProperties[VBTools.Globals.DEPENDENTVARIBLEDEFINEDTRANSFORM] = VBTools.Globals.DependentVariableTransforms.Ln.ToString();
            _projMgr.ModelingInfo.DependentVariableTransform = VBTools.Globals.DependentVariableTransforms.Ln;

            _state = _dtState.dirty;
        }
        /// <summary>
        /// response variable transform power(exp)
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void powerT(object o, EventArgs e)
        {
            frmPowerExponent frmExp = new frmPowerExponent(_dt, _selectedColIndex);
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
                string sexp = frmExp.Exponent.ToString("n2");
                //string newcolname = "POWER(" + sexp + ")" + "[" + _dt.Columns[_selectedColIndex].Caption + "]";
                string newcolname = "POWER" + "[" + sexp + "," + _dt.Columns[_selectedColIndex].Caption + "]";
                performTOperation(_dt, newcolname, _selectedColIndex, newvals);
                _dt.Columns[newcolname].ExtendedProperties[VBTools.Globals.DEPENDENTVARIBLEDEFINEDTRANSFORM] = VBTools.Globals.DependentVariableTransforms.Power.ToString() + "," + sexp;
                _projMgr.ModelingInfo.DependentVariableTransform = VBTools.Globals.DependentVariableTransforms.Power;
                _projMgr.ModelingInfo.PowerTransformExponent = Convert.ToDouble(sexp);

                _state = _dtState.dirty;
            }
            //addValues(_dt, newcolname, _selectedColIndex, newvals);
            //dgv.DataSource = null;
            //dgv.DataSource = _dt;

        }
        /// <summary>
        /// show the plots for the seleected column
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void seePlot(object o, EventArgs e)
        {
            //DataTable dt = filterDatableRows();
            frmPlot frmplot = new frmPlot(_responseVarColIndex, _selectedColIndex, filterDataTableRows(_dt));

            frmplot.Show();


            //set up a delegate for each plot form and listen for disable/enable point events
            frmplot.pointDisabled += new frmPlot.PointDisableEventHandler(frmplot_pointDisabled);
            frmplot.pointEnabled += new frmPlot.PointEnableEventHandler(frmplot_pointEnabled);
        }
        /// <summary>
        /// delete the select column
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void DeleteCol(object o, EventArgs e)
        {
            if (_dt.Columns[_selectedColIndex].ExtendedProperties.ContainsKey(VBTools.Globals.TRANSFORM) ||
                _dt.Columns[_selectedColIndex].ExtendedProperties.ContainsKey(VBTools.Globals.OPERATION) ||
                _dt.Columns[_selectedColIndex].ExtendedProperties.ContainsKey(VBTools.Globals.DECOMPOSITION) )
            {
                _dtCI.removeColumnFromDic(_dt.Columns[_selectedColIndex].Caption);
                
                int gridpos = dgv.FirstDisplayedScrollingColumnIndex;
                _dt.Columns.Remove(_dt.Columns[_selectedColIndex].Caption);
                
                _dt.AcceptChanges();

                _gridutils.maintainGrid(dgv, _dt, _selectedColIndex, _responseVarColName);

                dgv.DataSource = _dt;
                dgv.FirstDisplayedScrollingColumnIndex = gridpos;
                updateListView(_listvals.NCOLS, _dt.Columns.Count);
                updateListView(_listvals.NIVS, --_nivs);

                _state = _dtState.dirty;
                
            }
        }

        //private void antilog10T(object o, EventArgs e)
        //{
        //    Transform t = new Transform(_dt, _selectedColIndex);
        //    double[] newvals = new double[_dt.Rows.Count];
        //    newvals = t.ANTILOG10;

        //    string newcolname = "ANTILOG10[" + _dt.Columns[_selectedColIndex].Caption + "]";
        //    performTOperation(_dt, newcolname, _selectedColIndex, newvals);
        //    //dgv.DataSource = null;
        //    //dgv.DataSource = _dt;
        //}
        //private void antilnT(object o, EventArgs e)
        //{
        //    Transform t = new Transform(_dt, _selectedColIndex);
        //    double[] newvals = new double[_dt.Rows.Count];
        //    newvals = t.ANTILn;

        //    string newcolname = "ANTILN[" + _dt.Columns[_selectedColIndex].Caption + "]";
        //    performTOperation(_dt, newcolname, _selectedColIndex, newvals);
        //    //dgv.DataSource = null;
        //    //dgv.DataSource = _dt;
        //}


        #region handlers for plot interaction
        /// <summary>
        /// disable a row in the grid for a point selected by user on the plots
        /// </summary>
        /// <param name="tag">this is the record idetifier (the date/timestamp [column[0]] value)</param>
        private void frmplot_pointDisabled(string tag)
        {
            bool found = false;
            string d = tag;

            //find it in the datatable
            foreach (DataRow r in _dt.Rows)
            {
                if (r[0].ToString() != d) continue;
                //otherwise we found it
                _selectedRowIndex = _dt.Rows.IndexOf(r);
                DisableRow(null, null);
                found = true;

                //updateListView(_listvals.NDISABLEDROWS, ++_ndisabledrows);
                break;

            }
            if (!found)
            {
                MessageBox.Show("Unable to locate row with date/time " + tag,
                    "Disable Record Error.",
                    MessageBoxButtons.OK);
            }

        }

        /// <summary>
        /// enable a row in the grid for a point selected by user on the plots
        /// </summary>
        /// <param name="tag">this is the record idetifier (the date/timestamp [column[0]] value)</param>
        private void frmplot_pointEnabled(string tag)
        {
            bool found = false;
            string d = tag;

            //find it in the datatable
            foreach (DataRow r in _dt.Rows)
            {
                if (r[0].ToString() != d) continue;
                //otherwise we found it
                _selectedRowIndex = _dt.Rows.IndexOf(r);
                EnableRow(null, null);
                found = true;

                //updateListView(_listvals.NDISABLEDROWS, --_ndisabledrows);
                break;

            }
            if (!found)
            {
                MessageBox.Show("Unable to locate row with date/time " + tag,
                    "Disable Record Error.",
                    MessageBoxButtons.OK);
            }

        }

        #endregion plot delegates

        #endregion context menu handlers

        /// <summary>
        /// a transform of the response variable has been requested, add it to the table
        /// </summary>
        /// <param name="_dt"></param>
        /// <param name="newcolname"></param>
        /// <param name="_selectedColIndex"></param>
        /// <param name="newvals"></param>
        private void performTOperation(DataTable _dt, string newcolname, int _selectedColIndex, double[] newvals)
        {
            switch (_addreplace)
            {
                case AddReplace.Add:
                    transformRVValues(_dt, newcolname, _selectedColIndex, newvals);
                    break;
                case AddReplace.Replace:
                    replaceValues(_dt, newcolname, _selectedColIndex, newvals);
                    break;
            }

        }

        /// <summary>
        /// depricated - not replaceing columns any longer - but here if needed later
        /// </summary>
        /// <param name="_dt"></param>
        /// <param name="newcolname"></param>
        /// <param name="_selectedColIndex"></param>
        /// <param name="newvals"></param>
        private void replaceValues(DataTable _dt, string newcolname, int _selectedColIndex, double[] newvals)
        {

            //_dt.Columns.Add(newcolname, typeof(double));
            //for (int r = 0; r < _dt.Rows.Count; r++)
            //    _dt.Rows[r][newcolname] = newvals[r];

            //_dt.Columns[newcolname].SetOrdinal(_selectedColIndex + 1);

            //addValues(_dt, newcolname, _selectedColIndex, newvals);
            //_dt.Columns.Remove(_dt.Columns[_selectedColIndex]);
        }

        /// <summary>
        /// strips disabled records from the table passed to modeling
        /// </summary>
        /// <param name="dt">table to strip</param>
        /// <returns>striped table</returns>
        private DataTable filterDataTableRows(DataTable dt)
        {
            //filter out disabled rows
            DataTable dtCopy = dt.Copy();
            Dictionary<string, bool> rstatus = _dtRI.DTRowInfo;
            for (int i = 0; i < dtCopy.Rows.Count; i++)
            {
                if (!rstatus[dtCopy.Rows[i][0].ToString()])
                    dtCopy.Rows[i].Delete();
            }
            dtCopy.AcceptChanges();
            return dtCopy;
        }



        public void btnValidateData_Click(object sender, EventArgs e)
        {
            DataTable savedt = _dt.Copy();
            frmMissingValues frmMissing = new frmMissingValues(this.dgv, _dt);
            frmMissing.ShowDialog();
            //when whatever checks we're doing are good, enable the manipulation buttons
            if (frmMissing.Status)
            {
                _dt = frmMissing.ValidatedDT; //keep the grid (updated in tool) data but update the global table
                //btnComputeUV.Enabled = true;
                //btnManipulate.Enabled = true;
                //btnTransform.Enabled = true;
                //btnModeling.Enabled = true;
                dgv.Enabled = true;
                //update list in case they've deleted cols/rows
                updateListView(_listvals.NCOLS, _dt.Columns.Count);
                int nonivs = _nhiddencols > 0 ? 3 : 2;
                _nivs = _dt.Columns.Count - nonivs;
                updateListView(_listvals.NIVS, _nivs);
                int recount = _dt.Rows.Count;
                updateListView(_listvals.NROWS, recount);
                //_state = _dtState.clean;
                _state = _dtState.dirty;
                _validated = true;
            }
            else
            {
                //dgv.DataSource = _dt; //revert the grid to its previous state
                _dt = savedt;
                dgv.DataSource = _dt;
                //btnComputeUV.Enabled = false;
                //btnManipulate.Enabled = false;
                //btnTransform.Enabled = false;
                //btnModeling.Enabled = false;
                dgv.Enabled = false;
                _dtRI = dtRowInformation.getdtRI(_dt, true);
                _dtCI = dtColumnInformation.getdtCI(_dt, true);
                //hmmm, is this always true?
                //_state = _dtState.clean;
                _validated = false;
            }
        }

        private void dgv_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

            dgv.EndEdit();
            //_dt = (DataTable)dgv.DataSource;  // NG - this blows away column/row enable/disable attributes in the grid .....
            _dt.Rows[e.RowIndex][e.ColumnIndex] = dgv[e.ColumnIndex, e.RowIndex].Value;
            _dt.AcceptChanges();
            _state = _dtState.dirty;

        }

        private void dgv_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            double result;
            string scellval = dgv[e.ColumnIndex, e.RowIndex].Value.ToString();
            //double.TryParse(scellval, out result);
            if (double.TryParse(scellval, out result))
            //if (result == 0.0d)
            {
                MessageBox.Show("Grid cell values cannot be blank or non-numeric", "Cell Error", MessageBoxButtons.OK);
                e.Cancel = true;
                dgv[e.ColumnIndex, e.RowIndex].Selected = true;
            }
            else
            {
                e.Cancel = false;
            }
        }

        private void dgv_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.C) && (e.Modifiers == Keys.Control))
            {
                Clipboard.SetDataObject(dgv.GetClipboardContent());
            }

        }

        public void btnComputeAO_Click(object sender, EventArgs e)
        {

        }

        public void btnManipulate_Click(object sender, EventArgs e)
        {

        }

        public void btnTransform_Click(object sender, EventArgs e)
        {

        }

        public void btnGoToModeling_Click(object sender, EventArgs e)
        {
            DataTable dt = _dt.Copy();
            //modeling expects this
            dt.Columns[_responseVarColName].SetOrdinal(1);
            //filter disabled rows and columns
            dt = filterDataTableRows(dt);
            dt = _tableutils.filterRVHcols(dt);
            _projMgr.DataSheetDataTable = _dt;

            //show the modeling tab... should be disabled on import until validation...?
            if (_state == _dtState.dirty && !_initialPass)
            {
                DialogResult dlgr = MessageBox.Show("Changes in data and/or data attributes have occurred.\nPrevious modeling results will be erased. Proceed?", "Proceed to Modeling.", MessageBoxButtons.OKCancel);
                if (dlgr == DialogResult.OK)
                {
                    _projMgr.CorrelationDataTable = dt;
                    _savedDT = _dt;
                    _projMgr.DataSheetDataTable = _dt;
                    _state = _dtState.clean;

                    if (this.ResetProject != null)
                    {
                        EventArgs args = new EventArgs();
                        this.ResetProject(this, args);
                    }

                    _projMgr._comms.sendMessage("ShowModelingOnly", this);
                }
                else
                    return;
            }
            else if (_initialPass)
            {
                _projMgr.CorrelationDataTable = dt;
                _projMgr.ModelDataTable = dt;
                _state = _dtState.clean;
                _initialPass = false;
                _savedDT = _dt;
                _savedDGV = dgv;
                _projMgr._comms.sendMessage("ShowModelingOnly", this);
            }
            else
            {
                _projMgr._comms.sendMessage("Show", this);
            }

        } //then would go to MainForm MessageSentListener(message, senderForm) to show other forms

        /// <summary>
        /// test all cells in the daetime column for uniqueness
        /// could do this with linq but then how does one find where?
        /// </summary>
        /// <param name="dt">table to search</param>
        /// <param name="where">record number of offending timestamp</param>
        /// <returns>true iff all unique, false otherwise</returns>
        
        

        /// <summary>
        /// populate the UI list with some file/data information
        /// </summary>
        /// <param name="fn">filename of data imported</param>
        
    }

}
