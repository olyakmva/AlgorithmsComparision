using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using AlgorithmsLibrary;
using AlgorithmsLibrary.Features;
using DotSpatial.Data;
using SupportMapLibrary;

namespace BatchProcessing
{
    class Program
    {
        static void Main()
        {
            var set = new Settings
            {
                ErrorPercentValue = 1,
                Mode = ReducedMode.Points,
                IterationQuantity = 4,
                ReducedValues = new double[] {2, 4, 8, 16},
                DouglsSettings = new AlgmSettings {Params = new [] {400, 1500, 2500, 5500}},
                LiSettings = new AlgmSettings {Params = new [] {3000, 5000, 12000, 24000}},
                VisWSettings = new AlgmSettings {Params = new [] {800, 2000, 3000, 6000}},
                SleeveFitSettings = new AlgmSettings {Params = new [] {400, 1500, 2500, 5500}},
                FourierSettings = new AlgmSettings{ Params = new [] {1000, 1200, 1200, 1200}}
            };

            var fileName = "settings.xml";
            var xmlSer = new XmlSerializer(typeof(Settings));
            using (Stream fStream = new FileStream(fileName,
               FileMode.Create, FileAccess.Write, FileShare.None))
            {
                xmlSer.Serialize(fStream, set);
            }
            using (Stream fStream = new FileStream(fileName,
                FileMode.Open, FileAccess.Read, FileShare.None))
            {
                set = (Settings)xmlSer.Deserialize(fStream);
            }

            ComputeFeachers(set);

        }

        static void ComputeFeachers(Settings st)
        {
            string applicationPath = Environment.CurrentDirectory;
            // СЮДА ПУТЬ к данным
            var dataPath = Path.Combine(applicationPath, "Data");
            DirectoryInfo dir = new DirectoryInfo(dataPath);
            var dataFiles = dir.GetFiles("*.shp");
            string outFolder = @"Output";
            if (!Directory.Exists(outFolder))
            {
                Directory.CreateDirectory(outFolder);
            }
            var saveFilePath = Path.Combine(applicationPath, "Output");

            string outTableFolder = @"Tables";
            if (!Directory.Exists(outTableFolder))
            {
                Directory.CreateDirectory(outTableFolder);
            }
            var savePath = Path.Combine(applicationPath, outTableFolder);
            var outTableName = savePath + "\\results.txt";

            if (st.Mode == ReducedMode.Points)
            {
                ISimplificationAlgm[] algms =
                {
                    new DouglasPeuckerAlgmWithCriterion(new PointPercentCriterion()),
                    new SleeveFitWithCriterion(new PointPercentCriterion()),
                    new VisWhyattAlgmWithPercent(),
                    new LiOpenshawWithCriterion(new PointPercentCriterion()),
                    new FourierDescAlgm()
                };
                var paramValues = new List<int[]>
                {
                    st.DouglsSettings.Params,
                    st.SleeveFitSettings.Params,
                    st.VisWSettings.Params,
                    st.LiSettings.Params,
                    st.FourierSettings.Params
                };
               
                var layers = new List<Layer>();
                foreach (var file in dataFiles)
                {
                    var inputShape = FeatureSet.Open(file.FullName);
                    var inputMap = Converter.ToMapData(inputShape);
                    var inp = inputMap.Clone();
                   
                    Console.WriteLine(file.Name);

                    var first = new Layer(inputMap);
                    layers.Add(first);
                    int inputLayerNumber = 0;
                    int i = 0;
                    // вызвать все алгоритмы  и посчитать характеристики
                    foreach (var algm in algms)
                    {
                        int j = 0;
                        foreach (var k in st.ReducedValues)
                        {
                            var param = new SimplificationAlgmParameters
                            {
                                OutScale = paramValues[i][j],
                                PointNumberGap = st.ErrorPercentValue,
                                RemainingPercent = k
                            };
                            j++;
                            var map = inputMap.Clone();
                            algm.Options = param;
                            algm.Run(map);
                            string layerName = algm.ToString().Substring(18) + file.Name.Remove(file.Name.Length - 4);
                            var l = new Layer(map, layerName, algm.Options.OutScale)
                            {
                                GenHausdDist = Math.Round(GenHausdorfDistance.Get(inputMap, map)),
                            };
                            l.Characteristics.ParamValue = Math.Round(algm.Options.OutParam);
                            l.Characteristics.IsPercent = true;
                            l.Characteristics.Length = Math.Round((double) l.Characteristics.Length / layers[inputLayerNumber].Characteristics.Length, 2);
                            layers.Add(l);
                            Console.WriteLine(k);
                            IFeatureSet fs = Converter.ToShape(l.Map);
                            var fn = saveFilePath + "\\" + l.AlgorithmName + j + ".shp";
                            fs.SaveAs(fn, true);
                        }
                        i++;
                    }
                    LayerSaver.ToFile(outTableName, layers, savePath);
                    layers.Clear();
                }
            }      
        }
    }
}


