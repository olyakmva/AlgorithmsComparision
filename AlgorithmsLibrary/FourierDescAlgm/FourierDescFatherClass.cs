using System;
using System.Collections.Generic;

namespace AlgorithmsLibrary
{
    public abstract class FourierDescFatherClass
    {
        protected internal double polyLineLength = 0.0;
        protected internal long fourierSeriesLength;
        protected internal double[,] XParameter;
        protected internal double[,] YParameter;

        protected internal List<MapPoint> arrayOfMapPoints = null;
        protected internal int countOfPointsObject = 0;
        protected internal double[] sequentialCalculationPolylineLength = null;
        protected internal double[] arrayDistancesBetweenPoints = null;

        public void CalculateAllValue()
        {
            GetAllDist();
            GetFourierXparameter();
            GetFourierYparameter();
        }

        public abstract void GetAllDist();
        public double[,] GetFourierXparameter()
        {
            if (fourierSeriesLength <= 0)
                return null;

            for (int i = 1; i < countOfPointsObject; i++)
            {
                MapPoint p1 = arrayOfMapPoints[i - 1];
                MapPoint p2 = arrayOfMapPoints[i];
                double delx = (double)(p2.X - p1.X);
                double dels = sequentialCalculationPolylineLength[i] - sequentialCalculationPolylineLength[i - 1];
                double dels2 = sequentialCalculationPolylineLength[i] * sequentialCalculationPolylineLength[i] - sequentialCalculationPolylineLength[i - 1] * sequentialCalculationPolylineLength[i - 1];
                double cc = -1.0 * delx * sequentialCalculationPolylineLength[i - 1] / dels;
                double aa = p1.X + cc;
                double bb = delx / dels;
                double v1 = aa * dels;
                double v2 = bb / 2 * dels2;
                XParameter[0, 0] += v1 / polyLineLength + v2 / polyLineLength;
            }

            for (int k = 1; k < fourierSeriesLength + 1; k++)
            {
                double angle = 2 * Math.PI * k / polyLineLength;
                for (int i = 1; i < countOfPointsObject; i++)
                {
                    MapPoint p1 = arrayOfMapPoints[i - 1];
                    MapPoint p2 = arrayOfMapPoints[i];
                    double delx = p2.X - p1.X;
                    double dels = sequentialCalculationPolylineLength[i] - sequentialCalculationPolylineLength[i - 1];
                    double dels2 = sequentialCalculationPolylineLength[i] * sequentialCalculationPolylineLength[i] - sequentialCalculationPolylineLength[i - 1] * sequentialCalculationPolylineLength[i - 1];
                    double cc = -1.0 * delx * sequentialCalculationPolylineLength[i - 1] / dels;
                    double aa = p1.X + cc;
                    double bb = delx / dels;
                    double k1 = (double)k;
                    double aa1 = aa / Math.PI / k1;
                    double delsin = Math.Sin(2.0 * k1 * Math.PI * sequentialCalculationPolylineLength[i] / polyLineLength) - Math.Sin(2.0 * k1 * Math.PI * sequentialCalculationPolylineLength[i - 1] / polyLineLength);
                    double v1 = aa1 * delsin;
                    double bb1 = bb / Math.PI / k1;
                    double delsinS = Math.Sin(2.0 * k1 * Math.PI * sequentialCalculationPolylineLength[i] / polyLineLength) * sequentialCalculationPolylineLength[i] - Math.Sin(2.0 * k1 * Math.PI * sequentialCalculationPolylineLength[i - 1] / polyLineLength) * sequentialCalculationPolylineLength[i - 1];
                    double v2 = bb1 * delsinS;
                    double bb2 = bb1 * polyLineLength / 2.0 / Math.PI / k1;
                    double delcos = Math.Cos(2.0 * k1 * Math.PI * sequentialCalculationPolylineLength[i] / polyLineLength) - Math.Cos(2.0 * k1 * Math.PI * sequentialCalculationPolylineLength[i - 1] / polyLineLength);
                    double v3 = bb2 * delcos;
                    XParameter[k, 0] += v1 + v2 + v3;
                }

                for (int i = 1; i < countOfPointsObject; i++)
                {
                    MapPoint p1 = arrayOfMapPoints[i - 1];
                    MapPoint p2 = arrayOfMapPoints[i];
                    double delx = p2.X - p1.X;
                    double dels = sequentialCalculationPolylineLength[i] - sequentialCalculationPolylineLength[i - 1];
                    double dels2 = sequentialCalculationPolylineLength[i] * sequentialCalculationPolylineLength[i] - sequentialCalculationPolylineLength[i - 1] * sequentialCalculationPolylineLength[i - 1];
                    double cc = -1.0 * delx * sequentialCalculationPolylineLength[i - 1] / dels;
                    double aa = p1.X + cc;
                    double bb = delx / dels;
                    double k1 = (double)k;
                    double aa1 = (-1.0) * aa / Math.PI / k1;
                    double delcos = Math.Cos(2.0 * k1 * Math.PI * sequentialCalculationPolylineLength[i] / polyLineLength) - Math.Cos(2.0 * k1 * Math.PI * sequentialCalculationPolylineLength[i - 1] / polyLineLength);
                    double v1 = aa1 * delcos;
                    double bb1 = (-1.0) * bb / Math.PI / k1;
                    double delcosS = Math.Cos(2.0 * k1 * Math.PI * sequentialCalculationPolylineLength[i] / polyLineLength) * sequentialCalculationPolylineLength[i] - Math.Cos(2.0 * k1 * Math.PI * sequentialCalculationPolylineLength[i - 1] / polyLineLength) * sequentialCalculationPolylineLength[i - 1];
                    double v2 = bb1 * delcosS;
                    double bb2 = (-1.0) * bb1 * polyLineLength / 2.0 / Math.PI / k1;
                    double delsin = Math.Sin(2.0 * k1 * Math.PI * sequentialCalculationPolylineLength[i] / polyLineLength) - Math.Sin(2.0 * k1 * Math.PI * sequentialCalculationPolylineLength[i - 1] / polyLineLength);
                    double v3 = bb2 * delsin;
                    XParameter[k, 1] += v1 + v2 + v3;
                }
            }

            return XParameter;
        }
        public double[,] GetFourierYparameter()
        {
            if (fourierSeriesLength <= 0)
                return null;

            for (int i = 1; i < countOfPointsObject; i++)
            {
                MapPoint p1 = arrayOfMapPoints[i - 1];
                MapPoint p2 = arrayOfMapPoints[i];
                double delx = p2.Y - p1.Y;
                double dels = sequentialCalculationPolylineLength[i] - sequentialCalculationPolylineLength[i - 1];
                double dels2 = sequentialCalculationPolylineLength[i] * sequentialCalculationPolylineLength[i] - sequentialCalculationPolylineLength[i - 1] * sequentialCalculationPolylineLength[i - 1];
                double cc = -1.0 * delx * sequentialCalculationPolylineLength[i - 1] / dels;
                double aa = p1.Y + cc;
                double bb = delx / dels;
                double v1 = aa * dels;
                double v2 = bb / 2 * dels2;
                YParameter[0, 0] += v1 / polyLineLength + v2 / polyLineLength;
            }

            for (int k = 1; k < fourierSeriesLength + 1; k++)
            {
                double angle = 2 * Math.PI * k / polyLineLength;
                for (int i = 1; i < countOfPointsObject; i++)
                {
                    MapPoint p1 = arrayOfMapPoints[i - 1];
                    MapPoint p2 = arrayOfMapPoints[i];
                    double delx = p2.Y - p1.Y;
                    double dels = sequentialCalculationPolylineLength[i] - sequentialCalculationPolylineLength[i - 1];
                    double dels2 = sequentialCalculationPolylineLength[i] * sequentialCalculationPolylineLength[i] - sequentialCalculationPolylineLength[i - 1] * sequentialCalculationPolylineLength[i - 1];
                    double cc = -1.0 * delx * sequentialCalculationPolylineLength[i - 1] / dels;
                    double aa = p1.Y + cc;
                    double bb = delx / dels;
                    double k1 = (double)k;
                    double aa1 = aa / Math.PI / k1;
                    double delsin = Math.Sin(2.0 * k1 * Math.PI * sequentialCalculationPolylineLength[i] / polyLineLength) - Math.Sin(2.0 * k1 * Math.PI * sequentialCalculationPolylineLength[i - 1] / polyLineLength);
                    double v1 = aa1 * delsin;
                    double bb1 = bb / Math.PI / k1;
                    double delsinS = Math.Sin(2.0 * k1 * Math.PI * sequentialCalculationPolylineLength[i] / polyLineLength) * sequentialCalculationPolylineLength[i] - Math.Sin(2.0 * k1 * Math.PI * sequentialCalculationPolylineLength[i - 1] / polyLineLength) * sequentialCalculationPolylineLength[i - 1];
                    double v2 = bb1 * delsinS;
                    double bb2 = bb1 * polyLineLength / 2.0 / Math.PI / k1;
                    double delcos = Math.Cos(2.0 * k1 * Math.PI * sequentialCalculationPolylineLength[i] / polyLineLength) - Math.Cos(2.0 * k1 * Math.PI * sequentialCalculationPolylineLength[i - 1] / polyLineLength);
                    double v3 = bb2 * delcos;
                    YParameter[k, 0] += v1 + v2 + v3;
                }

                for (int i = 1; i < countOfPointsObject; i++)
                {
                    MapPoint p1 = arrayOfMapPoints[i - 1];
                    MapPoint p2 = arrayOfMapPoints[i];
                    double delx = p2.Y - p1.Y;
                    double dels = sequentialCalculationPolylineLength[i] - sequentialCalculationPolylineLength[i - 1];
                    double dels2 = sequentialCalculationPolylineLength[i] * sequentialCalculationPolylineLength[i] - sequentialCalculationPolylineLength[i - 1] * sequentialCalculationPolylineLength[i - 1];
                    double cc = -1.0 * delx * sequentialCalculationPolylineLength[i - 1] / dels;
                    double aa = p1.Y + cc;
                    double bb = delx / dels;
                    double k1 = (double)k;
                    double aa1 = (-1.0) * aa / Math.PI / k1;
                    double delcos = Math.Cos(2.0 * k1 * Math.PI * sequentialCalculationPolylineLength[i] / polyLineLength) - Math.Cos(2.0 * k1 * Math.PI * sequentialCalculationPolylineLength[i - 1] / polyLineLength);
                    double v1 = aa1 * delcos;
                    double bb1 = (-1.0) * bb / Math.PI / k1;
                    double delcosS = Math.Cos(2.0 * k1 * Math.PI * sequentialCalculationPolylineLength[i] / polyLineLength) * sequentialCalculationPolylineLength[i] - Math.Cos(2.0 * k1 * Math.PI * sequentialCalculationPolylineLength[i - 1] / polyLineLength) * sequentialCalculationPolylineLength[i - 1];
                    double v2 = bb1 * delcosS;
                    double bb2 = (-1.0) * bb1 * polyLineLength / 2.0 / Math.PI / k1;
                    double delsin = Math.Sin(2.0 * k1 * Math.PI * sequentialCalculationPolylineLength[i] / polyLineLength) - Math.Sin(2.0 * k1 * Math.PI * sequentialCalculationPolylineLength[i - 1] / polyLineLength);
                    double v3 = bb2 * delsin;
                    YParameter[k, 1] += v1 + v2 + v3;
                }
            }

            return YParameter;
        }
    }
}
