using System;
using System.Collections.Generic;
using AlgorithmsLibrary;
using DotSpatial.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SupportMapLibrary;

namespace UnitTestProject1
{
    [TestClass]
    public class DouglasPeuckerUnitTest
    {
        private DouglasPeuckerAlgm algm;
        
        [TestMethod]
        public void MapWithClosePoints()
        {
            algm = new DouglasPeuckerAlgm();
            var list1 = new List<MapPoint>() {
                new MapPoint(6, 2, 3, 1), new MapPoint(8, 3, 4, 1),new MapPoint(9, 3, 5, 1),
                new MapPoint(10, 4, 2, 1),new MapPoint(12, 6, 2, 1),new MapPoint(10, 5, 2, 1),
                new MapPoint(9, 4, 2, 1)
            };
            var map = new MapData();
            map.VertexList.Add(list1);
            var options = new SimplificationAlgmParameters {Tolerance = 1.2};
            algm.Options = options;
            algm.Run(map);
            int expected = 3;
            Assert.AreEqual(expected, list1.Count);
        }

        
    }
}
