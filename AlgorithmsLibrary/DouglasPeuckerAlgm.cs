using System.Collections.Generic;
using System;

namespace AlgorithmsLibrary
{
    public class Pair
    {
        public int Start { get; private set; }
        public int End { get;  set; }

        public Pair(int begin, int end)
        {
            Start = begin;
            End = end;
        }
    }
    /// <summary>
    /// Алгоритм Дугласа-Пейкера
    /// </summary>
    public abstract class DouglasPeuckerBaseAlgm : ISimplificationAlgm
    {
        public SimplificationAlgmParameters Options { get; set; }

        public virtual void Run(MapData map)
        {
            foreach (var chain in map.VertexList)
            {
                int endIndex = chain.Count - 1;
                Run(chain, 0, ref endIndex);
            }
            Options.OutParam = Options.Tolerance;
        }

        /// <summary>
        /// генерализация с помощью алгоритма Дугласа-Пейкера
        /// </summary>
        /// <param name="chain"> список точек ломаной</param>
        /// <param name="startIndex">индекс начальной точки</param>
        /// <param name="endIndex">индекс последней точки </param>
        protected virtual void Run(List<MapPoint> chain, int startIndex, ref int endIndex)
        {
            double tolerance = Options.Tolerance;
            const int weightUnremovingVertex = 100;
            const double epsilon = 0.1;
            if (chain.Count < 3)
            {
                ProcessOneEdge(chain, 0, chain.Count - 1, tolerance);
                return;
            }

            var indexes = new Stack<Pair>();
            var pair = new Pair(startIndex, endIndex);

            if (Math.Abs(chain[startIndex].X - chain[endIndex].X) < epsilon ||
                Math.Abs(chain[startIndex].Y - chain[endIndex].Y) < epsilon)
            {
                pair.End = endIndex - 1;
            }
            
            indexes.Push(pair);
            chain[startIndex].Weight = weightUnremovingVertex;
            chain[pair.End ].Weight = weightUnremovingVertex;

            while (indexes.Count > 0)
            {
                pair = indexes.Pop();
                double maxDistance;
                int maxDistanceIndex = FindMaxDistantion(chain, pair, out maxDistance);
                if (maxDistance <= tolerance) continue;
                chain[maxDistanceIndex].Weight = weightUnremovingVertex;
                if(maxDistanceIndex !=pair.Start)
                    indexes.Push(new Pair(pair.Start, maxDistanceIndex));
                if(maxDistanceIndex!= pair.End)
                        indexes.Push(new Pair(maxDistanceIndex, pair.End));
            }
            int i = startIndex + 1;
            while (i < endIndex)
            {
                if (Math.Abs(chain[i].Weight - weightUnremovingVertex) > double.Epsilon)
                {
                    chain.RemoveAt(i);
                    endIndex--;
                }
                else i++;
            }
        }
        private int FindMaxDistantion(List<MapPoint> vertices, Pair ind, out double maxDistance)
        {
            int maxDistanceIndex = ind.Start + 1;
            maxDistance = 0;
            if (Math.Abs(ind.End - ind.Start) <= 1) return maxDistanceIndex;
            var line = new Line(vertices[ind.Start], vertices[ind.End]);

            for (int i = ind.Start + 1; i < ind.End; i++)
            {
                double d = 0;
                if ((vertices[i].X <= Math.Max(vertices[ind.Start].X, vertices[ind.End].X) &&
                     vertices[i].X >= Math.Min(vertices[ind.Start].X, vertices[ind.End].X)) ||
                    (vertices[i].Y <= Math.Max(vertices[ind.Start].Y, vertices[ind.End].Y) &&
                     vertices[i].Y >= Math.Min(vertices[ind.Start].Y, vertices[ind.End].Y)))
                {
                    d = Math.Abs(line.GetDistance(vertices[i]));
                }
                else
                    d = Math.Min(vertices[i].DistanceToVertex(vertices[ind.Start]),
                        vertices[i].DistanceToVertex(vertices[ind.End]));
                if (!(maxDistance < d)) continue;
                maxDistance = d;
                maxDistanceIndex = i;
            }
            return maxDistanceIndex;
        }

        private void ProcessOneEdge(List<MapPoint> vertices, int start, int last, double tolerance)
        {
            const int douglasWeight = 100;
            vertices[start].Weight = douglasWeight;
            vertices[last].Weight = douglasWeight;
            var distance = vertices[last].DistanceToVertex(vertices[start]);
            if (distance > tolerance) return;
            if (start == 0 && last < vertices.Count - 1)
            {
                vertices.RemoveAt(last);
            }
            else if (start > 0 && last == vertices.Count - 1)
            {
                vertices.RemoveAt(start);
            }
            else if (start == 0 && last == vertices.Count - 1)
            {
                
            }
            else
            {
                vertices[start].X = (vertices[start].X + vertices[last].X) / 2;
                vertices[start].Y = (vertices[start].Y + vertices[last].Y) / 2;
                vertices.RemoveAt(last);
            }
        }
    }

    public class DouglasPeuckerAlgm : DouglasPeuckerBaseAlgm
    {
        
    }
    public class DouglasPeuckerAlgmWithCriterion : DouglasPeuckerBaseAlgm
    {
        // известен процент точек, которые должны остаться. Надо подобрать Tolerance
        private ICriterion _criterion;

        public DouglasPeuckerAlgmWithCriterion(ICriterion cr)
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
}
