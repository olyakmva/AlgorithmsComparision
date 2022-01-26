namespace MainForm.Controls
{
    partial class GaussControl
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
            this.lblSigma = new System.Windows.Forms.Label();
            this.pointNumberUpDown = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.samplingUpDown = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pointNumberUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplingUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // lblSigma
            // 
            this.lblSigma.AutoSize = true;
            this.lblSigma.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblSigma.Location = new System.Drawing.Point(3, 0);
            this.lblSigma.Name = "lblSigma";
            this.lblSigma.Size = new System.Drawing.Size(63, 24);
            this.lblSigma.TabIndex = 0;
            this.lblSigma.Text = "Sigma";
            // 
            // pointNumberUpDown
            // 
            this.pointNumberUpDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.pointNumberUpDown.Location = new System.Drawing.Point(89, 3);
            this.pointNumberUpDown.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.pointNumberUpDown.Name = "pointNumberUpDown";
            this.pointNumberUpDown.Size = new System.Drawing.Size(104, 27);
            this.pointNumberUpDown.TabIndex = 1;
            this.pointNumberUpDown.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(199, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 18);
            this.label1.TabIndex = 2;
            this.label1.Text = "points";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(3, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Sampling";
            // 
            // samplingUpDown
            // 
            this.samplingUpDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.samplingUpDown.Increment = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.samplingUpDown.Location = new System.Drawing.Point(89, 36);
            this.samplingUpDown.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.samplingUpDown.Name = "samplingUpDown";
            this.samplingUpDown.Size = new System.Drawing.Size(104, 27);
            this.samplingUpDown.TabIndex = 4;
            this.samplingUpDown.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(199, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(19, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "m";
            // 
            // GaussControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.samplingUpDown);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pointNumberUpDown);
            this.Controls.Add(this.lblSigma);
            this.Name = "GaussControl";
            this.Size = new System.Drawing.Size(249, 65);
            ((System.ComponentModel.ISupportInitialize)(this.pointNumberUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplingUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSigma;
        private System.Windows.Forms.NumericUpDown pointNumberUpDown;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown samplingUpDown;
        private System.Windows.Forms.Label label3;
    }
}
