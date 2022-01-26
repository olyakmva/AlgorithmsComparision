using System;
using System.IO;
using System.Windows.Forms;
using DotSpatial.Data;
using SupportMapLibrary;

namespace MainForm.Controls
{
    public class LayerSaveButton:Button
    {
        private readonly Layer _layer;
        public LayerSaveButton(Layer l)
        {
            Text = @"Save";
            _layer = l;
            Click += LayerSaveButtonClick;
        }

        public sealed override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        private void LayerSaveButtonClick(object sender, EventArgs e)
        {
            string fileName= _layer.AlgorithmName+_layer.OutScale +".shp";
            string outFolder = @"Output";
            if (!Directory.Exists(outFolder))
            {
                Directory.CreateDirectory(outFolder);
            }
            var fileNameWithPath= Path.Combine(outFolder, fileName);
            IFeatureSet fs = Converter.ToShape(_layer.Map);
            fs.SaveAs(fileNameWithPath , true);
        }
    }
}
