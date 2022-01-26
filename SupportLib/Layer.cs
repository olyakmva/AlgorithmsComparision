using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using AlgorithmsLibrary;
using AlgorithmsLibrary.Features;

namespace SupportMapLibrary
{
    public class CommonLayerCharacteristics
    {
        public int PointNumber { get; private set; }
       
        public bool IsPercent { get; set; }
        public bool IsBend { get; set; }
        public double ParamValue { get; set; }
        public int BendNumber { get; set; }
        public double AverageAngle { get; set; }
        public double Length { get; set; }
        public double WeightedAverageAngle { get; set; }
        public double Simplicity { get; set; }
        public double Smoothness { get; set; }
        
        public CommonLayerCharacteristics(MapData map)
        {
            PointNumber = map.Count;
            BendNumber = BendComputation.Get(map);
            Length = LengthComputation.Get(map);
            AverageAngle = AverageAngleComputation.Get(map);
            WeightedAverageAngle = AverageAngleComputation.GetWeightedAngle(map);
            Simplicity = Math.Round((double)BendNumber / (PointNumber - 2), 3);
            Smoothness = Math.Round(1 - WeightedAverageAngle / 180, 3);
        }

        public static string GetDescription()
        {
            return "PointNumber;%|p|t;ParamValue;BendNumber;Length;AverageAngle;WeightAveAngle;Simplicity;Smoothness;";
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(PointNumber.ToString());
            sb.Append(";");
            if (IsPercent)
                sb.Append("p;");
            else if (IsBend) sb.Append("b;");
            else sb.Append("t;");
            sb.Append(Math.Round(ParamValue).ToString(CultureInfo.InvariantCulture));
            sb.Append(";");
            sb.Append(BendNumber.ToString());
            sb.Append(";");
            sb.Append(Length.ToString(CultureInfo.InvariantCulture));
            sb.Append(";");
            sb.Append(AverageAngle.ToString(CultureInfo.InvariantCulture));
            sb.Append(";");
            sb.Append(WeightedAverageAngle.ToString(CultureInfo.InvariantCulture));
            sb.Append(";");
            sb.Append(Simplicity);
            sb.Append(";");
            sb.Append(Smoothness);
            sb.Append(";");
            return sb.ToString();
        }
    }

    
    public class Layer
    {
        public MapData Map { get; private set; }
        public string Color { get; private set; }
        public string AlgorithmName { get; private set; }
        public bool Visible { get; set; }
        public int OutScale { get; private set; }
        public CommonLayerCharacteristics Characteristics { get; set; }
        public double GenHausdDist { get; set; }
        public double FilterModifHausdDistance { get; set; } = 0;
        public MapFeatures BendFeachure { get; set; }
        public MapFeatures TripletFeachure { get; set; }
        public MapFeatures TripletToBend { get; set; }
        
        public Layer(MapData map,  string algName = "input")
        {
            Map = map;
            Color =Colors.GetNext();
            AlgorithmName = algName;
            Visible = true;
            Characteristics= new CommonLayerCharacteristics(map);
             BendFeachure = BendComputation.GetFeachure(Map);
            TripletFeachure = TripletComputation.Get(Map);

            TripletToBend = new MapFeatures
            {
                TotalArea = Math.Round(TripletFeachure.TotalArea / BendFeachure.TotalArea, 3)
            };
            TripletToBend.Average = Operation.Make(TripletFeachure.Average,BendFeachure.Average,Operation.GetRelation);
            TripletToBend.Max = Operation.Make(TripletFeachure.Max, BendFeachure.Max, Operation.GetRelation);
            TripletToBend.Min = Operation.Make(TripletFeachure.Min, BendFeachure.Min, Operation.GetRelation);
            TripletToBend.Sko = Operation.Make(TripletFeachure.Sko, BendFeachure.Sko, Operation.GetRelation);
        }
        public Layer(MapData map, string algName,  int outScale ):this(map,algName)
        {
           OutScale = outScale;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(AlgorithmName);
            sb.Append(";");
            sb.Append(OutScale.ToString());
            sb.Append(";");
            sb.Append(Characteristics.ToString());
            sb.Append(GenHausdDist.ToString(CultureInfo.InvariantCulture));
            sb.Append(";");
            sb.Append(FilterModifHausdDistance.ToString(CultureInfo.InvariantCulture));
            sb.Append(";");
            sb.Append(BendFeachure);
            sb.Append(TripletFeachure);
            sb.Append(TripletToBend);
            return sb.ToString();
        }

        public static string GetDescription()
        {
            string s = "AlgorithmName;OutScale;" + CommonLayerCharacteristics.GetDescription()+
                       "GHD;FMHD;" + MapFeatures.GetDescription("BEND") + 
                        MapFeatures.GetDescription("Triplet")
                       + MapFeatures.GetDescription("TripletToBend");
            return s;
        }


    }

    public static class Colors
    {
        private static int _index;

        private static readonly List<string> ColorList = new List<string>(new[]
        {
            "Black", "RoyalBlue","Red", "SpringGreen",  "DarkViolet", "LightSkyBlue",
            "Orange", "ForestGreen", "Pink", "SandyBrown"  
        });
       
    
        public static string GetNext()
        {
            var result = ColorList[_index];
            _index = (_index + 1)%ColorList.Count;
            return result; 
        }

        public static void Init()
        {
            _index = 0;
        }

    }
}
