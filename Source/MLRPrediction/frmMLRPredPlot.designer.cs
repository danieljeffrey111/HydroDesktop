namespace MLRPrediction
{
    partial class frmMLRPredPlot
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbShowModelEst = new System.Windows.Forms.CheckBox();
            this.cbShowUBE = new System.Windows.Forms.CheckBox();
            this.btnMakePlot = new System.Windows.Forms.Button();
            this.cboPlotList = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbThresholdReg = new System.Windows.Forms.TextBox();
            this.rbLogeValue = new System.Windows.Forms.RadioButton();
            this.rbLog10Value = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.rbValue = new System.Windows.Forms.RadioButton();
            this.cbPlotThreshold = new System.Windows.Forms.CheckBox();
            this.tbThresholdDec = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tbUBEFN = new System.Windows.Forms.TextBox();
            this.tbUBEFP = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tbFN = new System.Windows.Forms.TextBox();
            this.tbFP = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ctlModelEstPlot = new ZedGraph.ZedGraphControl();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbShowModelEst);
            this.groupBox1.Controls.Add(this.cbShowUBE);
            this.groupBox1.Controls.Add(this.btnMakePlot);
            this.groupBox1.Controls.Add(this.cboPlotList);
            this.groupBox1.Location = new System.Drawing.Point(12, 24);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(264, 101);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Available Plots";
            // 
            // cbShowModelEst
            // 
            this.cbShowModelEst.AutoSize = true;
            this.cbShowModelEst.Checked = true;
            this.cbShowModelEst.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbShowModelEst.Location = new System.Drawing.Point(16, 56);
            this.cbShowModelEst.Name = "cbShowModelEst";
            this.cbShowModelEst.Size = new System.Drawing.Size(133, 17);
            this.cbShowModelEst.TabIndex = 19;
            this.cbShowModelEst.Text = "Show Model Estimates";
            this.cbShowModelEst.UseVisualStyleBackColor = true;
            this.cbShowModelEst.Visible = false;
            // 
            // cbShowUBE
            // 
            this.cbShowUBE.AutoSize = true;
            this.cbShowUBE.Checked = true;
            this.cbShowUBE.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbShowUBE.Location = new System.Drawing.Point(16, 78);
            this.cbShowUBE.Name = "cbShowUBE";
            this.cbShowUBE.Size = new System.Drawing.Size(149, 17);
            this.cbShowUBE.TabIndex = 18;
            this.cbShowUBE.Text = "Show Unbiased Estimates";
            this.cbShowUBE.UseVisualStyleBackColor = true;
            this.cbShowUBE.Visible = false;
            // 
            // btnMakePlot
            // 
            this.btnMakePlot.Location = new System.Drawing.Point(177, 63);
            this.btnMakePlot.Name = "btnMakePlot";
            this.btnMakePlot.Size = new System.Drawing.Size(79, 25);
            this.btnMakePlot.TabIndex = 17;
            this.btnMakePlot.Text = "Plot";
            this.btnMakePlot.UseVisualStyleBackColor = true;
            this.btnMakePlot.Click += new System.EventHandler(this.btnMakePlot_Click);
            // 
            // cboPlotList
            // 
            this.cboPlotList.FormattingEnabled = true;
            this.cboPlotList.Location = new System.Drawing.Point(13, 29);
            this.cboPlotList.Name = "cboPlotList";
            this.cboPlotList.Size = new System.Drawing.Size(245, 21);
            this.cboPlotList.TabIndex = 16;
            this.cboPlotList.SelectedIndexChanged += new System.EventHandler(this.cboPlotList_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.tbThresholdReg);
            this.groupBox2.Controls.Add(this.rbLogeValue);
            this.groupBox2.Controls.Add(this.rbLog10Value);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.rbValue);
            this.groupBox2.Controls.Add(this.cbPlotThreshold);
            this.groupBox2.Controls.Add(this.tbThresholdDec);
            this.groupBox2.Location = new System.Drawing.Point(12, 131);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(265, 154);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Thresholds";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.OliveDrab;
            this.label4.Location = new System.Drawing.Point(70, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(148, 13);
            this.label4.TabIndex = 82;
            this.label4.Text = "Regulatory Standard (Vertical)";
            // 
            // tbThresholdReg
            // 
            this.tbThresholdReg.Location = new System.Drawing.Point(31, 44);
            this.tbThresholdReg.Name = "tbThresholdReg";
            this.tbThresholdReg.Size = new System.Drawing.Size(33, 20);
            this.tbThresholdReg.TabIndex = 82;
            this.tbThresholdReg.Validating += new System.ComponentModel.CancelEventHandler(this.tbThresholdReg_Validating);
            this.tbThresholdReg.Validated += new System.EventHandler(this.tbThresholdReg_Validated);
            // 
            // rbLogeValue
            // 
            this.rbLogeValue.AutoSize = true;
            this.rbLogeValue.Location = new System.Drawing.Point(79, 107);
            this.rbLogeValue.Name = "rbLogeValue";
            this.rbLogeValue.Size = new System.Drawing.Size(105, 17);
            this.rbLogeValue.TabIndex = 17;
            this.rbLogeValue.TabStop = true;
            this.rbLogeValue.Text = "Plot Loge (value)";
            this.rbLogeValue.UseVisualStyleBackColor = true;
            this.rbLogeValue.CheckedChanged += new System.EventHandler(this.rbLogeValue_CheckedChanged);
            // 
            // rbLog10Value
            // 
            this.rbLog10Value.AutoSize = true;
            this.rbLog10Value.Location = new System.Drawing.Point(79, 88);
            this.rbLog10Value.Name = "rbLog10Value";
            this.rbLog10Value.Size = new System.Drawing.Size(111, 17);
            this.rbLog10Value.TabIndex = 16;
            this.rbLog10Value.TabStop = true;
            this.rbLog10Value.Text = "Plot Log10 (value)";
            this.rbLog10Value.UseVisualStyleBackColor = true;
            this.rbLog10Value.CheckedChanged += new System.EventHandler(this.rbLog10Value_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Crimson;
            this.label5.Location = new System.Drawing.Point(70, 24);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(145, 13);
            this.label5.TabIndex = 81;
            this.label5.Text = "Decision Criterion (Horizontal)";
            // 
            // rbValue
            // 
            this.rbValue.AutoSize = true;
            this.rbValue.Checked = true;
            this.rbValue.Location = new System.Drawing.Point(79, 70);
            this.rbValue.Name = "rbValue";
            this.rbValue.Size = new System.Drawing.Size(72, 17);
            this.rbValue.TabIndex = 15;
            this.rbValue.TabStop = true;
            this.rbValue.Text = "Plot value";
            this.rbValue.UseVisualStyleBackColor = true;
            this.rbValue.CheckedChanged += new System.EventHandler(this.rbValue_CheckedChanged);
            // 
            // cbPlotThreshold
            // 
            this.cbPlotThreshold.AutoSize = true;
            this.cbPlotThreshold.Location = new System.Drawing.Point(53, 131);
            this.cbPlotThreshold.Name = "cbPlotThreshold";
            this.cbPlotThreshold.Size = new System.Drawing.Size(142, 17);
            this.cbPlotThreshold.TabIndex = 14;
            this.cbPlotThreshold.Text = "Include threshold on plot";
            this.cbPlotThreshold.UseVisualStyleBackColor = true;
            this.cbPlotThreshold.Visible = false;
            // 
            // tbThresholdDec
            // 
            this.tbThresholdDec.Location = new System.Drawing.Point(31, 19);
            this.tbThresholdDec.Name = "tbThresholdDec";
            this.tbThresholdDec.Size = new System.Drawing.Size(33, 20);
            this.tbThresholdDec.TabIndex = 13;
            this.tbThresholdDec.Validating += new System.ComponentModel.CancelEventHandler(this.tbThresholdDec_Validating);
            this.tbThresholdDec.Validated += new System.EventHandler(this.tbThresholdDec_Validated);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tbUBEFN);
            this.groupBox3.Controls.Add(this.tbUBEFP);
            this.groupBox3.Controls.Add(this.label20);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.tbFN);
            this.groupBox3.Controls.Add(this.tbFP);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Location = new System.Drawing.Point(12, 294);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(264, 106);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Model Failures";
            // 
            // tbUBEFN
            // 
            this.tbUBEFN.Location = new System.Drawing.Point(195, 81);
            this.tbUBEFN.Name = "tbUBEFN";
            this.tbUBEFN.ReadOnly = true;
            this.tbUBEFN.Size = new System.Drawing.Size(43, 20);
            this.tbUBEFN.TabIndex = 11;
            this.tbUBEFN.TabStop = false;
            this.tbUBEFN.Visible = false;
            // 
            // tbUBEFP
            // 
            this.tbUBEFP.Location = new System.Drawing.Point(195, 60);
            this.tbUBEFP.Name = "tbUBEFP";
            this.tbUBEFP.ReadOnly = true;
            this.tbUBEFP.Size = new System.Drawing.Size(43, 20);
            this.tbUBEFP.TabIndex = 10;
            this.tbUBEFP.TabStop = false;
            this.tbUBEFP.Visible = false;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(13, 84);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(177, 13);
            this.label20.TabIndex = 9;
            this.label20.Text = "Unbiased Estimate False Negatives:";
            this.label20.Visible = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(21, 67);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(171, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Unbiased Estimate False Positives:";
            this.label6.Visible = false;
            // 
            // tbFN
            // 
            this.tbFN.Location = new System.Drawing.Point(195, 34);
            this.tbFN.Name = "tbFN";
            this.tbFN.ReadOnly = true;
            this.tbFN.Size = new System.Drawing.Size(43, 20);
            this.tbFN.TabIndex = 7;
            this.tbFN.TabStop = false;
            // 
            // tbFP
            // 
            this.tbFP.Location = new System.Drawing.Point(195, 13);
            this.tbFP.Name = "tbFP";
            this.tbFP.ReadOnly = true;
            this.tbFP.Size = new System.Drawing.Size(43, 20);
            this.tbFP.TabIndex = 6;
            this.tbFP.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(161, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Model Estimate False Negatives:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(155, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Model Estimate False Positives:";
            // 
            // ctlModelEstPlot
            // 
            this.ctlModelEstPlot.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ctlModelEstPlot.EditButtons = System.Windows.Forms.MouseButtons.Left;
            this.ctlModelEstPlot.IsShowPointValues = true;
            this.ctlModelEstPlot.Location = new System.Drawing.Point(296, 24);
            this.ctlModelEstPlot.Name = "ctlModelEstPlot";
            this.ctlModelEstPlot.PanModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)));
            this.ctlModelEstPlot.ScrollGrace = 0D;
            this.ctlModelEstPlot.ScrollMaxX = 0D;
            this.ctlModelEstPlot.ScrollMaxY = 0D;
            this.ctlModelEstPlot.ScrollMaxY2 = 0D;
            this.ctlModelEstPlot.ScrollMinX = 0D;
            this.ctlModelEstPlot.ScrollMinY = 0D;
            this.ctlModelEstPlot.ScrollMinY2 = 0D;
            this.ctlModelEstPlot.Size = new System.Drawing.Size(480, 480);
            this.ctlModelEstPlot.TabIndex = 6;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Controls.Add(this.label12);
            this.groupBox4.Controls.Add(this.label13);
            this.groupBox4.Location = new System.Drawing.Point(42, 408);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(200, 96);
            this.groupBox4.TabIndex = 7;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Current US Regulatory Standards";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(146, 71);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(19, 13);
            this.label8.TabIndex = 5;
            this.label8.Text = "61";
            this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(140, 48);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(25, 13);
            this.label9.TabIndex = 4;
            this.label9.Text = "104";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(140, 26);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(25, 13);
            this.label10.TabIndex = 3;
            this.label10.Text = "235";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(13, 71);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(117, 13);
            this.label11.TabIndex = 2;
            this.label11.Text = "Enterococci, Saltwater:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(13, 48);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(125, 13);
            this.label12.TabIndex = 1;
            this.label12.Text = "Enterococci, Freshwater:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(13, 26);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(97, 13);
            this.label13.TabIndex = 0;
            this.label13.Text = "E. coli, Freshwater:";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // frmMLRPredPlot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 514);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.ctlModelEstPlot);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmMLRPredPlot";
            this.Text = "Standard Plots";
            this.Load += new System.EventHandler(this.frmMLRPredPlot_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private ZedGraph.ZedGraphControl ctlModelEstPlot;
        private System.Windows.Forms.RadioButton rbLogeValue;
        private System.Windows.Forms.RadioButton rbLog10Value;
        private System.Windows.Forms.RadioButton rbValue;
        private System.Windows.Forms.CheckBox cbPlotThreshold;
        private System.Windows.Forms.TextBox tbThresholdDec;
        private System.Windows.Forms.TextBox tbFN;
        private System.Windows.Forms.TextBox tbFP;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboPlotList;
        private System.Windows.Forms.Button btnMakePlot;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbThresholdReg;
        private System.Windows.Forms.CheckBox cbShowUBE;
        private System.Windows.Forms.TextBox tbUBEFN;
        private System.Windows.Forms.TextBox tbUBEFP;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox cbShowModelEst;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}