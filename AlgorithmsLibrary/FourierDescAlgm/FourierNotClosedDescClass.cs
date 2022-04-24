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
    }
}