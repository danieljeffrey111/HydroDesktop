namespace MLRPrediction
{
    partial class frmPLSPrediction
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtModel = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panelIVs = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dgvVariables = new System.Windows.Forms.DataGridView();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.dgvObs = new System.Windows.Forms.DataGridView();
            this.dgvStats = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.txtDecCrit = new System.Windows.Forms.TextBox();
            this.txtRegStd = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnChange = new System.Windows.Forms.Button();
            this.btnMakePredictions = new System.Windows.Forms.Button();
            this.btnImportObs = new System.Windows.Forms.Button();
            this.btnPlot = new System.Windows.Forms.Button();
            this.btnImportTable = new System.Windows.Forms.Button();
            this.btnSaveTable = new System.Windows.Forms.Button();
            this.btnClearTable = new System.Windows.Forms.Button();
            this.btnImportIVs = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
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
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.panel1.Controls.Add(this.txtModel);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(967, 96);
            this.panel1.TabIndex = 0;
            // 
            // txtModel
            // 
            this.txtModel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtModel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.txtModel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtModel.Location = new System.Drawing.Point(89, 15);
            this.txtModel.Multiline = true;
            this.txtModel.Name = "txtModel";
            this.txtModel.ReadOnly = true;
            this.txtModel.Size = new System.Drawing.Size(850, 70);
            this.txtModel.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(14, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Model: ";
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
            this.panel2.Size = new System.Drawing.Size(967, 257);
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
            this.panelIVs.Size = new System.Drawing.Size(891, 251);
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
            this.splitContainer1.Size = new System.Drawing.Size(891, 251);
            this.splitContainer1.SplitterDistance = 500;
            this.splitContainer1.TabIndex = 0;
            this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
            // 
            // dgvVariables
            // 
            this.dgvVariables.AllowUserToOrderColumns = true;
            this.dgvVariables.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVariables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvVariables.Location = new System.Drawing.Point(0, 0);
            this.dgvVariables.MultiSelect = false;
            this.dgvVariables.Name = "dgvVariables";
            this.dgvVariables.Size = new System.Drawing.Size(500, 251);
            this.dgvVariables.TabIndex = 2;
            this.dgvVariables.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvVariables_RowEnter);
            this.dgvVariables.RowLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvVariables_RowLeave);
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
            this.splitContainer2.Size = new System.Drawing.Size(387, 251);
            this.splitContainer2.SplitterDistance = 129;
            this.splitContainer2.TabIndex = 0;
            this.splitContainer2.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer2_SplitterMoved);
            // 
            // dgvObs
            // 
            this.dgvObs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvObs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvObs.Location = new System.Drawing.Point(0, 0);
            this.dgvObs.MultiSelect = false;
            this.dgvObs.Name = "dgvObs";
            this.dgvObs.Size = new System.Drawing.Size(129, 251);
            this.dgvObs.TabIndex = 3;
            this.dgvObs.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvObs_CellEndEdit);
            this.dgvObs.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvObs_RowEnter);
            this.dgvObs.RowLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvObs_RowLeave);
            // 
            // dgvStats
            // 
            this.dgvStats.AllowUserToOrderColumns = true;
            this.dgvStats.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStats.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvStats.Location = new System.Drawing.Point(0, 0);
            this.dgvStats.MultiSelect = false;
            this.dgvStats.Name = "dgvStats";
            this.dgvStats.ReadOnly = true;
            this.dgvStats.Size = new System.Drawing.Size(254, 251);
            this.dgvStats.TabIndex = 4;
            this.dgvStats.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStats_RowEnter);
            this.dgvStats.RowLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStats_RowLeave);
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
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.panel3.Controls.Add(this.label3);
            this.panel3.Location = new System.Drawing.Point(30, 141);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(86, 55);
            this.panel3.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 40);
            this.label3.TabIndex = 0;
            this.label3.Text = "Current\r\nValues";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.White;
            this.panel4.Controls.Add(this.txtDecCrit);
            this.panel4.Controls.Add(this.txtRegStd);
            this.panel4.Controls.Add(this.label5);
            this.panel4.Controls.Add(this.label4);
            this.panel4.Location = new System.Drawing.Point(122, 135);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(168, 67);
            this.panel4.TabIndex = 4;
            // 
            // txtDecCrit
            // 
            this.txtDecCrit.Location = new System.Drawing.Point(96, 37);
            this.txtDecCrit.Name = "txtDecCrit";
            this.txtDecCrit.ReadOnly = true;
            this.txtDecCrit.Size = new System.Drawing.Size(62, 20);
            this.txtDecCrit.TabIndex = 3;
            // 
            // txtRegStd
            // 
            this.txtRegStd.Location = new System.Drawing.Point(12, 37);
            this.txtRegStd.Name = "txtRegStd";
            this.txtRegStd.ReadOnly = true;
            this.txtRegStd.Size = new System.Drawing.Size(62, 20);
            this.txtRegStd.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(93, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 26);
            this.label5.TabIndex = 1;
            this.label5.Text = "Decision\r\nCriterion\r\n";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 26);
            this.label4.TabIndex = 0;
            this.label4.Text = "Regulatory\r\nStandard";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnChange
            // 
            this.btnChange.Enabled = false;
            this.btnChange.Location = new System.Drawing.Point(295, 157);
            this.btnChange.Name = "btnChange";
            this.btnChange.Size = new System.Drawing.Size(63, 23);
            this.btnChange.TabIndex = 5;
            this.btnChange.Text = "Change";
            this.btnChange.UseVisualStyleBackColor = true;
            this.btnChange.Click += new System.EventHandler(this.btnChange_Click);
            // 
            // btnMakePredictions
            // 
            this.btnMakePredictions.BackColor = System.Drawing.SystemColors.Control;
            this.btnMakePredictions.Location = new System.Drawing.Point(472, 162);
            this.btnMakePredictions.Name = "btnMakePredictions";
            this.btnMakePredictions.Size = new System.Drawing.Size(75, 39);
            this.btnMakePredictions.TabIndex = 6;
            this.btnMakePredictions.Text = "Make\r\nPredictions";
            this.btnMakePredictions.UseVisualStyleBackColor = false;
            this.btnMakePredictions.Click += new System.EventHandler(this.btnMakePredictions_Click);
            // 
            // btnImportObs
            // 
            this.btnImportObs.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnImportObs.Location = new System.Drawing.Point(472, 503);
            this.btnImportObs.Name = "btnImportObs";
            this.btnImportObs.Size = new System.Drawing.Size(75, 23);
            this.btnImportObs.TabIndex = 8;
            this.btnImportObs.Text = "Import Obs";
            this.btnImportObs.UseVisualStyleBackColor = true;
            this.btnImportObs.Click += new System.EventHandler(this.btnImportObs_Click);
            // 
            // btnPlot
            // 
            this.btnPlot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPlot.Enabled = false;
            this.btnPlot.Location = new System.Drawing.Point(766, 503);
            this.btnPlot.Name = "btnPlot";
            this.btnPlot.Size = new System.Drawing.Size(75, 23);
            this.btnPlot.TabIndex = 9;
            this.btnPlot.Text = "Plot";
            this.btnPlot.UseVisualStyleBackColor = true;
            // 
            // btnImportTable
            // 
            this.btnImportTable.BackColor = System.Drawing.SystemColors.Control;
            this.btnImportTable.Enabled = false;
            this.btnImportTable.Location = new System.Drawing.Point(647, 192);
            this.btnImportTable.Name = "btnImportTable";
            this.btnImportTable.Size = new System.Drawing.Size(75, 29);
            this.btnImportTable.TabIndex = 10;
            this.btnImportTable.Text = "Import Table";
            this.btnImportTable.UseVisualStyleBackColor = false;
            this.btnImportTable.Click += new System.EventHandler(this.btnImportTable_Click);
            // 
            // btnSaveTable
            // 
            this.btnSaveTable.BackColor = System.Drawing.SystemColors.Control;
            this.btnSaveTable.Enabled = false;
            this.btnSaveTable.Location = new System.Drawing.Point(738, 192);
            this.btnSaveTable.Name = "btnSaveTable";
            this.btnSaveTable.Size = new System.Drawing.Size(75, 29);
            this.btnSaveTable.TabIndex = 11;
            this.btnSaveTable.Text = "Save Table";
            this.btnSaveTable.UseVisualStyleBackColor = false;
            this.btnSaveTable.Click += new System.EventHandler(this.btnSaveTable_Click);
            // 
            // btnClearTable
            // 
            this.btnClearTable.BackColor = System.Drawing.SystemColors.Control;
            this.btnClearTable.Enabled = false;
            this.btnClearTable.Location = new System.Drawing.Point(832, 192);
            this.btnClearTable.Name = "btnClearTable";
            this.btnClearTable.Size = new System.Drawing.Size(75, 29);
            this.btnClearTable.TabIndex = 12;
            this.btnClearTable.Text = "Clear Table";
            this.btnClearTable.UseVisualStyleBackColor = false;
            this.btnClearTable.Click += new System.EventHandler(this.btnClearTable_Click);
            // 
            // btnImportIVs
            // 
            this.btnImportIVs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnImportIVs.Location = new System.Drawing.Point(148, 502);
            this.btnImportIVs.Name = "btnImportIVs";
            this.btnImportIVs.Size = new System.Drawing.Size(75, 23);
            this.btnImportIVs.TabIndex = 13;
            this.btnImportIVs.Text = "Import IVs";
            this.btnImportIVs.UseVisualStyleBackColor = true;
            this.btnImportIVs.Click += new System.EventHandler(this.btnImportIVs_Click);
            // 
            // frmPLSPrediction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(991, 537);
            this.Controls.Add(this.btnImportIVs);
            this.Controls.Add(this.btnClearTable);
            this.Controls.Add(this.btnSaveTable);
            this.Controls.Add(this.btnImportTable);
            this.Controls.Add(this.btnPlot);
            this.Controls.Add(this.btnImportObs);
            this.Controls.Add(this.btnMakePredictions);
            this.Controls.Add(this.btnChange);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmPLSPrediction";
            this.Text = "PLS Prediction";
            this.Load += new System.EventHandler(this.frmMLRPrediction_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
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
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtModel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TextBox txtDecCrit;
        private System.Windows.Forms.TextBox txtRegStd;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnChange;
        private System.Windows.Forms.Button btnMakePredictions;
        private System.Windows.Forms.Button btnImportObs;
        private System.Windows.Forms.Button btnPlot;
        private System.Windows.Forms.Button btnImportTable;
        private System.Windows.Forms.Button btnSaveTable;
        private System.Windows.Forms.Button btnClearTable;
        private System.Windows.Forms.Panel panelIVs;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dgvVariables;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.DataGridView dgvObs;
        private System.Windows.Forms.DataGridView dgvStats;
        private System.Windows.Forms.Button btnImportIVs;
    }
}