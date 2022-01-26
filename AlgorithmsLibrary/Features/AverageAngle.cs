using System;


namespace AlgorithmsLibrary.Features
{
    public static class AverageAngleComputation
    {
        public static double Get(MapData map)
        {
            double angle = 0;
            int count = 0;

            foreach (var chain in map.VertexList)
            {
                if (chain.Count < 3)
                    continue;
                for (int i = 0; i < chain.Count - 2; i++)
                {
                    angle += Angle(chain[i],chain[i + 1], chain[i+2]);
                    count++;
                }
            }
            return Math.Round(angle/count);
        }

        public static double GetWeightedAngle(MapData map)
        {
            double sum = 0,
                product =0;

           foreach (var chain in map.VertexList)
           {
                if (chain.Count < 3)
                    continue;
                
                for (int i = 0; i < chain.Count - 2; i++)
                {
                    var angle = Angle(chain[i], chain[i + 1], chain[i + 2]);
                    var edge1 = chain[i].DistanceToVertex(chain[i + 1]);
                    var edge2 = chain[i + 1].DistanceToVertex(chain[i + 2]);
                    var max = Math.Max(edge1, edge2);
                    sum += max;
                    product += angle * max;
                }
           }
            return Math.Round(product / sum);
        }

        /// <summary>
        ///  угoл между векторами [u,v] и [v,w]
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <param name="w"></param>
        /// <returns></returns>
        public static double Angle(MapPoint u, MapPoint v, MapPoint w)
       {
           double s = Math.Sqrt((Math.Pow(v.X - u.X, 2) + Math.Pow(v.Y - u.Y, 2)) *
                                 (Math.Pow(w.X - v.X, 2) + Math.Pow(w.Y - v.Y, 2)));
           if (s < double.Epsilon) return 0;
            double cosB = ((v.X - u.X) * (w.X - v.X) + (v.Y - u.Y) * (w.Y - v.Y)) / s;
           if (cosB > 1) return 0;
           if (cosB < -1) return 180;
            return Math.Round( 57.3*Math.Acos(cosB));
        }

        public static double CosA(MapPoint u, MapPoint v, MapPoint w)
        {
            double cosB = ((v.X - u.X) * (w.X - v.X) + (v.Y - u.Y) * (w.Y - v.Y)) / (Math.Sqrt((Math.Pow(v.X - u.X, 2) + Math.Pow(v.Y - u.Y, 2)) * (Math.Pow(w.X - v.X, 2) + Math.Pow(w.Y - v.Y, 2))));
            return cosB;
        }
    }
}
