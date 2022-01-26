using System;
using System.Globalization;
using System.Windows.Forms;
using AlgorithmsLibrary;

namespace MainForm.Controls
{
    public partial class AlgParamControl : UserControl
    {
        private ISimplificationAlgm _algm;
        public event EventHandler CopyingParams ;
        
        private double Tolerance
        {
            get => Convert.ToDouble(paramUpDown.Value);
            set => paramUpDown.Value = Convert.ToDecimal( value);
        }

        private double Percent
        {
            get => double.Parse(percentUpDown.Text);
            set => percentUpDown.Value = Convert.ToDecimal( value);
        }

        public string AlgmName
        {
            set => lblName.Text = value;
            get => lblName.Text;
        }

        public double OutScale
        {
            get => double.Parse(scaleUpDown.Text);
            set => scaleUpDown.Text = value.ToString(CultureInfo.InvariantCulture);
        }

        public bool IsChecked
        {
            get => checkRun.Checked;
            private set => checkRun.Checked = value;
        }

        public bool IsPercentParametr
        {
            get => checkBoxPercent.Checked;
            set => checkBoxPercent.Checked = value;
        }

        public bool IsBendParametr
        {
            get => checkBoxBend.Checked;
            set => checkBoxBend.Checked = value;
        }

        public double BendReduction
        {
            get => Convert.ToDouble(bendUpDown.Value);
            set => bendUpDown.Value = Convert.ToDecimal(value);
        }

        public AlgParamControl()
        {
            InitializeComponent();
        }

       
        public ISimplificationAlgm GetAlgorithm()
        {
            var p = new SimplificationAlgmParameters();
            _algm = AlgmFabrics.GetAlgmByNameAndParam(AlgmName, IsPercentParametr, IsBendParametr);
            p.Tolerance = Math.Truncate(OutScale * Convert.ToDouble(paramUpDown.Value));
            p.RemainingPercent = double.Parse(percentUpDown.Text);
            p.OutScale = Convert.ToInt32(OutScale);
            p.BendReduction = Convert.ToDouble(bendUpDown.Value);
            _algm.Options = p;
            
            return _algm;
        }

        
        private void BtnCopyClick(object sender, EventArgs e)
        {
            CopyingParams?.Invoke(this, EventArgs.Empty);
        }

        public void OnCopyingParams(object sender, EventArgs e)
        {
            AlgParamControl ctrl = (AlgParamControl) sender;
            Tolerance  = ctrl.Tolerance;
            Percent = ctrl.Percent;
            OutScale = ctrl.OutScale;
            IsChecked = ctrl.IsChecked;
            IsPercentParametr = ctrl.IsPercentParametr;
            IsBendParametr = ctrl.IsBendParametr;
            BendReduction = ctrl.BendReduction;

        }

        private void checkBoxPercent_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxPercent.Checked)
            {
                checkBoxBend.Checked = false;
            }
        }

        private void checkBoxBend_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxBend.Checked)
            {
                checkBoxPercent.Checked = false;
            }
        }
    }
}
