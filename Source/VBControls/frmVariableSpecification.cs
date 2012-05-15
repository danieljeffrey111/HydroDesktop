using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//need this for Kurt's ListItem class
//using GALibForm;
//TODO - move the ListItem class to VBtools; then:
using VBTools;
using Microsoft.VisualBasic;


namespace VBControls
{
    /// <summary>
    /// Class provides a simplified variable specification mechanism to allow users
    /// to identify variables classified as containing categorical data, or override those 
    /// automatically recognized as categorical (binary valued data columns)
    /// </summary>
    public partial class frmVariableSpecification : Form
    {
        private DataTable _dtFull = null;

        /// <summary>
        /// Accessor for the table with any possibley modified properties
        /// </summary>
        public DataTable Table
        {
            get { return _dtFull; }
        }

        /// <summary>
        /// Constructor takes a datatable for loading classified lists of variables
        /// </summary>
        /// <param name="dt"></param>
        public frmVariableSpecification(DataTable dt)
        {
            InitializeComponent();
            _dtFull = dt;
            SetData(_dtFull);
        }

        /// <summary>
        /// Loads the lists with datatable variable names (column headers)
        /// </summary>
        /// <param name="dt"></param>
        private void SetData(DataTable dt)
        {
            //populate the list boxes with column names
            //default col 1 is dependent variable, all later cols are independent
            _dtFull = dt;

            if (_dtFull == null)
                return;


            List<string> fieldList = new List<string>();
            //skip date/time column
            for (int i = 1; i < _dtFull.Columns.Count; i++)
                fieldList.Add(_dtFull.Columns[i].ColumnName);


            lbIndVariables.Items.Clear();

            //add items to independent variable listbox...but move them to ignored (alpha data) or
            //categorical (number of distinct values <= 2) listboxes
            for (int i = 1; i < fieldList.Count; i++)
            {
                ListItem li = new ListItem(fieldList[i], i.ToString());
                lbIndVariables.Items.Add(li);
                //if categorical, move it to categorical box
                bool hascat = _dtFull.Columns[li.ToString()].ExtendedProperties.ContainsKey(VBTools.Globals.CATEGORICAL);
                var distinctVals = (from row in _dtFull.Select() select row[li.ToString()]).Distinct();
                if (distinctVals.Count() <= 2 && Information.IsNumeric(distinctVals.First().ToString()))
                {
                    _dtFull.Columns[li.ToString()].ExtendedProperties[VBTools.Globals.CATEGORICAL] = true;
                    object btn = button3;
                    int ndx = lbIndVariables.FindString(li.ToString());
                    lbIndVariables.SelectedItem = lbIndVariables.Items[ndx];
                    btnRemoveInputVariable_Click_1(btn, new EventArgs());
                }
                //if its already been identified, add it to the list
                else if (hascat)
                {
                    //bool hascat = _dtFull.Columns[li.ToString()].ExtendedProperties.ContainsKey(VBTools.Globals.CATEGORICAL);
                    if (hascat == true)
                    {
                        bool iscat = (bool)_dtFull.Columns[li.ToString()].ExtendedProperties[VBTools.Globals.CATEGORICAL];
                        if (iscat)
                        {
                            object btn = button3;
                            int ndx = lbIndVariables.FindString(li.ToString());
                            lbIndVariables.SelectedItem = lbIndVariables.Items[ndx];
                            btnRemoveInputVariable_Click_1(btn, new EventArgs());
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Moves the selected variables from the categorical variable list to the available variable list
        /// and sets their extended properties appropriately
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddInputVariable_Click(object sender, EventArgs e)
        {
            bool btn4 = sender.Equals((object)button4);

            List<ListItem> items = new List<ListItem>();

            if (btn4)
            {
                int selectedIndices = listBox2.SelectedIndices.Count;
                for (int i = 0; i < selectedIndices; i++)
                {
                    ListItem li = (ListItem)listBox2.Items[listBox2.SelectedIndices[i]];
                    items.Add(li);
                    _dtFull.Columns[li.ToString()].ExtendedProperties[VBTools.Globals.CATEGORICAL] = false;
                }

                foreach (ListItem li in items)
                {
                    listBox2.Items.Remove(li);
                    lbIndVariables.Items.Add(li);
                }

            }
        }

        /// <summary>
        /// Moves the selected variables from the available variable list to the categorical variable list
        /// and sets their extended properties appropriately
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemoveInputVariable_Click_1(object sender, EventArgs e)
        {
            bool btn3 = sender.Equals((object)button3);

            List<ListItem> items = new List<ListItem>();
            
            for (int i = 0; i < lbIndVariables.SelectedIndices.Count; i++)
            {
                ListItem li = (ListItem)lbIndVariables.Items[lbIndVariables.SelectedIndices[i]];
                items.Add(li);
            }
            if (btn3)
            {
                foreach (ListItem li in items)
                {
                    lbIndVariables.Items.Remove(li);

                    bool foundIdx = false;
                    int j = 0;
                    for (j = 0; j < listBox2.Items.Count; j++)
                    {
                        ListItem li2 = (ListItem)listBox2.Items[j];
                        if (Convert.ToInt32(li2.ValueItem) > Convert.ToInt32(li.ValueItem))
                        {
                            listBox2.Items.Insert(j, li);
 
                            foundIdx = true;
                            break;
                        }
                    }
                    if (foundIdx == false)
                        listBox2.Items.Insert(j, li);
                    _dtFull.Columns[li.ToString()].ExtendedProperties[VBTools.Globals.CATEGORICAL] = true;

                }
            }
        }

        /// <summary>
        /// Move a variable from the available variable list to the categorical variable list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            btnRemoveInputVariable_Click_1(sender, new EventArgs());
        }

        /// <summary>
        /// Move a variable from the categorical variable list to the available variable list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            btnAddInputVariable_Click(sender, new EventArgs());
        }

        /// <summary>
        /// Close the dialog - set ok
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Close the dialog - set cancel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

    }
}
