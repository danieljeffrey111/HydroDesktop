namespace MLRPrediction
{
    partial class frmMLRPrediction
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.panelIVs = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dgvVariables = new System.Windows.Forms.DataGridView();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.dgvObs = new System.Windows.Forms.DataGridView();
            this.dgvStats = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.btnMakePredictions = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.txtPower = new System.Windows.Forms.TextBox();
            this.rbPower = new System.Windows.Forms.RadioButton();
            this.rbLn = new System.Windows.Forms.RadioButton();
            this.rbNone = new System.Windows.Forms.RadioButton();
            this.rbLog10 = new System.Windows.Forms.RadioButton();
            this.label23 = new System.Windows.Forms.Label();
            this.txtRegStd = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.txtDecCrit = new System.Windows.Forms.TextBox();
            this.txtModel = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnPlot = new System.Windows.Forms.Button();
            this.btnImportObs = new System.Windows.Forms.Button();
            this.btnImportIVs = new System.Windows.Forms.Button();
            this.btnClearTable = new System.Windows.Forms.Button();
            this.btnSaveTable = new System.Windows.Forms.Button();
            this.btnIVDataValidation = new System.Windows.Forms.Button();
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            this.panel2.SuspendLayout();
            this.panelIVs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVariables)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvObs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStats)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.panel2.Controls.Add(this.panelIVs);
            this.panel2.Location = new System.Drawing.Point(12, 231);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(967, 294);
            this.panel2.TabIndex = 1;
            // 
            // panelIVs
            // 
            this.panelIVs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelIVs.Controls.Add(this.splitContainer1);
            this.panelIVs.Location = new System.Drawing.Point(4, 3);
            this.panelIVs.Name = "panelIVs";
            this.panelIVs.Size = new System.Drawing.Size(960, 288);
            this.panelIVs.TabIndex = 3;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dgvVariables);
            this.splitContainer1.Panel1MinSize = 100;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(960, 288);
            this.splitContainer1.SplitterDistance = 538;
            this.splitContainer1.TabIndex = 0;
            this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
            // 
            // dgvVariables
            // 
            this.dgvVariables.AllowUserToOrderColumns = true;
            this.dgvVariables.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVariables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvVariables.Location = new System.Drawing.Point(0, 0);
            this.dgvVariables.Name = "dgvVariables";
            this.dgvVariables.Size = new System.Drawing.Size(538, 288);
            this.dgvVariables.TabIndex = 2;
            this.dgvVariables.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvVariables_CellPainting);
            this.dgvVariables.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvVariables_CellValueChanged);
            this.dgvVariables.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvVariables_DataError);
            this.dgvVariables.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvVariables_RowEnter);
            this.dgvVariables.RowLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvVariables_RowLeave);
            this.dgvVariables.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dgvVariables_Scroll);
            this.dgvVariables.SelectionChanged += new System.EventHandler(this.dgvVariables_SelectionChanged);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.dgvObs);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.dgvStats);
            this.splitContainer2.Size = new System.Drawing.Size(418, 288);
            this.splitContainer2.SplitterDistance = 139;
            this.splitContainer2.TabIndex = 0;
            this.splitContainer2.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer2_SplitterMoved);
            // 
            // dgvObs
            // 
            this.dgvObs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvObs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvObs.Location = new System.Drawing.Point(0, 0);
            this.dgvObs.Name = "dgvObs";
            this.dgvObs.Size = new System.Drawing.Size(139, 288);
            this.dgvObs.TabIndex = 3;
            this.dgvObs.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvObs_CellEndEdit);
            this.dgvObs.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvObs_DataError);
            this.dgvObs.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvObs_RowEnter);
            this.dgvObs.RowLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvObs_RowLeave);
            this.dgvObs.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dgvObs_Scroll);
            this.dgvObs.SelectionChanged += new System.EventHandler(this.dgvObs_SelectionChanged);
            this.dgvObs.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvObs_MouseUp);
            // 
            // dgvStats
            // 
            this.dgvStats.AllowUserToOrderColumns = true;
            this.dgvStats.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStats.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvStats.Location = new System.Drawing.Point(0, 0);
            this.dgvStats.Name = "dgvStats";
            this.dgvStats.ReadOnly = true;
            this.dgvStats.Size = new System.Drawing.Size(275, 288);
            this.dgvStats.TabIndex = 4;
            this.dgvStats.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStats_RowEnter);
            this.dgvStats.RowLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStats_RowLeave);
            this.dgvStats.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dgvStats_Scroll);
            this.dgvStats.SelectionChanged += new System.EventHandler(this.dgvStats_SelectionChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 208);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(133, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Predictive Record";
            // 
            // btnMakePredictions
            // 
            this.btnMakePredictions.BackColor = System.Drawing.SystemColors.Control;
            this.btnMakePredictions.Enabled = false;
            this.btnMakePredictions.Location = new System.Drawing.Point(472, 155);
            this.btnMakePredictions.Name = "btnMakePredictions";
            this.btnMakePredictions.Size = new System.Drawing.Size(75, 39);
            this.btnMakePredictions.TabIndex = 6;
            this.btnMakePredictions.Text = "Make\r\nPredictions";
            this.btnMakePredictions.UseVisualStyleBackColor = false;
            this.btnMakePredictions.Click += new System.EventHandler(this.btnMakePredictions_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox7);
            this.groupBox1.Controls.Add(this.label23);
            this.groupBox1.Controls.Add(this.txtRegStd);
            this.groupBox1.Controls.Add(this.label24);
            this.groupBox1.Controls.Add(this.txtDecCrit);
            this.groupBox1.Location = new System.Drawing.Point(22, 76);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(401, 129);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Model Evaluation Thresholds";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.txtPower);
            this.groupBox7.Controls.Add(this.rbPower);
            this.groupBox7.Controls.Add(this.rbLn);
            this.groupBox7.Controls.Add(this.rbNone);
            this.groupBox7.Controls.Add(this.rbLog10);
            this.groupBox7.Location = new System.Drawing.Point(223, 12);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(147, 106);
            this.groupBox7.TabIndex = 96;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = " Threshold Transform";
            // 
            // txtPower
            // 
            this.txtPower.Location = new System.Drawing.Point(82, 79);
            this.txtPower.Name = "txtPower";
            this.txtPower.Size = new System.Drawing.Size(45, 20);
            this.txtPower.TabIndex = 19;
            this.txtPower.Text = "1.0";
            // 
            // rbPower
            // 
            this.rbPower.AutoSize = true;
            this.rbPower.Location = new System.Drawing.Point(24, 81);
            this.rbPower.Name = "rbPower";
            this.rbPower.Size = new System.Drawing.Size(55, 17);
            this.rbPower.TabIndex = 18;
            this.rbPower.Text = "Power";
            this.rbPower.UseVisualStyleBackColor = true;
            this.rbPower.CheckedChanged += new System.EventHandler(this.rbPower_CheckedChanged);
            // 
            // rbLn
            // 
            this.rbLn.AutoSize = true;
            this.rbLn.Location = new System.Drawing.Point(24, 60);
            this.rbLn.Name = "rbLn";
            this.rbLn.Size = new System.Drawing.Size(37, 17);
            this.rbLn.TabIndex = 17;
            this.rbLn.Text = "Ln";
            this.rbLn.UseVisualStyleBackColor = true;
            // 
            // rbNone
            // 
            this.rbNone.AutoSize = true;
            this.rbNone.Checked = true;
            this.rbNone.Location = new System.Drawing.Point(24, 18);
            this.rbNone.Name = "rbNone";
            this.rbNone.Size = new System.Drawing.Size(51, 17);
            this.rbNone.TabIndex = 15;
            this.rbNone.TabStop = true;
            this.rbNone.Text = "None";
            this.rbNone.UseVisualStyleBackColor = true;
            // 
            // rbLog10
            // 
            this.rbLog10.AutoSize = true;
            this.rbLog10.Location = new System.Drawing.Point(24, 39);
            this.rbLog10.Name = "rbLog10";
            this.rbLog10.Size = new System.Drawing.Size(55, 17);
            this.rbLog10.TabIndex = 16;
            this.rbLog10.Text = "Log10";
            this.rbLog10.UseVisualStyleBackColor = true;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.ForeColor = System.Drawing.Color.OliveDrab;
            this.label23.Location = new System.Drawing.Point(54, 67);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(148, 13);
            this.label23.TabIndex = 95;
            this.label23.Text = "Regulatory Standard (Vertical)";
            // 
            // txtRegStd
            // 
            this.txtRegStd.Location = new System.Drawing.Point(15, 67);
            this.txtRegStd.Name = "txtRegStd";
            this.txtRegStd.Size = new System.Drawing.Size(33, 20);
            this.txtRegStd.TabIndex = 94;
            this.txtRegStd.Text = "235";
            this.txtRegStd.Leave += new System.EventHandler(this.txtRegStd_Leave);
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.ForeColor = System.Drawing.Color.Crimson;
            this.label24.Location = new System.Drawing.Point(54, 34);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(145, 13);
            this.label24.TabIndex = 93;
            this.label24.Text = "Decision Criterion (Horizontal)";
            // 
            // txtDecCrit
            // 
            this.txtDecCrit.Location = new System.Drawing.Point(15, 31);
            this.txtDecCrit.Name = "txtDecCrit";
            this.txtDecCrit.Size = new System.Drawing.Size(33, 20);
            this.txtDecCrit.TabIndex = 92;
            this.txtDecCrit.Text = "235";
            this.txtDecCrit.Leave += new System.EventHandler(this.txtDecCrit_Leave);
            // 
            // txtModel
            // 
            this.txtModel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtModel.BackColor = System.Drawing.Color.White;
            this.txtModel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtModel.Location = new System.Drawing.Point(93, 9);
            this.txtModel.Multiline = true;
            this.txtModel.Name = "txtModel";
            this.txtModel.ReadOnly = true;
            this.txtModel.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtModel.Size = new System.Drawing.Size(850, 46);
            this.txtModel.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(18, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 20);
            this.label1.TabIndex = 14;
            this.label1.Text = "Model: ";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.btnPlot);
            this.groupBox2.Controls.Add(this.btnImportObs);
            this.groupBox2.Controls.Add(this.btnImportIVs);
            this.groupBox2.Controls.Add(this.btnClearTable);
            this.groupBox2.Controls.Add(this.btnSaveTable);
            this.groupBox2.Location = new System.Drawing.Point(653, 88);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(290, 128);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Prediction Grid";
            // 
            // btnPlot
            // 
            this.btnPlot.Location = new System.Drawing.Point(16, 81);
            this.btnPlot.Name = "btnPlot";
            this.btnPlot.Size = new System.Drawing.Size(75, 29);
            this.btnPlot.TabIndex = 18;
            this.btnPlot.Text = "Plot";
            this.btnPlot.UseVisualStyleBackColor = false;
            this.btnPlot.Click += new System.EventHandler(this.btnPlot_Click);
            // 
            // btnImportObs
            // 
            this.btnImportObs.Location = new System.Drawing.Point(149, 27);
            this.btnImportObs.Name = "btnImportObs";
            this.btnImportObs.Size = new System.Drawing.Size(75, 41);
            this.btnImportObs.TabIndex = 17;
            this.btnImportObs.Text = "Import Obs";
            this.btnImportObs.UseVisualStyleBackColor = false;
            this.btnImportObs.Click += new System.EventHandler(this.btnImportObs_Click);
            // 
            // btnImportIVs
            // 
            this.btnImportIVs.Location = new System.Drawing.Point(63, 27);
            this.btnImportIVs.Name = "btnImportIVs";
            this.btnImportIVs.Size = new System.Drawing.Size(75, 41);
            this.btnImportIVs.TabIndex = 16;
            this.btnImportIVs.Text = "Import IVs";
            this.btnImportIVs.UseVisualStyleBackColor = false;
            this.btnImportIVs.Click += new System.EventHandler(this.btnImportIVs_Click);
            // 
            // btnClearTable
            // 
            this.btnClearTable.BackColor = System.Drawing.SystemColors.Control;
            this.btnClearTable.Location = new System.Drawing.Point(103, 81);
            this.btnClearTable.Name = "btnClearTable";
            this.btnClearTable.Size = new System.Drawing.Size(75, 29);
            this.btnClearTable.TabIndex = 15;
            this.btnClearTable.Text = "Clear";
            this.btnClearTable.UseVisualStyleBackColor = false;
            this.btnClearTable.Click += new System.EventHandler(this.btnClearTable_Click);
            // 
            // btnSaveTable
            // 
            this.btnSaveTable.BackColor = System.Drawing.SystemColors.Control;
            this.btnSaveTable.Location = new System.Drawing.Point(190, 81);
            this.btnSaveTable.Name = "btnSaveTable";
            this.btnSaveTable.Size = new System.Drawing.Size(86, 29);
            this.btnSaveTable.TabIndex = 14;
            this.btnSaveTable.Text = "Export As CSV";
            this.btnSaveTable.UseVisualStyleBackColor = false;
            this.btnSaveTable.Click += new System.EventHandler(this.btnExportTable_Click);
            // 
            // btnIVDataValidation
            // 
            this.btnIVDataValidation.Location = new System.Drawing.Point(472, 110);
            this.btnIVDataValidation.Name = "btnIVDataValidation";
            this.btnIVDataValidation.Size = new System.Drawing.Size(75, 34);
            this.btnIVDataValidation.TabIndex = 17;
            this.btnIVDataValidation.Text = "IV Data Validation";
            this.btnIVDataValidation.UseVisualStyleBackColor = false;
            this.btnIVDataValidation.Click += new System.EventHandler(this.btnIVDataValidation_Click);
            // 
            // frmMLRPrediction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(991, 537);
            this.Controls.Add(this.btnIVDataValidation);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.txtModel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnMakePredictions);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel2);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmMLRPrediction";
            this.Text = "MLR Prediction";
            this.Load += new System.EventHandler(this.frmMLRPrediction_Load);
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.frmMLRPrediction_HelpRequested);
            this.Enter += new System.EventHandler(this.frmMLRPrediction_Enter);
            this.panel2.ResumeLayout(false);
            this.panelIVs.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvVariables)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvObs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStats)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnMakePredictions;
        private System.Windows.Forms.Panel panelIVs;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dgvVariables;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.DataGridView dgvObs;
        private System.Windows.Forms.DataGridView dgvStats;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.RadioButton rbPower;
        private System.Windows.Forms.RadioButton rbLn;
        private System.Windows.Forms.RadioButton rbNone;
        private System.Windows.Forms.RadioButton rbLog10;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TextBox txtRegStd;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.TextBox txtDecCrit;
        private System.Windows.Forms.TextBox txtModel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPower;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnClearTable;
        private System.Windows.Forms.Button btnSaveTable;
        private System.Windows.Forms.Button btnIVDataValidation;
        private System.Windows.Forms.Button btnPlot;
        private System.Windows.Forms.Button btnImportObs;
        private System.Windows.Forms.Button btnImportIVs;
        private System.Windows.Forms.HelpProvider helpProvider1;
    }
}