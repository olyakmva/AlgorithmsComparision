using System.Collections.Generic;

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

        private void Run(List<MapPoint> chain, int startIndex, int endIndex)
        {
            
            if (chain[startIndex].Equals(chain[endIndex]))
            {
                Fourier fourier = new Fourier(chain, 1000); //fourier length

                double Dist = fourier.GetAllDist();
                fourier.GetFourierXparameter();
                fourier.GetFourierYparameter();

                long fitt = (long)(chain.Count * 0.05); //output points
                var t = fourier.GetRecoveryPoints(fitt);

                for (int i =0; i < fitt; i++)
                {
                    chain[i].X = t[i].X;
                    chain[i].Y = t[i].Y;
                }

                while (chain.Count > fitt)
                {
                    chain.RemoveAt(chain.Count - 1);    
                }
            }
            else
            {
                Fourier_NotClosed fourier = new Fourier_NotClosed(chain, 100);

                double Dist = fourier.GetAllDist();
                fourier.GetFourierXparameter();
                fourier.GetFourierYparameter();

                var t = fourier.GetRecoveryPoints(chain.Count, fourier.Ax.Length);

                int k = t.Length / 2;
                chain.Clear();
                for (int i = 0; i < k; i++)
                {
                    chain.Add(new MapPoint { X = t[i, 0], Y = t[i, 1] });
                }
            }
        }
    }
}
