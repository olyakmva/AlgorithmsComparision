using System;
using System.Collections.Generic;
using System.Linq;
using AlgorithmsLibrary.Features;


namespace AlgorithmsLibrary
{
    public class HausdorfDistanceCriterion : ICriterion
    {
        private int _neededGhDistance;
        private int _currentGhDistance;
        private int _errorValue;
        private int _n;
        private MapData _initMap;
        private double _prevTolerance;
        private int _prevGhd;
        SortedDictionary<int, double> _iterationValues;

        public void Init(MapData initMap, SimplificationAlgmParameters options)
        {
            _neededGhDistance = (int)Math.Round(options.GhDistance);
            _errorValue = Convert.ToInt32(Math.Round(_neededGhDistance * options.PointNumberGap / 100));
            options.Tolerance = _neededGhDistance;
            _initMap = initMap;
            _prevTolerance = 0;
            _prevGhd = 0;
            _n = 0;
            _iterationValues = new SortedDictionary<int, double>();
        }
        public bool IsSatisfy(MapData map)
        {
            _n++;
            _currentGhDistance = (int)Math.Round(GenHausdorfDistance.Get(_initMap, map));
            if (Math.Abs(_currentGhDistance - _neededGhDistance) < _errorValue)
            {
                return true;
            }
            if (_n > 10)
            {
                return true;
            }
            return false;
        }



        public void GetParamByCriterion(SimplificationAlgmParameters options)
        {
            if (!_iterationValues.ContainsKey(_currentGhDistance))
            {
                _iterationValues.Add(_currentGhDistance, options.Tolerance);
            }
            if (_n == 1)
            {
                _prevTolerance = options.Tolerance;
                _prevGhd = _currentGhDistance;
                options.Tolerance =
                Math.Round(options.Tolerance * ((double)_neededGhDistance / (double)_currentGhDistance));
            }
            else
            {
                var p2 = options.Tolerance;
                if (_currentGhDistance == _prevGhd)
                {
                    options.Tolerance =
                        Math.Round(options.Tolerance * ((double)_neededGhDistance / (double)_currentGhDistance));
                }
                else
                {
                    options.Tolerance = Math.Round(
                        (p2 * (_prevGhd - _neededGhDistance) -
                         _prevTolerance * (_currentGhDistance - _neededGhDistance)) /
                        (_prevGhd - _currentGhDistance));
                    if (options.Tolerance < 0)
                    {
                        options.Tolerance =
                            Math.Round(p2 * ((double)_neededGhDistance / (double)_currentGhDistance));
                    }
                }
                _prevTolerance = p2;
                _prevGhd = _currentGhDistance;
            }
            if (_n == 10)
            {
                var pair = (from p in _iterationValues orderby Math.Abs(_neededGhDistance - p.Key) select p).Take(1)
                    .ToList();
                options.Tolerance = pair[0].Value;
            }
        }
    }
}
