using System;
using System.Windows.Forms;
using SupportMapLibrary;

namespace MainForm.Controls
{
    public partial class LayerControl : UserControl
    {
        private Layer MapLayer { get; }
        public event EventHandler CheckedChanged;
        public LayerControl(Layer l)
        {
            InitializeComponent();
            MapLayer = l;
            layerCheckBox.Checked = true;
            layerCheckBox.Text = l.AlgorithmName.PadRight(20);
        }
        private void LayerCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            MapLayer.Visible = layerCheckBox.Checked;
            if(CheckedChanged !=null)
                CheckedChanged (this, EventArgs.Empty);
        }
    }
}
