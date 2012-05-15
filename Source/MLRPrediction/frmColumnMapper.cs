using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MLRPrediction
{
    public partial class frmColumnMapper : Form
    {
        //private Dictionary<string, string> _mainEffects = null;
        private string[] _mainEffects = null;
        private DataTable _dtMapped;
        private bool _idColumn;

        //The captions for the two columns in the mapping grid
        private string[] _headerCaptions;

        //The columns names in the imported datatable
        private List<string> _impColNames = null;

        public DataTable MappedTable
        {
            get { return _dtMapped; }
        }

       
        //public frmColumnMapper(Dictionary<string, string> mainEffects, DataTable dt, string[] headerCaptions, bool IDColumn)
        public frmColumnMapper(string[] mainEffects, DataTable dt, string[] headerCaptions, bool IDColumn)
        {
            InitializeComponent();
            _mainEffects = mainEffects;
            _dtMapped = dt.Copy();

            _headerCaptions = headerCaptions.ToArray();

            _impColNames = new List<string>();

            _idColumn = IDColumn;

            foreach (DataColumn dc in _dtMapped.Columns)
                _impColNames.Add(dc.ColumnName);

            
        }

        private void frmColumnMapper_Load(object sender, EventArgs e)
        {

            DataGridViewTextBoxColumn dgTextCol = new DataGridViewTextBoxColumn();
            dgTextCol.HeaderText = _headerCaptions[0];
            dgTextCol.Width = 220;

            DataGridViewComboBoxColumn dgComboCol = new DataGridViewComboBoxColumn();
            dgComboCol.HeaderText = _headerCaptions[1];
            dgComboCol.Width = 220;
            dgComboCol.Items.AddRange(_impColNames.ToArray());

            dataGridView1.Columns.Add(dgTextCol);
            dataGridView1.Columns.Add(dgComboCol);
            

            //Add the Main effect IVs to the grid.

            for(int i=0; i<_mainEffects.Length; i++)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[i].Cells[0].Value = _mainEffects[i];

                if (_dtMapped.Columns.Contains(_mainEffects[i]))
                    dataGridView1.Rows[i].Cells[1].Value = _mainEffects[i];
            }

            //default the id/obs col selections to first/second cols
            if(_idColumn)
            {
                dataGridView1.Rows[0].Cells[1].Value = _impColNames[0];
                //dataGridView1.Rows[1].Cells[1].Value = _impColNames[1];
            }
        }


        //Assign the correct main effect column names to the right imported dataset columns.
        private void btnOk_Click(object sender, EventArgs e)
        {
            int numRows = dataGridView1.Rows.Count;
            Dictionary<string, string> colMap = new Dictionary<string, string>(); 

            for (int i = 0; i < numRows; i++)
            {
                string me    = dataGridView1.Rows[i].Cells[0].Value as string;
                string idata = dataGridView1.Rows[i].Cells[1].Value as string;

                if ((String.IsNullOrEmpty(idata)) || (idata == ""))
                {
                    string msg = "Model variable {0} is not mapped to an imported data column.";
                    MessageBox.Show(String.Format(msg, me));                    
                    return;
                }

                colMap.Add(me, idata);                
            }

            DataTable dt = new DataTable();

            if (colMap.ContainsKey("ID"))
                dt.Columns.Add("ID", typeof(string));     

            foreach (string meKey in colMap.Keys)
            {
                if (String.Compare(meKey,"ID",true) != 0)                
                    dt.Columns.Add(meKey, typeof(double));                                                                      
            }

            //Populate the new data table with data from the old.
            for (int i = 0; i < _dtMapped.Rows.Count; i++)
            {
                DataRow dr = dt.NewRow();
                foreach (string meKey in colMap.Keys)
                {
                    if (String.Compare(meKey, "ID",true) == 0)
                        dr[meKey] = _dtMapped.Rows[i][colMap[meKey]].ToString();
                    else
                        dr[meKey] = _dtMapped.Rows[i][colMap[meKey]];
                }
                dt.Rows.Add(dr);
            }

            if (dt.Columns.Contains("ID"))
                dt.Columns["ID"].SetOrdinal(0);

            _dtMapped = dt;

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            //Not sure why this exception is getting thrown.  Might be because of non numeric data in test set.
            string msg = e.Exception.Message;
            e.ThrowException = false;
        }
    }
}
