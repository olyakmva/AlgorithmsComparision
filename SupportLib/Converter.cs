using System.Collections.Generic;
using AlgorithmsLibrary;
using DotSpatial.Data;
using DotSpatial.Topology;
using MapPoint = AlgorithmsLibrary.MapPoint;

namespace SupportMapLibrary
{
    public static class Converter
    {
        public static MapData ToMapData(IFeatureSet fSet)
        {
            var map = new MapData();
            var list = fSet.Features;
            
            foreach (var item in list)
            {
                var xy = item.Coordinates;
                var points = new List<MapPoint>();
                
                foreach (var t in xy)
                {
                    var p = new MapPoint(t.X , t.Y, item.Fid, 1.0);
                    points.Add(p);
                }
                map.VertexList .Add(points);
            }
            return map;
        }

        public static IFeatureSet ToShape(MapData map)
        {
            Feature f = new Feature();
            FeatureSet fs = new FeatureSet(f.FeatureType);
            foreach (var list in map.VertexList)
            {
                Coordinate[] coord = new Coordinate[list.Count];
                for (int i = 0; i < list.Count; i++)
                {
                    coord[i] = new Coordinate(list[i].X, list[i].Y);
                }
                f = new Feature(FeatureType.Line, coord);
                fs.Features.Add(f);

            }
            return fs;
        }

    }
}
