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
        private double[] sequentialCalculationPolylineLength;
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
            for (int i = basePointCount; i < countOfPointsObject; i++)
            {
                double X1 = arrayOfMapPoints[0].X;
                double Y1 = arrayOfMapPoints[0].Y;
                double X2 = arrayOfMapPoints[basePointCount - 1].X;
                double Y2 = arrayOfMapPoints[basePointCount - 1].Y;
                double A = Y2 - Y1;
                double B = X1 - X2;
                double C = X2 * Y1 - X1 * Y2;
                double C_after = (A * arrayOfMapPoints[countOfPointsObject - 1 - i].X + B * arrayOfMapPoints[countOfPointsObject - 1 - i].Y + C) / (A * A + B * B);
                double X_symmetry = arrayOfMapPoints[countOfPointsObject - 1 - i].X - 2 * A * C_after;
                double Y_symmetry = arrayOfMapPoints[countOfPointsObject - 1 - i].Y - 2 * B * C_after;

                arrayOfMapPoints.Add(new MapPoint { X = X_symmetry, Y = Y_symmetry });
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
