using System;
using System.Collections.Generic;

namespace AlgorithmsLibrary
{
    public class Fourier
    {
        private double polyLineLength;
        private int fourierSeriesLength;
        private double[,] XParameter;
        private double[,] YParameter;

        private List<MapPoint> arrayOfMapPoints;
        private int countOfPointsObject;
        private double[] S; //sequentialPolylineLength
        private double[] arrayDistancesBetweenPoints;

        private bool FourierClosedType;

        public Fourier(List<MapPoint> mapPoints, int fourierSeriesLength, double approximationRatio, bool FourierClosedType)
        {
            this.FourierClosedType = FourierClosedType;
            this.fourierSeriesLength = fourierSeriesLength;

            while (true)
            {
                bool distanceBetweenMapPointsNormal = true;
                arrayOfMapPoints = mapPoints;
                for (int i = 1; i < arrayOfMapPoints.Count; i++)
                {
                    MapPoint p1 = arrayOfMapPoints[i - 1];
                    MapPoint p2 = arrayOfMapPoints[i];
                    if (Math.Abs(p1.X - p2.X) < approximationRatio && Math.Abs(p1.Y - p2.Y) < approximationRatio)
                    {
                        arrayOfMapPoints.RemoveAt(i--);
                        distanceBetweenMapPointsNormal = false;
                    }
                }

                if (distanceBetweenMapPointsNormal)
                {
                    countOfPointsObject = arrayOfMapPoints.Count;
                    if (!FourierClosedType)
                        countOfPointsObject = 2 * countOfPointsObject - 1;

                    break;
                }
            }

            XParameter = new double[fourierSeriesLength + 1, 2];
            YParameter = new double[fourierSeriesLength + 1, 2];

            polyLineLength = 0.0;
        }

        public void CalculateAllValue()
        {
            GetAllDist();
            GetFourierXparameter();
            GetFourierYparameter();
        }

        private void AddSymmetricPolyline()
        {
            int basePointCount = arrayOfMapPoints.Count;
            MapPoint startPoint = arrayOfMapPoints[0];
            MapPoint endPoint = arrayOfMapPoints[basePointCount - 1];
            Line SymmetryAxis = new Line(startPoint, endPoint);
            
            for (int i = basePointCount; i < countOfPointsObject; i++)
            {
                MapPoint SymmetryPoint = SymmetryAxis.GetSymmetricPoint(arrayOfMapPoints[countOfPointsObject - 1 - i]);
                arrayOfMapPoints.Add(SymmetryPoint);
            }
        }

        public void GetAllDist()
        {
            arrayDistancesBetweenPoints = new double[countOfPointsObject - 1];
            S = new double[countOfPointsObject];

            if (!FourierClosedType)
                AddSymmetricPolyline();

            for (int i = 0; i < countOfPointsObject - 1; i++)
            {
                MapPoint p2 = arrayOfMapPoints[i + 1];
                MapPoint p1 = arrayOfMapPoints[i];

                arrayDistancesBetweenPoints[i] = p1.DistanceToVertex(p2);
            }

            S[0] = 0.0;
            for (int i = 1; i < countOfPointsObject; i++)
            {
                double previousSequentialLength = S[i - 1];
                S[i] = previousSequentialLength + arrayDistancesBetweenPoints[i - 1];
            }

            polyLineLength = S[countOfPointsObject - 1];
        }

        public double[,] GetFourierXparameter()
        {
            for (int k = 0; k < countOfPointsObject - 1; k++)
            {
                double diffX = arrayOfMapPoints[k + 1].X - arrayOfMapPoints[k].X;
                double diffS = S[k + 1] - S[k];
                double summ1 = arrayOfMapPoints[k].X * diffS;
                double summ2 = 0.5 * diffX * diffS;
                XParameter[0, 0] += (summ1 + summ2) / polyLineLength;
            }

            for (int n = 1; n < fourierSeriesLength + 1; n++)
            {
                double angle = 2.0 * Math.PI * n / polyLineLength;
                for (int k = 0; k < countOfPointsObject - 1; k++)
                {
                    double XK1 = arrayOfMapPoints[k + 1].X;
                    double XK = arrayOfMapPoints[k].X;
                    double divXS = (XK1 - XK) / (S[k + 1] - S[k]);

                    double SinK1 = Math.Sin(angle * S[k + 1]);
                    double SinK = Math.Sin(angle * S[k]);
                    double CosK1 = Math.Cos(angle * S[k + 1]);
                    double CosK = Math.Cos(angle * S[k]);

                    double v1 = (XK - divXS * S[k]) * (SinK1 - SinK) / angle;
                    double v2 = divXS * (SinK1 * S[k + 1] - SinK * S[k]) / angle;
                    double v3 = divXS * (CosK1 - CosK) / (angle * angle);

                    XParameter[n, 0] += (v1 + v2 + v3) * 2 / polyLineLength;
                }

                for (int k = 0; k < countOfPointsObject - 1; k++)
                {
                    double XK1 = arrayOfMapPoints[k + 1].X;
                    double XK = arrayOfMapPoints[k].X;
                    double divXS = (XK1 - XK) / (S[k + 1] - S[k]);

                    double SinK1 = Math.Sin(angle * S[k + 1]);
                    double SinK = Math.Sin(angle * S[k]);
                    double CosK1 = Math.Cos(angle * S[k + 1]);
                    double CosK = Math.Cos(angle * S[k]);

                    double v1 = -(XK - divXS * S[k]) * (CosK1 - CosK) / angle;
                    double v2 = - divXS * (CosK1 * S[k + 1] - CosK * S[k]) / angle;
                    double v3 = divXS * (SinK1 - SinK) / (angle * angle);

                    XParameter[n, 1] += (v1 + v2 + v3) * 2 / polyLineLength;
                }
            }

            return XParameter;
        }
        public double[,] GetFourierYparameter()
        {
            for (int k = 0; k < countOfPointsObject - 1; k++)
            {
                double diffY = arrayOfMapPoints[k + 1].Y - arrayOfMapPoints[k].Y;
                double diffS = S[k + 1] - S[k];
                double summ1 = arrayOfMapPoints[k].Y * diffS;
                double summ2 = 0.5 * diffY * diffS;
                YParameter[0, 0] += (summ1 + summ2) / polyLineLength;
            }

            for (int n = 1; n < fourierSeriesLength + 1; n++)
            {
                double angle = 2.0 * Math.PI * n / polyLineLength;
                for (int k = 0; k < countOfPointsObject - 1; k++)
                {
                    double YK1 = arrayOfMapPoints[k + 1].Y;
                    double YK = arrayOfMapPoints[k].Y;
                    double divYS = (YK1 - YK) / (S[k + 1] - S[k]);

                    double SinK1 = Math.Sin(angle * S[k + 1]);
                    double SinK = Math.Sin(angle * S[k]);
                    double CosK1 = Math.Cos(angle * S[k + 1]);
                    double CosK = Math.Cos(angle * S[k]);

                    double v1 = (YK - divYS * S[k]) * (SinK1 - SinK) / angle;
                    double v2 = divYS * (SinK1 * S[k + 1] - SinK * S[k]) / angle;
                    double v3 = divYS * (CosK1 - CosK) / (angle * angle);

                    YParameter[n, 0] += (v1 + v2 + v3) * 2 / polyLineLength;
                }

                for (int k = 0; k < countOfPointsObject - 1; k++)
                {
                    double YK1 = arrayOfMapPoints[k + 1].Y;
                    double YK = arrayOfMapPoints[k].Y;
                    double divYS = (YK1 - YK) / (S[k + 1] - S[k]);

                    double SinK1 = Math.Sin(angle * S[k + 1]);
                    double SinK = Math.Sin(angle * S[k]);
                    double CosK1 = Math.Cos(angle * S[k + 1]);
                    double CosK = Math.Cos(angle * S[k]);

                    double v1 = - (YK - divYS * S[k]) * (CosK1 - CosK) / angle;
                    double v2 = - divYS * (CosK1 * S[k + 1] - CosK * S[k]) / angle;
                    double v3 = divYS * (SinK1 - SinK) / (angle * angle);

                    YParameter[n, 1] += (v1 + v2 + v3) * 2 / polyLineLength;
                }
            }

            return YParameter;
        }

        public List<MapPoint> GetRecoveryPoints(int OutputPointCount)
        {
            var RecoveryPoints = new List<MapPoint>();

            double[] ss = new double[OutputPointCount + 1];
            double s_average = polyLineLength / OutputPointCount;
            for (int i = 1; i < OutputPointCount; i++)
            {
                ss[i] = ss[i - 1] + s_average;
            }

            for (int j = 1; j < OutputPointCount + 1; j++)
            {
                double Xin = 0.0, Yin = 0.0;
                for (int i = 1; i < fourierSeriesLength + 1; i++)
                {
                    double angle = 2.0 * Math.PI * i * ss[j - 1] / polyLineLength;
                    Xin += XParameter[i, 0] * Math.Cos(angle) + XParameter[i, 1] * Math.Sin(angle);
                    Yin += YParameter[i, 0] * Math.Cos(angle) + YParameter[i, 1] * Math.Sin(angle);
                }

                RecoveryPoints.Add(new MapPoint { X = Xin + XParameter[0, 0], Y = Yin + YParameter[0, 0] });
            }

            return RecoveryPoints;
        }
    }
}
