using System;
using System.Collections.Generic;

namespace AlgorithmsLibrary
{
    public class Fourier : FourierDescFatherClass
    {
        public Fourier(List<MapPoint> mapPoints, long fourierSeriesLength, double approximationRatio)
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
                    countOfPointsObject = arrayOfMapPoints.Count;
                    break;
                }
            }

            XParameter = new double[fourierSeriesLength + 1, 2];
            YParameter = new double[fourierSeriesLength + 1, 2];
        }

        public override void GetAllDist()
        {
            arrayDistancesBetweenPoints = new double[countOfPointsObject - 1];
            sequentialCalculationPolylineLength = new double[countOfPointsObject];

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