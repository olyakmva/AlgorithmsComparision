using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsLibrary
{
    public class MapPolygon
    {
        private readonly MapData Map;
        private readonly List<MapPoint> Vertices;
        public MapPolygon(MapData Map)
        {
            this.Map = Map;
            this.Vertices = Map.GetAllVertices();
        }

        /// <summary>
        /// Вычисление площади полигона
        /// </summary>
        /// <returns>Площадь полигона</returns>
        public double GetArea()
        {
            double Area = 0;
            int VerticesCount = Vertices.Count;

            for (int i = 1; i < Vertices.Count + 1; i++)
            {
                Area += Vertices[i % VerticesCount].X * (Vertices[(i + 1) % VerticesCount].Y - Vertices[(i - 1) % VerticesCount].Y);
            }

            return Area / 2;
        }

        /// <summary>
        /// Вычисление периметра полигона
        /// </summary>
        /// <returns>Периметр полигона</returns>
        public double GetLength()
        {
            return Map.GetLength();
        }

        /// <summary>
        /// Возвращает все вершины полигона
        /// </summary>
        /// <returns>Список с вершинами</returns>
        public List<MapPoint> GetAllVertices()
        {
            return Vertices;
        }
    }
}
