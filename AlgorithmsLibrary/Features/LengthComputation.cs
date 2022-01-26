using System;

namespace AlgorithmsLibrary.Features
{
    public static class LengthComputation
    {
        public static double Get(MapData map)
        {
            double length = 0;

            foreach (var chain in map.VertexList)
            {
                for (int i = 0; i < chain.Count - 1; i++)
                {
                    length += chain[i].DistanceToVertex(chain[i + 1]);
                }
            }
            return Math.Round(length);
        }
    }
}
