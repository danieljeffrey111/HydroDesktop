using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VBTools;

namespace VBDatasheet
{
    public partial class frmPowerExponent : Form
    {
        private double _exp = double.NaN;
        private DataTable _dt = null;
        private int _cndx = -1;
        private double[] _v = null;
        private string _tmessage = string.Empty;


        /// <summary>
        /// class constructor needs the datatable and the index of the column
        /// in the table to operate on
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="cndx"></param>
        public frmPowerExponent(DataTable dt, int cndx)
        {
            InitializeComponent();
            _cndx = cndx;
            _dt = dt;
        }

        /// <summary>
        /// accessor to return a array of transformed values
        /// </summary>
        public double[] TransformedValues
        {
            set { _v = value; }
            get { return _v; }
        }
        public string TransformMessage
        {
            set { _tmessage = value; }
            get { return _tmessage; }
        }
        /// <summary>
        /// accessor to return the exponent used in the transform
        /// </summary>
        public double Exponent
        {
            get { return _exp; }
        }

        /// <summary>
        /// button go clicked, perform the calculation, save the data and go away
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton8.Checked && string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Must enter a value for exponent.", "Exponent Cannot be Blank", MessageBoxButtons.OK);
                return;
            }
            else if (!_exp.Equals(double.NaN))
            {
                Transform t = new Transform(_dt, _cndx, _exp);
                double[] v = t.POWER;
                _tmessage = t.Message;
                _v = v;
                this.DialogResult = DialogResult.OK;
                Close();
            }

        }

        /// <summary>
        /// button cancel clicked, go away
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        #region radio button maintenance for selection of one of the commonly used exponents
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked) _exp = -1.0d;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked) _exp = 2.0d;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked) _exp = 0.5d;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked) _exp = 3.0d;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton5.Checked) _exp = 1.0d / 3.0d;
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton6.Checked) _exp = 4.0d;
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton7.Checked) _exp = 0.25d;
        }
        #endregion

        /// <summary>
        /// user selected to enter a value for an exponent, validate it and save
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton8.Checked)
            {
                if (!string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    _exp = Convert.ToDouble(textBox1.Text);
                }
            }
        }


        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            if (!double.TryParse(textBox1.Text, out _exp))
            {
                e.Cancel = true;
                textBox1.Select(0, textBox1.Text.Length);
                this.errorProvider1.SetError(textBox1, "Text must convert to a number.");
            }
        }

        private void textBox1_Validated(object sender, EventArgs e)
        {
            errorProvider1.SetError(textBox1, "");
        }

    }
}
