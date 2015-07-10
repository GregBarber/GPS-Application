namespace MapViewer
{
    partial class TimeDistanceFilterForm
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
            this.saveButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.deltaDistanceNum = new System.Windows.Forms.NumericUpDown();
            this.deltaTimeNum = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.deltaDistanceNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deltaTimeNum)).BeginInit();
            this.SuspendLayout();
            // 
            // saveButton
            // 
            this.saveButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.saveButton.Location = new System.Drawing.Point(162, 134);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(96, 23);
            this.saveButton.TabIndex = 0;
            this.saveButton.Text = "Save Track";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(167, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Maximum delta time (sec)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(201, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Minimum delta distance (miles)";
            // 
            // deltaDistanceNum
            // 
            this.deltaDistanceNum.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.deltaDistanceNum.Location = new System.Drawing.Point(233, 81);
            this.deltaDistanceNum.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.deltaDistanceNum.Name = "deltaDistanceNum";
            this.deltaDistanceNum.Size = new System.Drawing.Size(120, 22);
            this.deltaDistanceNum.TabIndex = 5;
            // 
            // deltaTimeNum
            // 
            this.deltaTimeNum.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.deltaTimeNum.Location = new System.Drawing.Point(233, 28);
            this.deltaTimeNum.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.deltaTimeNum.Name = "deltaTimeNum";
            this.deltaTimeNum.Size = new System.Drawing.Size(120, 22);
            this.deltaTimeNum.TabIndex = 6;
            // 
            // TimeDistanceFilterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(377, 181);
            this.Controls.Add(this.deltaTimeNum);
            this.Controls.Add(this.deltaDistanceNum);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.saveButton);
            this.Name = "TimeDistanceFilterForm";
            this.Text = "TimeDistanceFilterForm";
            ((System.ComponentModel.ISupportInitialize)(this.deltaDistanceNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deltaTimeNum)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown deltaDistanceNum;
        private System.Windows.Forms.NumericUpDown deltaTimeNum;
    }
}