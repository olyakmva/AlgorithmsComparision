using System;
using System.Collections.Generic;

namespace AlgorithmsLibrary.Features
{
    public static class GenHausdorfDistance
        {
            public static double Get(MapData inMap, MapData outMap)
            {
                int n = inMap.VertexList.Count;
                double distSum = 0;

                for (var i = 0; i < inMap.VertexList.Count; i++)
                {
                    var lstIn = inMap.VertexList[i];
                    var lstOut = outMap.VertexList[i];
                    double max = 0;
                    if (lstOut.Count == 0)
                        continue;
                    if (lstIn.Count == lstOut.Count)
                    {
                        for (int j = 1; j < lstIn.Count; j++)
                        {
                            max = Math.Max(max, lstIn[j].DistanceToVertex(lstOut[j]));
                        }
                    }
                    else
                    {
                        max = Math.Max(Max(lstOut, lstIn), Max(lstIn, lstOut));
                    }
                    distSum += max;
                }

                return Math.Round(distSum / n);
            }

            private static double Max(List<MapPoint> lst1, List<MapPoint> lst2)
            {
                double max = 0;
                for (int j = 1; j < (lst1.Count - 1); j++)
                {
                    double min = double.MaxValue;
                    for (int k = 0; k < (lst2.Count - 1); k++)
                    {
                        double d;
                        if (lst2[k].Equals(lst2[k + 1]))
                        {
                            d = lst2[k].DistanceToVertex(lst1[j]);
                        }
                        else
                        {
                            var line = new Line(lst2[k], lst2[k + 1]);
                            d = line.GetDistance(lst1[j]);
                        }
                        if (d < min)
                            min = d;
                        if (min < double.Epsilon)
                            break;
                    }
                    if (min > max)
                        max = min;
                }
                return max;
            }
        }
    }

