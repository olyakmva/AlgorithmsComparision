using System;
using AlgorithmsLibrary.Features;


namespace AlgorithmsLibrary
{
    public class BendNumberCriterion : ICriterion
    {
        private int _neededNumberOfBends;
        private int _currentNumberOfBends;
        private int _initNumberOfBends;
        private int _errorValue;
        private int _n;

        private double _prevTolerance;
        private int _prevBendNumber;

        public void Init(MapData initMap, SimplificationAlgmParameters parms)
        {
            _initNumberOfBends = BendComputation.Get(initMap);
            _neededNumberOfBends = Convert.ToInt32(Math.Round(_initNumberOfBends / parms.BendReduction));
            _errorValue = Convert.ToInt32(Math.Round(_neededNumberOfBends * parms.PointNumberGap / 100));
            parms.Tolerance = Math.Round(parms.OutScale * 1.0);
            _n = 0;
            _currentNumberOfBends = 0;
            _prevBendNumber = 0;
            _prevTolerance = 0;
        }
        
        public void GetParamByCriterion(SimplificationAlgmParameters p)
        {
            if (_n == 1)
            {
                _prevTolerance = p.Tolerance;
                _prevBendNumber = _currentNumberOfBends;
                p.Tolerance =
                    Math.Round(p.Tolerance * ((double)_currentNumberOfBends / (double)_neededNumberOfBends));
            }
            else
            {
                var p2 = p.Tolerance;
                if (_currentNumberOfBends == _prevBendNumber)
                {
                    p.Tolerance =
                        Math.Round(p2 * ((double)_currentNumberOfBends / (double)_neededNumberOfBends));
                }
                else
                {
                    p.Tolerance = Math.Round(
                        (p2 * (_prevBendNumber - _neededNumberOfBends) -
                         _prevTolerance * (_currentNumberOfBends - _neededNumberOfBends)) /
                        (_prevBendNumber - _currentNumberOfBends));
                    if (p.Tolerance < 0)
                    {
                        p.Tolerance =
                            Math.Round(p2 *
                                       ((double)_currentNumberOfBends / (double)_neededNumberOfBends));
                    }
                }
                _prevTolerance = p2;
                _prevBendNumber = _currentNumberOfBends;
            }
        }

        public bool IsSatisfy(MapData map)
        {
            _n++;
            _currentNumberOfBends = BendComputation.Get(map);
            if (Math.Abs(_currentNumberOfBends - _neededNumberOfBends) <= _errorValue)
            {
                return true;
            }
            return false;
        }
    }

    public class VisWBendNumberCriterion : ICriterion
    {
        private int _neededNumberOfBends;
        private int _currentNumberOfBends;
        private int _initNumberOfBends;
        private int _errorValue;
        private int _n;
        private double _prevTolerance;
        private int _prevBendNumber;

        public void Init(MapData initMap, SimplificationAlgmParameters parms)
        {
            _initNumberOfBends = BendComputation.Get(initMap);
            _neededNumberOfBends = Convert.ToInt32(Math.Round(_initNumberOfBends / parms.BendReduction));
            _errorValue = Convert.ToInt32(Math.Round(_neededNumberOfBends * parms.PointNumberGap / 100));
            parms.Tolerance = parms.OutScale ;
            _n = 0;
            _currentNumberOfBends = 0;
            _prevBendNumber = 0;
            _prevTolerance = 0;
        }

        public void GetParamByCriterion(SimplificationAlgmParameters p)
        {
            if (_n == 1)
            {
                _prevTolerance = Math.Sqrt(p.Tolerance);
                _prevBendNumber = _currentNumberOfBends;
                p.Tolerance =
                    Math.Round(Math.Sqrt(p.Tolerance) * ((double)_currentNumberOfBends / (double)_neededNumberOfBends));
            }
            else
            {
                var p2 = Math.Sqrt(p.Tolerance);
                if (_currentNumberOfBends == _prevBendNumber)
                {
                    p.Tolerance =
                        Math.Round(p2 * ((double)_currentNumberOfBends / (double)_neededNumberOfBends));
                }
                else
                {
                    p.Tolerance = Math.Round(
                        (p2 * (_prevBendNumber - _neededNumberOfBends) -
                         _prevTolerance * (_currentNumberOfBends - _neededNumberOfBends)) /
                        (_prevBendNumber - _currentNumberOfBends));
                    if (p.Tolerance < 0)
                    {
                        p.Tolerance =
                            Math.Round(p2 *
                                       ((double)_currentNumberOfBends / (double)_neededNumberOfBends));
                    }
                }
                _prevTolerance = p2;
                _prevBendNumber = _currentNumberOfBends;
            }
        }

        public bool IsSatisfy(MapData map)
        {
            _n++;
            _currentNumberOfBends = BendComputation.Get(map);
            if (Math.Abs(_currentNumberOfBends - _neededNumberOfBends) <= _errorValue)
            {
                return true;
            }
            return false;
        }
    }



    public class PointPercentCriterion : ICriterion
    {
        private int _neededPointNumber;
        private int _currentPointNumber;
        private int _initPointNumber;
        private int _errorValue;
        private int _prevPointNum;
        private double _prevTolerance;

        private int _n ;

        public void GetParamByCriterion(SimplificationAlgmParameters options)
        {
            if (_n == 1 )
            {
                _prevTolerance = options.Tolerance;
                _prevPointNum = _currentPointNumber;
                options.Tolerance =
                    Math.Round(options.Tolerance * ((double) _currentPointNumber / (double) _neededPointNumber));
            }
            else
            {
                var p2 = options.Tolerance;
                if (_currentPointNumber == _prevPointNum)
                {
                    options.Tolerance =
                        Math.Round(p2 * ((double) _currentPointNumber / (double) _neededPointNumber));
                }
                else
                {
                    options.Tolerance = Math.Round(
                        (p2 * (_prevPointNum - _neededPointNumber) -
                         _prevTolerance * (_currentPointNumber - _neededPointNumber)) /
                        (_prevPointNum - _currentPointNumber));
                    if (options.Tolerance < 0)
                    {
                        options.Tolerance =
                            Math.Round(p2 *
                                       ((double) _currentPointNumber / (double) _neededPointNumber));
                    }
                }
                _prevTolerance = p2;
                _prevPointNum = _currentPointNumber;
            }
        }

        public void Init(MapData initMap, SimplificationAlgmParameters options)
        {
            _initPointNumber = initMap.Count;
            _neededPointNumber = Convert.ToInt32(Math.Round((options.RemainingPercent * _initPointNumber) / 100));
            _errorValue = Convert.ToInt32(Math.Round(_initPointNumber * options.PointNumberGap / 100));
            options.Tolerance = Math.Round(options.OutScale * 1.0);
            _prevPointNum = 0;
            _prevTolerance = 0;
            _n = 0;
            _currentPointNumber = 0;
        }

        public bool IsSatisfy(MapData map)
        {
            _n++;
            _currentPointNumber = map.Count;
            if (Math.Abs(_currentPointNumber - _neededPointNumber) < _errorValue)
            {
                return true;
            }
            return false;
        }
    }


}
