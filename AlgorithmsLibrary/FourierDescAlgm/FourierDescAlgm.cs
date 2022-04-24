﻿using System.Collections.Generic;

namespace AlgorithmsLibrary
{
    public class FourierDescAlgm : ISimplificationAlgm
    {
        public SimplificationAlgmParameters Options { get; set; }
        public virtual void Run(MapData map)
        {
            foreach (var chain in map.VertexList)
            {
                int endIndex = chain.Count - 1;
                Run(chain, 0, endIndex);
            }
        }

        private const int OUTPUT_PERCENT_POINTS = 50;
        private const int FOURIER_SERIES_LENGTH = 500;
        private const double APPROXIMATION_RATIO = 0.1;
        private void Run(List<MapPoint> chain, int startIndex, int endIndex)
        {
            if (chain[startIndex].Equals(chain[endIndex]))
            {
                Fourier fourier = new Fourier(chain, FOURIER_SERIES_LENGTH, APPROXIMATION_RATIO, true); 

                fourier.GetAllDist();
                fourier.GetFourierXparameter();
                fourier.GetFourierYparameter();

                int outputPointCount = chain.Count * OUTPUT_PERCENT_POINTS / 100; 
                var outputs = fourier.GetRecoveryPoints(outputPointCount);

                for (int i = 0; i < outputPointCount; i++)
                {
                    chain[i].X = outputs[i].X;
                    chain[i].Y = outputs[i].Y;
                }

                chain[outputPointCount] = new MapPoint { X = chain[0].X, Y = chain[0].Y };

                while (chain.Count > outputPointCount + 1)
                {
                    chain.RemoveAt(chain.Count - 1);
                }
            }
            else
            {
                Fourier fourier = new Fourier(chain, FOURIER_SERIES_LENGTH, APPROXIMATION_RATIO, false);

                fourier.GetAllDist();
                fourier.GetFourierXparameter();
                fourier.GetFourierYparameter();

                int outputPointCount = chain.Count * OUTPUT_PERCENT_POINTS / 100;
                var outputs = fourier.GetRecoveryPoints(outputPointCount);

                int k = outputs.Count / 2;
                chain.Clear();
                for (int i = 0; i < k; i++)
                {
                    chain.Add(new MapPoint { X = outputs[i].X, Y = outputs[i].Y });
                }
            }
        }
    }
}