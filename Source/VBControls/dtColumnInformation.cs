using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace VBControls
{
    /// <summary>
    /// class to carry datatable column enable/disable information
    /// largely superceded by use of table column extended properties
    /// but still used by the column plots enable/disable functions
    /// </summary>
    public class dtColumnInformation
    {
        //table to operate with
        private DataTable _dt = null;
        //dictionary to hold column info
        private Dictionary<string, bool> _colstatus = null;
        //class variable
        private static dtColumnInformation dtCI = null;

        /// <summary>
        /// method initializes a datatable cols information structure to all enabled
        /// </summary>
        /// <param name="dt"></param>
        private dtColumnInformation(DataTable dt)
        {
            if (dt != null)
            {
                _dt = dt.Copy();
                _colstatus = new Dictionary<string, bool>();

                for (int c = 0; c < _dt.Columns.Count; c++)
                {
                    _colstatus.Add(_dt.Columns[c].ColumnName.ToString(), true);
                }
            }
        }

        /// <summary>
        /// constructor optionally calls method to init the column information structure 
        /// and return the itself - singleton
        /// </summary>
        /// <param name="dt">table</param>
        /// <param name="init">if true, initialize; for example, on import</param>
        /// <returns></returns>
        public static dtColumnInformation getdtCI(DataTable dt, bool init)
        {
            //pass null after initialization to access the DTcolInfo property
            //or pass init=true (after import) to initialize
            if (dtCI == null || init) dtCI = new dtColumnInformation(dt);
            return dtCI;
        }

        /// <summary>
        /// method returns the enable/disable status of the column name (key)
        /// </summary>
        /// <param name="key">the table column name to check</param>
        /// <returns>true if enable, false if disable</returns>
        public bool getColStatus(string key)
        {
            //returns the status of a row
            bool status;
            _colstatus.TryGetValue(key, out status);
            return status;
        }

        /// <summary>
        /// method to set a table column to enable/disable
        /// </summary>
        /// <param name="key">table column name</param>
        /// <param name="val">true to enable, false to disable</param>
        public void setColStatus(string key, bool val)
        {
            //sets the status of a column
            _colstatus[key] = val;
        }

        /// <summary>
        /// property - structure to return column status of all columns in table...
        /// </summary>
        public Dictionary<string, bool> DTColInfo
        {
            //returns a col-status dictionary for all cols in the datatable
            set { _colstatus = value; }
            get { return _colstatus; }
        }

        /// <summary>
        /// method adds a new column to the column information structure, used when new columns are added to the table
        /// </summary>
        /// <param name="colname">column name to add</param>
        /// <returns>true iff sucessful, false if it exists already</returns>
        public bool addColumnNameToDic(string colname)
        {
            bool retval = true;
            try
            {
                _colstatus.Add(colname, true);
            }
            catch (Exception e)
            {
                retval = false;
            }
            return retval;
        }

        /// <summary>
        /// method removes a column from the table column information structure
        /// </summary>
        /// <param name="colname">col name to remove</param>
        /// <returns>true if successful, false iff not found</returns>
        public bool removeColumnFromDic(string colname)
        {
            bool retval = true;
            try
            {
                _colstatus.Remove(colname);
            }
            catch (Exception e)
            {
                return false;
            }
            return retval;
        }

    }
}
