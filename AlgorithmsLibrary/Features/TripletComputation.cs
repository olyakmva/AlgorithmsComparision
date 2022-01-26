using System;

namespace AlgorithmsLibrary.Features
{
/*1. Длину
2. Длину базовой линии
3. Высоту
4. Площадь
5. Отношение длины и базовой линии (извилистость)
6. Отношение высоты и базовой линии (пропорция)
7. Отношение периметра и площади (компактность). 
Периметр получается сложением длины и базовой линии изгиба/тройки.
*/

    public static class TripletComputation
    {
        public  const double MinArea = 10;
        public static MapFeatures Get(MapData map)
        {
            var feachureSet = new MapFeatures();
            ICharacteristicsComputation[] computators =
            {
                new AverageCharacteristicsComputation(),
                new MaxCharacteristicsComputation(),
                new MinCharacteristicsComputation(),
                new SkoCharacteristics()
            };
            foreach (var chain in map.VertexList)
            {
                if(chain.Count < 3)
                    continue;
                int count = 0;
                int index = 0;
                
                double totalArea = 0;
               

                while (index < chain.Count - 2)
                {
                    var t = new Triangle(chain[index], chain[index+1], chain[index+2]);
                    var s= t.Square();
                    if (s < MinArea)
                    {
                        if (count == 0) count = 1;
                        index += 2;
                        continue;
                    }
                    totalArea += s;
                    var tFeachures = new BaseFeatures
                    {
                        Height = t.GetHeight(),
                        Width = t.GetWidth(),
                        BaseLineLength = t.GetBaseLine(),
                        Length = t.GetLength(),
                        Area = s,
                        Compactness = Compactness.Get(s, t.GetLength() + t.GetBaseLine()),
                        HeightBaselineRatio = t.GetHeight() / t.GetBaseLine(),
                        HeightWidthRatio = t.GetHeight() / t.GetWidth(),
                        Sinuosity = t.GetLength() / t.GetBaseLine()
                    };
                    foreach (var computer in computators)
                    {
                        computer.Add(tFeachures);
                    }
                    count++;
                    index = index + 2;
                }
                feachureSet.TotalArea += totalArea;
            }
            feachureSet.Average = computators[0].GetResult();
            feachureSet.Max = computators[1].GetResult();
            feachureSet.Min = computators[2].GetResult();
            var skoComputer = computators[3] as SkoCharacteristics;
            if (skoComputer != null)
            {
                skoComputer.AverageFeatures = feachureSet.Average;
                feachureSet.Sko = skoComputer.GetResult();
            }
            feachureSet.TotalArea = Math.Round(feachureSet.TotalArea);
            return feachureSet;
        }

        public static double GetMinSquare(MapData map)
        {
            double minTrArea = Double.MaxValue;
            foreach (var chain in map.VertexList)
            {
                if (chain.Count < 3)
                    continue;
                int index = 0;

                while (index < chain.Count - 2)
                {
                    var t = new Triangle(chain[index], chain[index + 1], chain[index + 2]);
                    var s = t.Square();
                    if (s < MinArea)
                    {
                        index += 2;
                        continue;
                    }
                    if (s < minTrArea)
                        minTrArea = s;
                    index = index + 2;
                }
            }
            return Math.Round(minTrArea,3);
        }
    }
}
