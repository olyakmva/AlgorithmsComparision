using System;
using System.Collections.Generic;

namespace AlgorithmsLibrary
{
    public class FourierNotClosed : FourierDescFatherClass
    {
        public FourierNotClosed(List<MapPoint> mapPoints, long fourierSeriesLength, double approximationRatio)
        {
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
                    countOfPointsObject = arrayOfMapPoints.Count * 2 - 1;
                    break;
                }
            }

            XParameter = new double[fourierSeriesLength + 1, 2];
            YParameter = new double[fourierSeriesLength + 1, 2];
        }

        private void AddSymmetricPolyline()
        {
            int basePointCount = arrayOfMapPoints.Count;
            for (int i = basePointCount; i < countOfPointsObject; i++)
            {
                MapPoint p;
                p = new MapPoint();
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
                p.X = X_symmetry;
                p.Y = Y_symmetry;
                arrayOfMapPoints.Add(p);
            }
        }

        public override void GetAllDist()
        {
            arrayDistancesBetweenPoints = new double[countOfPointsObject - 1];
            sequentialCalculationPolylineLength = new double[countOfPointsObject];

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

        public List<MapPoint> GetRecoveryPoints(long OutputPointCount)
        {
            var RecoveryPoints = new List<MapPoint>();

            GetAllDist();
            double[] ss = new double[OutputPointCount + 1];
            double s_average = polyLineLength / (double)OutputPointCount;
            for (int i = 1; i < OutputPointCount; i++)
            {
                ss[i] = ss[i - 1] + s_average;
            }

            for (int j = 1; j < OutputPointCount + 1; j++)
            {
                double Xin = 0.0, Yin = 0.0;
                for (int i = 1; i < fourierSeriesLength + 1; i++)
                {
                    double angle = 2.0 * Math.PI * (double)i * ss[j - 1] / polyLineLength;
                    Xin += XParameter[i, 0] * Math.Cos(angle) + XParameter[i, 1] * Math.Sin(angle);
                    Yin += YParameter[i, 0] * Math.Cos(angle) + YParameter[i, 1] * Math.Sin(angle);
                }

                RecoveryPoints.Add(new MapPoint(Xin + XParameter[0, 0], Yin + YParameter[0, 0], j - 1, 1));
            }

            return RecoveryPoints;
        }
    }
}