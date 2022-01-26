using System;
using System.Collections.Generic;
using AlgorithmsLibrary;
using AlgorithmsLibrary.Features;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class BendTesting
    {
        [TestMethod]
        public void ProperBendSquare()
        {
            var list = new List<MapPoint>() {new MapPoint(), new MapPoint(2, 2, 2, 1), new MapPoint(4, 0, 3, 1)};
            Bend bend = new Bend(list);
            double expected = 4;
            Assert.AreEqual(expected,bend.Area());
            list = new List<MapPoint>() { new MapPoint(), new MapPoint(0, 2, 2, 1), new MapPoint(2, 2, 3, 1) };
            bend = new Bend(list);
            expected = 2;
            Assert.AreEqual(expected, bend.Area());
        }
        [TestMethod]
        public void ProperBendPerimetr()
        {
            var list = new List<MapPoint>() { new MapPoint(), new MapPoint(2, 2, 2, 1), new MapPoint(4, 0, 3, 1) };
            Bend bend = new Bend(list);
            double expected = 4*Math.Sqrt(2)+4;
            Assert.AreEqual(expected, bend.Perimeter());
            list = new List<MapPoint>() { new MapPoint(), new MapPoint(0, 2, 2, 1), new MapPoint(2, 2, 3, 1) };
            bend = new Bend(list);
            expected = 4+2*Math.Sqrt(2);
            Assert.AreEqual(expected, bend.Perimeter());
        }
        //[TestMethod]
        //public void ProperBendOrientation()
        //{
        //    var list = new List<MapPoint>() { new MapPoint(), new MapPoint(2, 2, 2, 1),
        //        new MapPoint(4, 0, 3, 1), new MapPoint(6, 0, 4, 1),new MapPoint(6, -2, 5, 1),
        //        new MapPoint(8, -2, 2, 1),new MapPoint(9, 0, 2, 1),new MapPoint(10, 2, 2, 1),
        //        new MapPoint(11, 1, 2, 1),new MapPoint(12, 3, 2, 1),new MapPoint(13, 0, 2, 1),
        //        new MapPoint(14, 1, 2, 1),new MapPoint(15, 1, 2, 1),new MapPoint(16, 2, 2, 1)};
        //    Bend bend = new Bend(list);
        //    double expected = 4;
        //    Assert.AreEqual(expected, bend.Area());
        //    list = new List<MapPoint>() { new MapPoint(), new MapPoint(0, 2, 2, 1), new MapPoint(2, 2, 3, 1) };
        //    bend = new Bend(list);
        //    expected = 2;
        //    Assert.AreEqual(expected, bend.Area());
        //}
        [TestMethod]
        public void ProperPeakIndex()
        {
            var list1 = new List<MapPoint>() {
                new MapPoint(2, 0, 3, 1), new MapPoint(3, 1, 4, 1),new MapPoint(4, 3, 5, 1),
                new MapPoint(5, 5, 2, 1),new MapPoint(6, 6, 2, 1),new MapPoint(7, 5, 2, 1),
                new MapPoint(9, 4, 2, 1),new MapPoint(10, 2, 2, 1),new MapPoint(11, 1, 2, 1),
                new MapPoint(12, 0, 2, 1)
            };
            Bend b1 = new Bend(list1);
            int expectedPeakIndex = 4;
            Assert .AreEqual(expectedPeakIndex,b1.PeakIndex());
            var list2 = new List<MapPoint>() {
                new MapPoint(13, 0, 2, 1),new MapPoint(14, 2, 2, 1),
                new MapPoint(16, 4, 2, 1),new MapPoint(18, 6, 2, 1),new MapPoint(19, 5, 2, 1),
                new MapPoint(20, 4, 2, 1),new MapPoint(21, 2, 2, 1),new MapPoint(20, 0, 2, 1)
            };
            Bend b2 = new Bend(list2);
            expectedPeakIndex = 3;
            Assert.AreEqual(expectedPeakIndex, b2.PeakIndex());
        }

        [TestMethod]
        public void TwoSimilarBendsAreCombined()
        {
            var list1 = new List<MapPoint>() { 
                new MapPoint(2, 0, 3, 1), new MapPoint(3, 1, 4, 1),new MapPoint(4, 3, 5, 1),
                new MapPoint(5, 5, 2, 1),new MapPoint(6, 6, 2, 1),new MapPoint(7, 5, 2, 1),
                new MapPoint(9, 4, 2, 1),new MapPoint(10, 2, 2, 1),new MapPoint(11, 1, 2, 1),
                new MapPoint(12, 0, 2, 1)
            };
            var list2 =  new List<MapPoint>() {
                new MapPoint(12, 0, 2, 1),new MapPoint(14, 2, 2, 1),
                new MapPoint(16, 4, 2, 1),new MapPoint(18, 6, 2, 1),new MapPoint(19, 5, 2, 1),
                new MapPoint(20, 4, 2, 1),new MapPoint(21, 2, 2, 1),new MapPoint(20, 0, 2, 1)
            };
            Bend b1 = new Bend(list1);
            int peakB1 = b1.PeakIndex();
            Bend b2 = new Bend(list2);
            int peakB2 = b2.PeakIndex();
            b1.CombineWith(b2);
            Assert.AreEqual(3, peakB2);
            for (int i = 0; i <= peakB1; i++)
            {
                Assert.AreEqual(true, b1.BendNodeList.Contains(list1[i]));
            }
            for (int i = peakB1 + 1; i < list1.Count; i++)
            {
                Assert.AreEqual(false, b1.BendNodeList.Contains(list1[i]));
            }
           for (int i = peakB2; i < list2.Count; i++)
           {
               Assert.AreEqual(true, b1.BendNodeList.Contains(list2[i]));
           }
            int exceptedPointCount = 11;
            Assert.AreEqual(exceptedPointCount, b1.BendNodeList.Count);
        }
        [TestMethod]
        public void ProperBendExaggregation()
        {
            var list1 = new List<MapPoint>() {
                new MapPoint(2, 0, 3, 1), new MapPoint(3, 1, 4, 1),new MapPoint(4, 3, 5, 1),
                new MapPoint(5, 5, 2, 1),new MapPoint(6, 6, 2, 1),new MapPoint(7, 5, 2, 1),
                new MapPoint(9, 4, 2, 1),new MapPoint(10, 2, 2, 1),new MapPoint(11, 1, 2, 1),
                new MapPoint(12, 0, 2, 1)
            };
            Bend b1 = new Bend(list1);
            double factor = 1.2;
            b1.ExaggerateRadial(factor);
            int expectedPeakIndex = 4;
            Assert.AreEqual(expectedPeakIndex, b1.PeakIndex());
            var list2 = new List<MapPoint>() {
                new MapPoint(13, 0, 2, 1),new MapPoint(14, 2, 2, 1),
                new MapPoint(16, 4, 2, 1),new MapPoint(18, 6, 2, 1),new MapPoint(19, 5, 2, 1),
                new MapPoint(20, 4, 2, 1),new MapPoint(21, 2, 2, 1),new MapPoint(20, 0, 2, 1)
            };
            Bend b2 = new Bend(list2);
            expectedPeakIndex = 3;
            Assert.AreEqual(expectedPeakIndex, b2.PeakIndex());
        }

        [TestMethod]
        public void ProperBendWidth()
        {
            MapPoint start = new MapPoint(0, 0, 2, 1);
            var list2 = new List<MapPoint>() {
                new MapPoint(0, 0, 2, 1),new MapPoint(4, 4, 2, 1),
                new MapPoint(8, 1, 2, 1)};
           Bend triangleBend = new Bend(list2);
            int expected = (int) Math.Round(start.DistanceToVertex(new MapPoint(8, 1, 2, 1)));
            int actual = (int) Math.Round(triangleBend.GetWidth());
            Assert.AreEqual(expected,actual);

            var list3 = new List<MapPoint>() {
                new MapPoint(4, 0, 2, 1),new MapPoint(2, 1, 2, 1), new MapPoint(0,2,2,1),
                new MapPoint(1, 4, 2, 1),new MapPoint(5, 6, 2, 1),new MapPoint(7, 3, 2, 1),
                new MapPoint(8,1,2,1)
            };
            Bend bend = new Bend(list3);
            var left = new MapPoint(0, 2, 2, 1);
            var right = new MapPoint(8, 1, 2, 1);
            start = new MapPoint(4, 0, 2, 1);
            var baseLine = new Line(start, right);
            left = baseLine.GetPerpendicularFoundationPoint(left);
            expected = (int)Math.Round(left.DistanceToVertex(right));
            actual = (int)Math.Round(bend.GetWidth());
            Assert.AreEqual(expected, actual);

        }

        [TestMethod]
        public void RealDataTest()
        {
            var list = new List<MapPoint>() {
            new MapPoint(1969919.7278, 10714777.1624,2, 1),new MapPoint(1968798.7405, 10713766.8177, 2, 1),
            new MapPoint(1972229.6072, 10712410.657, 2, 1)};
            var lineLength = list[0].DistanceToVertex(list[1]) + list[1].DistanceToVertex(list[2]);
            var b = new Bend(list);
            var s = b.Area();
            var openClose = 2 * b.Length() / b.Perimeter() - 1;
            var p = b.GetHeight() / b.GetWidth();
            var detail = p * openClose / (p + 1) * (2 - b.Length() / lineLength) * s;
            Assert.AreEqual(detail>0, true);
        }
        [TestMethod]
        public void ProperBendWidthWithVariousBendForms()
        {
           //// треугольник наклоненный сильно влево
           // var list4 = new List<MapPoint>() {
           //     new MapPoint(4, 0, 2, 1),new MapPoint(2, 1, 2, 1),
           //     new MapPoint(0, 3, 2, 1), new MapPoint(7,0,2,1)};
           // Bend triangleBend = new Bend(list4);
           // int expected = 7;
           // int actual = (int)Math.Round(triangleBend.GetWidth());
           // Assert.AreEqual(expected, actual);
            // треугольник наклоненный сильно вправо
            var list2 = new List<MapPoint>() {
                new MapPoint(0, 0, 2, 1),new MapPoint(8, 6, 2, 1),
                new MapPoint(6, 3, 2, 1), new MapPoint(4,0,2,1)};
            Bend triangleBend2 = new Bend(list2);
            int expected = 8;
            int actual = (int)Math.Round(triangleBend2.GetWidth());
            Assert.AreEqual(expected, actual);
            // гриб
            var list3 = new List<MapPoint>() {
                new MapPoint(4, 0, 2, 1),new MapPoint(2, 2, 2, 1), new MapPoint(0,3,2,1),
                new MapPoint(6, 4, 2, 1),new MapPoint(8, 2, 2, 1),new MapPoint(5, 0, 2, 1)
            };
            Bend bend = new Bend(list3);
            expected = 8;
            actual = (int)Math.Round(bend.GetWidth());
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ProperBendExtraction()
        {
            var list = new List<MapPoint>() {
                new MapPoint(7, 12, 2, 1),new MapPoint(9, 10, 2, 1),
                new MapPoint(9.5, 7, 2, 1),new MapPoint(9, 5, 2, 1),
                new MapPoint(7, 3, 2, 1),new MapPoint(4, 3, 2, 1),
                new MapPoint(2, 3.5, 2, 1),new MapPoint(1.5, 4.5, 2, 1),
                new MapPoint(2, 5, 2, 1),new MapPoint(3, 5.5, 2, 1),
                new MapPoint(5, 5, 2, 1),new MapPoint(7, 5, 2, 1),
                new MapPoint(8, 5.5, 2, 1),new MapPoint(8, 6, 2, 1),
                new MapPoint(7, 7.5, 2, 1), new MapPoint(5,8,2,1)};
            int count = 0;
            int i = 0;
            while (i < list.Count-2)
            {
                Bend b;
                BendComputation.ExtractBend(ref i, list,out b);
                count++;
            }

            int expected = 2;
            int actual = count;
            Assert.AreEqual(expected, actual);
        }


    }
}
