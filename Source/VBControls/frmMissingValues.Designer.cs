namespace VBControls
{
    partial class frmMissingValues
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
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.rbActionDelRow = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.btnAction = new System.Windows.Forms.Button();
            this.txtReplaceWith = new System.Windows.Forms.TextBox();
            this.rbActionReplace = new System.Windows.Forms.RadioButton();
            this.cboCols = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnScan = new System.Windows.Forms.Button();
            this.txtFindVal = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnReturn = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.lblStatus = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.btnScan);
            this.groupBox1.Controls.Add(this.txtFindVal);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(238, 291);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Data Validation ";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.rbActionDelRow);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.btnAction);
            this.groupBox4.Controls.Add(this.txtReplaceWith);
            this.groupBox4.Controls.Add(this.rbActionReplace);
            this.groupBox4.Controls.Add(this.cboCols);
            this.groupBox4.Enabled = false;
            this.groupBox4.Location = new System.Drawing.Point(9, 119);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(223, 157);
            this.groupBox4.TabIndex = 25;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Action:";
            // 
            // rbActionDelRow
            // 
            this.rbActionDelRow.AutoSize = true;
            this.rbActionDelRow.Location = new System.Drawing.Point(13, 46);
            this.rbActionDelRow.Name = "rbActionDelRow";
            this.rbActionDelRow.Size = new System.Drawing.Size(81, 17);
            this.rbActionDelRow.TabIndex = 25;
            this.rbActionDelRow.Text = "Delete Row";
            this.rbActionDelRow.UseVisualStyleBackColor = true;
            this.rbActionDelRow.CheckedChanged += new System.EventHandler(this.rbActionDelRow_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(45, 69);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 13);
            this.label4.TabIndex = 23;
            this.label4.Text = "Take Action Within:";
            // 
            // btnAction
            // 
            this.btnAction.Location = new System.Drawing.Point(62, 123);
            this.btnAction.Name = "btnAction";
            this.btnAction.Size = new System.Drawing.Size(95, 23);
            this.btnAction.TabIndex = 4;
            this.btnAction.Text = "Take Action";
            this.btnAction.UseVisualStyleBackColor = true;
            this.btnAction.Click += new System.EventHandler(this.btnAction_Click);
            // 
            // txtReplaceWith
            // 
            this.txtReplaceWith.Location = new System.Drawing.Point(112, 22);
            this.txtReplaceWith.Name = "txtReplaceWith";
            this.txtReplaceWith.Size = new System.Drawing.Size(89, 20);
            this.txtReplaceWith.TabIndex = 24;
            this.txtReplaceWith.Validating += new System.ComponentModel.CancelEventHandler(this.txtReplaceWith_Validating);
            this.txtReplaceWith.Validated += new System.EventHandler(this.txtReplaceWith_Validated);
            // 
            // rbActionReplace
            // 
            this.rbActionReplace.AutoSize = true;
            this.rbActionReplace.Checked = true;
            this.rbActionReplace.Location = new System.Drawing.Point(13, 24);
            this.rbActionReplace.Name = "rbActionReplace";
            this.rbActionReplace.Size = new System.Drawing.Size(93, 17);
            this.rbActionReplace.TabIndex = 23;
            this.rbActionReplace.TabStop = true;
            this.rbActionReplace.Text = "Replace With:";
            this.rbActionReplace.UseVisualStyleBackColor = true;
            this.rbActionReplace.CheckedChanged += new System.EventHandler(this.rbActionReplace_CheckedChanged);
            // 
            // cboCols
            // 
            this.cboCols.FormattingEnabled = true;
            this.cboCols.Location = new System.Drawing.Point(43, 86);
            this.cboCols.Name = "cboCols";
            this.cboCols.Size = new System.Drawing.Size(149, 21);
            this.cboCols.TabIndex = 10;
            this.cboCols.SelectedIndexChanged += new System.EventHandler(this.cboCols_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "(Optional) Find:";
            // 
            // btnScan
            // 
            this.btnScan.Location = new System.Drawing.Point(68, 30);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(87, 36);
            this.btnScan.TabIndex = 12;
            this.btnScan.Text = "Scan";
            this.btnScan.UseVisualStyleBackColor = true;
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // txtFindVal
            // 
            this.txtFindVal.Location = new System.Drawing.Point(101, 81);
            this.txtFindVal.Name = "txtFindVal";
            this.txtFindVal.Size = new System.Drawing.Size(105, 20);
            this.txtFindVal.TabIndex = 21;
            this.txtFindVal.TextChanged += new System.EventHandler(this.txtFindVal_TextChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(138, 326);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnReturn
            // 
            this.btnReturn.Location = new System.Drawing.Point(50, 326);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.Size = new System.Drawing.Size(75, 23);
            this.btnReturn.TabIndex = 1;
            this.btnReturn.Text = "Return";
            this.btnReturn.UseVisualStyleBackColor = true;
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(18, 306);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 16);
            this.lblStatus.TabIndex = 16;
            // 
            // frmMissingValues
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(262, 355);
            this.ControlBox = false;
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnReturn);
            this.Name = "frmMissingValues";
            this.Text = "Validation";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnAction;
        private System.Windows.Forms.ComboBox cboCols;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.Button btnReturn;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton rbActionDelRow;
        private System.Windows.Forms.TextBox txtReplaceWith;
        private System.Windows.Forms.RadioButton rbActionReplace;
        private System.Windows.Forms.TextBox txtFindVal;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label lblStatus;
    }
}