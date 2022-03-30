using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsLibrary
{
    public class FourierDescAlgm : ISimplificationAlgm
    {
        public SimplificationAlgmParameters Options { get; set; }
        long F = 32;
        long preFourier = 32 + 5;

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
                Fourier fourier = new Fourier(chain, preFourier);

                double Dist = fourier.GetAllDist();
                fourier.GetFourierXparameter();
                fourier.GetFourierYparameter();
                double[] vector = fourier.CalculateShapeVector(true);
                double[] Ax = fourier.Ax;
                double[] Ay = fourier.Ay;
                double[] Bx = fourier.Bx;
                double[] By = fourier.By;
                long A = vector.Length;

                //CalculateInformation
                double All_Information = default(double);
                int EntropyLength = fourier.CalculateEntropy2(F).Length;
                double[] CalculateEntropy = fourier.CalculateEntropy2(F);
                for (int i = 0; i < F; i++)
                {
                    All_Information += CalculateEntropy[i];
                }
                double temp;
                double[] order = CalculateEntropy;
                for (int i = 0; i < order.Length; i++)
                {
                    for (int j = i + 1; j < order.Length; j++)
                    {
                        if (order[j] > order[i])
                        {
                            temp = order[j];
                            order[j] = order[i];
                            order[i] = temp;
                        }
                    }
                }

                
            }
        }
    }
}
