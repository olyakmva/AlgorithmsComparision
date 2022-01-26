using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using AlgorithmsLibrary;
using DotSpatial .Data;
using MainForm.Controls;
using AlgorithmsLibrary.Features;
using SupportMapLibrary;

namespace MainForm
{
    public partial class MainForm : Form
    {
        private MapData _inputMap;
        private IFeatureSet _inputShape;
        private List<Layer> _layers;
        readonly GraphicsState _state = new GraphicsState();
        private double _currentScale;
        private List<AlgParamControl> _listCtrls;
        private const byte ResultTableColumnNumber = 12;
        private readonly string _applicationPath;
        private GaussControl _gaussControl;
        private MapData _gaussFilterMap;

        public MainForm()
        {
            InitializeComponent();
            _layers = new List<Layer>();
            _state.Scale = 2;
            mapPictureBox.MouseWheel += MapPictureBoxMouseWheel;
            CreateAlgmControls();
            _applicationPath = Environment.CurrentDirectory;
        }

        private void CreateAlgmControls()
        {
            int x = 0, y = 55, ctrlHeight = 140;
            _listCtrls = new List<AlgParamControl>();
            AlgParamControl douglasCtrl = new AlgParamControl()
            {
                Location = new Point(x, y),
                AlgmName = "DouglasPeuckerAlgm"
            };
            _listCtrls.Add(douglasCtrl);
            y += ctrlHeight;
            AlgParamControl liCtrl = new AlgParamControl()
            {
                AlgmName = "LiOpenshawAlgm",
                Location = new Point(x, y)
            };
            _listCtrls.Add(liCtrl);
            y += ctrlHeight;
            AlgParamControl visvWhyatCtrl = new AlgParamControl()
            {
                AlgmName = "VisvWhyattAlgm",
                Location = new Point(x, y)
            };
            y += ctrlHeight;
            _listCtrls.Add(visvWhyatCtrl);

            AlgParamControl sleeveFitCtrl = new AlgParamControl()
            {
                AlgmName = "SleeveFitAlgm",
                Location = new Point(x, y)
            };
            y += ctrlHeight;
            _listCtrls.Add(sleeveFitCtrl);

            
            mainContainer.Panel1.Controls .Add(douglasCtrl);
            mainContainer.Panel1.Controls.Add(liCtrl);
            mainContainer.Panel1 .Controls.Add(visvWhyatCtrl);
            mainContainer .Panel1.Controls.Add(sleeveFitCtrl);
           

            foreach (var ctrl in _listCtrls)
            {
                foreach (var otherCtrl in _listCtrls)
                {
                    if(ctrl == otherCtrl)
                        continue;
                    ctrl.CopyingParams += otherCtrl.OnCopyingParams;
                }
            }
         
            btnProcess.Location = new Point(x,y);
            y += 43;
            _gaussControl = new GaussControl {Location = new Point(x, y)};
            mainContainer.Panel1.Controls.Add(_gaussControl);
            btnFilter.Location = new Point(x, y+65);


        }
        private void OpenToolStripMenuItemClick(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Path.Combine(_applicationPath, "Data");
            openFileDialog1.Filter = @"shape files (*.shp)|*.shp|All files (*.*)|*.*";
            openFileDialog1.DefaultExt = "*.shp";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                try
                {
                    string shapeFileName = openFileDialog1.FileName;
                    _inputShape = FeatureSet.Open(shapeFileName);
                    _inputMap = Converter.ToMapData(_inputShape);
                    _layers.Clear();
                    Colors.Init();
                    ClearResultTable();
                    var l = new Layer(_inputMap);
                    _layers.Add(l);
                    AddNewRowToResultTable(l);
                    SetGraphicsParams(_inputMap);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, @"Error : " + ex.Message);
                    return;
                }
            string fileName = Path.GetFileName(openFileDialog1.FileName);
            Text = $@"Algorithm Comparision  {fileName} ";
             mapPictureBox.Invalidate();
        }

        private void ClearResultTable()
        {
            int end = resultTablePanel.Controls.Count;
            for (int i = ResultTableColumnNumber; i < end; i++)
            {
                resultTablePanel.Controls.RemoveAt(resultTablePanel.Controls.Count - 1);
            }
            resultTablePanel.RowCount = 1;
        }

        private void AddNewRowToResultTable(Layer l)
        {
            var ctrl = new LayerControl(l);
            ctrl.CheckedChanged += OnLayerVisibleChanged;
            var btnSave = new LayerSaveButton(l);
            var paramType = "t";
            if (l.Characteristics.IsPercent)
                paramType = "p";
            if (l.Characteristics.IsBend)
                paramType = "b";

            var colorBox = new PictureBox() { BackColor = Color.FromName(l.Color) };
            resultTablePanel.RowCount++;
            int rowIndex = resultTablePanel.RowCount - 1;
            resultTablePanel.Controls.Add(ctrl, 0, rowIndex);
            resultTablePanel.Controls.Add(colorBox, 1, rowIndex);
            resultTablePanel.Controls.Add(btnSave, 2, rowIndex);
            resultTablePanel.Controls.Add(new Label() {Text = l.OutScale.ToString()}, 3, rowIndex);
            resultTablePanel.Controls.Add(new Label() {Text = l.Characteristics.PointNumber.ToString()}, 4, rowIndex);
            resultTablePanel.Controls.Add(new Label() { Text = paramType }, 5, rowIndex);
            resultTablePanel.Controls.Add(new Label() { Text = l.Characteristics.ParamValue.ToString(CultureInfo.InvariantCulture) }, 6, rowIndex);
            resultTablePanel.Controls.Add(new Label() { Text = l.GenHausdDist.ToString(CultureInfo.InvariantCulture) }, 7, rowIndex);
            resultTablePanel.Controls.Add(new Label() { Text = l.Characteristics.BendNumber.ToString() }, 8, rowIndex);
            resultTablePanel.Controls.Add(new Label() { Text = l.Characteristics.Length.ToString(CultureInfo.InvariantCulture) }, 9, rowIndex);
            resultTablePanel.Controls.Add(new Label() { Text = Math.Round(l.Characteristics.AverageAngle).ToString(CultureInfo.InvariantCulture) }, 10, rowIndex);
            resultTablePanel.Controls.Add(new Label() { Text = Math.Round(l.Characteristics.WeightedAverageAngle).ToString(CultureInfo.InvariantCulture) }, 11, rowIndex);
        }

       private void SetGraphicsParams(MapData inputMap)
        {
            var l1 = inputMap.GetAllVertices();
            double xmin, xmax;
            double ymin, ymax;

            MinMaxValues.Compute(l1, out xmin, out ymin, out xmax, out ymax);
            var g = CreateGraphics();
            if ((xmax - xmin) / mapPictureBox.Width > (ymax - ymin) / mapPictureBox.Height)
            {
                _state.Scale = (xmax - xmin) / (mapPictureBox.Width - 40);
                _state.InitG(g, 1);
            }
            else
            {
                _state.Scale = (ymax - ymin) / (mapPictureBox.Height - 40);
                _state.InitG(g, 2);
            }
            _currentScale = Math.Truncate(_state.Scale * _state.PixelPerMm);
            lblCurScaleValue.Text = _currentScale .ToString(CultureInfo.InvariantCulture);
            _state.CenterX = xmin;
            _state.CenterY = ymax;
            _state.DefscaleX = (xmax - xmin);
            _state.DefscaleY = (ymax - ymin);
            _state.DefCenterX = xmin;
            _state.DefCenferY = ymax;
        }
        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var layer in _layers)
            {
                IFeatureSet fs = Converter.ToShape(layer.Map);
                fs.SaveAs(layer.AlgorithmName + ".shp", true);
            }
        }

        #region Отрисовка карты

        private void MapPictureBoxPaint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Color.White);

            foreach (var layer in _layers)
            {
                if (!layer.Visible)
                    continue;
                var c = Color.FromName(layer.Color) ; 
                var pen = new Pen(c, 1.75f);
                Display(g, layer.Map, pen);
            }
            g.Flush();
        }

        /// <summary>
        /// Отображение MapData md на графике g
        /// </summary>
        /// <param name="g"></param>
        /// <param name="md"></param>
        /// <param name="pen">Цвет в случае отображения нескольких MapData на одном picturebox</param>
        private void Display(Graphics g, MapData md, Pen pen)
        {
            //var brush = new SolidBrush(Color.DeepPink);
            foreach (var list in md.VertexList)
            {
                if (list.Count == 0)
                    continue;
                for (var j = 0; j < (list.Count - 1); j++)
                {
                    var pt1 = _state.GetPoint(list[j], mapPictureBox.Height - 1);
                    var pt2 = _state.GetPoint(list[j + 1], mapPictureBox.Height - 1);
                    //g.FillRectangle(brush, pt1.X, pt1.Y, 3, 3);
                    //if (list[j].Weight >= 5)
                    //{
                    //    if (list[j].Weight >= 25)
                    //        g.DrawRectangle(pen, pt1.X, pt1.Y, 3, 3);
                    //    else g.FillRectangle(brush, pt1.X, pt1.Y, 3, 3);
                    //}
                    g.DrawLine(pen, pt1, pt2);
                }
            }
        }
        private void MapPictureBoxMouseLeave(object sender, EventArgs e)
        {
            if (mapPictureBox.Focused)
                mapPictureBox.Parent.Focus();
        }

        private void MapPictureBoxMouseUp(object sender, MouseEventArgs e)
        {
            mapPictureBox.Invalidate();
        }

        private void MapPictureBoxMouseDown(object sender, MouseEventArgs e)
        {
            _state.Mousex = e.X;
            _state.Mousey = e.Y;
        }

        private void MapPictureBoxMouseEnter(object sender, EventArgs e)
        {
            if (!mapPictureBox.Focused)
                mapPictureBox.Focus();
        }

        private void MapPictureBoxMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button.HasFlag(MouseButtons.Left))
            {
                _state.CenterX += (_state.Mousex - e.X) * _state.Scale;
                _state.CenterY -= (_state.Mousey - e.Y) * _state.Scale;
                _state.Mousex = e.X;
                _state.Mousey = e.Y;
            }
        }
        private void MapPictureBoxMouseWheel(object sender, MouseEventArgs e)
        {
            var x = 0;
            if (e.Delta > 0)
                x = -1;
            else if (e.Delta < 0)
                x = 1;
            _state.Zoom(x, mapPictureBox.Width, mapPictureBox.Height);
            _currentScale = Math.Truncate(_state.Scale * _state.PixelPerMm);
            lblCurScaleValue.Text = _currentScale.ToString(CultureInfo.InvariantCulture);
            mapPictureBox.Invalidate();
        }
        private void MapSplitContainerPanel1Resize(object sender, EventArgs e)
        {
            mapPictureBox.Width = Width;
            mapPictureBox.Height = Height;
            mapPictureBox.Invalidate();
        }
        private void MapPictureBoxMouseDoubleClick(object sender, MouseEventArgs e)
        {
            _state.Scale = Math.Max(_state.DefscaleX / mapPictureBox.Width, _state.DefscaleY / mapPictureBox.Height);
            _state.CenterX = _state.DefCenterX;
            _state.CenterY = _state.DefCenferY;
            _currentScale = Math.Truncate(_state.Scale * _state.PixelPerMm);
            lblCurScaleValue.Text = _currentScale.ToString(CultureInfo.InvariantCulture);
            mapPictureBox.Invalidate();
        }
        private void BtnSetScaleClick(object sender, EventArgs e)
        {
            if (viewScaleUpDown.Text.Length <= 2)
            {
                MessageBox.Show(@"Задайте масштаб. ");
                return;
            }
            var viewScale = double.Parse(viewScaleUpDown.Text);
            _state.Scale = Math.Round(viewScale / _state.PixelPerMm);
            _currentScale = Math.Truncate(_state.Scale * _state.PixelPerMm);
            lblCurScaleValue.Text = _currentScale.ToString(CultureInfo.InvariantCulture);
            mapPictureBox.Invalidate();
        }


        #endregion

        private void BtnProcessClick(object sender, EventArgs e)
        {
            if (_inputMap == null)
            {
                MessageBox.Show(@"Please, load a map ");
                return;
            }
            foreach (var ctrl in _listCtrls)
            {
                if (!ctrl.IsChecked)
                    continue;
                var map = _inputMap.Clone();
                
                ISimplificationAlgm algm = ctrl.GetAlgorithm();
                if (ctrl.IsPercentParametr || ctrl.IsBendParametr)
                {
                    algm.Options.PointNumberGap = Convert.ToDouble(percentErrUpDn.Value);
                }
                algm.Run(map);
                
                var layerName = $"{ctrl.AlgmName}";
                var l = new Layer(map, layerName, algm.Options.OutScale)
                {
                    GenHausdDist = GenHausdorfDistance.Get(_inputMap, map)
                };
                l.Characteristics.ParamValue = Math.Round(algm.Options.OutParam);
                if (ctrl.IsPercentParametr)
                {
                    l.Characteristics.IsPercent = true;
                }
                if (ctrl.IsBendParametr)
                {
                    l.Characteristics.IsBend = true;
                }
                l.Characteristics.Length = Math.Round(l.Characteristics.Length / _layers[0].Characteristics.Length, 2);

                if (_gaussFilterMap != null)
                {
                    l.FilterModifHausdDistance = GenHausdorfDistance.Get(_gaussFilterMap, map);
                }
                _layers.Add(l);
                AddNewRowToResultTable(l);
            }
            mapPictureBox.Invalidate();
        }

        private void OnLayerVisibleChanged(object sender, EventArgs e)
        {
            mapPictureBox.Invalidate();
        }

        private void SaveResultTableToolStripMenuItemClick(object sender, EventArgs e)
        {
            string fileName = "ResultTable.txt";
            string outFolder = @"Output";
            if (!Directory.Exists(outFolder))
            {
                Directory.CreateDirectory(outFolder);
            }
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = @"txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog.DefaultExt = "*.txt";
           saveFileDialog.InitialDirectory = Path.Combine(_applicationPath, outFolder);
            saveFileDialog.FileName = fileName;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                var fileNameWithPath = saveFileDialog.FileName;
                using (var swriter = new StreamWriter(fileNameWithPath))
                {
                    swriter.WriteLine(Layer.GetDescription());
                    foreach (var lr in _layers)
                    {
                        swriter.WriteLine(lr.ToString());
                    }
                }
            }
        }

        private void BtnFilterClick(object sender, EventArgs e)
        {
               var filter  = new FilterLine(_gaussControl.PointNumber, _gaussControl.Sampling);
               var map = _inputMap.Clone();
                _gaussFilterMap = filter.Process(map);
            var layer = new Layer(map, "GaussFilter", 0)
            {
                GenHausdDist = GenHausdorfDistance.Get(_inputMap, _gaussFilterMap)
            };
            _layers.Add(layer);
                AddNewRowToResultTable(layer);
            mapPictureBox.Invalidate();
        }
    }

    public class GraphicsState
    {
        public double DefscaleX { get; set; }
        public double DefscaleY { get; set; }
        public double DefCenterX { get; set; }
        public double DefCenferY { get; set; }
        public int Mousex { get; set; }
        public int Mousey { get; set; }
        public double CenterX { get; set; }//центр, относительно которого вводятся координаты
        public double CenterY { get; set; }
        public double Scale { get; set; }//Масштаб метр на пиксель
        public double PixelPerMm { get; set; }

        /// <summary>
        /// Перевод вершины в координаты для рисования
        /// </summary>
        /// <param name="v">Вершина</param>
        /// <param name="height">Высота PictureBox</param>
        /// <returns></returns>
        public Point GetPoint(MapPoint v, int height)
        {
            var x = (int)((v.X - CenterX) / Scale);
            var y = (int)((height - (v.Y - CenterY)) / Scale);
            return new Point(x, y);
        }
        /// <summary>
        /// Изменение масштаба
        /// </summary>
        /// <param name="z"></param>
        /// <param name="sizex">Ширина picturebox</param>
        /// <param name="sizey">Высота picturebox</param>
        public void Zoom(int z, int sizex, int sizey)
        {
            if (z > 0)
                Scale *= Math.Pow(2, z);
            double temp = sizex * z;
            CenterX -= temp / 4 * Scale;
            temp = sizey * z;
            CenterY += temp / 4 * Scale;
            if (z < 0)
                Scale *= Math.Pow(2, z);
        }

        public void InitG(Graphics g, int xy)
        {
            const double mmPerInch = 25.41;
            if (xy == 1)
                PixelPerMm = g.DpiX / mmPerInch;
            else PixelPerMm = g.DpiY / mmPerInch;
        }

    }
}
