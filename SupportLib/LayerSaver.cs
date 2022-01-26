using System.Collections.Generic;
using System.IO;
using AlgorithmsLibrary.Features;

namespace SupportMapLibrary
{
    public static class LayerSaver
    {
        public static void ToFile(string outTableName, List<Layer> layers, string savePath)
        {
            using (var swriter = new StreamWriter(outTableName, true))
            {
                swriter.WriteLine("Name;GHD;FMHD;{0}", CommonLayerCharacteristics.GetDescription());
                foreach (var lr in layers)
                {
                    swriter.Write("{0};{1};{2};",lr.AlgorithmName,lr.GenHausdDist, lr.FilterModifHausdDistance );
                    swriter.WriteLine(lr.Characteristics.ToString());
                }
            }
            using (var swriter = new StreamWriter(savePath + "\\bend_results.txt", true))
            {
                swriter.WriteLine("Name;{0}", MapFeatures.GetDescription("Bend"));
                foreach (var lr in layers)
                {
                    swriter.Write(lr.AlgorithmName + ";");
                    swriter.WriteLine(lr.BendFeachure);
                }
            }
            using (var swriter = new StreamWriter(savePath + "\\triplet_results.txt", true))
            {
                swriter.WriteLine("Name;{0}", MapFeatures.GetDescription("Triplet"));
                foreach (var lr in layers)
                {
                    swriter.Write(lr.AlgorithmName + ";");
                    swriter.WriteLine(lr.TripletFeachure);
                }
            }
            using (var swriter = new StreamWriter(savePath + "\\tripletToBend_results.txt", true))
            {
                swriter.WriteLine("Name;{0}", MapFeatures.GetDescription("TripletToBend"));
                foreach (var lr in layers)
                {
                    swriter.Write(lr.AlgorithmName + ";");
                    swriter.WriteLine(lr.TripletToBend);
                }
            }
        }
    }
}
