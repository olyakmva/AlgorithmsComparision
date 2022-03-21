using AlgorithmsLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        [TestMethod]
        public void LengthOnePiece_GetAllDistMethod()
        {
            var list = new List<MapPoint>() { new MapPoint(6, 0, 1, 1), new MapPoint(0, 0, 1, 1) };

            Fourier fourier = new Fourier(list, 0);

            double expected = 6;
            double actual = fourier.GetAllDist();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void LengthOfPolyline_GetAllDistMethod()
        {
            var list = new List<MapPoint>() { new MapPoint(0, 0, 1, 1), new MapPoint(1, 0, 1, 1),
            new MapPoint(1, 1, 1, 1), new MapPoint(0, 1, 1, 1), new MapPoint(0, 0, 1, 1)};

            Fourier fourier = new Fourier(list, 0);

            double expected = 4;
            double actual = fourier.GetAllDist();

            Assert.AreEqual(expected, actual);
        }
    }
}
