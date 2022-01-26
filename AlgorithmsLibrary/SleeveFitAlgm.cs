using System;
using System.Collections.Generic;


namespace AlgorithmsLibrary
{
    public class SleeveFitAlgm : ISimplificationAlgm
    {
        public SimplificationAlgmParameters Options { get; set; }

        public virtual void Run(MapData map)
        {
            foreach (var chain in map.VertexList)
            {
                Process(chain);
            }
            Options.OutParam = Options.Tolerance;
        }
        void Process(List<MapPoint> chain)
        {
            var i = 0;
            while (i < chain.Count - 1)
            {
                var indStart = i;
                var indEnd = ExstractLineSegment(chain, indStart);
                i = indEnd;
                chain[indStart].Weight = 2;
                chain[indEnd].Weight = 2;
            }
            chain.RemoveAll(point => point.Weight < 2);
        }
        private int ExstractLineSegment(List<MapPoint> chain, int indStart)
        {
            int indEnd = indStart + 2;

            bool isLine = true;
            while (indEnd < chain.Count && isLine)
            {
                Line line;
                if (indEnd < chain.Count && chain[indStart].CompareTo(chain[indEnd]) == 0)
                {
                    break;
                }
                try
                {
                    line = new Line(chain[indStart], chain[indEnd]);
                }
                catch (LineCoefEqualsZeroException e)
                {
                    throw new ApplicationException(e.Message);
                }

                for (int j = indStart + 1; j < indEnd; j++)
                {
                    var dist = line.GetDistance(chain[j]);
                    if (dist > Options.Tolerance)
                    {
                        isLine = false;
                        indEnd--;
                        break;
                    }
                }
                indEnd++;
            }
            // конец прямолинейного участка
            indEnd--;
            return indEnd;
        }

    }

    public class SleeveFitWithCriterion : SleeveFitAlgm
    {
        ICriterion _criterion;

        public SleeveFitWithCriterion(ICriterion cr)
        {
            _criterion = cr;
        }

        public override void Run(MapData map)
        {
            _criterion.Init(map, Options);

            var tempMap = map.Clone();
            while (true)
            {
                base.Run(tempMap);
                if (_criterion.IsSatisfy(tempMap))
                {
                    break;
                }
                _criterion.GetParamByCriterion(Options);
                tempMap = map.Clone();
            }
            base.Run(map);
            Options.OutParam = Options.Tolerance;
        }

    }

    public class SleeveFitWithPercent : SleeveFitAlgm
    {
        public override void Run(MapData map)
        {
            int mapCount = map.Count;
            int neededMapCount = Convert.ToInt32(Math.Round((Options.RemainingPercent * mapCount) / 100));
            int delta = Convert.ToInt32(Math.Round( mapCount*Options.PointNumberGap / 100));
            var tempMap = map.Clone();
            Options.Tolerance = Math.Round(Options.OutScale * 1.0);

            while (true)
            {
                base.Run(tempMap);
                int resultPointNumber = tempMap.Count;
                if (Math.Abs(resultPointNumber - neededMapCount) < delta)
                {
                    break;
                }
                Options.Tolerance = Options.Tolerance * (mapCount - neededMapCount) / (mapCount - resultPointNumber);
                tempMap = map.Clone();
            }
            base.Run(map);
            Options.OutParam = Options.Tolerance;
        }
    }

}
