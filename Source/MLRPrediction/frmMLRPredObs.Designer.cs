namespace MLRPrediction
{
    partial class frmMLRPredObs
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
            this.mlrPredObs1 = new VBControls.MLRPredObs();
            this.btnClose = new System.Windows.Forms.Button();
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            this.SuspendLayout();
            // 
            // mlrPredObs1
            // 
            this.mlrPredObs1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.mlrPredObs1.Location = new System.Drawing.Point(12, 12);
            this.mlrPredObs1.Name = "mlrPredObs1";
            this.mlrPredObs1.PowerExponent = double.NaN;
            this.mlrPredObs1.Size = new System.Drawing.Size(678, 345);
            this.mlrPredObs1.TabIndex = 0;
            this.mlrPredObs1.Transform = VBTools.Globals.DependentVariableTransforms.none;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Location = new System.Drawing.Point(318, 374);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmMLRPredObs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(711, 404);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.mlrPredObs1);
            this.Name = "frmMLRPredObs";
            this.Text = "Plot";
            this.Load += new System.EventHandler(this.frmMLRPredObs_Load);
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.frmMLRPredObs_HelpRequested);
            this.ResumeLayout(false);

        }

        #endregion

        private VBControls.MLRPredObs mlrPredObs1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.HelpProvider helpProvider1;
    }
}