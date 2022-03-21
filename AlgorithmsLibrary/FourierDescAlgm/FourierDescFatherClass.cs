using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsLibrary
{
    public abstract class FourierDescFatherClass
    {
        protected internal List<MapPoint> arrayOfMapPoints = null;

        protected internal double[] d = null;
        protected internal long nTerm = 0;

        protected internal long countOfPointsObject = 0;

        protected internal double[] ratio = null;

        protected internal double[] Ax = null;
        protected internal double[] Bx = null;
        protected internal double[] Ay = null;
        protected internal double[] By = null;

        protected internal double[] sequentialCalculationPolylineLength = null;
        protected internal double[] arrayDistancesBetweenPoints = null;

        //public double GetAx(int index) => Ax[index];
        //public double GetBx(int index) => Bx[index];
        //public double GetAy(int index) => Ay[index];
        //public double GetBy(int index) => By[index];
        //public double GetShapeVector(int index) => d[index];
        //public double GetRatio(int index) => ratio[index];
        //public double[] ArrayAx() => Ax;
        //public double[] ArrayBx() => Bx;
        //public double[] ArrayAy() => Ay;
        //public double[] ArrayBy() => By;

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
                double t = Math.Sqrt(CX * CX + CY * CY);
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
