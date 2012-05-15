using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZedGraph;
using VBTools;
using VBStatistics;

namespace VBDatasheet
{
    /// class to show plots of user selected datatable column.
    /// plots (all except frequency plot) are point enable/disable active
    /// meaning any point selected from a plot can be enabled/disabled
    /// in the datasheet grid.  zedgraph control is a master, plots are added
    /// to the master as seperate panes.  users can replot the column data
    /// subsequent to points enable/disable.  a table of column statistics
    /// is maintained and displayed.  form is not modal; users can open 
    /// multiple plot windows for any plottable column
    public partial class frmPlot : Form
    {
        //global for data 
        private DataTable _dt = null;
        //indicies for selected column and response variable
        private int _responsevarcol;
        private int _selectedcol;

        //plot information used in plot enable/diable row event handlers
        private GraphPane _gp = null;
        private CurveItem _curve = null;
        private int _ptndx = -1;
        private string _activeplot = string.Empty;

        //structure to help find related points in plots other than the one under interaction
        //contains date/time and double array of x value and y value of plot point
        private Dictionary<string, double[]> _dxy = null;

        //events/delegates to inform caller of enable/disable point (row) status.
        public delegate void PointDisableEventHandler(string tag);
        public event PointDisableEventHandler pointDisabled;
        public delegate void PointEnableEventHandler(string tag);
        public event PointEnableEventHandler pointEnabled;

        private double _regressionadstat;
        private double _regressionadpval;

        /// <summary>
        /// method initializs form, sets up context menus for plots
        /// and initializes globals
        /// </summary>
        /// <param name="responsevarcol">response variable column index</param>
        /// <param name="selectedcol">selected variable column index</param>
        /// <param name="dt">populated datatable from which other params are taken</param>
        public frmPlot(int responsevarcol, int selectedcol, DataTable dt)
        {
            InitializeComponent();
            this.Text = "Variable " + dt.Columns[selectedcol].Caption;
            this.WindowState = FormWindowState.Maximized;

            //add new items to the default right-click zedgraph context menu (plots info)
            zgc.ContextMenuBuilder += new ZedGraphControl.ContextMenuBuilderEventHandler(BoxWhiskerInfo);
            zgc.ContextMenuBuilder += new ZedGraphControl.ContextMenuBuilderEventHandler(TimeSeriesInfo);
            zgc.ContextMenuBuilder += new ZedGraphControl.ContextMenuBuilderEventHandler(ScatterPlotInfo);
            zgc.ContextMenuBuilder += new ZedGraphControl.ContextMenuBuilderEventHandler(FrequencyPlotInfo);

            _dt = dt.Copy();
            _responsevarcol = responsevarcol;
            _selectedcol = selectedcol;
            _dxy = new Dictionary<string, double[]>();

            renderPlots();

            showColInfo();

        }

        /// <summary>
        /// shows some stats for the selected variable
        /// </summary>
        private void showColInfo()
        {

            string varName = _dt.Columns[_selectedcol].Caption;
            var count = (from DataRow r in _dt.Rows select r[_selectedcol]).Count();
            var max = (from DataRow r in _dt.Rows select r[_selectedcol]).Max();
            var min = (from DataRow r in _dt.Rows select r[_selectedcol]).Min();
            var avg = (from DataRow r in _dt.Rows select r[_selectedcol]).Average(r => (double)r);
            var unique = (from DataRow r in _dt.Rows select r[_selectedcol]).Distinct().Count();
            var zerocount = (from DataRow r in _dt.Rows where (double)r[_selectedcol] == 0 select r[_selectedcol]).Count();

            double dmax = Convert.ToDouble(max);
            double dmin = Convert.ToDouble(min);

            AndersonDarlingNormality adtest = new AndersonDarlingNormality();
            double[] vals = (from DataRow r in _dt.Rows select (double)r[_selectedcol]).ToArray<double>();
            adtest.getADstat(vals);

            double norm = adtest.ADStat;
            double pvnorm = adtest.ADStatPval;

            DescriptiveStats ds = new DescriptiveStats();
            ds.getStats(vals);


            double median = ds.Median;//nv.Median;
            double range = ds.Range;// nv.Range;

            double mean = ds.Mean;// nv.Mean;
            double stddev = ds.StdDev;// nv.StandardDeviation;
            double variance = ds.Variance;// nv.Variance;
            double kurtosis = ds.Kurtosis; //nv.Kurtosis;
            double skewness = ds.Kurtosis; //nv.Skewness;


            listView1.View = View.Details;

            ListViewItem lvi;
            if (listView1.Items.Count > 0)
            {
                lvi = new ListViewItem("*****");
                lvi.SubItems.Add("");
                listView1.Items.Add(lvi);
            }


            lvi = new ListViewItem("Variable Name");
            lvi.SubItems.Add(varName);
            listView1.Items.Add(lvi);

            lvi = new ListViewItem("Row Count");
            lvi.SubItems.Add(count.ToString());
            listView1.Items.Add(lvi);

            lvi = new ListViewItem("Maximum Value");
            lvi.SubItems.Add(dmax.ToString("n2"));
            listView1.Items.Add(lvi);

            lvi = new ListViewItem("Minimum Value");
            lvi.SubItems.Add(dmin.ToString("n2"));
            listView1.Items.Add(lvi);

            lvi = new ListViewItem("Average Value");
            lvi.SubItems.Add(avg.ToString("n2"));
            listView1.Items.Add(lvi);

            lvi = new ListViewItem("Unique Values");
            lvi.SubItems.Add(unique.ToString());
            listView1.Items.Add(lvi);

            lvi = new ListViewItem("Zero Count");
            lvi.SubItems.Add(zerocount.ToString());
            listView1.Items.Add(lvi);

            lvi = new ListViewItem("Median Value");
            lvi.SubItems.Add(median.ToString("n3"));
            listView1.Items.Add(lvi);

            lvi = new ListViewItem("Data Range");
            lvi.SubItems.Add(range.ToString("n3"));
            listView1.Items.Add(lvi);

            lvi = new ListViewItem("");
            lvi.SubItems.Add("");
            listView1.Items.Add(lvi);

            lvi = new ListViewItem("AD Statistic");
            lvi.SubItems.Add(_regressionadstat.ToString("n4"));
            //lvi.SubItems.Add(norm.ToString("n3"));
            listView1.Items.Add(lvi);

            lvi = new ListViewItem("AD Stat P-Value");
            lvi.SubItems.Add(_regressionadpval.ToString("n4"));
            //lvi.SubItems.Add(pvnorm.ToString("n3"));
            listView1.Items.Add(lvi);

            lvi = new ListViewItem("Mean Value");
            lvi.SubItems.Add(mean.ToString("n3"));
            listView1.Items.Add(lvi);

            lvi = new ListViewItem("Standard Deviation");
            lvi.SubItems.Add(stddev.ToString("n3"));
            listView1.Items.Add(lvi);

            lvi = new ListViewItem("Variance");
            lvi.SubItems.Add(variance.ToString("n3"));
            listView1.Items.Add(lvi);

            lvi = new ListViewItem("Kurtosis");
            lvi.SubItems.Add(kurtosis.ToString("n3"));
            listView1.Items.Add(lvi);

            lvi = new ListViewItem("Skewness");
            lvi.SubItems.Add(skewness.ToString("n3"));
            listView1.Items.Add(lvi);


            //magic numbers for widths: -1 set to max characters in subitems, -2 == autosize
            listView1.Columns.Add("Data", -1, HorizontalAlignment.Right);
            listView1.Columns.Add("Value", -2, HorizontalAlignment.Left);
        }

        /// <summary>
        /// creates a zg master, individual plot panes and displays the plots
        /// </summary>
        private void renderPlots()
        {
            MasterPane master = zgc.MasterPane;
            master.PaneList.Clear();

            master.Title.IsVisible = false;
            master.Margin.All = 10;

            //pass a datatable column index to extract data to arrays
            double[] response = getValues<double>(_responsevarcol);
            double[] iv = getValues<double>(_selectedcol);
            string[] tags = getValues<string>(0);

            //build a structure with point information - usefull for graph-point/datatable-row manipulation
            _dxy = buildPoints(tags, iv, response);

            //build and add plots to the master pane
            GraphPane gpXY = addPlotXY(response, iv, tags);
            GraphPane gpTS = addPlotTS(iv, tags);
            GraphPane gpBW = addPlotBW(iv, tags);
            //GraphPane gpSV = addPlotSV(iv, tags);
            GraphPane gpFQ = addPlotFQ(iv);

            master.Add(gpXY);
            master.Add(gpTS);
            master.Add(gpBW);
            //master.Add(gpSV);
            master.Add(gpFQ);

            using (Graphics g = this.CreateGraphics())
            { master.SetLayout(g, PaneLayout.SquareColPreferred); }

            zgc.IsShowPointValues = true;
            zgc.AxisChange();
            master.AxisChange();
            zgc.Refresh();
        }

        /// <summary>
        /// populates a date, x,y[] information structure
        /// </summary>
        /// <param name="tags">date or string (from col 0 in the datattable)</param>
        /// <param name="iv">double x (from the col[selectedvariable] in datatable)</param>
        /// <param name="response">double y (from col[responsevariable] in datatable)</param>
        /// <returns></returns>
        private Dictionary<string, double[]> buildPoints(string[] tags, double[] iv, double[] response)
        {

            Dictionary<string, double[]> retDic = new Dictionary<string, double[]>();

            for (int i = 0; i < tags.Length; i++)
            {
                double[] xy = new double[2];
                xy[0] = iv[i];
                xy[1] = response[i];
                retDic.Add(tags[i], xy);
            }
            return retDic;
        }

        #region region - methods create custom plot panes

        //private GraphPane addPlotSV(double[] iv, string[] tags)
        //{
        //    DateTime date;
        //    bool dateAxis = false;
        //    //SortedList<string, double> sl = getlist(tags, iv);

        //    SortedList<double, string> sl = getSVList(iv, tags);

        //    //ArrayList sl = getaList(iv, tags);

        //    PointPairList ppl = new PointPairList();
        //    string tag = string.Empty;
        //    //might be a smarter way to add tags to points but I can't find it.
        //    for (int i = 0; i < sl.Count; i++)
        //    {
        //        tag = "(" + iv[i].ToString("n2") + " , " + tags[i] + ") ";
        //        if (dateAxis)
        //        {

        //            DateTime d = Convert.ToDateTime(tags[i]);
        //            //ppl.Add((XDate)d, iv[i], d.ToShortDateString());
        //            ppl.Add((XDate)d, iv[i], tag);
        //        }
        //        else
        //        {
        //            //double n = Convert.ToDouble(tags[i]);
        //            ppl.Add(i, sl.Keys[i], sl.Values[i]);
        //            //ppl.Add(sl.Keys[i], sl.Values[i].ToString());
        //        }
        //    }
        //    GraphPane gp = new GraphPane();
        //    LineItem curve = gp.AddCurve(_dt.Columns[_selectedcol].ColumnName, ppl, Color.Blue);
        //    curve.Line.IsVisible = true;
        //    //if (dateAxis) gp.XAxis.Type = AxisType.Date;
        //    gp.XAxis.Type = AxisType.Linear;
        //    gp.XAxis.Title.Text = _dt.Columns[0].ColumnName;
        //    gp.YAxis.Title.Text = _dt.Columns[_selectedcol].ColumnName;

        //    gp.Tag = "SVPlot";
        //    gp.Title.Text = "Sorted Values Plot";

        //    return gp;
        //}

        /// <summary>
        /// create a time series plot of selected variable
        /// </summary>
        /// <param name="iv">selected variable</param>
        /// <param name="tags">tags for points - should be a date/timestamp</param>
        /// <returns></returns>
        private GraphPane addPlotTS(double[] iv, string[] tags)
        {
            DateTime date;
            bool dateAxis = false;
            if (IsDate(tags[0], out date)) dateAxis = true;

            PointPairList ppl = new PointPairList();
            string tag = string.Empty;
            //might be a smarter way to add tags to points but I can't find it.
            for (int i = 0; i < iv.Length; i++)
            {
                //tag = "(" + iv[i].ToString("n2") + " , " + tags[i] + ") ";
                tag = tags[i];
                if (dateAxis)
                {

                    DateTime d = Convert.ToDateTime(tags[i]);
                    ppl.Add((XDate)d, iv[i], tag);
                }
                else
                {
                    double n = Convert.ToDouble(tags[i]);
                    ppl.Add(n, iv[i], tag);
                }
            }
            GraphPane gp = new GraphPane();
            LineItem curve = gp.AddCurve(_dt.Columns[_selectedcol].ColumnName, ppl, Color.Blue);
            curve.Line.IsVisible = true;
            if (dateAxis) gp.XAxis.Type = AxisType.Date;
            else gp.XAxis.Type = AxisType.Linear;
            gp.XAxis.Title.Text = _dt.Columns[0].ColumnName;
            gp.YAxis.Title.Text = _dt.Columns[_selectedcol].ColumnName;

            gp.Tag = "TSPlot";
            gp.Title.Text = "Time Series Plot";

            return gp;
        }

        /// <summary>
        /// create a scatter plot of dependent variable and selected variable
        /// </summary>
        /// <param name="response">response variable values</param>
        /// <param name="iv">selected independent variable valuess</param>
        /// <param name="tags">tags for points - should be date/timestamp</param>
        /// <returns></returns>
        private GraphPane addPlotXY(double[] response, double[] iv, string[] tags)
        {
            PointPairList ppl = new PointPairList();
            string tag = string.Empty;
            //might be a smarter way to add tags to points but I can't find it.
            for (int i = 0; i < response.Length; i++)
            {
                //tag = "(" + iv[i].ToString("n2") + " , " + response[i].ToString("n2") + ") " + tags[i];
                tag = tags[i];
                ppl.Add(iv[i], response[i], tag);
            }

            GraphPane gp = new GraphPane();

            if (_dt.Rows.Count > 6)
            {
                //mc wants a regression line and some new stats on the plot
                double pcoeff = Statistics.Correlation(response, iv);
                double pval = Statistics.Pvalue4Correlation(pcoeff, _dt.Rows.Count);

                string[] ivname = new string[] { _dt.Columns[_selectedcol].Caption };
                VBStatistics.MultipleRegression mlr = new MultipleRegression(_dt, _dt.Columns[_responsevarcol].Caption, ivname);
                mlr.Compute();

                //andersondarling stats for the regression
                _regressionadstat = mlr.ADResidNormStatVal;
                _regressionadpval = mlr.ADResidPvalue;

                PointPairList ppl2 = new PointPairList(iv, mlr.PredictedValues);
                string fmtstring = formatNumberString(pval);
                fmtstring = "r = {0:f4}, P-Value = " + fmtstring;
                string annot = string.Format(fmtstring, pcoeff, pval);
                //string annot = string.Format("r = {0:f4}, p-value = {1:e4}", pcoeff, pval);
                LineItem curve2 = gp.AddCurve(annot, ppl2, Color.Red);
                curve2.Symbol.IsVisible = false;
                curve2.Line.IsVisible = true;
            }
            //end new stuff

            //GraphPane gp = new GraphPane();
            LineItem curve = gp.AddCurve(_dt.Columns[_selectedcol].ColumnName, ppl, Color.Black);
            curve.Line.IsVisible = false;
            gp.XAxis.Title.Text = _dt.Columns[_selectedcol].ColumnName;
            gp.YAxis.Title.Text = _dt.Columns[_responsevarcol].ColumnName;

            gp.Tag = "XYPlot";
            gp.Title.Text = "Scatter Plot";


            return gp;
        }

        private string formatNumberString(double number)
        {
            string fmtstring = string.Empty;
            if (number.Equals(double.NaN)) return fmtstring;
            if (Math.Abs(number) >= 10000 || Math.Abs(number) <= 0.0001)
                fmtstring = "{1:e4}";
            else fmtstring = "{1:f4}";
            //return string.Format(fmtstring, number);
            return fmtstring;
        }

        /// <summary>
        /// a hacked mess to show box-wisker plot for outlier manipulation
        /// </summary>
        /// <param name="iv">double array of independent variable values</param>
        /// <param name="tags">string array of what should be date/time tags</param>
        /// <returns>a graph pane to display in the master</returns>
        private GraphPane addPlotBW(double[] iv, string[] tags)
        {
            //http://www.sharpstatistics.co.uk/index.php?option=com_content&view=article&id=12&Itemid=13
            //and hacked by mog 4/1/11

            //For each set of data for the boxplot calculate the median and the inter quartile (IQR) range 
            //which is just the value of the 75th percentile minus the 25th percentile. The median is where 
            //the horizontal bar goes. Using the 25th and 75th percentile a HiLowBarIten can be set which is 
            //just a bar where the top and base are specified. 

            //An error bar that has upper and lower values of the 25th percentile minus 1.5 times the IQR and 
            //the 75th percentile plus 1.5 times the IQR is set to give the whiskers. As with the HiLowItem 
            //barList is a PointPairList with the high and low values.

            //All that is left to do is add any points that are above or below the end of the whiskers.

            //Dictionary<string, double> datevalue = getdic(tags, iv);
            SortedList<string, double> datevalue = getlist(tags, iv);

            //SortedList<double, string> valuedate = getvaluedate(iv, tags);



            GraphPane gp = new GraphPane();

            //gp.Title.Text = "Box encompasses 25th to 75th percentile of data points.\n" +
            //    "Median designated within box.\n" +
            //    "Whiskers are +- 1.5 * IQR (IQR = 75th percentile - 25th percentile)\n" +
            //    "Points above/below whiskers are designated as outliers.";

            //median of each array
            PointPairList medians = new PointPairList();
            //75th and 25th percentile, defines the box
            PointPairList hiLowList = new PointPairList();
            //+/- 1.5*Interquartile range, extentent of wiskers
            PointPairList barList = new PointPairList();
            //outliers
            PointPairList outs = new PointPairList();

            //Add the values
            //medians.Add(CSStatistics.BasicStats.Median(iv));
            DescriptiveStats ds = new DescriptiveStats();
            ds.getStats(iv);
            double median = ds.Median;
            medians.Add(0, median);
            double hivalue = percentile(iv, 75);
            double lovalue = percentile(iv, 25);
            //hiLowList.Add(percentile(iv, 75), percentile(iv, 25));
            //hiLowList.Add(hivalue, lovalue);
            hiLowList.Add(0, hivalue, lovalue);
            //hiLowList.Add(1, lovalue);
            //double iqr = 1.5 * (percentile(iv, 75) - percentile(iv, 25));
            double iqr = 1.5 * (hivalue - lovalue);
            //double upperLimit = percentile(iv, 75) + iqr;
            double upperLimit = hivalue + iqr;
            //double lowerLimit = percentile(iv, 25) - iqr;
            double lowerLimit = lovalue - iqr;
            //The wiskers must end on an actual data point
            double wiskerlo = ValueNearestButGreater(iv, lowerLimit);
            double wiskerhi = ValueNearestButLess(iv, upperLimit);
            //barList.Add(ValueNearestButGreater(iv, lowerLimit), ValueNearestButLess(iv, upperLimit));
            barList.Add(0, wiskerlo, wiskerhi);

            ////Sort out the outliers
            //foreach (double aValue in iv)
            //{
            //    if (aValue > upperLimit)
            //    {
            //        outs.Add(0, aValue);
            //    }
            //    if (aValue < lowerLimit)
            //    {
            //        outs.Add(0, aValue);
            //    }
            //}

            var upperouts = (from kv in datevalue
                             where kv.Value > upperLimit
                             select kv);
            foreach (var v in upperouts) outs.Add(0, v.Value, v.Key);

            var lowerouts = (from kv in datevalue
                             where kv.Value < lowerLimit
                             select kv);
            foreach (var v in lowerouts) outs.Add(0, v.Value, v.Key);


            //Plot the items, first the median values
            CurveItem meadian = gp.AddCurve("", medians, Color.Black, SymbolType.HDash);
            LineItem myLine = (LineItem)meadian;
            myLine.Line.IsVisible = false;
            myLine.Symbol.Fill.Type = FillType.Solid;

            //Box
            HiLowBarItem myCurve = gp.AddHiLowBar("", hiLowList, Color.Black);
            myCurve.Bar.Fill.Type = FillType.None;

            //Wiskers
            ErrorBarItem myerror = gp.AddErrorBar("", barList, Color.Black);

            //Outliers
            CurveItem upper = gp.AddCurve(_dt.Columns[_selectedcol].ColumnName + " outliers", outs, Color.Green, SymbolType.Circle);
            LineItem bLine = (LineItem)upper;
            //bLine.Symbol.Size = 5;
            bLine.Line.IsVisible = false;

            gp.YAxis.Title.Text = _dt.Columns[_selectedcol].ColumnName;
            gp.BarSettings.Type = BarType.Overlay;
            gp.XAxis.IsVisible = false;
            gp.Legend.IsVisible = true;
            //gp.Title.Text = _dt.Columns[_selectedcol].ColumnName;

            gp.Tag = "BWPlot";
            gp.Title.Text = "BoxWhisker Plot";

            //ToolTip tt = new ToolTip();
            //tt.AutoPopDelay = 15000;
            //tt.InitialDelay = 2000;
            //tt.ReshowDelay = 8000;
            //tt.UseFading = true;
            //tt.SetToolTip(this.zgc,"HHHHHHHEEELLLLPPPPPP!");

            return gp;
        }

        /// <summary>
        /// create a frequency plot of the selected variable; uses 9 categories
        /// </summary>
        /// <param name="iv">double array of independent variable values</param>
        /// <param name="tags"></param>
        /// <returns>graph pane to display in the master pane</returns>
        private GraphPane addPlotFQ(double[] iv)
        {
            //ncats is the number of categories
            const int ncats = 9;

            VBStatistics.Frequency counts = new VBStatistics.Frequency();

            counts.getHist(iv, ncats);
            double[] c = counts.Counts;
            double[] center = counts.Center;

            GraphPane gp = new GraphPane();
            PointPairList ppl = new PointPairList();
            ppl.Add(center, c);
            BarItem bar = gp.AddBar(_dt.Columns[_selectedcol].ColumnName, ppl, Color.Black);
            gp.XAxis.Title.Text = _dt.Columns[_selectedcol].ColumnName;
            gp.YAxis.Title.Text = "Count";

            gp.Title.Text = "Frequency Plot";
            gp.Tag = "FREQPlot";

            return gp;


        }

        #endregion

        /// <summary>
        /// used for selecting outliers for the BW plot
        /// </summary>
        /// <param name="tags"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        private SortedList<string, double> getlist(string[] tags, double[] iv)
        {
            SortedList<string, double> list = new SortedList<string, double>();
            for (int i = 0; i < tags.Length; i++)
                list.Add(tags[i], iv[i]);

            return list;
        }

        //private SortedList<double, string> getSVList(double[] iv, string[] tags)
        //{
        //    SortedList<double, string> list = new SortedList<double, string>();
        //    for (int i = 0; i < iv.Length; i++)
        //        if (! list.Keys.Contains(iv[i])) list.Add(iv[i], tags[i]);

        //    return list;
        //}

        /// <summary>
        /// used for extracting columns of data from the datatable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="colndx"></param>
        /// <returns></returns>
        private T[] getValues<T>(int colndx)
        {
            //try
            //{
            //    var values = (from row in _dt.Select()
            //                  select row.Field<T>(colndx)).ToArray<T>();
            //    return values;
            //}

            //catch (InvalidCastException)
            //{
            //    var values = (from row in _dt.Select()
            //                  select row[colndx]).Cast<string>().ToArray();
            //    return values.Cast<T>().ToArray();
            //}

            string t = typeof(T).ToString();
            switch (t)
            {
                case "System.Double":
                    var dvalues = (from row in _dt.Select()
                                   select row.Field<T>(colndx)).ToArray<T>();
                    return dvalues;
                case "System.String":
                    //var svalues = (from row in _dt.Select()
                    //          select row[colndx]).Cast<string>().ToArray();
                    //return svalues.Cast<T>().ToArray();
                    string[] svalues = new string[_dt.Rows.Count];
                    for (int r = 0; r < _dt.Rows.Count; r++)
                    {
                        svalues[r] = _dt.Rows[r][colndx].ToString();
                    }
                    return svalues.Cast<T>().ToArray();
                default:
                    return null;
            }

        }

        /// <summary>
        /// determines if the passed string can be parsed into a date
        /// used by the ts plot to set appropriate x axis type
        /// </summary>
        /// <param name="anyString"></param>
        /// <param name="resultDate"></param>
        /// <returns></returns>
        private bool IsDate(string anyString, out DateTime resultDate)
        {
            bool isDate = true;

            if (anyString == null)
            {
                anyString = "";
            }
            try
            {
                resultDate = DateTime.Parse(anyString);
            }
            catch
            {
                resultDate = DateTime.MinValue;
                isDate = false;
            }

            return isDate;
        }

        #region region - create/control plot event handlers for point manipulation

        /// <summary>
        /// acts as a controller for point manipulation on the BW plot
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void zgc_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ZedGraphControl zg = (ZedGraphControl)sender;
            GraphPane pane = zg.MasterPane.FindPane(e.Location);

            if (pane == null) return;

            //if (pane.Tag.ToString() != "BWPlot") return;
            //if (pane.Tag.ToString() != "BWPlot" && pane.Tag.ToString() != "TSPlot") return;
            //crap - mike now wants the scattplot enabled as well...
            if (pane.Tag.ToString() != "BWPlot" && pane.Tag.ToString() != "TSPlot" && pane.Tag.ToString() != "XYPlot") return;
            _activeplot = pane.Tag.ToString();

            //set the pane, curve and point for use by eventhandlers
            _gp = pane;

            CurveItem curve;
            int pointndx;
            if (!pane.FindNearestPoint(e.Location, out curve, out pointndx)) return;

            _curve = curve;
            _ptndx = pointndx;


            PointPair pt = curve.Points[pointndx];
            //boxwhisker plot points do not all have tags - go away if you find one
            if (string.IsNullOrWhiteSpace((string)pt.Tag)) return;
            string tag = pt.Tag.ToString();

            //build and show context menu items to enable/disable rows in the data sheet
            ContextMenu menuForPlot = new ContextMenu();
            MenuItem disable = new MenuItem("Disable Row containing " + tag, new EventHandler(disableRow));
            MenuItem enable = new MenuItem("Enable Row containing " + tag, new EventHandler(enableRow));
            menuForPlot.MenuItems.Add(disable);
            menuForPlot.MenuItems.Add(enable);

            //enable/disable the menu items appropriately (if disabled point, enable enable menu item... and conversely)
            CurveList cl1 = zgc.MasterPane.PaneList[2].CurveList;
            CurveList cl2 = zgc.MasterPane.PaneList[1].CurveList;
            if (cl1.Contains(zgc.MasterPane.PaneList[2].CurveList[tag]) || cl2.Contains(zgc.MasterPane.PaneList[2].CurveList[tag]))
            {
                enable.Enabled = true;
                disable.Enabled = false;
            }
            else
            {
                enable.Enabled = false;
                disable.Enabled = true;
            }

            //show the enable/disable contextmenu items
            menuForPlot.Show(zgc, new Point(e.X, e.Y));

        }

        /// <summary>
        /// event handler for BW plot point-select disable 
        /// adds point curves with 1 point to all plots 
        /// and paints them red - these are tracked by their labels
        /// for removal by the enable point event handler
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void disableRow(object o, EventArgs e)
        {
            //tag is different if it comes from 
            //ts plot (value, datetimestamp) 
            //bw plot (datetimestamp)
            int count = 0;

            //add a disabled point curve to the active plot
            PointPair pt = _curve.Points[_ptndx];
            PointPairList ppl = new PointPairList();
            ppl.Add(pt.X, pt.Y, pt.Tag.ToString());
            LineItem curve2 = _gp.AddCurve(null, ppl, Color.Red, SymbolType.Circle);
            curve2.Symbol.Fill = new Fill(Color.Firebrick);
            curve2.Label.Text = pt.Tag.ToString();

            //add a disabled point curve to the xy plot
            if (_activeplot != "XYPlot")
            {
                ppl = getPPL(pt.Tag.ToString(), "XYPlot");
                zgc.MasterPane.PaneList[0].AddCurve("", ppl, Color.Red, SymbolType.Square);
                count = zgc.MasterPane.PaneList[0].CurveList.Count - 1;
                LineItem curve = (LineItem)zgc.MasterPane.PaneList[0].CurveList[count];
                curve.Symbol.Fill = new Fill(Color.Firebrick);
                curve.Label.Text = pt.Tag.ToString();
            }

            //add a disabled curve to the ts plot
            if (_activeplot != "TSPlot")
            {
                ppl = getPPL(pt.Tag.ToString(), "TSPlot");
                zgc.MasterPane.PaneList[1].AddCurve("", ppl, Color.Red, SymbolType.Square);
                count = zgc.MasterPane.PaneList[1].CurveList.Count - 1;
                LineItem curve3 = (LineItem)zgc.MasterPane.PaneList[1].CurveList[count];
                curve3.Symbol.Fill = new Fill(Color.Firebrick);
                curve3.Label.Text = pt.Tag.ToString();
            }

            //add a disabled point curve to the box-whisker plot
            if (_activeplot != "BWPlot")
            {
                ppl = getPPL(pt.Tag.ToString(), "BWPlot");
                zgc.MasterPane.PaneList[2].AddCurve("", ppl, Color.Red, SymbolType.Circle);
                count = zgc.MasterPane.PaneList[2].CurveList.Count - 1;
                LineItem curve4 = (LineItem)zgc.MasterPane.PaneList[2].CurveList[count];
                curve4.Symbol.Fill = new Fill(Color.Firebrick);
                curve4.Label.Text = pt.Tag.ToString();
            }



            zgc.Refresh();
            zgc.AxisChange();

            //just fire off event to subscribers
            //this tells the subscriber to disable the row containing the point
            //by passing the point tag which should be the date/time stamp

            if (this.pointDisabled != null)
                this.pointDisabled(pt.Tag.ToString());


        }

        /// <summary>
        /// event handler for BW plot point-select enable 
        /// removes point curves from all plots that have
        /// the globally defined selected point point index
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void enableRow(object o, EventArgs e)
        {
            //remove the pane curve that signifies a disabled point from active plot
            PointPair pt = _curve.Points[_ptndx];
            string tag = pt.Tag.ToString();
            _gp.CurveList.Remove(_gp.CurveList[tag]);


            //scatterplot
            zgc.MasterPane.PaneList[0].CurveList.Remove(zgc.MasterPane.PaneList[0].CurveList[tag]);
            //timeseriesplot
            if (_activeplot != "TSPlot")
                zgc.MasterPane.PaneList[1].CurveList.Remove(zgc.MasterPane.PaneList[1].CurveList[tag]);
            //boxwhiskerplot
            if (_activeplot != "BWPlot")
                zgc.MasterPane.PaneList[2].CurveList.Remove(zgc.MasterPane.PaneList[2].CurveList[tag]);


            zgc.AxisChange();
            zgc.Refresh();

            ////enable row list maintenance
            //string dt = extractDateTime(tag);
            //if (_ptsDisable.Contains(dt))
            //{
            //    _ptsDisable.Remove(dt);
            //}
            //if (!_ptsEnable.Contains(dt))
            //{
            //    _ptsEnable.Add(dt);
            //}

            //this tells the subscriber to enable the row containing the point
            //by passing the point tag which should be the date/time stamp
            if (this.pointEnabled != null)
                this.pointEnabled(pt.Tag.ToString());
        }

        #endregion

        /// <summary>
        /// pass a key (date/timestamp) and a plot to update, get a point list for that plot
        /// used for enableing/disabling points from the BWplot and reflecting those points 
        /// in the other plots.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="plotname"></param>
        /// <returns>list of points for specified plot</returns>
        private PointPairList getPPL(string key, string plotname)
        {

            PointPairList retppl = new PointPairList();
            double[] xy = new double[2];

            if (plotname == "XYPlot")
            {
                if (!_dxy.TryGetValue(key, out xy)) return null;
                double x = xy[0];
                double y = xy[1];
                retppl.Add(x, y, key);
            }
            else if (plotname == "TSPlot")
            {
                if (!_dxy.TryGetValue(key, out xy)) return null;
                double x = xy[0];

                DateTime date;
                if (IsDate(key, out date))
                {
                    DateTime d = Convert.ToDateTime(key);
                    retppl.Add((XDate)d, x, key);
                }
                else
                {
                    retppl.Add(Convert.ToDouble(key), x, key);
                }
            }
            else if (plotname == "BWPlot")
            {
                if (!_dxy.TryGetValue(key, out xy)) return null;
                //double x = 1;
                double y = xy[0];
                retppl.Add(0, y, key);
                //outs.Add(0, v.Value, v.Key)

            }
            else { return null; }
            return retppl;
        }

        //private string extractDateTime(string tag)
        //{
        //    //string[] s = tag.Split(')');
        //    //string d = s[1].Trim();
        //    //return d;
        //    string[] s = tag.Split(',');
        //    string d = s[1].Replace(')',' ');
        //    d = d.Trim();
        //    return d;

        //}

        #region region: lots of hooha for the boxwhisker plot

        /// <summary>
        /// used by boxwhisker plot for lower whisker
        /// </summary>
        /// <param name="data"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        private double ValueNearestButLess(double[] data, double number)
        {
            var lowNums = from n in data where n <= number select n;
            return lowNums.Max();
        }

        /// <summary>
        /// used by boxwhisker plot for upper whisker
        /// </summary>
        /// <param name="data"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        private double ValueNearestButGreater(double[] data, double number)
        {
            var lowNums = from n in data where n >= number select n;
            return lowNums.Min();
        }

        /// <summary>
        /// used by boxwhisker plot for data partioning into quartiles
        /// </summary>
        /// <param name="Data">array of doubles to partition</param>
        /// <param name="p">partion (quartile; 25, 75, etc)</param>
        /// <returns></returns>
        private double percentile(double[] Data, double p)
        {
            //http://www.codeproject.com/KB/recipes/DescriptiveStatisticClass.aspx
            Array.Sort(Data);
            if (p >= 100.0d) return Data[Data.Length - 1];
            double position = (double)(Data.Length + 1) * p / 100.0;
            double leftNumber = 0.0d, rightNumber = 0.0d;
            double n = p / 100.0d * (Data.Length - 1) + 1.0d;
            if (position >= 1)
            {
                leftNumber = Data[(int)System.Math.Floor(n) - 1];
                rightNumber = Data[(int)System.Math.Floor(n)];
            }
            else
            {
                leftNumber = Data[0]; // first data
                rightNumber = Data[1]; // first data
            }
            if (leftNumber == rightNumber)
                return leftNumber;
            else
            {
                double part = n - System.Math.Floor(n);
                return leftNumber + part * (rightNumber - leftNumber);
            }
        }

        #endregion

        #region region - event handlers for custom modified zg context menus - just shows info about plots

        /// <summary>
        /// event handler for the custom zedgraph contextmenu item BoxWhiskerInfo
        /// </summary>
        /// <param name="control"></param>
        /// <param name="menuStrip"></param>
        /// <param name="mousePt"></param>
        /// <param name="objState"></param>
        private void BoxWhiskerInfo(ZedGraphControl control, ContextMenuStrip menuStrip, Point mousePt,
            ZedGraphControl.ContextMenuObjectState objState)
        {
            // create a new menu item
            ToolStripMenuItem item = new ToolStripMenuItem();
            // This is the user-defined Tag so you can find this menu item later if necessary
            item.Name = "BoxWhiskerPlotInfo";
            item.Tag = "BoxWhiskerPlotInfo";
            // This is the text that will show up in the menu
            item.Text = "Box Whisker Plot Info";
            // Add a handler that will respond when that menu item is selected
            item.Click += new System.EventHandler(ShowBoxWhiskerPlotInfo);
            // Add the menu item to the menu
            menuStrip.Items.Insert(menuStrip.Items.Count, item);

        }

        /// <summary>
        /// event handler for when the custom zedgraph contextmenu item BoxWhiskerInfo is clicked
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void ShowBoxWhiskerPlotInfo(object o, EventArgs e)
        {
            string s = "Box encompasses 25th to 75th percentile of data points.\n" +
                "Median designated within box.\n" +
                "Whiskers are +- 1.5 * IQR (IQR = 75th percentile - 25th percentile)\n" +
                "(the nearest actual datapoint to +- 1.5 * IRQ).\n" +
                "Points above/below whiskers are designated as outliers.";
            MessageBox.Show(s, "BoxWhisker Plot", MessageBoxButtons.OK);

        }

        /// <summary>
        /// method adds TS plot information item to the default zedgraph menu
        /// </summary>
        /// <param name="control"></param>
        /// <param name="menuStrip"></param>
        /// <param name="mousePt"></param>
        /// <param name="objState"></param>
        private void TimeSeriesInfo(ZedGraphControl control, ContextMenuStrip menuStrip, Point mousePt,
            ZedGraphControl.ContextMenuObjectState objState)
        {
            // create a new menu item
            ToolStripMenuItem item = new ToolStripMenuItem();
            // This is the user-defined Tag so you can find this menu item later if necessary
            item.Name = "TimeSeriesPlotInfo";
            item.Tag = "TimeSeriesPlotInfo";
            // This is the text that will show up in the menu
            item.Text = "Time Series Plot Info";
            // Add a handler that will respond when that menu item is selected
            item.Click += new System.EventHandler(ShowTimeSeriesPlotInfo);
            // Add the menu item to the menu
            menuStrip.Items.Insert(menuStrip.Items.Count, item);
        }

        /// <summary>
        /// method show the timeseries plot info when menu item selected
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void ShowTimeSeriesPlotInfo(object o, EventArgs e)
        {
            string s = "Time series plot shows the selected variable values\n" +
               "plotted against the date or date-timestamp values in the\n" +
               "first column of the imported data";
            MessageBox.Show(s, "TimeSeries Plot", MessageBoxButtons.OK);
        }

        /// <summary>
        /// method adds scatterplot information item to the default zedgraph menu
        /// </summary>
        /// <param name="control"></param>
        /// <param name="menuStrip"></param>
        /// <param name="mousePt"></param>
        /// <param name="objState"></param>
        private void ScatterPlotInfo(ZedGraphControl control, ContextMenuStrip menuStrip, Point mousePt,
            ZedGraphControl.ContextMenuObjectState objState)
        {
            // create a new menu item
            ToolStripMenuItem item = new ToolStripMenuItem();
            // This is the user-defined Tag so you can find this menu item later if necessary
            item.Name = "ScatterPlotInfo";
            item.Tag = "ScatterPlotInfo";
            // This is the text that will show up in the menu
            item.Text = "Scatter Plot Info";
            // Add a handler that will respond when that menu item is selected
            item.Click += new System.EventHandler(ShowScatterPlotInfo);
            // Add the menu item to the menu
            menuStrip.Items.Insert(menuStrip.Items.Count, item);
        }

        /// <summary>
        /// method shows scatterplot information when menu item selected
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void ShowScatterPlotInfo(object o, EventArgs e)
        {
            string s = "Scatter plot shows the selected variable values\n" +
                "plotted against the default or user-select response variable\n" +
                "values contained in the imported data";
            MessageBox.Show(s, "TimeSeries Plot", MessageBoxButtons.OK);
        }

        /// <summary>
        /// method adds frequency plot information items to the zedgraph menu items
        /// </summary>
        /// <param name="control"></param>
        /// <param name="menuStrip"></param>
        /// <param name="mousePt"></param>
        /// <param name="objState"></param>
        private void FrequencyPlotInfo(ZedGraphControl control, ContextMenuStrip menuStrip, Point mousePt,
            ZedGraphControl.ContextMenuObjectState objState)
        {
            // create a new menu item
            ToolStripMenuItem item = new ToolStripMenuItem();
            // This is the user-defined Tag so you can find this menu item later if necessary
            item.Name = "FrequencyPlotInfo";
            item.Tag = "FrequencyPlotInfo";
            // This is the text that will show up in the menu
            item.Text = "Frequency Plot Info";
            // Add a handler that will respond when that menu item is selected
            item.Click += new System.EventHandler(ShowFrequencyPlotInfo);
            // Add the menu item to the menu
            menuStrip.Items.Insert(menuStrip.Items.Count, item);

        }

        /// <summary>
        /// method show frequenct plot infomation when the item selected
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void ShowFrequencyPlotInfo(object o, EventArgs e)
        {
            string s = "Frequency plot shows the distribution of selected variable values\n" +
                "arranged into 9 categories.  Statistically normal data is assumed for\n" +
                "MLR model building.";

            MessageBox.Show(s, "Frequency Plot", MessageBoxButtons.OK);
        }

        # endregion

        /// <summary>
        /// extracts disabled points resulting from plot UI interaction...
        /// gets the datatable from the caller form and removes rows in a
        /// copy of the datatable for re-plotting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnrePlot_Click(object sender, EventArgs e)
        {
            //filter the datatable for deleted records
            frmDatasheet frmmain = new frmDatasheet();
            DataTable dt = frmmain.DT.Copy();
            //you're on you own if you pass (null, true) .....
            dtRowInformation dtRI = dtRowInformation.getdtRI(null, false);
            Dictionary<string, bool> rstatus = dtRI.DTRowInfo;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!rstatus[dt.Rows[i][0].ToString()])
                    dt.Rows[i].Delete();
            }
            dt.AcceptChanges();

            //update the global datatable copy
            _dt = dt;

            //and redo the plots and list information
            renderPlots();
            showColInfo();

        }

        private void frmPlot_HelpRequested(object sender, HelpEventArgs hlpevent)
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

    }

    //public class SortedAL
    //{
    //    public class myComp : IComparer
    //    {
    //        int IComparer.Compare(Object x, Object y)
    //        {
    //            return ((new CaseInsensitiveComparer()).Compare(y, x));
    //        }

    //    }
    //}
}
