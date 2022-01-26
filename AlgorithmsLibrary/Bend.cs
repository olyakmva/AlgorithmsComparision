using System;
using System.Collections.Generic;

namespace AlgorithmsLibrary
{
    public class Bend
    {
        /// <summary>
        /// список вершин в последовательности, в которой они идут в изгибе
        /// </summary>
        public List<MapPoint> BendNodeList { set; get; }

        #region Constructors

        public Bend()
        {
            BendNodeList = new List<MapPoint>();
        }

        public Bend(List<MapPoint> vert)
            : this()
        {
            BendNodeList.AddRange(vert);
        }

        #endregion

        /// <summary>
        /// вычисление площади изгиба
        /// </summary>
        /// <returns>площадь изгиба</returns>
        public double Area()
        {
            if (BendNodeList.Count < 3)
                return 0;

            double result = 0;
            for (var i = 0; i < BendNodeList.Count - 1; i++)
            {
                result += (BendNodeList[i].X + BendNodeList[i + 1].X) * (BendNodeList[i].Y - BendNodeList[i + 1].Y);
            }

            result += (BendNodeList[BendNodeList.Count - 1].X + BendNodeList[0].X) * (BendNodeList[BendNodeList.Count - 1].Y - BendNodeList[0].Y);

            return Math.Abs(result / 2);
        }

        /// <summary>
        /// вычисление периметра изгиба
        /// </summary>
        /// <returns>периметр изгиба, включая основание</returns>
        public double Perimeter()
        {
            if (BendNodeList.Count < 3)
                return 0;

            return Length() + BaseLineLength();
        }
        /// <summary>
        /// вычисление длины линии, образующей изгиб
        /// </summary>
        /// <returns>длина</returns>
        public double Length()
        {
            if (BendNodeList.Count < 3)
                return 0;

            double result = 0;

            for (var i = 0; i < BendNodeList.Count - 1; i++)
            {
                result += Math.Sqrt((BendNodeList[i + 1].X - BendNodeList[i].X) * (BendNodeList[i + 1].X - BendNodeList[i].X) + (BendNodeList[i + 1].Y - BendNodeList[i].Y) * (BendNodeList[i + 1].Y - BendNodeList[i].Y));
            }
            return result;
        }
        /// <summary>
        /// подсчёт индекса компактности
        /// </summary>
        /// <returns>индекс компактности</returns>
        public double CompactIndex()
        {
            if (BendNodeList.Count < 3)
                return 0;

            return 4 * Math.PI * Area() / Math.Pow(Perimeter(), 2);
        }

        public double BaseLineLength()
        {
            if (BendNodeList.Count < 2)
                return 0;
            return Math.Sqrt((BendNodeList[BendNodeList.Count - 1].X - BendNodeList[0].X) * (BendNodeList[BendNodeList.Count - 1].X - BendNodeList[0].X) + (BendNodeList[BendNodeList.Count - 1].Y - BendNodeList[0].Y) * (BendNodeList[BendNodeList.Count - 1].Y - BendNodeList[0].Y));
        }

        public double[] BendMiddlePoint()
        {
            if (BendNodeList.Count == 0)
                return new double[2];

            return new[]{(BendNodeList[0].X + BendNodeList[BendNodeList.Count - 1].X) / 2, (BendNodeList[0].Y + BendNodeList[BendNodeList.Count - 1].Y) / 2};
        }

        /// <summary>
        /// поиск пика изгиба
        /// </summary>
        /// <returns>индекс пика изгиба</returns>
        public int PeakIndex()
        {
            if (BendNodeList.Count == 0)
                return 0;

            MapPoint begin = BendNodeList[0];
            MapPoint end = BendNodeList[BendNodeList.Count - 1];
            var peakIndex = 0;
            double maxSum = 0;
            for (var i = 1; i < BendNodeList.Count - 1; i++)
            {
                var tempSum = Math.Sqrt(Math.Pow((BendNodeList[i].X - begin.X), 2) + Math.Pow((BendNodeList[i].Y - begin.Y), 2)) +
                                 Math.Sqrt(Math.Pow((BendNodeList[i].X - end.X), 2) + Math.Pow((BendNodeList[i].Y - end.Y), 2));

                if (!(tempSum > maxSum)) continue;
                maxSum = tempSum;
                peakIndex = i;
            }
            return peakIndex;
        }
        /// <summary>
        /// вычисление высоты изгиба
        /// </summary>
        /// <returns>высота</returns>
        public double GetHeight()
        {
            var baseLine = new Line(BendNodeList[0], BendNodeList[BendNodeList.Count-1]);
            int peakIndex = PeakIndex();
            return baseLine.GetDistance(BendNodeList[peakIndex]);
        }
        /// <summary>
        /// вычисление ширины изгиба
        /// </summary>
        /// <returns> ширина</returns>
        public double GetWidth()
        {
            if (BendNodeList.Count < 3)
                return 0;

            var baseLine = new Line(BendNodeList[0], BendNodeList[BendNodeList.Count - 1]);
            //  проведем перпендикулярную прямую
            var leftLine = baseLine.GetPerpendicularLine(BendNodeList[0]);
            var leftPoint = BendNodeList[0];
            
            if (BendNodeList.Count == 3)
            {
                leftPoint = baseLine.GetPerpendicularFoundationPoint(BendNodeList[1]);
                leftLine = new Line(BendNodeList[1], leftPoint);
                if (leftLine.GetSign(BendNodeList[0]) != leftLine.GetSign(BendNodeList[BendNodeList.Count - 1]))
                {
                    return BendNodeList[0].DistanceToVertex(BendNodeList[2]);
                }
                return Math.Max(BendNodeList[0].DistanceToVertex(leftPoint), BendNodeList[2].DistanceToVertex(leftPoint));
            }

            if (leftLine.GetSign(BendNodeList[1]) * leftLine.GetSign(BendNodeList[BendNodeList.Count-1]) <0)
            {
                leftPoint = baseLine.GetPerpendicularFoundationPoint(BendNodeList[1]);
                if( baseLine.GetSign(leftPoint) !=0)
                    leftLine = new Line(BendNodeList[1], leftPoint);
                else leftLine = baseLine.GetPerpendicularLine(BendNodeList[1]);
                int i = 2;
                while (i < BendNodeList.Count)
                {
                    if (leftLine.GetSign(BendNodeList[0]) == leftLine.GetSign(BendNodeList[i]))
                    {
                        i++;
                    }
                    else
                    {
                        leftPoint = baseLine.GetPerpendicularFoundationPoint(BendNodeList[i]);
                        if (baseLine.GetSign(leftPoint) != 0)
                            leftLine = new Line(BendNodeList[i], leftPoint);
                        else leftLine = baseLine.GetPerpendicularLine(BendNodeList[i]);
                        i++;
                    }
                }
            }
            // то же самое справа
            var endIndex = BendNodeList.Count - 1;
            
            var max = leftPoint.DistanceToVertex(BendNodeList[endIndex]);
            int j = endIndex - 1;
            while (j > 0)
            {

                var p = baseLine.GetPerpendicularFoundationPoint(BendNodeList[j]);
                var d = leftPoint.DistanceToVertex(p);
                if (d > max)
                    max = d;
                j--;
            }
            return Math.Round(max);

            
        }

        /// <summary>
        /// вычисление квадрата евклидова расстояния между изгибами, где
        /// нормализованная площадь по оси x
        /// нормализованная высота основания по оси y
        /// нормализованный иднекс компактности по оси z
        /// </summary>
        /// <param name="otherBend">изгиб, расстояние до которого вычисляем</param>
        /// <returns>квадрат расстояния между изгибами</returns>
        public double SquareDistanceToBend(Bend otherBend)
        {
            var size1 = Area();
            var size2 = otherBend.Area();
            var base1 = BaseLineLength();
            var base2 = otherBend.BaseLineLength();
            var cmp1 = CompactIndex();
            var cmp2 = otherBend.CompactIndex();

            return Math.Pow((size2 - size1) / (size2 + size1), 2) +
                Math.Pow((cmp1 - cmp2) / (cmp1 + cmp2), 2) +
                Math.Pow((base2 - base1) / (base2 + base1), 2);
        }

        /// <summary>
        /// раздувание изгиба
        /// при помощи радиального расширения
        /// </summary>
        /// <param name="factor">множитель раздувания</param>
        public void ExaggerateRadial(double factor)
        {
            var c = BendMiddlePoint();
            for (var i = 1; i < BendNodeList.Count - 1; i++)
            {
                BendNodeList[i].X = (1 - factor) * c[0] + factor * BendNodeList[i].X;
                BendNodeList[i].Y = (1 - factor) * c[1] + factor * BendNodeList[i].Y;
            }
        }

        ///// <summary>
        ///// поиск самопересечений в изгибе с левой стороны
        ///// </summary>
        ///// <param name="ch">цепочка, из которой удаляется изгиб</param>
        ///// <param name="firstIndex">номер первой точки изгиба в цепи</param>
        ///// <param name="lastIndex">длина изгиба в точках</param>
        ///// <param name="depth">глубина проверки самопересечений</param>
        //public static void eliminate(Chain ch, int firstIndex, int bendLength, int depth)
        //{
        //    for (var i = 0; i < bendLength - 2; i++)
        //    {
        //        ch.vert.RemoveAt(firstIndex + 1);
        //    }

        //    //проверка самопересечений
        //    var noSelfIntersections = false;
        //    Point v1;
        //    Point v2;
        //    Point w1;
        //    Point w2;

        //    while (!noSelfIntersections)
        //    {
        //        v1 = ch.vert[firstIndex];
        //        v2 = ch.vert[firstIndex + 1];
        //        noSelfIntersections = false;

        //        //TODO: Добавить Bounding Box
        //        if (firstIndex > 2)
        //        {
        //            w1 = ch.vert[firstIndex - 1];
        //            w2 = ch.vert[firstIndex - 2];
        //            if ((Math.Sign((v2.X - v1.X) * (w2.Y - v1.Y) - (v2.Y - v1.Y) * (w2.X - v1.X)) * Math.Sign((v2.X - v1.X) * (w1.Y - v1.Y) - (v2.Y - v1.Y) * (w1.X - v1.X)) < 0) &&
        //                (Math.Sign((w2.X - w1.X) * (v2.Y - w1.Y) - (w2.Y - w1.Y) * (v2.X - w1.X)) * Math.Sign((w2.X - w1.X) * (v1.Y - w1.Y) - (w2.Y - w1.Y) * (v1.X - w1.X)) < 0))
        //            {
        //                //самопересечение найдено
        //                ch.vert.RemoveAt(firstIndex - 1);
        //                firstIndex--;
        //                v1 = ch.vert[firstIndex];
        //            }
        //            else
        //            {
        //                noSelfIntersections = true;
        //            }
        //        }
        //        else
        //        {
        //            noSelfIntersections = true;
        //        }
        //    }
        //}

        /// <summary>
        /// сливает два последовательных похожих изгиба
        /// </summary> 
        /// <param name="nextBend">смежный изгиб</param>
        public void CombineWith(Bend nextBend)
        {
            double factor = 1.2;
                MapPoint baseVertex = nextBend.BendNodeList[0];
            int peakIndex = PeakIndex();
                MapPoint thisPeak = BendNodeList[peakIndex];
            int nextPeakInd = nextBend.PeakIndex();
                MapPoint nextPeak = nextBend.BendNodeList[nextPeakInd];
                var centerX = (thisPeak.X + nextPeak.X) / 2;
                var centerY = (thisPeak.Y + nextPeak.Y) / 2;

                var x = (1 - factor) * baseVertex.X + factor * centerX;
                var y = (1 - factor) * baseVertex.Y + factor * centerY;

                baseVertex.X = x;
                baseVertex.Y = y;
                BendNodeList.RemoveRange(peakIndex+1, BendNodeList.Count - peakIndex-1 );
                BendNodeList.Add(baseVertex);
               
            for (int i = nextPeakInd; i < nextBend.BendNodeList.Count; i++)
            {
                BendNodeList.Add(nextBend.BendNodeList[i]);
            }
        }

        /// <summary>
        /// сливает два последовательных похожих изгиба
        /// </summary> 
        /// <param name="nextNextBend">смежный изгиб</param>
        public void CombineWith2(Bend nextNextBend)
        {
            double factor = 1.2;
            MapPoint baseVertex = nextNextBend.BendNodeList[0];
            int peakIndex = PeakIndex();
            MapPoint thisPeak = BendNodeList[peakIndex];
            int nextPeakInd = nextNextBend.PeakIndex();
            MapPoint nextPeak = nextNextBend.BendNodeList[nextPeakInd];
            var centerX = (thisPeak.X + nextPeak.X) / 2;
            var centerY = (thisPeak.Y + nextPeak.Y) / 2;

            var x = (1 - factor) * baseVertex.X + factor * centerX;
            var y = (1 - factor) * baseVertex.Y + factor * centerY;

            baseVertex.X = x;
            baseVertex.Y = y;
            BendNodeList.RemoveRange(peakIndex + 1, BendNodeList.Count - peakIndex - 1);
            BendNodeList.Add(baseVertex);

            for (int i = nextPeakInd; i < nextNextBend.BendNodeList.Count; i++)
            {
                BendNodeList.Add(nextNextBend.BendNodeList[i]);
            }
        }

    }
    }

