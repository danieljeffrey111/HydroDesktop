namespace IPyModelingControl
{
    partial class IPyModelingControl
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.lvValidation = new System.Windows.Forms.ListView();
            this.columnHeader16 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader17 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader18 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader19 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pnlThresholdingButtons = new System.Windows.Forms.Panel();
            this.btnRight25 = new System.Windows.Forms.Button();
            this.btnRight1 = new System.Windows.Forms.Button();
            this.btnLeft1 = new System.Windows.Forms.Button();
            this.btnLeft25 = new System.Windows.Forms.Button();
            this.lblSpec = new System.Windows.Forms.Label();
            this.chartValidation = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.lvModel = new System.Windows.Forms.ListView();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbExponent = new System.Windows.Forms.TextBox();
            this.rbPower = new System.Windows.Forms.RadioButton();
            this.rbLoge = new System.Windows.Forms.RadioButton();
            this.rbValue = new System.Windows.Forms.RadioButton();
            this.rbLog10 = new System.Windows.Forms.RadioButton();
            this.groupBox13 = new System.Windows.Forms.GroupBox();
            this.label32 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.label39 = new System.Windows.Forms.Label();
            this.tbThreshold = new System.Windows.Forms.TextBox();
            this.btnSelectModel = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.lblDecisionThreshold = new System.Windows.Forms.Label();
            this.groupBox10.SuspendLayout();
            this.pnlThresholdingButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartValidation)).BeginInit();
            this.groupBox6.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.groupBox12.SuspendLayout();
            this.groupBox13.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.lvValidation);
            this.groupBox10.Location = new System.Drawing.Point(3, 476);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(369, 100);
            this.groupBox10.TabIndex = 106;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "Model Validation";
            // 
            // lvValidation
            // 
            this.lvValidation.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader16,
            this.columnHeader17,
            this.columnHeader18,
            this.columnHeader19});
            this.lvValidation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvValidation.FullRowSelect = true;
            this.lvValidation.GridLines = true;
            this.lvValidation.Location = new System.Drawing.Point(3, 16);
            this.lvValidation.Name = "lvValidation";
            this.lvValidation.Size = new System.Drawing.Size(363, 81);
            this.lvValidation.TabIndex = 52;
            this.lvValidation.UseCompatibleStateImageBehavior = false;
            this.lvValidation.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader16
            // 
            this.columnHeader16.Text = "True Positives";
            this.columnHeader16.Width = 88;
            // 
            // columnHeader17
            // 
            this.columnHeader17.Text = "True Negatives";
            this.columnHeader17.Width = 86;
            // 
            // columnHeader18
            // 
            this.columnHeader18.Text = "False Positives";
            this.columnHeader18.Width = 83;
            // 
            // columnHeader19
            // 
            this.columnHeader19.Text = "False Negatives";
            this.columnHeader19.Width = 95;
            // 
            // pnlThresholdingButtons
            // 
            this.pnlThresholdingButtons.Controls.Add(this.btnRight25);
            this.pnlThresholdingButtons.Controls.Add(this.btnRight1);
            this.pnlThresholdingButtons.Controls.Add(this.btnLeft1);
            this.pnlThresholdingButtons.Controls.Add(this.btnLeft25);
            this.pnlThresholdingButtons.Location = new System.Drawing.Point(428, 543);
            this.pnlThresholdingButtons.Name = "pnlThresholdingButtons";
            this.pnlThresholdingButtons.Size = new System.Drawing.Size(504, 30);
            this.pnlThresholdingButtons.TabIndex = 105;
            this.pnlThresholdingButtons.Visible = false;
            // 
            // btnRight25
            // 
            this.btnRight25.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRight25.Location = new System.Drawing.Point(426, 3);
            this.btnRight25.Name = "btnRight25";
            this.btnRight25.Size = new System.Drawing.Size(75, 23);
            this.btnRight25.TabIndex = 112;
            this.btnRight25.Text = ">>";
            this.btnRight25.UseVisualStyleBackColor = true;
            this.btnRight25.Click += new System.EventHandler(this.btnRight25_Click);
            // 
            // btnRight1
            // 
            this.btnRight1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRight1.Location = new System.Drawing.Point(345, 3);
            this.btnRight1.Name = "btnRight1";
            this.btnRight1.Size = new System.Drawing.Size(75, 23);
            this.btnRight1.TabIndex = 111;
            this.btnRight1.Text = ">";
            this.btnRight1.UseVisualStyleBackColor = true;
            this.btnRight1.Click += new System.EventHandler(this.btnRight1_Click);
            // 
            // btnLeft1
            // 
            this.btnLeft1.Location = new System.Drawing.Point(84, 3);
            this.btnLeft1.Name = "btnLeft1";
            this.btnLeft1.Size = new System.Drawing.Size(75, 23);
            this.btnLeft1.TabIndex = 110;
            this.btnLeft1.Text = "<";
            this.btnLeft1.UseVisualStyleBackColor = true;
            this.btnLeft1.Click += new System.EventHandler(this.btnLeft1_Click);
            // 
            // btnLeft25
            // 
            this.btnLeft25.Location = new System.Drawing.Point(3, 3);
            this.btnLeft25.Name = "btnLeft25";
            this.btnLeft25.Size = new System.Drawing.Size(75, 23);
            this.btnLeft25.TabIndex = 109;
            this.btnLeft25.Text = "<<";
            this.btnLeft25.UseVisualStyleBackColor = true;
            this.btnLeft25.Click += new System.EventHandler(this.btnLeft25_Click);
            // 
            // lblSpec
            // 
            this.lblSpec.AutoSize = true;
            this.lblSpec.Location = new System.Drawing.Point(363, 5);
            this.lblSpec.Name = "lblSpec";
            this.lblSpec.Size = new System.Drawing.Size(103, 13);
            this.lblSpec.TabIndex = 115;
            this.lblSpec.Text = "specificity goes here";
            this.lblSpec.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblSpec.Visible = false;
            // 
            // chartValidation
            // 
            chartArea2.AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.NotSet;
            chartArea2.AxisX.MajorTickMark.TickMarkStyle = System.Windows.Forms.DataVisualization.Charting.TickMarkStyle.AcrossAxis;
            chartArea2.AxisX2.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.NotSet;
            chartArea2.AxisX2.MajorTickMark.TickMarkStyle = System.Windows.Forms.DataVisualization.Charting.TickMarkStyle.AcrossAxis;
            chartArea2.AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.NotSet;
            chartArea2.AxisY.MajorTickMark.TickMarkStyle = System.Windows.Forms.DataVisualization.Charting.TickMarkStyle.AcrossAxis;
            chartArea2.AxisY2.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.NotSet;
            chartArea2.AxisY2.MajorTickMark.TickMarkStyle = System.Windows.Forms.DataVisualization.Charting.TickMarkStyle.AcrossAxis;
            chartArea2.Name = "ChartArea1";
            this.chartValidation.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chartValidation.Legends.Add(legend2);
            this.chartValidation.Location = new System.Drawing.Point(378, 5);
            this.chartValidation.Name = "chartValidation";
            series3.BorderWidth = 2;
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StepLine;
            series3.Legend = "Legend1";
            series3.Name = "True positives";
            series4.BorderWidth = 2;
            series4.ChartArea = "ChartArea1";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StepLine;
            series4.Legend = "Legend1";
            series4.Name = "True negatives";
            this.chartValidation.Series.Add(series3);
            this.chartValidation.Series.Add(series4);
            this.chartValidation.Size = new System.Drawing.Size(701, 550);
            this.chartValidation.TabIndex = 114;
            this.chartValidation.Text = "chart1";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.lvModel);
            this.groupBox6.Location = new System.Drawing.Point(3, 218);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(369, 252);
            this.groupBox6.TabIndex = 111;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Model Summary";
            // 
            // lvModel
            // 
            this.lvModel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvModel.FullRowSelect = true;
            this.lvModel.GridLines = true;
            this.lvModel.Location = new System.Drawing.Point(3, 16);
            this.lvModel.Name = "lvModel";
            this.lvModel.Size = new System.Drawing.Size(363, 233);
            this.lvModel.TabIndex = 51;
            this.lvModel.UseCompatibleStateImageBehavior = false;
            this.lvModel.View = System.Windows.Forms.View.Details;
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.groupBox12);
            this.groupBox11.Controls.Add(this.groupBox13);
            this.groupBox11.Controls.Add(this.label39);
            this.groupBox11.Controls.Add(this.tbThreshold);
            this.groupBox11.Location = new System.Drawing.Point(6, 5);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(369, 149);
            this.groupBox11.TabIndex = 108;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "Model Evaluation Threshold";
            // 
            // groupBox12
            // 
            this.groupBox12.Controls.Add(this.label1);
            this.groupBox12.Controls.Add(this.tbExponent);
            this.groupBox12.Controls.Add(this.rbPower);
            this.groupBox12.Controls.Add(this.rbLoge);
            this.groupBox12.Controls.Add(this.rbValue);
            this.groupBox12.Controls.Add(this.rbLog10);
            this.groupBox12.Location = new System.Drawing.Point(6, 45);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Size = new System.Drawing.Size(162, 96);
            this.groupBox12.TabIndex = 91;
            this.groupBox12.TabStop = false;
            this.groupBox12.Text = "Dependent Variable is:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(101, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "exp:";
            // 
            // tbExponent
            // 
            this.tbExponent.Enabled = false;
            this.tbExponent.Location = new System.Drawing.Point(128, 69);
            this.tbExponent.Name = "tbExponent";
            this.tbExponent.Size = new System.Drawing.Size(29, 20);
            this.tbExponent.TabIndex = 19;
            this.tbExponent.Text = "1";
            this.tbExponent.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // rbPower
            // 
            this.rbPower.AutoSize = true;
            this.rbPower.Location = new System.Drawing.Point(9, 70);
            this.rbPower.Name = "rbPower";
            this.rbPower.Size = new System.Drawing.Size(90, 17);
            this.rbPower.TabIndex = 18;
            this.rbPower.Text = "Power (value)";
            this.rbPower.UseVisualStyleBackColor = true;
            // 
            // rbLoge
            // 
            this.rbLoge.AutoSize = true;
            this.rbLoge.Location = new System.Drawing.Point(9, 52);
            this.rbLoge.Name = "rbLoge";
            this.rbLoge.Size = new System.Drawing.Size(84, 17);
            this.rbLoge.TabIndex = 17;
            this.rbLoge.Text = "Loge (value)";
            this.rbLoge.UseVisualStyleBackColor = true;
            this.rbLoge.CheckedChanged += new System.EventHandler(this.rbLogeValue_CheckedChanged);
            // 
            // rbValue
            // 
            this.rbValue.AutoSize = true;
            this.rbValue.Checked = true;
            this.rbValue.Location = new System.Drawing.Point(9, 16);
            this.rbValue.Name = "rbValue";
            this.rbValue.Size = new System.Drawing.Size(52, 17);
            this.rbValue.TabIndex = 15;
            this.rbValue.TabStop = true;
            this.rbValue.Text = "Value";
            this.rbValue.UseVisualStyleBackColor = true;
            this.rbValue.CheckedChanged += new System.EventHandler(this.rbValue_CheckedChanged);
            // 
            // rbLog10
            // 
            this.rbLog10.AutoSize = true;
            this.rbLog10.Location = new System.Drawing.Point(9, 34);
            this.rbLog10.Name = "rbLog10";
            this.rbLog10.Size = new System.Drawing.Size(90, 17);
            this.rbLog10.TabIndex = 16;
            this.rbLog10.Text = "Log10 (value)";
            this.rbLog10.UseVisualStyleBackColor = true;
            this.rbLog10.CheckedChanged += new System.EventHandler(this.rbLog10Value_CheckedChanged);
            // 
            // groupBox13
            // 
            this.groupBox13.Controls.Add(this.label32);
            this.groupBox13.Controls.Add(this.label33);
            this.groupBox13.Controls.Add(this.label34);
            this.groupBox13.Controls.Add(this.label35);
            this.groupBox13.Controls.Add(this.label36);
            this.groupBox13.Controls.Add(this.label37);
            this.groupBox13.Location = new System.Drawing.Point(174, 45);
            this.groupBox13.Name = "groupBox13";
            this.groupBox13.Size = new System.Drawing.Size(189, 96);
            this.groupBox13.TabIndex = 90;
            this.groupBox13.TabStop = false;
            this.groupBox13.Text = "Current US Regulatory Standards";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(139, 72);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(19, 13);
            this.label32.TabIndex = 5;
            this.label32.Text = "61";
            this.label32.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(133, 54);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(25, 13);
            this.label33.TabIndex = 4;
            this.label33.Text = "104";
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(133, 36);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(25, 13);
            this.label34.TabIndex = 3;
            this.label34.Text = "235";
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(6, 72);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(117, 13);
            this.label35.TabIndex = 2;
            this.label35.Text = "Enterococci, Saltwater:";
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(6, 54);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(125, 13);
            this.label36.TabIndex = 1;
            this.label36.Text = "Enterococci, Freshwater:";
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(6, 36);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(97, 13);
            this.label37.TabIndex = 0;
            this.label37.Text = "E. coli, Freshwater:";
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.ForeColor = System.Drawing.Color.OliveDrab;
            this.label39.Location = new System.Drawing.Point(49, 22);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(104, 13);
            this.label39.TabIndex = 82;
            this.label39.Text = "Regulatory Standard";
            // 
            // tbThreshold
            // 
            this.tbThreshold.Location = new System.Drawing.Point(10, 19);
            this.tbThreshold.Name = "tbThreshold";
            this.tbThreshold.Size = new System.Drawing.Size(33, 20);
            this.tbThreshold.TabIndex = 82;
            this.tbThreshold.Text = "235";
            this.tbThreshold.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbThreshold.TextChanged += new System.EventHandler(this.tbThreshold_TextChanged);
            // 
            // btnSelectModel
            // 
            this.btnSelectModel.Enabled = false;
            this.btnSelectModel.Location = new System.Drawing.Point(265, 189);
            this.btnSelectModel.Name = "btnSelectModel";
            this.btnSelectModel.Size = new System.Drawing.Size(107, 23);
            this.btnSelectModel.TabIndex = 102;
            this.btnSelectModel.Text = "Go to Prediction";
            this.btnSelectModel.UseVisualStyleBackColor = true;
            this.btnSelectModel.Click += new System.EventHandler(this.btnSelectModel_Click);
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(265, 160);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(107, 23);
            this.btnRun.TabIndex = 107;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Crimson;
            this.label4.Location = new System.Drawing.Point(55, 184);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(145, 13);
            this.label4.TabIndex = 112;
            this.label4.Text = "Decision Criterion (Horizontal)";
            // 
            // lblDecisionThreshold
            // 
            this.lblDecisionThreshold.AutoSize = true;
            this.lblDecisionThreshold.Location = new System.Drawing.Point(13, 184);
            this.lblDecisionThreshold.Name = "lblDecisionThreshold";
            this.lblDecisionThreshold.Size = new System.Drawing.Size(0, 13);
            this.lblDecisionThreshold.TabIndex = 113;
            // 
            // IPyModelingControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox10);
            this.Controls.Add(this.btnSelectModel);
            this.Controls.Add(this.pnlThresholdingButtons);
            this.Controls.Add(this.lblSpec);
            this.Controls.Add(this.chartValidation);
            this.Controls.Add(this.lblDecisionThreshold);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox11);
            this.Controls.Add(this.btnRun);
            this.Name = "IPyModelingControl";
            this.Size = new System.Drawing.Size(1085, 580);
            this.groupBox10.ResumeLayout(false);
            this.pnlThresholdingButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartValidation)).EndInit();
            this.groupBox6.ResumeLayout(false);
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            this.groupBox12.ResumeLayout(false);
            this.groupBox12.PerformLayout();
            this.groupBox13.ResumeLayout(false);
            this.groupBox13.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.GroupBox groupBox10;
        protected System.Windows.Forms.ListView lvValidation;
        protected System.Windows.Forms.ColumnHeader columnHeader16;
        protected System.Windows.Forms.ColumnHeader columnHeader17;
        protected System.Windows.Forms.ColumnHeader columnHeader18;
        protected System.Windows.Forms.ColumnHeader columnHeader19;
        protected System.Windows.Forms.Panel pnlThresholdingButtons;
        protected System.Windows.Forms.Button btnRight25;
        protected System.Windows.Forms.Button btnRight1;
        protected System.Windows.Forms.Button btnLeft1;
        protected System.Windows.Forms.Button btnLeft25;
        protected System.Windows.Forms.Label lblSpec;
        protected System.Windows.Forms.DataVisualization.Charting.Chart chartValidation;
        protected System.Windows.Forms.GroupBox groupBox6;
        protected System.Windows.Forms.GroupBox groupBox11;
        protected System.Windows.Forms.GroupBox groupBox12;
        protected System.Windows.Forms.RadioButton rbLoge;
        protected System.Windows.Forms.RadioButton rbValue;
        protected System.Windows.Forms.RadioButton rbLog10;
        protected System.Windows.Forms.GroupBox groupBox13;
        protected System.Windows.Forms.Label label32;
        protected System.Windows.Forms.Label label33;
        protected System.Windows.Forms.Label label34;
        protected System.Windows.Forms.Label label35;
        protected System.Windows.Forms.Label label36;
        protected System.Windows.Forms.Label label37;
        protected System.Windows.Forms.Label label39;
        protected System.Windows.Forms.TextBox tbThreshold;
        protected System.Windows.Forms.Button btnSelectModel;
        protected System.Windows.Forms.Button btnRun;
        protected System.Windows.Forms.Label label1;
        protected System.Windows.Forms.TextBox tbExponent;
        protected System.Windows.Forms.RadioButton rbPower;
        protected System.Windows.Forms.ListView lvModel;
        protected System.Windows.Forms.Label label4;
        protected System.Windows.Forms.Label lblDecisionThreshold;
    }
}
