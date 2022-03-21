using AlgorithmsLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace UnitTestProject1
{
    [TestClass]
    public class FourierAlgmUnitTest
    {
        private FourierAlgmUnitTest algm;

        // Квадрат
        [TestMethod]
        public void MapSquarePolygonArea()
        {
            var list = new List<MapPoint>() { new MapPoint(1, 1, 1, 1), new MapPoint(5, 1, 2, 1), new MapPoint(5, 6, 3, 1), new MapPoint(1, 6, 4, 1) };
            MapPolygon mp = new MapPolygon(list);

            double expected = 20;
            double actual = mp.GetArea();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MapTrianglePolygonArea()
        {
            var list = new List<MapPoint>() { new MapPoint(0, 0, 1, 1), new MapPoint(3, 0, 2, 1), new MapPoint(0, 6, 3, 1) };
            MapPolygon mp = new MapPolygon(list);

            double expected = 9;
            double actual = mp.GetArea();

            Assert.AreEqual(expected, actual);
        }
    }
}
