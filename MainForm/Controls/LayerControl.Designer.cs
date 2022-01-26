namespace MainForm.Controls
{
    partial class LayerControl
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
            this.layerCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // layerCheckBox
            // 
            this.layerCheckBox.AutoSize = true;
            this.layerCheckBox.Location = new System.Drawing.Point(0, 3);
            this.layerCheckBox.Name = "layerCheckBox";
            this.layerCheckBox.Size = new System.Drawing.Size(61, 21);
            this.layerCheckBox.TabIndex = 0;
            this.layerCheckBox.Text = "layer";
            this.layerCheckBox.UseVisualStyleBackColor = true;
            this.layerCheckBox.CheckedChanged += new System.EventHandler(this.LayerCheckBoxCheckedChanged);
            // 
            // LayerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layerCheckBox);
            this.Name = "LayerControl";
            this.Size = new System.Drawing.Size(254, 38);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox layerCheckBox;
    }
}
