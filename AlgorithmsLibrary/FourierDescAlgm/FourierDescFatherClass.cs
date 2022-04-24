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
        protected internal long countOfPointsObject = 0;
        protected internal double[] sequentialCalculationPolylineLength = null;
        protected internal double[] arrayDistancesBetweenPoints = null;

        public void CalculateAllValue()
        {
            GetAllDist();
            GetFourierXparameter();
            GetFourierYparameter();
        }

        public abstract void GetAllDist();
        public abstract double[,] GetFourierXparameter();
        public abstract double[,] GetFourierYparameter();
    }
}
