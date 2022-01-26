using System;
using System.Collections.Generic;

namespace AlgorithmsLibrary.Features
{
    public static class BendComputation
    {
        public const double MinArea = 50;
        public static int Get(MapData map)
        {
            int bendNumber = 0;

            foreach (var chain in map.VertexList)
            {
                int count = 0;
                int index = 0;
                while (index < chain.Count - 2)
                {
                    ExtractBend(ref index, chain, out var b);
                    count++;
                }
                bendNumber += count;
            }
            return bendNumber;
        }

        public static MapFeatures GetFeachure(MapData map)
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
                var count = 0;
                var index = 0;

                double lineLength = 0;
                for (int i = 0; i < chain.Count - 1; i++)
                {
                    lineLength += chain[i].DistanceToVertex(chain[i + 1]);
                }
                
                double totalArea = 0;
                double shapeProminence = 0;
                if(chain.Count < 3)
                    continue;
                if (chain.Count == 3 && chain[0].CompareTo(chain[2]) == 0)
                    continue;

                while (index < chain.Count - 2)
                {
                    Bend b;
                    ExtractBend(ref index, chain, out b);
                    var s = b.Area();
                    if (s < MinArea )
                    {
                        if (count == 0) count = 1;
                          continue;
                    }
                    if (b.BendNodeList[0].CompareTo(b.BendNodeList[b.BendNodeList.Count - 1]) == 0)
                    {
                        if (b.BendNodeList.Count > 3)
                        {
                            b.BendNodeList.RemoveAt(b.BendNodeList.Count-1);
                        }
                        else
                        {
                            if (count == 0) count++;
                        }
                    }

                    try
                    {
                        totalArea += s;
                        var bendFeachures = new BaseFeatures
                        {
                            Height = b.GetHeight(),
                            Width = b.GetWidth(),
                            BaseLineLength = b.BaseLineLength(),
                            Length = b.Length(),
                            Area =s,
                            Compactness = Compactness.Get(s,b.Perimeter())
                        };
                        bendFeachures.HeightWidthRatio = bendFeachures.Height / bendFeachures.Width;
                        bendFeachures.HeightBaselineRatio = bendFeachures.Height / bendFeachures.BaseLineLength;
                        bendFeachures.Sinuosity = bendFeachures.Length / bendFeachures.BaseLineLength;
                        var openClose = 2 * b.Length() / b.Perimeter() - 1;
                        var p = bendFeachures.HeightWidthRatio;
                        shapeProminence += p * openClose / (p + 1) * (2 - b.Length() / lineLength) * s;
                        count++;
                        foreach (var computator in computators)
                        {
                            computator.Add(bendFeachures);
                        }
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
                feachureSet.TotalArea += totalArea;
                feachureSet.ShapeProminence += shapeProminence/totalArea;
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
            feachureSet.ShapeProminence = Math.Round(feachureSet.ShapeProminence / map.VertexList.Count, 3);
            feachureSet.TotalArea = Math.Round(feachureSet.TotalArea);
            return feachureSet;
        }

        public static void ExtractBend(ref int index, List<MapPoint> chain, out Bend b)
        {
            int firstIndex = index;
            b = new Bend();
            b.BendNodeList.Add(chain[index]);
            b.BendNodeList.Add(chain[index + 1]);
            b.BendNodeList.Add(chain[index + 2]);
            index += 3;
            var bendOrient = Orientation(b.BendNodeList[0], b.BendNodeList[1], b.BendNodeList[2]);

            //ищем конец изгиба
            while (index < chain.Count)
            {
                var orient = Orientation(b.BendNodeList[b.BendNodeList.Count - 2],
                    b.BendNodeList[b.BendNodeList.Count - 1], chain[index]);
                if (orient != bendOrient)
                    break;
                b.BendNodeList.Add(chain[index]);
                index++;
            }
            index--;
            //добавление крайних точек к изгибу при достаточно маленьком отклонении от 180 градусов и уменьшении ширины основания
            //к концу
            var lastIndex = index;
            while (index < chain.Count - 1)
            {
                if ((Angle(chain[index - 1], chain[index], chain[index + 1]) > 0.9)
                    && (Math.Pow(b.BaseLineLength(), 2) <
                        Math.Pow(chain[firstIndex].X - chain[index + 1].X, 2) +
                        Math.Pow(chain[firstIndex].Y - chain[index + 1].Y, 2)))
                {
                    index++;
                    b.BendNodeList.Add(chain[index]);
                }
                else
                {
                    break;
                }
            }
            index= lastIndex-1;
        }

        public static List<Bend> CheckBend(Bend bend)
        {
            if (bend.BendNodeList.Count < 5)
                return new List<Bend> {bend};
            var bendList = new List<Bend>();
            var i = 0;
            while (i< bend.BendNodeList.Count-2)
            {
                var u = bend.BendNodeList[i];
            var v = bend.BendNodeList[i+1];
            var w = bend.BendNodeList[i+2];
            var angle = Math.Acos(Angle(u, v, w));
                i = i + 3;
            while (i < bend.BendNodeList.Count && angle < Math.PI)
            {
                u = v;
                v = w;
                w = bend.BendNodeList[i];
                    var a = Math.Acos(Angle(u, v, w));
                    angle += a;
                i++;
            }
                if (i != bend.BendNodeList.Count)
                {
                    var smallBend = new Bend();
                    for (int j = 0; j < i; j++)
                    {
                        smallBend.BendNodeList.Add(bend.BendNodeList[j]);
                    }
                    bendList.Add(smallBend);
                    if (bend.BendNodeList.Count - i - 1 >= 3)
                    {
                        bend.BendNodeList.RemoveRange(0, i - 1);
                        i = 0;
                    }
                    else
                    {
                        bend.BendNodeList.RemoveRange(0, bend.BendNodeList.Count - 3);
                        bendList.Add(bend);
                         break;
                    }
                }
                else
                {
                    bendList.Add(bend);
                    break;
                }
            }
            return bendList;
        }

        private static bool Orientation(MapPoint u, MapPoint v, MapPoint w)
        {
            return (!((v.X - u.X) * (w.Y - u.Y) - (w.X - u.X) * (v.Y - u.Y) < 0));
        }
        /// <summary>
        /// косинус угла между векторами [u,v] и [v,w]
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <param name="w"></param>
        /// <returns></returns>
        public static double Angle(MapPoint u, MapPoint v, MapPoint w)
        {
            return ((v.X - u.X) * (w.X - v.X) + (v.Y - u.Y) * (w.Y - v.Y)) / (Math.Sqrt((Math.Pow(v.X - u.X, 2) + Math.Pow(v.Y - u.Y, 2)) * (Math.Pow(w.X - v.X, 2) + Math.Pow(w.Y - v.Y, 2))));
        }
    }
}
