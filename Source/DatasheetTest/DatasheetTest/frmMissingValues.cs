using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Collections;
using VBTools;

namespace VBDatasheet
{
    /// <summary>
    /// Class provides a mechanism to validate imported datasets.
    /// Validation consists of idenifying and changing "missing"
    /// data values.  Several options exist for replacing "missing"
    /// data values.  
    /// 
    /// Very different from the first two implementations of this class.
    /// Implementation of this beast is hugely inefficient if users
    /// target cell replacement one at a time as the table is re-scanned
    /// with each iteration.
    /// </summary>
    public partial class frmMissingValues : Form
    {
        private DataTable _dt = null;
        private string _withString = string.Empty;
        private string _targetCols = string.Empty;
        private DataGridView _dgv = null;
        private bool _status = false;

        private double _replacevalue;
        private string _replacestring = string.Empty;
        private string _findstring = string.Empty;

        private Utilities.TableUtils _tu = null;
        private dtColumnInformation _dtCI = null;
        private dtRowInformation _dtRI = null;

        //private frmBadCellsRpt _rpt = null;
        private List<int[]> _badCells = null;

        //target dropdownlists content depends on which radio button action seleced
        private string[] _ddlReplaceWith = { "Only This Cell", "Entire Row", "Entire Column", "Entire Sheet" };
        private string[] _ddlDeleteRow = { "Only This Row", "Entire Column", "Entire Sheet" };
        private string[] _ddlDeletColumn = { "Only This Column", "Entire Row", "Entire Sheet" };

        private bool _updatetargetrow = true;
        private bool _updatetargetcol = true;

        private static int _targetcol = 0;
        private static int _targetrow = 0;


        /// <summary>
        /// accessor to return the working copy of the datatable to the caller
        /// </summary>
        public DataTable ValidatedDT
        {
            get { return _dt; }
        }

        /// <summary>
        /// accessor to return the validation tool's status to the caller
        /// note status is only true after successful scan and btnReturn_Click()
        /// i.e., if they return via cancel, the grid is still disabled
        /// </summary>
        public bool Status
        {
            get { return _status; }
        }

        /// <summary>
        /// Constructor takes the datagridview to show changes implemented in this class
        /// </summary>
        /// <param name="dgv">datasheet grid reference</param>
        /// <param name="dt">datasource for the grid</param>
        public frmMissingValues(DataGridView dgv, DataTable dt)
        {
            InitializeComponent();

            _tu = new Utilities.TableUtils(dt);
            _dtRI = dtRowInformation.getdtRI(_dt, false);
            _dtCI = dtColumnInformation.getdtCI(dt, false);

            //get a working copy of the dataset
            _dt = dt.Copy();
            _dgv = dgv;

            cboCols.DataSource = _ddlReplaceWith;

            btnReturn.Enabled = false;

            //this.TopMost = true;
        }

        /// <summary>
        /// Scan the table of anomolous data values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnScan_Click(object sender, EventArgs e)
        {
            //_badCells = getBadCells();
            _badCells = getBadCellsByRow();
            if (_badCells.Count > 0)
                groupBox4.Enabled = true;
        }

        /// <summary>
        /// method gathers bad cells (blanks, nulls, text, search criterion) by datatable row
        /// </summary>
        /// <returns>list of int arrays containg column index and row index of bad cells</returns>
        private List<int[]> getBadCellsByRow()
        {
            int[] cellNdxs = new int[2];
            List<int[]> badCells = new List<int[]>();

            //foreach (DataColumn dc in _dt.Columns)
            foreach (DataRow dr in _dt.Rows)
            { 
                //int cndx = _dt.Columns.IndexOf(dc);
                int rndx = _dt.Rows.IndexOf(dr);
                //foreach (DataRow dr in _dt.Rows)
                foreach (DataColumn dc in _dt.Columns)
                {
                    //int rndx = _dt.Rows.IndexOf(dr);
                    int cndx = _dt.Columns.IndexOf(dc);
                    //gather blanks...
                    if (string.IsNullOrEmpty(dr[dc].ToString()))
                    {
                        cellNdxs[0] = cndx;
                        cellNdxs[1] = rndx;
                        badCells.Add(cellNdxs);
                        cellNdxs = new int[2];
                    }
                    // ...AND alpha cells only (blanks captured above)...
                    else if (!Information.IsNumeric(dr[dc].ToString()) && _dt.Columns.IndexOf(dc) != 0)
                    {
                        cellNdxs[0] = cndx;
                        cellNdxs[1] = rndx;
                        badCells.Add(cellNdxs);
                        cellNdxs = new int[2];
                    }
                    //...AND user input
                    //if (dr[dc].ToString() == _findstring && rbActionReplace.Checked && !string.IsNullOrWhiteSpace(_findstring)) 
                    if (dr[dc].ToString() == _findstring && !string.IsNullOrWhiteSpace(_findstring))
                    {
                        cellNdxs[0] = cndx;
                        cellNdxs[1] = rndx;
                        badCells.Add(cellNdxs);
                        cellNdxs = new int[2];
                    }
                }
            }

            if (badCells.Count > 0)
            {
                cellNdxs = badCells[0];
                _dgv.Rows[cellNdxs[1]].Cells[cellNdxs[0]].Selected = true;
                _dgv.CurrentCell = _dgv.SelectedCells[0];
                btnReturn.Enabled = false; //can't return until clean
                _status = false;
                return badCells;
            }
            else
            {
                lblStatus.Text = "No anomalous data values found.";
                btnReturn.Enabled = true;  //can return if user selects
                _status = true;
                groupBox4.Enabled = false;
                return badCells;
            }
        }

        /// <summary>
        /// depricated - gathers bad cells (blanks, nulls, text, search criterion) by datatable column
        /// MC wants the action to work by row.
        /// </summary>
        /// <returns>list of int arrays containg column index and row index of bad cells</returns>
        private List<int[]> getBadCells()
        {
            int[] cellNdxs = new int[2];
            List<int[]> badCells = new List<int[]>();

            foreach (DataColumn dc in _dt.Columns)
            {
                int cndx = _dt.Columns.IndexOf(dc);
                foreach (DataRow dr in _dt.Rows)
                {
                    int rndx = _dt.Rows.IndexOf(dr);
                    //gather blanks...
                    if (string.IsNullOrEmpty(dr[dc].ToString())) 
                    {
                        cellNdxs[0] = cndx;
                        cellNdxs[1] = rndx;
                        badCells.Add(cellNdxs);
                        cellNdxs = new int[2];
                    }
                    // ...AND alpha cells only (blanks captured above)...
                    else if (!Information.IsNumeric(dr[dc].ToString()) && _dt.Columns.IndexOf(dc) != 0) 
                    {
                        cellNdxs[0] = cndx;
                        cellNdxs[1] = rndx;
                        badCells.Add(cellNdxs);
                        cellNdxs = new int[2];
                    }
                    //...AND user input
                    //if (dr[dc].ToString() == _findstring && rbActionReplace.Checked && !string.IsNullOrWhiteSpace(_findstring)) 
                    if (dr[dc].ToString() == _findstring && !string.IsNullOrWhiteSpace(_findstring)) 
                    {
                        cellNdxs[0] = cndx;
                        cellNdxs[1] = rndx;
                        badCells.Add(cellNdxs);
                        cellNdxs = new int[2];
                    }
                }
            }

            if (badCells.Count > 0)
            {
                lblStatus.Text = string.Empty;
                cellNdxs = badCells[0];
                _dgv.Rows[cellNdxs[1]].Cells[cellNdxs[0]].Selected = true;
                _dgv.CurrentCell = _dgv.SelectedCells[0];
                btnReturn.Enabled = false; //can't return until clean
                _status = false;
                return badCells;
            }
            else
            {
                lblStatus.Text = "No anomolous data values found.";
                btnReturn.Enabled = true;  //can return if user selects
                _status = true;
                groupBox4.Enabled = false;
                return badCells;
            }
        }

        /// <summary>
        /// Event handler for btnAction
        /// controls the action for a replace value in bad cells, row delete or column delete
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAction_Click(object sender, EventArgs e)
        {
            lblStatus.Text = string.Empty;

            _findstring = txtFindVal.Text;
            _replacestring = txtReplaceWith.Text;

            //validate the user-supplied replacement value if replacement is what we're doing
            if (rbActionReplace.Checked && string.IsNullOrWhiteSpace(_replacestring))
            {
                CancelEventArgs ce = new CancelEventArgs();
                txtReplaceWith_Validating(txtReplaceWith, ce);
                return;
            }

            //foreach bad cell (action modified by radio button selected and dropdown target selection)
            //perform the cell update, column delete or row delete
            //possible targets are specified in the global _ddreplacewith, _dddeletecol and _dddeleterow lists
            //and the selectable targets available are controled by which UI radio button is checked
            bool stop = false;
            for (int ndx = 0; ndx < _badCells.Count; ndx++)
            {
                if (stop = doAction(ndx)) break;
            }

            //re-scan the table - maybe the target was limited and bad cells remain
            //if so, the next bad cell will be selected
            //if not, the scan will enable the appropriate controls allowing return
           // if (!stop)
           // {
                btnScan_Click(null, null);
                if (_badCells.Count > 0)
                {
                    if (!stop)
                    {
                        _updatetargetcol = true;
                        _updatetargetrow = true;
                    }
                    else
                    {
                        _updatetargetcol = false;
                        _updatetargetrow = false;
                    }
                }
            //}

        }

        /// <summary>
        /// performs a replace value in bad cells, row delete or column delete limited by target selection
        /// </summary>
        /// <param name="ndx">index of the bad cell list to work on</param>
        /// <returns>bool to control the interation loop on the bad cell list</returns>
        private bool doAction(int ndx)
        {
            bool breakloop = false;
            string target = cboCols.SelectedValue.ToString();

            int[] location = _badCells[ndx];
            int rndx = location[1];
            int cndx = location[0];

            #region replace cell value
            if (rbActionReplace.Checked)
            {
                if (target == "Only This Cell")
                {
                    _dgv.Rows[rndx].Cells[cndx].Selected = true;
                    _dgv[cndx, rndx].Value = _replacevalue;
                    _badCells.RemoveAt(ndx);
                    _dt.Rows[rndx][cndx] = _dgv[cndx, rndx].Value;
                    _dt.AcceptChanges();
                    breakloop = true;
                }
                else if (target == "Entire Row")
                {
                    if (_updatetargetrow)
                    {
                        _updatetargetrow = false;
                        _targetrow = rndx;
                    }
                    if (rndx == _targetrow)
                    {
                        _dgv.Rows[rndx].Cells[cndx].Selected = true;
                        _dgv[cndx, rndx].Value = _replacevalue;
                        _dt.Rows[rndx][cndx] = _dgv[cndx, rndx].Value;
                        _dt.AcceptChanges();
                    }
                    breakloop = false;
                }
                else if (target == "Entire Column")
                {
                    if (_updatetargetcol)
                    {
                        _updatetargetcol = false;
                        _targetcol = cndx;
                    }
                    if (cndx == _targetcol)
                    {
                        _dgv.Rows[rndx].Cells[cndx].Selected = true;
                        _dgv[cndx, rndx].Value = _replacevalue;
                        _dt.Rows[rndx][cndx] = _dgv[cndx, rndx].Value;
                        _dt.AcceptChanges();
                    }
                    breakloop = false;
                }
                else if (target == "Entire Sheet")
                {
                    _dgv.Rows[rndx].Cells[cndx].Selected = true;
                    _dgv[cndx, rndx].Value = _replacevalue;
                    _dt.Rows[rndx][cndx] = _dgv[cndx, rndx].Value;
                    _dt.AcceptChanges();
                    breakloop = false;
                }
            }
            #endregion
            #region delete row
            else if (rbActionDelRow.Checked)
            {
                if (target == "Only This Row")
                {
                    _dgv.Rows[rndx].Cells[cndx].Selected = true;
                    _dtRI.setRowStatus(_dt.Rows[rndx][0].ToString(), false);
                    _dt = _tu.filterDataTableRows(_dt);
                    _dt.AcceptChanges();
                    _dgv.DataSource = _dt;
                    breakloop = true;
                }
                else if (target == "Entire Column")
                {
                    for (int i = 0; i < _badCells.Count; i++)
                    {
                        int[] ndxs = _badCells[i];
                        int c = ndxs[0];
                        int r = ndxs[1];
                        if (c != cndx) continue;
                        _dtRI.setRowStatus(_dt.Rows[r][0].ToString(), false);

                    }
                    _dt = _tu.filterDataTableRows(_dt);
                    _dt.AcceptChanges();
                    _dgv.DataSource = _dt;
                    breakloop = true; 
                }
                else if (target == "Entire Sheet")
                {
                    for (int i = 0; i < _badCells.Count; i++)
                    {
                        int[] ndxs = _badCells[i];
                        int c = ndxs[0];
                        int r = ndxs[1];
                        _dtRI.setRowStatus(_dt.Rows[r][0].ToString(), false);

                    }
                    _dt = _tu.filterDataTableRows(_dt);
                    _dt.AcceptChanges();
                    _dgv.DataSource = _dt;
                    breakloop = true;
                }
            }
            #endregion
            #region delete column
            else if (rbActionDelCol.Checked)
            {
                if (target == "Only This Column")
                {
                    _dgv.Rows[rndx].Cells[cndx].Selected = true;
                    _dtCI.setColStatus(_dt.Columns[cndx].Caption, false);
                    _dt = _tu.filterDisabledCols(_dt);
                    _dt.AcceptChanges();
                    _dgv.DataSource = _dt;
                    breakloop = true;
                }
                else if (target == "Entire Row")
                {
                    for (int i = 0; i < _badCells.Count; i++)
                    {
                        int[] ndxs = _badCells[i];
                        int c = ndxs[0];
                        int r = ndxs[1];
                        if (r != rndx) continue;
                        _dtCI.setColStatus(_dt.Columns[c].Caption, false);

                    }
                    _dt = _tu.filterDisabledCols(_dt);
                    _dt.AcceptChanges();
                    _dgv.DataSource = _dt;
                    breakloop = true;
                }
                else if (target == "Entire Sheet")
                {
                    for (int i = 0; i < _badCells.Count; i++)
                    {
                        int[] ndxs = _badCells[i];
                        int c = ndxs[0];
                        int r = ndxs[1];
                        _dtCI.setColStatus(_dt.Columns[c].Caption, false);

                    }
                    _dt = _tu.filterDisabledCols(_dt);
                    _dt.AcceptChanges();
                    _dgv.DataSource = _dt;
                    breakloop = true;
                }
            }
            #endregion

            return breakloop;
        }

        # region maintain dropdowns selections - these are find/replace/target selections and/or type-ins

        private void cboCols_SelectedIndexChanged(object sender, EventArgs e)
        {
            _targetCols = cboCols.SelectedItem.ToString();
        }

        private void rbActionReplace_CheckedChanged(object sender, EventArgs e)
        {
            if (rbActionReplace.Checked) cboCols.DataSource = _ddlReplaceWith;
        }

        private void rbActionDelRow_CheckedChanged(object sender, EventArgs e)
        {
            if (rbActionDelRow.Checked) cboCols.DataSource = _ddlDeleteRow;
        }

        private void rbActionDelCol_CheckedChanged(object sender, EventArgs e)
        {
            if (rbActionDelCol.Checked) cboCols.DataSource = _ddlDeletColumn;
        }

        #endregion


        /// <summary>
        /// Shows a simplified variable specification dialog to identify categorical variables only
        /// updates the global table with variables that the user might have identified as categorical
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCat_Click(object sender, EventArgs e)
        {
            frmVariableSpecification frmCat = new frmVariableSpecification(_dt);
            frmCat.ShowDialog();
            //table has column extended properties set for categorical variables
            DataTable dt = frmCat.Table;
            _dt = dt;

        }

        /// <summary>
        /// Users can only return if the table scans as clean (all cells numeric); if here then it's clean
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReturn_Click(object sender, EventArgs e)
        {
            _status = true;
            Close();
        }

        /// <summary>
        /// If we cancel out of the missing dialog, close, but set the status
        /// appropriately.  Users MUST return to the caller via the Return
        /// button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            _status = false;
            //if (_rpt != null) _rpt.Close();
            Close();
        }

        private void txtFindVal_TextChanged(object sender, EventArgs e)
        {
            _findstring = txtFindVal.Text.ToString();
        }

        private void txtReplaceWith_Validating(object sender, CancelEventArgs e)
        {

            if (!double.TryParse(txtReplaceWith.Text, out _replacevalue))
            {
                e.Cancel = true;
                txtReplaceWith.Select(0, txtReplaceWith.Text.Length);
                this.errorProvider1.SetError(txtReplaceWith, "Text must convert to a number.");
            }
        }

        private void txtReplaceWith_Validated(object sender, EventArgs e)
        {
            errorProvider1.SetError(txtReplaceWith, "");
        }

        private void frmMissingValues_HelpRequested(object sender, HelpEventArgs hlpevent)
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

        private void button1_Click(object sender, EventArgs e)
        {

        }



    }
}
