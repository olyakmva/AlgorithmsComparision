using System;
using System.Collections.Generic;

namespace AlgorithmsLibrary
{
    public class LiOpenshawAlgm : ISimplificationAlgm
    {
        private const double LiWeight = 200;
        private const double RightAngleWeight = -90;
        private double _cellSize;
        private double _shift;
        public SimplificationAlgmParameters Options { get; set; }
        
        /// <summary>
        /// генерализация с помощью алгоритма Ли-Оупеншоу
        /// На исходное множество линий накладывается регулярная сетка 
        /// (квадратная), вычисляются  точки пересечения исходных линий с сеткой.
        /// В результат помещаются середины отрезков от одной точки пересечения с сеткой до другой.
        /// Шаг сетки зависит от масштаба и соответствует минимально различимой детали (2 мм, например)
        /// </summary>
        /// <param name="vertices">данные в виде последовательности точек</param>
        /// <param name="endIndx">индекс последней точки</param>
        /// <param name="startIndx">начальный индекс</param>
        /// <returns>упрощенные данные</returns>
        private void Run(List<MapPoint> vertices, int startIndx, ref int endIndx)
        {

            _cellSize = Options.Tolerance;
            _shift = Math.Truncate(_cellSize / 2);
            switch (endIndx - startIndx)
            {
                case 0:
                    return;
                case 1:
                    ProcessOneEdge(vertices, startIndx, ref endIndx);
                    return;
                case 2:
                    var distance23 = vertices[endIndx].DistanceToVertex(vertices[startIndx + 1]);
                    var distance12 = vertices[startIndx].DistanceToVertex(vertices[startIndx + 1]);
                    if (Math.Min(distance12, distance23) > _cellSize)
                    {
                        return;
                    }
                    if (distance12 < _cellSize && distance23 < _cellSize)
                    {
                        vertices.RemoveAt(startIndx + 1);
                        endIndx = startIndx + 1;
                        ProcessOneEdge(vertices, startIndx, ref endIndx);
                    }
                    else if (distance12 < _cellSize)
                    {
                        int k = startIndx + 1;
                        ProcessOneEdge(vertices, startIndx, ref k);
                    }
                    else
                    {
                        ProcessOneEdge(vertices, startIndx + 1, ref endIndx);
                    }
                    return;
            }
            if (vertices[startIndx].Weight > RightAngleWeight)
                vertices[startIndx].Weight = LiWeight;
            for (int j = startIndx + 1; j < endIndx; j++)
            {
                vertices[j].Weight = LiWeight;
            }
            if (vertices[endIndx].Weight > RightAngleWeight)
            {
                vertices[endIndx].Weight = LiWeight;
            }
            // добавление новых вершин
            int i = startIndx;
            while (i < endIndx - 1)
            {
                var d = vertices[i].DistanceToVertex(vertices[i + 1]);
                if (d < _cellSize)
                {
                    i++; continue;
                }
                double x = (vertices[i].X + vertices[i + 1].X) / 2;
                double y = (vertices[i].Y + vertices[i + 1].Y) / 2;
                var vertex = new MapPoint(x, y, vertices[i].Id, LiWeight);
                vertices.Insert(i + 1, vertex);
                endIndx++;
            }
            // Ли-Оупеншоу 
            double xmin, xmax, ymin, ymax;
            MinMaxValues.Compute(new List<MapPoint>(vertices), out xmin, out ymin, out xmax, out ymax);
            var outChain = new List<MapPoint>();
            var vStack = new Stack<MapPoint>();

            i = startIndx;
            double nextCellNumX = 0;

            while (i < endIndx)
            {
                MapPoint v = vertices[i];
                var cellNumX = GetCellNumberX(v, xmin);
                var cellNumY = GetCellNumberY(v, ymin);
                i++;
                MapPoint nextVertex = null;
                double nextCellNumY = 0;
                var findVertexFromOtherSell = false;
                while (i < endIndx)
                {
                    nextVertex = vertices[i];
                    nextCellNumX = GetCellNumberX(nextVertex, xmin);
                    nextCellNumY = GetCellNumberY(nextVertex, ymin);
                    if (Math.Abs(cellNumX - nextCellNumX) > 0 || Math.Abs(cellNumY - nextCellNumY) > 0)
                    {
                        findVertexFromOtherSell = true;
                        break;
                    }
                    i++;
                }
                // нашли вершину из другой ячейки
                if (!findVertexFromOtherSell) continue;
                // найти точку пересечения с сеткой
                var gridIntersectionVtx = new MapPoint(0, 0, vertices[i - 1].Id, 1);
                var line = new Line(v, nextVertex);
                if (Math.Abs(cellNumX - nextCellNumX) > 0)
                {
                    gridIntersectionVtx.X = (xmin - _shift) + _cellSize * Math.Max(nextCellNumX, cellNumX);
                    gridIntersectionVtx.Y = line.GetY(gridIntersectionVtx.X);
                }
                else if (Math.Abs(cellNumY - nextCellNumY) > 0)
                {
                    gridIntersectionVtx.Y = (ymin - _shift) + _cellSize * Math.Max(nextCellNumY, cellNumY);
                    gridIntersectionVtx.X = line.GetX(gridIntersectionVtx.Y);
                }
                else
                {
                    ErrorLog.WriteToLogFile(" изменилась ячейка по х и по у");
                }

                vStack.Push(gridIntersectionVtx);
                if (vStack.Count <= 1) continue;
                // ищем середину отрезка
                var v2 = vStack.Pop();
                var v1 = vStack.Pop();

                var middleVertex = new MapPoint((v2.X + v1.X) / 2, (v2.Y + v1.Y) / 2, v1.Id, LiWeight);
                // добавить  в цепь выходную
                outChain.Add(middleVertex);
                vStack.Push(v2);
            }
            vertices.RemoveRange(startIndx + 1, endIndx - startIndx - 1);
            vertices.InsertRange(startIndx + 1, outChain);
            endIndx = startIndx + outChain.Count;

        }

        private int GetCellNumberY(MapPoint v, double ymin)
        {
            return (int)Math.Truncate((v.Y - ymin + _shift) / _cellSize);
        }

        private int GetCellNumberX(MapPoint v, double xmin)
        {
            return (int)Math.Truncate((v.X - xmin + _shift) / _cellSize);
        }

        private void ProcessOneEdge(List<MapPoint> vertices, int start, ref int last)
        {
            if (vertices[start].Weight > RightAngleWeight)
                vertices[start].Weight = LiWeight;
            if (vertices[last].Weight > RightAngleWeight)
                vertices[last].Weight = LiWeight;
            var distance = vertices[last].DistanceToVertex(vertices[start]);
            if (distance > _cellSize) return;
            if (start == 0 && last < vertices.Count - 1)
            {
                vertices.RemoveAt(last);
                vertices[start].Weight = LiWeight;
                last--;
            }
            else if (start > 0 && last == vertices.Count - 1)
            {
                vertices.RemoveAt(start);
                last--;
            }
            else if (start == 0 && last == vertices.Count - 1)
            {
            }
            else
            {
                vertices[start].X = (vertices[start].X + vertices[last].X) / 2;
                vertices[start].Y = (vertices[start].Y + vertices[last].Y) / 2;
                vertices.RemoveAt(last);
                last--;
            }
        }

        

        public virtual void Run(MapData map)
        {
            foreach (var chain in map.VertexList)
            {
                int endIndex = chain.Count - 1;
                Run(chain, 0, ref endIndex);
            }
            Options.OutParam = Options .Tolerance;
        }
    }

    public class LiOpenshawWithCriterion : LiOpenshawAlgm
    {
        private readonly ICriterion _criterion;

        public LiOpenshawWithCriterion(ICriterion cr)
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
