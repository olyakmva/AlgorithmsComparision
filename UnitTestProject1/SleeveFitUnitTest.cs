using AlgorithmsLibrary;
using DotSpatial.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SupportMapLibrary;

namespace UnitTestProject1
{
    [TestClass]
    public class SleeveFitUnitTest
    {
        private SleeveFitWithCriterion algm;
        private MapData _map;
        
        [TestInitialize]
        public void TestInit()
        {
            string shapeFileName = @"..\..\Data\komi\KomiArchRayLinesClear.shp";
            _map = Converter.ToMapData(FeatureSet.Open(shapeFileName));
            SimplificationAlgmParameters options = new SimplificationAlgmParameters()
            {
                OutScale = 6000,
                Tolerance = 7500,
                RemainingPercent = 20
            };

            algm = new SleeveFitWithCriterion( new PointPercentCriterion());
            algm.Options = options;
        }

        [TestMethod]
        public void MapHasOnlyChain()
        {
            _map.VertexList.RemoveRange(1,_map.VertexList.Count-1);
            int expected = _map.VertexList.Count;
            algm.Run(_map);
            Assert.AreEqual(expected, _map.VertexList.Count);
        }

        [TestMethod]
        public void ChainHasFewPoints()
        {
            _map.VertexList.RemoveRange(1, _map.VertexList.Count - 1);
            int length = _map.VertexList[0].Count;
            _map.VertexList[0].RemoveRange(1, length -2);
            int expected = _map.VertexList.Count;
            algm.Run(_map);
            Assert.AreEqual(expected, _map.VertexList.Count);
        }

    }
}
