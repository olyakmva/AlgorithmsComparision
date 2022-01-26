
using System;

namespace AlgorithmsLibrary
{
    public interface ICriterion
    {
        void GetParamByCriterion(SimplificationAlgmParameters options);
        void Init(MapData initMap, SimplificationAlgmParameters options);
        bool IsSatisfy(MapData map);
    }
    public interface ISimplificationAlgm
    {
        SimplificationAlgmParameters Options { get; set; }
        void Run(MapData map);
    }

    public class SimplificationAlgmParameters
    {
        public double Tolerance { get; set; }
        public int OutScale { get; set; }
        public double OutParam { get; set; }
        public double PointNumberGap { get; set; }
        public double RemainingPercent
        {
            get => _remainingPercent;
            set
            {
                double p = Math.Round( 100 / value); //100 - value
                if (p > 100 || p < 1)
                    throw new ArgumentException("reduction percent must be between 1-99");
                _remainingPercent = p;
            }
        }
        private double _remainingPercent;

        public double BendReduction { get; set; }
        public double GhDistance { get; set; }
        
    }
}
