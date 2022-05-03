using System;
using System.Collections.Generic;

namespace AlgorithmsLibrary
{
    public class Fourier
    {
        private double polyLineLength;
        private readonly int fourierSeriesLength;
        private readonly double[,] XParameter;
        private readonly double[,] YParameter;

        private readonly List<MapPoint> arrayOfMapPoints;
        private readonly int countOfPointsObject;
        private double[] sequentialCalculationPolylineLength;
        private double[] arrayDistancesBetweenPoints;

        private readonly bool FourierClosedType;

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
            sequentialCalculationPolylineLength = new double[countOfPointsObject];

            if (!FourierClosedType)
                AddSymmetricPolyline();

            for (int i = 0; i < countOfPointsObject - 1; i++)
            {
                MapPoint p2 = arrayOfMapPoints[i + 1];
                MapPoint p1 = arrayOfMapPoints[i];

                arrayDistancesBetweenPoints[i] = p1.DistanceToVertex(p2);
            }

            double tmpDist = 0.0;
            sequentialCalculationPolylineLength[0] = 0.0;
            for (int i = 0; i < countOfPointsObject - 1; i++)
            {
                sequentialCalculationPolylineLength[i + 1] = tmpDist + arrayDistancesBetweenPoints[i];
                tmpDist = sequentialCalculationPolylineLength[i + 1];
            }

            polyLineLength = sequentialCalculationPolylineLength[countOfPointsObject - 1];
        }

        public double GetRealParameter(int k, double TK1, double TK, double angle)
        {
            double SK1 = sequentialCalculationPolylineLength[k + 1];
            double SK = sequentialCalculationPolylineLength[k];

            double divTS = (TK1 - TK) / (SK1 - SK);

            double SinK1 = Math.Sin(angle * SK1);
            double SinK = Math.Sin(angle * SK);
            double CosK1 = Math.Cos(angle * SK1);
            double CosK = Math.Cos(angle * SK);

            double v1 = (TK - divTS * SK) * (SinK1 - SinK) / angle;
            double v2 = divTS * (SinK1 * SK1 - SinK * SK) / angle;
            double v3 = divTS * (CosK1 - CosK) / (angle * angle);

            return (v1 + v2 + v3) * 2 / polyLineLength;
        }

        public double GetImagineParameter(int k, double TK1, double TK, double angle)
        {
            double SK1 = sequentialCalculationPolylineLength[k + 1];
            double SK = sequentialCalculationPolylineLength[k];

            double divTS = (TK1 - TK) / (SK1 - SK);

            double SinK1 = Math.Sin(angle * SK1);
            double SinK = Math.Sin(angle * SK);
            double CosK1 = Math.Cos(angle * SK1);
            double CosK = Math.Cos(angle * SK);

            double v1 = -(TK - divTS * SK) * (CosK1 - CosK) / angle;
            double v2 = -divTS * (CosK1 * SK1 - CosK * SK) / angle;
            double v3 = divTS * (SinK1 - SinK) / (angle * angle);

            return (v1 + v2 + v3) * 2 / polyLineLength;
        }

        public double[,] GetFourierXparameter()
        {
            for (int k = 0; k < countOfPointsObject - 1; k++)
            {
                double diffX = arrayOfMapPoints[k + 1].X - arrayOfMapPoints[k].X;
                double diffS = sequentialCalculationPolylineLength[k + 1] - sequentialCalculationPolylineLength[k];
                double summ1 = arrayOfMapPoints[k].X * diffS;
                double summ2 = 0.5 * diffX * diffS;
                XParameter[0, 0] += (summ1 + summ2) / polyLineLength;
            }

            for (int n = 1; n < fourierSeriesLength + 1; n++)
            {
                double angle = 2.0 * Math.PI * n / polyLineLength;
                for (int k = 0; k < countOfPointsObject - 1; k++)
                {
                    XParameter[n, 0] += GetRealParameter(k, arrayOfMapPoints[k + 1].X, arrayOfMapPoints[k].X, angle);
                }

                for (int k = 0; k < countOfPointsObject - 1; k++)
                {
                    XParameter[n, 1] += GetImagineParameter(k, arrayOfMapPoints[k + 1].X, arrayOfMapPoints[k].X, angle);
                }
            }

            return XParameter;
        }
        public double[,] GetFourierYparameter()
        {
            for (int k = 0; k < countOfPointsObject - 1; k++)
            {
                double diffY = arrayOfMapPoints[k + 1].Y - arrayOfMapPoints[k].Y;
                double diffS = sequentialCalculationPolylineLength[k + 1] - sequentialCalculationPolylineLength[k];
                double summ1 = arrayOfMapPoints[k].Y * diffS;
                double summ2 = 0.5 * diffY * diffS;
                YParameter[0, 0] += (summ1 + summ2) / polyLineLength;
            }

            for (int n = 1; n < fourierSeriesLength + 1; n++)
            {
                double angle = 2.0 * Math.PI * n / polyLineLength;
                for (int k = 0; k < countOfPointsObject - 1; k++)
                {
                    YParameter[n, 0] += GetRealParameter(k, arrayOfMapPoints[k + 1].Y, arrayOfMapPoints[k].Y, angle);
                }

                for (int k = 0; k < countOfPointsObject - 1; k++)
                {
                    YParameter[n, 1] += GetImagineParameter(k, arrayOfMapPoints[k + 1].Y, arrayOfMapPoints[k].Y, angle);
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
