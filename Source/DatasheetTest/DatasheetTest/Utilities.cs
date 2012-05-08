using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Data;
using Microsoft.VisualBasic;
using System.Drawing;
using System.Windows.Forms;

namespace DatasheetTest
{
    /// <summary>
    /// class contains a number of useful methods applicable to datatable and datagridview
    /// controls.  code was collected here to help readablilty of datasheet code and avoid 
    /// duplicating these functional requirements.
    /// </summary>
    public class Utilities
    {
        public bool testValueAttribute(DataColumn dc, string attr)
        {
            if (dc.ExtendedProperties.ContainsKey(attr))
            {
                //if (dc.ExtendedProperties[attr].Equals(true))
                if (dc.ExtendedProperties[attr].ToString() == "True")
                    return true;
                else
                    return false;
                //bool value = (bool)dc.ExtendedProperties[attr];
                //return value;
            }
            return false;
        }
        public void setAttributeValue(DataColumn dc, string attr, bool value)
        {
            dc.ExtendedProperties[attr] = value;
        }

        /// <summary>
        /// datatable methods for filtering and adding columns based on table extended properties
        /// </summary>
        public class TableUtils
        {
            private dtRowInformation _dtRI = null;
            private dtColumnInformation _dtCI = null;
            private DataTable _dt = null;

            public TableUtils(DataTable dt)
            {
                _dt = dt;
                _dtCI = dtColumnInformation.getdtCI(dt, false);
                _dtRI = dtRowInformation.getdtRI(dt, false);

            }
            public void registerNewCols(DataTable dt)
            {
                _dtCI = dtColumnInformation.getdtCI(dt, false);
                foreach (DataColumn c in dt.Columns)
                {
                    if (!_dtCI.getColStatus(c.ColumnName))
                    {
                        _dtCI.addColumnNameToDic(c.ColumnName);
                    }

                }
            }
            public DataTable filterDisabledCols(DataTable dt)
            {
                //filter out disabled columns
                DataTable dtCopy = dt.Copy();

                dtColumnInformation dtCI = dtColumnInformation.getdtCI(dt, false);
                foreach (KeyValuePair<string, bool> kv in dtCI.DTColInfo)
                {
                    if (kv.Value) continue; 
                    if (dtCopy.Columns.Contains(kv.Key))
                        dtCopy.Columns.Remove(kv.Key);
                }
                dtCopy.AcceptChanges();
                return dtCopy;
            }
            public DataTable filterDataTableRows(DataTable dt)
            {
                //filter out disabled rows
                DataTable dtCopy = dt.Copy();
                Dictionary<string, bool> rstatus = _dtRI.DTRowInfo;
                //for (int i = 0; i < dtCopy.Rows.Count; i++)
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (!rstatus[dtCopy.Rows[i][0].ToString()])
                        //if (dtCopy.Rows.Contains(dtCopy.Rows[i][0].ToString()))
                            dtCopy.Rows[i].Delete();
                }
                dtCopy.AcceptChanges();
                return dtCopy;
            }
            public DataTable filterTcols(DataTable dt)
            {
                //filter transformed columns
                DataTable dtCopy = dt.Copy();

                for (int c = 0; c < dt.Columns.Count; c++)
                {
                    bool transformed = dt.Columns[c].ExtendedProperties.ContainsKey(VBTools.Globals.TRANSFORM);
                    if (transformed == true) 
                        if (dtCopy.Columns.Contains(dt.Columns[c].Caption.ToString()))
                            dtCopy.Columns.Remove(dt.Columns[c].Caption.ToString());
                    //_dtCI.removeColumnFromDic(dt.Columns[c].Caption);
                }

                dtCopy.AcceptChanges();
                return dtCopy;
            }
            public DataTable filterRVHcols(DataTable dt)
            {
                //filter hidden response variable columns
                DataTable dtCopy = dt.Copy();

                for (int c = 0; c < dt.Columns.Count; c++)
                {
                    bool isrv = dt.Columns[c].ExtendedProperties.ContainsKey(VBTools.Globals.DEPENDENTVAR);
                    bool istrv = dt.Columns[c].ExtendedProperties.ContainsKey(VBTools.Globals.DEPENDENTVARIBLETRANSFORM);
                    if (isrv == true)
                    {
                        bool isH = dt.Columns[c].ExtendedProperties.ContainsKey(VBTools.Globals.HIDDEN);
                        if (isH == true)
                        {
                            //bool isVis = (bool)dt.Columns[c].ExtendedProperties[VBTools.Globals.HIDDEN];
                            //if (isVis == true)
                            if (dt.Columns[c].ExtendedProperties[VBTools.Globals.HIDDEN].ToString() == "True")
                            {
                                if (dtCopy.Columns.Contains(dt.Columns[c].Caption))
                                    dtCopy.Columns.Remove(dt.Columns[c].Caption);
                                //_dtCI.removeColumnFromDic(dt.Columns[c].Caption);
                            }
                        }
                    }
                }

                dtCopy.AcceptChanges();
                return dtCopy;
            }
            public DataTable filterCatVars(DataTable dt)
            {
                DataTable dtCopy = dt.Copy();

                for (int c = 0; c < dt.Columns.Count; c++)
                {
                    bool hascat = dt.Columns[c].ExtendedProperties.ContainsKey(VBTools.Globals.CATEGORICAL);
                    //bool istrv = dt.Columns[c].ExtendedProperties.ContainsKey(VBTools.Globals.DEPENDENTVARIBLETRANSFORM);
                    if (hascat == true)
                    {
                        //bool iscat = dt.Columns[c].ExtendedProperties.ContainsKey(VBTools.Globals.CATEGORICAL);
                        //if (iscat == true)
                        if (dt.Columns[c].ExtendedProperties[VBTools.Globals.CATEGORICAL].ToString() == "True")
                        {

                            if (dtCopy.Columns.Contains(dt.Columns[c].Caption))
                                dtCopy.Columns.Remove(dt.Columns[c].Caption);
                        }
                    }
                }

                dtCopy.AcceptChanges();
                return dtCopy;
            }
            public DataTable addCatCols(DataTable dtnew, DataTable dt)
            {
                DataTable dtCopy = dtnew.Copy();
                foreach (DataColumn dc in dt.Columns)
                {
                    bool hasAttribute = dc.ExtendedProperties.ContainsKey(VBTools.Globals.CATEGORICAL);
                    if (!hasAttribute) continue;
                    if (!dtCopy.Columns.Contains(dc.Caption))
                    {
                        int ndx = dt.Columns.IndexOf(dc);
                        var dvalues = (from row in dt.Select() select row.Field<double>(dt.Columns.IndexOf(dc))).ToArray<double>();
                        dtCopy.Columns.Add(dc.Caption, typeof(double));
                        for (int r = 0; r < dtCopy.Rows.Count; r++)
                            dtCopy.Rows[r][dc.Caption] = dvalues[r];

                        if (ndx > dtCopy.Columns.Count)
                        {
                            ndx = dtCopy.Columns.Count - 1;
                        }
                        dtCopy.Columns[dc.Caption].SetOrdinal(ndx);
                        //dtCopy.Columns[dc.Caption].ExtendedProperties[VBTools.Globals.ENABLED] = false;
                        dtCopy = copyAllColAttributes(dc, dt, dtCopy);


                    }
                }
                dtCopy.AcceptChanges();
                return dtCopy;
            }
            public DataTable addDisabledCols(DataTable dtnew, DataTable dt)
            {
                DataTable dtCopy = dtnew.Copy();
                foreach (DataColumn dc in dt.Columns)
                {
                    bool hasAttribute = dc.ExtendedProperties.ContainsKey(VBTools.Globals.ENABLED);
                    if (!hasAttribute) continue;
                    //bool enabled = (bool)dc.ExtendedProperties[VBTools.Globals.ENABLED];
                    //if (!enabled)
                    if (dc.ExtendedProperties[VBTools.Globals.ENABLED].ToString() != "True")
                    {
                        if (!dtCopy.Columns.Contains(dc.Caption))
                        {
                            int ndx = dt.Columns.IndexOf(dc);
                            var dvalues = (from row in dt.Select() select row.Field<double>(dt.Columns.IndexOf(dc))).ToArray<double>();
                            dtCopy.Columns.Add(dc.Caption, typeof(double));
                            for (int r = 0; r < dtCopy.Rows.Count; r++)
                                dtCopy.Rows[r][dc.Caption] = dvalues[r];

                            if (ndx > dtCopy.Columns.Count)
                            {
                                ndx = dtCopy.Columns.Count - 1;
                            }
                            dtCopy.Columns[dc.Caption].SetOrdinal(ndx);
                            //dtCopy.Columns[dc.Caption].ExtendedProperties[VBTools.Globals.ENABLED] = false;
                            dtCopy = copyAllColAttributes(dc, dt, dtCopy);
                        }
                        
                    }
                }
                dtCopy.AcceptChanges();
                return dtCopy;
            }
            public DataTable addOldTCols(DataTable dtnew, DataTable dt)
            {
                DataTable dtCopy = dtnew.Copy();
                foreach (DataColumn dc in dt.Columns)
                {
                    if (dc.ExtendedProperties.ContainsKey(VBTools.Globals.TRANSFORM))
                    {
                        if (!dtCopy.Columns.Contains(dc.Caption))
                        {
                            var dvalues = (from row in dt.Select() select row.Field<double>(dt.Columns.IndexOf(dc))).ToArray<double>();
                            dtCopy.Columns.Add(dc.Caption, typeof(double));
                            for (int r = 0; r < dt.Rows.Count; r++)
                                dtCopy.Rows[r][dc.Caption] = dvalues[r];

                            //dtCopy.Columns[dc.Caption].ExtendedProperties[VBTools.Globals.TRANSFORM] = true;
                            dtCopy = copyAllColAttributes(dc, dt, dtCopy);

                        }
                    }
                }
                dtCopy.AcceptChanges();
                return dtCopy;
            }
            public DataTable addHiddenResponseVarCols(DataTable dtnew, DataTable dt)
            {

                DataTable dtCopy = dtnew.Copy();
                foreach (DataColumn dc in dt.Columns)
                {
                    bool hasAttribute = dc.ExtendedProperties.ContainsKey(VBTools.Globals.DEPENDENTVAR);
                    if (!hasAttribute) continue;

                    bool hasHidden = dc.ExtendedProperties.ContainsKey(VBTools.Globals.HIDDEN);
                    if (!hasHidden) continue;

                    //bool isHidden = (bool)dc.ExtendedProperties[VBTools.Globals.HIDDEN];
                    //if (isHidden)
                    if (dc.ExtendedProperties[VBTools.Globals.HIDDEN].ToString() == "True")
                    {
                        if (!dtCopy.Columns.Contains(dc.Caption))
                        {
                            int ndx = dt.Columns.IndexOf(dc);
                            var dvalues = (from row in dt.Select() select row.Field<double>(dt.Columns.IndexOf(dc))).ToArray<double>();
                            dtCopy.Columns.Add(dc.Caption, typeof(double));
                            for (int r = 0; r < dtCopy.Rows.Count; r++)
                                dtCopy.Rows[r][dc.Caption] = dvalues[r];

                            dtCopy.Columns[dc.Caption].SetOrdinal(ndx);
                            dtCopy = copyAllColAttributes(dc, dt, dtCopy);
                            //dtCopy.Columns[dc.Caption].ExtendedProperties[VBTools.Globals.HIDDEN] = true;
                            //dtCopy.Columns[dc.Caption].ExtendedProperties[VBTools.Globals.DEPENDENTVAR] = true;
                        }
                    }

                }
                dtCopy.AcceptChanges();
                return dtCopy;
            }
            public DataTable setHiddenIVstoUnhidden(DataTable dt)
            {
                DataTable dtCopy = dt.Copy();
                foreach (DataColumn dc in dtCopy.Columns)
                {
                    bool hasAttr = dc.ExtendedProperties.ContainsKey(VBTools.Globals.MAINEFFECT);
                    if (hasAttr)
                    {
                        hasAttr = dc.ExtendedProperties.ContainsKey(VBTools.Globals.HIDDEN);
                        if (hasAttr)
                        {
                            //bool isHidden = (bool)dc.ExtendedProperties[VBTools.Globals.HIDDEN];
                            //if (isHidden)
                            if (dc.ExtendedProperties[VBTools.Globals.HIDDEN].ToString() == "True")
                            {
                                dc.ExtendedProperties[VBTools.Globals.HIDDEN] = false;
                            }
                        }
                    }
                }
                return dtCopy;
            }
            private DataTable copyAllColAttributes(DataColumn dc, DataTable sourceDT, DataTable targetDT)
            {
                PropertyCollection sourceproperties = sourceDT.Columns[dc.Caption].ExtendedProperties;    
                foreach (DictionaryEntry kv in sourceproperties)
                {
                    targetDT.Columns[dc.Caption].ExtendedProperties[kv.Key] = kv.Value;
                }
                return targetDT;
            }
        }

        /// <summary>
        /// datagridview methods for maintaining the view of the datatable data in the grid
        /// </summary>
        public class GridUtils
        {
            private dtRowInformation _dtRI = null;

            public GridUtils(DataGridView dgv)
            {
            }
            public void maintainGrid(DataGridView dgv, DataTable dt, int selectedColIndex, string responseVarColName)
            {
                //reset the grid
                dgv.DataSource = null;
                dgv.DataSource = dt;

                //mark all grid cols visible, not sortable
                for (int c = 0; c < dgv.Columns.Count; c++)
                {
                    //dgv.Columns[c].Visible = true;
                    dgv.Columns[c].SortMode = DataGridViewColumnSortMode.NotSortable;
                }

                //hide any main effect cols that have been transformed (hidden attribute is set in the transform class)
                foreach (DataColumn c in dt.Columns)
                {

                    bool hashidden = c.ExtendedProperties.ContainsKey(VBTools.Globals.HIDDEN);
                    if (hashidden == true)
                    {
                        //bool hide = c.ExtendedProperties[VBTools.Globals.HIDDEN].Equals(true);
                        //bool hide = (bool)c.ExtendedProperties[VBTools.Globals.HIDDEN];
                        //if (hide) 
                        if (c.ExtendedProperties[VBTools.Globals.HIDDEN].ToString() == "True")
                        { dgv.Columns[c.ColumnName].Visible = false; }
                    }
                    //reset the column disabled properties
                    bool hasattribute = c.ExtendedProperties.ContainsKey(VBTools.Globals.ENABLED);
                    if (hasattribute)
                    {
                        selectedColIndex = dt.Columns.IndexOf(c);
                        //this is not working after serialization....
                        //bool enabled = (bool)c.ExtendedProperties[VBTools.Globals.ENABLED];
                        if (c.ExtendedProperties[VBTools.Globals.ENABLED].ToString() != "True")
                        //if (!enabled) 
                        {
                            for (int r = 0; r < dgv.Rows.Count; r++)
                                dgv[selectedColIndex, r].Style.ForeColor = Color.Red;
                        }
                        else
                        {
                            for (int r = 0; r < dgv.Rows.Count; r++)
                                dgv[selectedColIndex, r].Style.ForeColor = Color.Black;
                        }
                    }

                }

                //reset the UI clues for the response variable
                dgv.Columns[responseVarColName].DefaultCellStyle.BackColor = Color.LightBlue;

                //reset disable rows
                _dtRI = dtRowInformation.getdtRI(dt, false);
                for (int r = 0; r < dt.Rows.Count; r++)
                {
                    bool enabled = _dtRI.getRowStatus(dt.Rows[r][0].ToString());
                    if (!enabled)
                    {
                        for (int c = 0; c < dgv.Columns.Count; c++)
                            dgv[c, r].Style.ForeColor = Color.Red;

                    }
                }

                ////hide any main effect cols that have been transformed (hidden attribute is set in the transform class)
                //foreach (DataColumn c in dt.Columns)
                //{

                //    bool hasattribute = c.ExtendedProperties.ContainsKey(VBTools.Globals.HIDDEN);
                //    if (hasattribute == true)
                //    {
                //        bool hide = (bool)c.ExtendedProperties[VBTools.Globals.HIDDEN];
                //        if (hide) { dgv.Columns[c.ColumnName].Visible = false; }
                //    }
                //    //reset the column disabled properties
                //    hasattribute = c.ExtendedProperties.ContainsKey(VBTools.Globals.ENABLED);
                //    if (hasattribute)
                //    {
                //        selectedColIndex = dt.Columns.IndexOf(c);
                //        bool enabled = (bool)c.ExtendedProperties[VBTools.Globals.ENABLED];
                //        if (!enabled) //{ DisableCol(null, null); }
                //        {
                //            for (int r = 0; r < dgv.Rows.Count; r++)
                //                dgv[selectedColIndex, r].Style.ForeColor = Color.Red;
                //        }
                //    }

                //}

                //set the numerical precision for display
                setViewOnGrid(dgv);
            }
            public void setViewOnGrid(DataGridView dgv)
            {
                //utility method used to set numerical precision displayed in grid

                //seems to be the only way I can figure to get a string in col 1 that may
                //(or may not) be a date and numbers in all other columns.
                //in design mode set "no format" for the dgv defaultcellstyle
                string testcellval = string.Empty;
                //double numval = double.NaN;
                for (int col = 1; col < dgv.Columns.Count; col++)
                {
                    //skips col 0 (date/time/string/whatever)
                    testcellval = dgv[col, 0].Value.ToString();
                    //double.TryParse(testcellval, out numval); //dumb: "0", a number, returns 0 as well as nonsense does
                    bool isNum = Information.IsNumeric(testcellval); //try a little visualbasic magic

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
            public void disableGridCol(DataGridView dgv, int selectedColIndex)
            {
                for (int r = 0; r < dgv.Rows.Count; r++)
                    dgv[selectedColIndex, r].Style.ForeColor = Color.Red;
            }
            public void enableGridCol(DataGridView dgv, int selectedColIndex, DataTable dt)
            {
                dtRowInformation dtRI = dtRowInformation.getdtRI(dt, false);
                for (int r = 0; r < dgv.Rows.Count; r++)
                {
                    //set style to black unless the row is disabled
                    if (!dtRI.getRowStatus(dgv[0, r].Value.ToString())) continue;
                    dgv[selectedColIndex, r].Style.ForeColor = Color.Black;
                }
            }
            public void setResponseVarCol(DataGridView dgv, int selectedColIndex, int responseVarColIndex)
            {
                dgv.Columns[responseVarColIndex].DefaultCellStyle.BackColor = Color.White;
                dgv.Columns[selectedColIndex].DefaultCellStyle.BackColor = Color.LightBlue;
            }
            public void setDisabledColandRows(DataGridView dgv, DataTable dt)
            {
                //reset the column disabled properties
                foreach (DataColumn c in dt.Columns)
                {
                    bool hasattribute = c.ExtendedProperties.ContainsKey(VBTools.Globals.ENABLED);
                    if (hasattribute)
                    {
                        int selectedColIndex = dt.Columns.IndexOf(c);
                        //bool enabled = (bool)c.ExtendedProperties[VBTools.Globals.ENABLED];
                        //if (!enabled) //{ DisableCol(null, null); }
                        if (c.ExtendedProperties[VBTools.Globals.ENABLED].ToString() != "True")
                        {
                            for (int r = 0; r < dgv.Rows.Count; r++)
                                dgv[selectedColIndex, r].Style.ForeColor = Color.Red;
                        }
                    }

                }

                //reset disable rows
                _dtRI = dtRowInformation.getdtRI(dt, false);
                for (int r = 0; r < dt.Rows.Count; r++)
                {
                    bool enabled = _dtRI.getRowStatus(dt.Rows[r][0].ToString());
                    if (!enabled)
                    {
                        for (int c = 0; c < dgv.Columns.Count; c++)
                            dgv[c, r].Style.ForeColor = Color.Red;

                    }
                }
            }
            public void unHideHiddenCols(DataGridView dgv, DataTable dt)
            {
                foreach (DataColumn dc in dt.Columns)
                {
                    bool hasH = dc.ExtendedProperties.ContainsKey(VBTools.Globals.HIDDEN);
                    if (hasH)
                    {
                        bool isHidden = (bool)dc.ExtendedProperties[VBTools.Globals.HIDDEN];
                        if (isHidden)
                        {
                            //if (!dt.Columns[dc.Caption].ExtendedProperties.ContainsKey(VBTools.Globals.DEPENDENTVAR))
                            //{
                                dt.Columns[dc.Caption].ExtendedProperties[VBTools.Globals.HIDDEN] = false;
                                dgv.Columns[dc.Caption].Visible = true;
                            //}
                        }
                    }
                }
            }
        }

    }
}
