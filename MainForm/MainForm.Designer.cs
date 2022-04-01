namespace MainForm
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveResultTableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainContainer = new System.Windows.Forms.SplitContainer();
            this.lblPercentError = new System.Windows.Forms.Label();
            this.percentErrUpDn = new System.Windows.Forms.NumericUpDown();
            this.btnProcess = new System.Windows.Forms.Button();
            this.btnSetScale = new System.Windows.Forms.Button();
            this.viewScaleUpDown = new System.Windows.Forms.NumericUpDown();
            this.lblViewScale = new System.Windows.Forms.Label();
            this.lblCurScaleValue = new System.Windows.Forms.Label();
            this.lblCurScale = new System.Windows.Forms.Label();
            this.rightSplitContainer = new System.Windows.Forms.SplitContainer();
            this.mapPictureBox = new System.Windows.Forms.PictureBox();
            this.resultTablePanel = new System.Windows.Forms.TableLayoutPanel();
            this.lblColor = new System.Windows.Forms.Label();
            this.lblBend = new System.Windows.Forms.Label();
            this.lblOutN = new System.Windows.Forms.Label();
            this.lblLayer = new System.Windows.Forms.Label();
            this.lblSave = new System.Windows.Forms.Label();
            this.lblOutScale = new System.Windows.Forms.Label();
            this.lblGhd = new System.Windows.Forms.Label();
            this.lblIsPercent = new System.Windows.Forms.Label();
            this.lblParam = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainContainer)).BeginInit();
            this.mainContainer.Panel1.SuspendLayout();
            this.mainContainer.Panel2.SuspendLayout();
            this.mainContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.percentErrUpDn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewScaleUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rightSplitContainer)).BeginInit();
            this.rightSplitContainer.Panel1.SuspendLayout();
            this.rightSplitContainer.Panel2.SuspendLayout();
            this.rightSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mapPictureBox)).BeginInit();
            this.resultTablePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.aboutToolStripMenuItem,
            this.saveResultTableToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1347, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(46, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(128, 26);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.OpenToolStripMenuItemClick);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(128, 26);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.SaveToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(128, 26);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(64, 24);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // saveResultTableToolStripMenuItem
            // 
            this.saveResultTableToolStripMenuItem.Name = "saveResultTableToolStripMenuItem";
            this.saveResultTableToolStripMenuItem.Size = new System.Drawing.Size(132, 24);
            this.saveResultTableToolStripMenuItem.Text = "Save result table";
            this.saveResultTableToolStripMenuItem.Click += new System.EventHandler(this.SaveResultTableToolStripMenuItemClick);
            // 
            // mainContainer
            // 
            this.mainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.mainContainer.Location = new System.Drawing.Point(0, 28);
            this.mainContainer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.mainContainer.Name = "mainContainer";
            // 
            // mainContainer.Panel1
            // 
            this.mainContainer.Panel1.AutoScroll = true;
            this.mainContainer.Panel1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.mainContainer.Panel1.Controls.Add(this.lblPercentError);
            this.mainContainer.Panel1.Controls.Add(this.percentErrUpDn);
            this.mainContainer.Panel1.Controls.Add(this.btnProcess);
            this.mainContainer.Panel1.Controls.Add(this.btnSetScale);
            this.mainContainer.Panel1.Controls.Add(this.viewScaleUpDown);
            this.mainContainer.Panel1.Controls.Add(this.lblViewScale);
            this.mainContainer.Panel1.Controls.Add(this.lblCurScaleValue);
            this.mainContainer.Panel1.Controls.Add(this.lblCurScale);
            // 
            // mainContainer.Panel2
            // 
            this.mainContainer.Panel2.Controls.Add(this.rightSplitContainer);
            this.mainContainer.Size = new System.Drawing.Size(1347, 710);
            this.mainContainer.SplitterDistance = 251;
            this.mainContainer.TabIndex = 1;
            // 
            // lblPercentError
            // 
            this.lblPercentError.AutoSize = true;
            this.lblPercentError.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblPercentError.Location = new System.Drawing.Point(153, 2);
            this.lblPercentError.Name = "lblPercentError";
            this.lblPercentError.Size = new System.Drawing.Size(24, 20);
            this.lblPercentError.TabIndex = 7;
            this.lblPercentError.Text = "%";
            // 
            // percentErrUpDn
            // 
            this.percentErrUpDn.DecimalPlaces = 2;
            this.percentErrUpDn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.percentErrUpDn.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.percentErrUpDn.Location = new System.Drawing.Point(183, 2);
            this.percentErrUpDn.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.percentErrUpDn.Name = "percentErrUpDn";
            this.percentErrUpDn.Size = new System.Drawing.Size(66, 27);
            this.percentErrUpDn.TabIndex = 6;
            this.percentErrUpDn.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btnProcess
            // 
            this.btnProcess.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProcess.Location = new System.Drawing.Point(0, 380);
            this.btnProcess.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(252, 42);
            this.btnProcess.TabIndex = 5;
            this.btnProcess.Text = "Process";
            this.btnProcess.UseVisualStyleBackColor = true;
            this.btnProcess.Click += new System.EventHandler(this.BtnProcessClick);
            // 
            // btnSetScale
            // 
            this.btnSetScale.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSetScale.Location = new System.Drawing.Point(183, 28);
            this.btnSetScale.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSetScale.Name = "btnSetScale";
            this.btnSetScale.Size = new System.Drawing.Size(66, 27);
            this.btnSetScale.TabIndex = 4;
            this.btnSetScale.Text = "Set";
            this.btnSetScale.UseVisualStyleBackColor = true;
            this.btnSetScale.Click += new System.EventHandler(this.BtnSetScaleClick);
            // 
            // viewScaleUpDown
            // 
            this.viewScaleUpDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewScaleUpDown.Increment = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.viewScaleUpDown.Location = new System.Drawing.Point(98, 28);
            this.viewScaleUpDown.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.viewScaleUpDown.Maximum = new decimal(new int[] {
            20000,
            0,
            0,
            0});
            this.viewScaleUpDown.Minimum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.viewScaleUpDown.Name = "viewScaleUpDown";
            this.viewScaleUpDown.Size = new System.Drawing.Size(81, 27);
            this.viewScaleUpDown.TabIndex = 3;
            this.viewScaleUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.viewScaleUpDown.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            this.viewScaleUpDown.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // lblViewScale
            // 
            this.lblViewScale.AutoSize = true;
            this.lblViewScale.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblViewScale.Location = new System.Drawing.Point(3, 33);
            this.lblViewScale.Name = "lblViewScale";
            this.lblViewScale.Size = new System.Drawing.Size(95, 20);
            this.lblViewScale.TabIndex = 2;
            this.lblViewScale.Text = "View scale:";
            // 
            // lblCurScaleValue
            // 
            this.lblCurScaleValue.AutoSize = true;
            this.lblCurScaleValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurScaleValue.Location = new System.Drawing.Point(94, 2);
            this.lblCurScaleValue.Name = "lblCurScaleValue";
            this.lblCurScaleValue.Size = new System.Drawing.Size(19, 20);
            this.lblCurScaleValue.TabIndex = 1;
            this.lblCurScaleValue.Text = "0";
            // 
            // lblCurScale
            // 
            this.lblCurScale.AutoSize = true;
            this.lblCurScale.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblCurScale.Location = new System.Drawing.Point(3, 2);
            this.lblCurScale.Name = "lblCurScale";
            this.lblCurScale.Size = new System.Drawing.Size(85, 20);
            this.lblCurScale.TabIndex = 0;
            this.lblCurScale.Text = "Cur.scale:";
            // 
            // rightSplitContainer
            // 
            this.rightSplitContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rightSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rightSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.rightSplitContainer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rightSplitContainer.Name = "rightSplitContainer";
            // 
            // rightSplitContainer.Panel1
            // 
            this.rightSplitContainer.Panel1.Controls.Add(this.mapPictureBox);
            this.rightSplitContainer.Panel1.Resize += new System.EventHandler(this.MapSplitContainerPanel1Resize);
            // 
            // rightSplitContainer.Panel2
            // 
            this.rightSplitContainer.Panel2.Controls.Add(this.resultTablePanel);
            this.rightSplitContainer.Size = new System.Drawing.Size(1092, 710);
            this.rightSplitContainer.SplitterDistance = 692;
            this.rightSplitContainer.TabIndex = 0;
            // 
            // mapPictureBox
            // 
            this.mapPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapPictureBox.Location = new System.Drawing.Point(0, 0);
            this.mapPictureBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.mapPictureBox.Name = "mapPictureBox";
            this.mapPictureBox.Size = new System.Drawing.Size(690, 708);
            this.mapPictureBox.TabIndex = 0;
            this.mapPictureBox.TabStop = false;
            this.mapPictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.MapPictureBoxPaint);
            this.mapPictureBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.MapPictureBoxMouseDoubleClick);
            this.mapPictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MapPictureBoxMouseDown);
            this.mapPictureBox.MouseEnter += new System.EventHandler(this.MapPictureBoxMouseEnter);
            this.mapPictureBox.MouseLeave += new System.EventHandler(this.MapPictureBoxMouseLeave);
            this.mapPictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MapPictureBoxMouseMove);
            this.mapPictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MapPictureBoxMouseUp);
            // 
            // resultTablePanel
            // 
            this.resultTablePanel.AutoSize = true;
            this.resultTablePanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.resultTablePanel.ColumnCount = 12;
            this.resultTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.17518F));
            this.resultTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.061831F));
            this.resultTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.072135F));
            this.resultTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.082642F));
            this.resultTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.082642F));
            this.resultTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 3.030915F));
            this.resultTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8.062234F));
            this.resultTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8.062234F));
            this.resultTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8.062234F));
            this.resultTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8.062234F));
            this.resultTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8.08244F));
            this.resultTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8.163264F));
            this.resultTablePanel.Controls.Add(this.lblColor, 1, 0);
            this.resultTablePanel.Controls.Add(this.lblBend, 8, 0);
            this.resultTablePanel.Controls.Add(this.lblOutN, 4, 0);
            this.resultTablePanel.Controls.Add(this.lblLayer, 0, 0);
            this.resultTablePanel.Controls.Add(this.lblSave, 2, 0);
            this.resultTablePanel.Controls.Add(this.lblOutScale, 3, 0);
            this.resultTablePanel.Controls.Add(this.lblGhd, 7, 0);
            this.resultTablePanel.Controls.Add(this.lblIsPercent, 5, 0);
            this.resultTablePanel.Controls.Add(this.lblParam, 6, 0);
            this.resultTablePanel.Controls.Add(this.label1, 9, 0);
            this.resultTablePanel.Controls.Add(this.label2, 10, 0);
            this.resultTablePanel.Controls.Add(this.label3, 11, 0);
            this.resultTablePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.resultTablePanel.Location = new System.Drawing.Point(0, 0);
            this.resultTablePanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.resultTablePanel.Name = "resultTablePanel";
            this.resultTablePanel.RowCount = 1;
            this.resultTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.resultTablePanel.Size = new System.Drawing.Size(394, 87);
            this.resultTablePanel.TabIndex = 1;
            // 
            // lblColor
            // 
            this.lblColor.AutoSize = true;
            this.lblColor.Location = new System.Drawing.Point(70, 1);
            this.lblColor.Name = "lblColor";
            this.lblColor.Size = new System.Drawing.Size(17, 85);
            this.lblColor.TabIndex = 1;
            this.lblColor.Text = "Color";
            // 
            // lblBend
            // 
            this.lblBend.AutoSize = true;
            this.lblBend.Location = new System.Drawing.Point(265, 1);
            this.lblBend.Name = "lblBend";
            this.lblBend.Size = new System.Drawing.Size(24, 51);
            this.lblBend.TabIndex = 6;
            this.lblBend.Text = "Bend";
            // 
            // lblOutN
            // 
            this.lblOutN.AutoSize = true;
            this.lblOutN.Location = new System.Drawing.Point(156, 1);
            this.lblOutN.Name = "lblOutN";
            this.lblOutN.Size = new System.Drawing.Size(27, 34);
            this.lblOutN.TabIndex = 4;
            this.lblOutN.Text = "Out N";
            // 
            // lblLayer
            // 
            this.lblLayer.AutoSize = true;
            this.lblLayer.Location = new System.Drawing.Point(4, 1);
            this.lblLayer.Name = "lblLayer";
            this.lblLayer.Size = new System.Drawing.Size(57, 34);
            this.lblLayer.TabIndex = 0;
            this.lblLayer.Text = "AlgmName";
            // 
            // lblSave
            // 
            this.lblSave.AutoSize = true;
            this.lblSave.Location = new System.Drawing.Point(94, 1);
            this.lblSave.Name = "lblSave";
            this.lblSave.Size = new System.Drawing.Size(17, 68);
            this.lblSave.TabIndex = 3;
            this.lblSave.Text = "Save";
            // 
            // lblOutScale
            // 
            this.lblOutScale.AutoSize = true;
            this.lblOutScale.Location = new System.Drawing.Point(121, 1);
            this.lblOutScale.Name = "lblOutScale";
            this.lblOutScale.Size = new System.Drawing.Size(28, 51);
            this.lblOutScale.TabIndex = 2;
            this.lblOutScale.Text = "OutScale";
            // 
            // lblGhd
            // 
            this.lblGhd.AutoSize = true;
            this.lblGhd.Location = new System.Drawing.Point(234, 1);
            this.lblGhd.Name = "lblGhd";
            this.lblGhd.Size = new System.Drawing.Size(19, 51);
            this.lblGhd.TabIndex = 5;
            this.lblGhd.Text = "GHD";
            // 
            // lblIsPercent
            // 
            this.lblIsPercent.AutoSize = true;
            this.lblIsPercent.Location = new System.Drawing.Point(191, 1);
            this.lblIsPercent.Name = "lblIsPercent";
            this.lblIsPercent.Size = new System.Drawing.Size(5, 17);
            this.lblIsPercent.TabIndex = 3;
            this.lblIsPercent.Text = "%";
            // 
            // lblParam
            // 
            this.lblParam.AutoSize = true;
            this.lblParam.Location = new System.Drawing.Point(203, 1);
            this.lblParam.Name = "lblParam";
            this.lblParam.Size = new System.Drawing.Size(21, 68);
            this.lblParam.TabIndex = 5;
            this.lblParam.Text = "Param";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(296, 1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 51);
            this.label1.TabIndex = 7;
            this.label1.Text = "Length";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(327, 1);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 51);
            this.label2.TabIndex = 8;
            this.label2.Text = "Angle";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(358, 1);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 68);
            this.label3.TabIndex = 9;
            this.label3.Text = "WeAvAngl";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1347, 738);
            this.Controls.Add(this.mainContainer);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "MainForm";
            this.Text = "AlgorithmsComparision";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.mainContainer.Panel1.ResumeLayout(false);
            this.mainContainer.Panel1.PerformLayout();
            this.mainContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainContainer)).EndInit();
            this.mainContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.percentErrUpDn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewScaleUpDown)).EndInit();
            this.rightSplitContainer.Panel1.ResumeLayout(false);
            this.rightSplitContainer.Panel2.ResumeLayout(false);
            this.rightSplitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rightSplitContainer)).EndInit();
            this.rightSplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mapPictureBox)).EndInit();
            this.resultTablePanel.ResumeLayout(false);
            this.resultTablePanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.SplitContainer mainContainer;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.SplitContainer rightSplitContainer;
        private System.Windows.Forms.PictureBox mapPictureBox;
        private System.Windows.Forms.Button btnSetScale;
        private System.Windows.Forms.NumericUpDown viewScaleUpDown;
        private System.Windows.Forms.Label lblViewScale;
        private System.Windows.Forms.Label lblCurScaleValue;
        private System.Windows.Forms.Label lblCurScale;
        private System.Windows.Forms.Button btnProcess;
        private System.Windows.Forms.TableLayoutPanel resultTablePanel;
        private System.Windows.Forms.Label lblLayer;
        private System.Windows.Forms.Label lblColor;
        private System.Windows.Forms.Label lblOutScale;
        private System.Windows.Forms.Label lblIsPercent;
        private System.Windows.Forms.Label lblOutN;
        private System.Windows.Forms.Label lblGhd;
        private System.Windows.Forms.Label lblBend;
        private System.Windows.Forms.Label lblParam;
        private System.Windows.Forms.Label lblSave;
        private System.Windows.Forms.ToolStripMenuItem saveResultTableToolStripMenuItem;
        private System.Windows.Forms.Label lblPercentError;
        private System.Windows.Forms.NumericUpDown percentErrUpDn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}

