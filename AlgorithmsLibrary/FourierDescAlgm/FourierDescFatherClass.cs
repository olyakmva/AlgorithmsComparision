using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsLibrary
{
    public abstract class FourierDescFatherClass
    {
        public double[] d = null;
        public long nTerm = 0;

        public double[] CalculateShapeVector(bool closed)
        {
            long n = nTerm + 1;
            double[] D = new double[n];
            d = new double[n - 1];
            ratio = new double[n - 1];
            double sum = 0;
            for (int i = 1; i < n; i++)
            {
                double CX = Ax[i] + By[i];
                double CY = Bx[i] - Ay[i];
                dobule t = Math.Sqrt(CX * CX + CY * CY);
                D[i] = closed ? Math.Abs(t) : t;
                sum = sum + D[i];
            }
            for (int i = 1; i < n; i++)
            {
                ratio[i - 1] = D[i] / sum;
                d[i - 1] = D[i] / D[1];
            }
            return d;
        }
        public double[] CalculateEntropy2(long Number)
        {
            double[] Proportion = new double[Number];

            double d_sum = default(double);

            for (int i = 0; i < Proportion.Length; i++)
            {
                d_sum += d[i + 1];
            }

            for (int i = 0; i < Proportion.Length; i++)
            {
                Proportion[i] = d[i + 1] / d_sum;
            }

            double[] Entropy = new double[Proportion.Length];
            for (int i = 0; i < Entropy.Length; i++)
            {
                Entropy[i] = -Proportion[i] * Math.Log(Proportion[i], 2);
            }
            return Entropy;
        }

        

    }
}
