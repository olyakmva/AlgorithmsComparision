using System;
using System.Collections.Generic;

namespace AlgorithmsLibrary.Features
{
    public interface ICharacteristicsComputation
    {
        void Add(BaseFeatures obj);
        BaseFeatures GetResult();
    }

    public class MaxCharacteristicsComputation: ICharacteristicsComputation
    {
        private BaseFeatures _result;

        public MaxCharacteristicsComputation()
        {
            _result = new BaseFeatures();
        }

        public void Add(BaseFeatures features)
        {
            _result.Compactness = Math.Max(_result.Compactness, features.Compactness);
            _result.Area = Math.Max(_result.Area, features.Area);
            _result.BaseLineLength = Math.Max(_result.BaseLineLength, features.BaseLineLength);
            _result.Height = Math.Max(_result.Height, features.Height);
            _result.HeightBaselineRatio = Math.Max(_result.HeightBaselineRatio, features.HeightBaselineRatio);
            _result.Length = Math.Max(_result.Length, features.Length);
            _result.HeightWidthRatio = Math.Max(_result.HeightWidthRatio, features.HeightWidthRatio);
            _result.Sinuosity = Math.Max(_result.Sinuosity, features.Sinuosity);
            _result.Width = Math.Max(_result.Width, features.Width);
        }

        public BaseFeatures GetResult()
        {
            Operation.MakeRound(_result);
            return _result;
        }
    }

    public class MinCharacteristicsComputation : ICharacteristicsComputation
    {
        private BaseFeatures _result;
        private const double InitValue = 1000000000;
        public MinCharacteristicsComputation()
        {
            _result = new BaseFeatures();
            _result.Compactness = InitValue;
            _result.Area = InitValue;
            _result.BaseLineLength = InitValue;
            _result.Height = InitValue;
            _result.HeightBaselineRatio = InitValue;
            _result.Length = InitValue;
            _result.HeightWidthRatio = InitValue;
            _result.Sinuosity = InitValue;
            _result.Width = InitValue;
        }

        public void Add(BaseFeatures features)
        {
            _result = Operation.Make(features, _result, Operation.GetPositiveMin);
        }

        public BaseFeatures GetResult()
        {
            Operation.MakeRound(_result);
            return _result;
        }
    }

    public class AverageCharacteristicsComputation: ICharacteristicsComputation
    {
        private BaseFeatures _result;
        private int _count;


        public AverageCharacteristicsComputation()
        {
            _count = 0;
            _result = new BaseFeatures();
        }

        public void Add(BaseFeatures features)
        {
            _count++;
            _result.Compactness +=  features.Compactness;
            _result.Area +=  features.Area;
            _result.BaseLineLength += features.BaseLineLength;
            _result.Height += features.Height;
            _result.HeightBaselineRatio +=  features.HeightBaselineRatio;
            _result.Length +=features.Length;
            _result.HeightWidthRatio += features.HeightWidthRatio;
            _result.Sinuosity += features.Sinuosity;
            _result.Width +=  features.Width;
        }

        public BaseFeatures GetResult()
        {
            _result.Compactness /= _count;
            _result.Area /= _count; 
            _result.BaseLineLength /= _count;
            _result.Height /= _count;
            _result.HeightBaselineRatio /= _count;
            _result.Length /= _count;
            _result.HeightWidthRatio /= _count;
            _result.Sinuosity /= _count;
            _result.Width /= _count;
            Operation.MakeRound(_result);
            return _result;
        }
    }

    public class SkoCharacteristics : ICharacteristicsComputation
    {
        private BaseFeatures _result;
        private List<BaseFeatures> _list;

        public BaseFeatures AverageFeatures { get; set; }

        public SkoCharacteristics()
        {
            _list = new List<BaseFeatures>();
            _result = new BaseFeatures();
        }

        public void Add(BaseFeatures obj)
        {
            _list.Add(obj);
        }

        public BaseFeatures GetResult()
        {
            if (AverageFeatures == null)
                throw new ArgumentNullException();
            foreach (var obj in _list)
            {
                var slagaemoe = Operation.Make(obj, AverageFeatures, Operation.GetSqrDifference);
                _result = Operation.Make(slagaemoe, _result, Operation.Sum);
            }
            Operation.Divide(_result, _list.Count);
            Operation.Sqrt(_result);
            Operation.MakeRound(_result);
            return _result;
        }
    }


    public static class Operation
    {
        public static void MakeRound(BaseFeatures f)
        {
            f.Area = Math.Round(f.Area, 3);
            f.BaseLineLength = Math.Round(f.BaseLineLength, 3);
            f.Compactness = Math.Round(f.Compactness, 3);
            f.Height = Math.Round(f.Height, 3);
            f.HeightBaselineRatio = Math.Round(f.HeightBaselineRatio, 3);
            f.HeightWidthRatio = Math.Round(f.HeightWidthRatio, 3);
            f.Length = Math.Round(f.Length, 3);
            f.Sinuosity = Math.Round(f.Sinuosity, 3);
            f.Width = Math.Round(f.Width, 3);

        }

        public static void Divide(BaseFeatures f,int n)
        {
            f.Area =  f.Area/n;
            f.BaseLineLength =  f.BaseLineLength/n;
            f.Compactness =  f.Compactness/n;
            f.Height =  f.Height/n;
            f.HeightBaselineRatio =  f.HeightBaselineRatio/n;
            f.HeightWidthRatio =  f.HeightWidthRatio/n;
            f.Length =  f.Length/n;
            f.Sinuosity =  f.Sinuosity/n;
            f.Width =  f.Width/n;
        }

        public static void Sqrt(BaseFeatures f)
        {
            f.Area = Math.Sqrt(f.Area );
            f.BaseLineLength = Math.Sqrt(f.BaseLineLength );
            f.Compactness = Math.Sqrt(f.Compactness );
            f.Height = Math.Sqrt(f.Height );
            f.HeightBaselineRatio = Math.Sqrt(f.HeightBaselineRatio );
            f.HeightWidthRatio = Math.Sqrt(f.HeightWidthRatio );
            f.Length = Math.Sqrt(f.Length );
            f.Sinuosity = Math.Sqrt(f.Sinuosity );
            f.Width = Math.Sqrt(f.Width );
        }




        public static BaseFeatures Make(BaseFeatures b1, BaseFeatures b2, Func<double, double, double> operation)
        {
            var f = new BaseFeatures();
            f.Area = operation(b1.Area, b2.Area);
            f.BaseLineLength = operation(b1.BaseLineLength, b2.BaseLineLength);
            f.Compactness = operation(b1.Compactness, b2.Compactness);
            f.Height = operation(b1.Height, b2.Height);
            f.HeightBaselineRatio = operation(b1.HeightBaselineRatio, b2.HeightBaselineRatio);
            f.HeightWidthRatio = operation(b1.HeightWidthRatio, b2.HeightWidthRatio);
            f.Length = operation(b1.Length, b2.Length);
            f.Sinuosity = operation(b1.Sinuosity, b2.Sinuosity);
            f.Width = operation(b1.Width, b2.Width);
            return f;
        }

        public static double GetMax(double d1, double d2)
        {
            return Math.Max(d1, d2);
        }
        public static double GetMin(double d1, double d2)
        {
            return Math.Min(d1, d2);
        }
        public static double GetRelation(double d1, double d2)
        {
            return Math.Round(d1/ d2, 3);
        }

        public static double GetSqrDifference(double d1, double d2)
        {
            return (d1 - d2) * (d1 - d2);
        }

        public static double Sum(double d1, double d2)
        {
            return d1 + d2;
        }
        public static double GetPositiveMin(double d1, double d2)
        {
            double  epsilon= 0.001;
            if(d1> epsilon)
                return Math.Min(d1, d2);
            return d2;
        }
    }
}
