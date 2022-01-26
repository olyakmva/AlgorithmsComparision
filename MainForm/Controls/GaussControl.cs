using System;
using System.Windows.Forms;

namespace MainForm.Controls
{
    public partial class GaussControl : UserControl
    {
        public int PointNumber
        {
            get { return Convert.ToInt32(pointNumberUpDown.Value); }
        }

        public int Sampling
        {
            get { return Convert.ToInt32(samplingUpDown.Value); }
        }
        
        public GaussControl()
        {
            InitializeComponent();
        }
    }
}
