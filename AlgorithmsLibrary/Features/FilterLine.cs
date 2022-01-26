using System;
using System.Collections.Generic;

namespace AlgorithmsLibrary.Features
{
     public  class FilterLine
     {
         private readonly int _sigma;
         private readonly double _mnoz;
         private readonly double _znam;
         private double _step;

         public FilterLine(int s, int step)
         {
             _sigma = s;
             _mnoz = 1.0 / (Math.Sqrt(Math.PI * 2) * _sigma);
             _znam = 2 * _sigma * _sigma;
             _step = step;
         }
         public MapData Process(MapData map)
         {
             var result = new List<List<MapPoint>>();
             foreach (var chain in map.VertexList)
             {
                 var list = new List<MapPoint>();
                 SetSamplingPoints(chain);
                 for (int i = 0; i < chain.Count ; i++)
                 {
                    var delta = 4*_sigma;
                     if (delta >= chain.Count)
                         delta = chain.Count - 1;
                     var additionalPts = GetAdditionalPts(chain, delta);
                     var endAddPonts = GetEndAdditionalPts(chain, delta);

                     double x = 0, y = 0;
                    var k = (-1) * delta;
                     for (int j = i-delta; j <=  i+delta; j++)
                     {
                         if (j < 0)
                         {
                             x += additionalPts[Math.Abs(j)-1].X * Gauss(k);
                             y += additionalPts[Math.Abs(j)-1].Y * Gauss(k);
                         }
                         else if (j > chain.Count - 1)
                         {
                             x += endAddPonts[j-chain.Count].X * Gauss(k);
                             y += endAddPonts[j - chain.Count].Y * Gauss(k);
                         }
                         else
                         {
                            x += chain[j].X * Gauss(k);
                             y += chain[j].Y * Gauss(k);
                        }
                         k++;
                     }
                     list.Add(new MapPoint(x, y, chain[i].Id, 1));

                 }
                 result.Add(list);
             }
             map.VertexList.Clear();
             map.VertexList.AddRange(result);
             return map;

         }

         private static List<MapPoint> GetAdditionalPts(List<MapPoint> chain, int delta)
         {
             if (delta >= chain.Count)
                 delta = chain.Count - 1;
             var additionalPts = new List<MapPoint>();
             var x0 = chain[0].X;
             var y0 = chain[0].Y;
             for (int j = 1; j <= delta; j++)
             {
                 var p = new MapPoint(2 * x0 - chain[j].X, 2 * y0 - chain[j].Y, 1, 1);
                 additionalPts.Add(p);
             }
             return additionalPts;
         }
         private static List<MapPoint> GetEndAdditionalPts(List<MapPoint> chain, int delta)
         {
             if (delta >= chain.Count)
                 delta = chain.Count - 1;
            var additionalPts = new List<MapPoint>();
             var xn = chain[chain.Count-1].X;
             var yn = chain[chain.Count-1].Y;
             var n = chain.Count - 1;
             for (int j = 1; j <= delta; j++)
             {
                 var p = new MapPoint(2 * xn - chain[n-j].X, 2 * yn - chain[n-j].Y, 1, 1);
                 additionalPts.Add(p);
             }
             return additionalPts;
         }

        public void SetSamplingPoints(List<MapPoint> chain)
        {
             var delta = _step / 10;
             int i = 0;
             while (i < chain.Count - 1)
             {
                 var d = chain[i].DistanceToVertex(chain[i + 1]);
                 if (d > _step + delta)
                 {
                     var lymbda = _step / d;
                     var x = (chain[i].X + lymbda * chain[i + 1].X) / (1 + lymbda);
                     var y = (chain[i].Y + lymbda * chain[i + 1].Y) / (1 + lymbda);
                     chain.Insert(i + 1, new MapPoint(x, y, chain[i].Id, 1));
                     i++;
                 }
                 else if (Math.Abs(d - _step) <= delta)
                 {
                     i++;
                 }
                 else // расстояние до точки < шага
                 {
                     if (i + 2 < chain.Count)
                     {
                         int j = i + 1;
                         var need = _step - d;
                         d = chain[j].DistanceToVertex(chain[j + 1]);
                         while (j < chain.Count - 1 && (d + delta) < need)
                         {
                             need = need - d;
                             chain.RemoveAt(j);
                             if (j >= chain.Count - 1) break;
                             d = chain[j].DistanceToVertex(chain[j + 1]);
                         }
                         if (j + 1 >= chain.Count - 1)
                         {
                             chain.RemoveAt(j);
                             d = chain[chain.Count - 2].DistanceToVertex(chain[chain.Count - 1]);
                             if (d > _step + delta)
                             {
                                 var x = (chain[chain.Count - 2].X + chain[chain.Count - 1].X) / 2;
                                 var y = (chain[chain.Count - 2].Y + chain[chain.Count - 1].Y) / 2;
                                 chain.Insert(chain.Count - 1, new MapPoint(x, y, chain[chain.Count - 1].Id, 1));
                             }
                             return;
                         }
                         if (Math.Abs(d - need) <= delta)
                         {
                             chain.RemoveRange(i + 1, j - i);
                             i++;
                         }
                         else
                         {
                             var lymbda = need / d;
                             var x = (chain[j].X + lymbda * chain[j + 1].X) / (1 + lymbda);
                             var y = (chain[j].Y + lymbda * chain[j + 1].Y) / (1 + lymbda);
                             chain[j].X = x;
                             chain[j].Y = y;
                             i = j;
                         }
                     }
                     else break;
                 }

             }
         }


         private double Gauss(int k)
         {
             return _mnoz * Math.Exp(-1 * k * k / _znam);
         }
    }
}
