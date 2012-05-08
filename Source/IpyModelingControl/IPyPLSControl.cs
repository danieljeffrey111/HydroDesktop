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
using System.Globalization;
using VBTools;
using IPyModeling;

namespace IPyModelingControl
{
    public partial class IPyPLSControl : IPyModelingControl
    {

        public IPyPLSControl() : base()
        {
            InitializeComponent();
        }
        

        //Clear the control
        public new void Clear()
        {
            lvModel.Items.Clear();
            lblDecisionThreshold.Text = "";
            lblNcomp.Text = "";

            base.Clear();
        }


        //Enable or disable controls, then raise an event to do the same up the chain in the containing Form.
        private new void ChangeControlStatus(bool enable)
        {
            base.ChangeControlStatus(enable);
        }


		protected override void PopulateResults(dynamic model)
        {
            //Declare the variables we'll use in this routine
            ListViewItem lvi;
            string[] item;

            //Extract the variables and their coefficients from the model object
            List<string> names = ((IList<object>)model.Extract("names")).Cast<string>().ToList();
            List<double> coefficients = ((IList<object>)model.Extract("coef")).Cast<double>().ToList();

            //Run the Get_Influence method to get the relative influence (coefficient x standard dev.) of each variable
            dynamic dictInfluence = model.GetInfluence();
            List<string> listKeys = ((IList<object>)dictInfluence.keys()).Cast<string>().ToList();
            List<double> listInfluence = ((IList<object>)dictInfluence.values()).Cast<double>().ToList();

            //Clear the old list of model coefficients
            lvModel.Items.Clear();
            Dictionary<ListViewItem, double> dictUnorderedEntries = new Dictionary<ListViewItem, double>();

			//Make a list of each model parameter, its coefficient, and its influence.
            for(int i=0; i<names.Count; i++)
            {
                //int index = listKeys.FindIndex(arg => arg == names[i]);
                int index = listKeys.FindIndex(arg => String.Compare(arg, names[i], CultureInfo.InvariantCulture, CompareOptions.IgnoreSymbols) == 0);
                bool minor = false;
                double influence;

                //populate a row with the name, coefficient, and influence of this variable.
                item = new string[4];
                
                //index is -1 if the header wasn't found (e.g. Intercept)
                if (index > -1)
                {
                    influence = listInfluence[index];
                    item[0] = listKeys[index];
                    item[1] = String.Format("{0:F4}", coefficients[i]);
                    item[2] = String.Format("{0:F4}", influence);
                    if (influence <= 0.05) minor = true;
                }
                else
                {
                    //Set the influence for the Intercept to 2 (this won't be displayed).
                    //The others all sum to 1 so the Intercept is guaranteed to come first in the list.
                    item[0] = names[i];
                    item[1] = String.Format("{0:F4}", coefficients[i]);
                    item[2] = "";
                    influence = 2;
                }

                //Create a new ListViewItem, coloring it red if this variable is considered to have minor influence.
                lvi = new ListViewItem(item);
                if (minor == true)
                    lvi.ForeColor = Color.Red;

                //Add the ListViewItem to the Dictionary of entries, keyed by the influence.
                dictUnorderedEntries.Add(lvi, influence);
            }

            //Order by the negative of influence because default sort order is ascending and we want descending.
            foreach (KeyValuePair<ListViewItem, double> pair in dictUnorderedEntries.OrderBy(entry => -entry.Value))
            {
                ListViewItem entry = pair.Key;
                lvModel.Items.Add(entry);
            }

            //Now post the decision threshold and the number of PLS components
            lblDecisionThreshold.Text = String.Format("{0:F3}", UntransformThreshold(model.threshold));
            lblNcomp.Text = model.ncomp.ToString();
        }
	
    }
}
