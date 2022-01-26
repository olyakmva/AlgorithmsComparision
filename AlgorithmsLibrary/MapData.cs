using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace AlgorithmsLibrary
{
    // точка на карте. Имеет координаты, 
    //  идентификатор - к какому объекту принадлежит 
    [Serializable]
    public class MapPoint : IComparable<MapPoint>
    {
        public int Id { get; private set; }
        public double X { set; get; }
        public double Y { set; get; }
        public double Weight { get; set; }

        public MapPoint()
        {
            X = 0;
            Y = 0;
            Weight = 1;
        }

        public MapPoint(double coordX, double coordY, int id, double w)
        {
            X = coordX;
            Y = coordY;
            Id = id;
            Weight = w;
        }
        
        public double DistanceToVertex(MapPoint v)
        {
            return Math.Sqrt(Math.Pow(X - v.X, 2) + Math.Pow(Y - v.Y, 2));
        }

        public override string ToString()
        {
            return $"x={X}   y={Y}   id={Id} ";
        }
        public override bool Equals(object obj)
        {
            var other = obj as MapPoint;
            if (other == null) return false;
            return Math.Abs(other.X - X) < double.Epsilon && Math.Abs(other.Y - Y) < double.Epsilon;
        }
       
        public override int GetHashCode()
        {
            return (int)(X * 1000000 + Y);
        }

        #region IComparable Members

        public int CompareTo(MapPoint other)
        {
            if (Math.Abs(other.X - X) < double.Epsilon && Math.Abs(other.Y - Y) < double.Epsilon)
            {
                return 0;
            }
            if (Math.Abs(other.X - X) < double.Epsilon)
            {
                if (other.Y < Y)
                    return 1;
                return -1;
            }
            if (other.X < X)
            {
                return 1;
            }
            return -1;
        }
        #endregion
    }
    
    // Данные карты. В списке хранятся точки на карте  
    [Serializable]
    public class MapData
    {
       public List< List<MapPoint>> VertexList { get; }

        public int Count => GetAllVertices().Count;

        public MapData()
        {
            VertexList = new List<List<MapPoint>>();
        }


        public List<MapPoint> GetAllVertices()
        {
            var resultList = new List<MapPoint>();
            foreach (var lst in VertexList)
            {
                resultList.AddRange(lst);
            }
            return resultList;
        }

        
        public MapData Clone()
        {
            MapData clone ;
            var bf = new BinaryFormatter();
            using (Stream fs = new FileStream("temp.bin", FileMode.Create, FileAccess.Write, FileShare.None))
            {
                bf.Serialize(fs, this);
            }
            using (Stream fs = new FileStream("temp.bin", FileMode.Open , FileAccess.Read , FileShare.None))
            {
                clone = (MapData) bf.Deserialize(fs);
            }
            return clone;
        }


        public void ClearWeights()
        {
            foreach (var chain in VertexList)
            {
                foreach (var vertex in chain)
                {
                    vertex.Weight = 1;
                }
            }
        }

    }

   

    public static class ErrorLog
    {
        public static void WriteToLogFile(string msg)
        {
            string fileName = "log.txt";
            FileStream f = new FileStream(fileName ,FileMode.Append,FileAccess.Write);
            using (var sr = new StreamWriter(f))
            {
                sr.WriteLine("{0}", msg);
            }
        }
    }
    public static class MinMaxValues
    {
        public static void Compute(List<MapPoint> vList, out double xmin, out double ymin, out double xmax, out double ymax)
        {
            xmin = vList[0].X;
            xmax = xmin;
            ymin = vList[0].Y;
            ymax = ymin;
            foreach (var v in vList)
            {
                if (v.X < xmin)
                    xmin = v.X;
                if (v.X > xmax)
                    xmax = v.X;
                if (v.Y < ymin)
                    ymin = v.Y;
                if (v.Y > ymax)
                    ymax = v.Y;
            }
        }
    }
    [Serializable]
    public class LineCoefEqualsZeroException : Exception
    {
        public LineCoefEqualsZeroException(string message) : base(message)
        { }

        protected LineCoefEqualsZeroException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        { }
    }

    public class Line
    {
        public double A { get; set; }
        public double B { get; set; }
        public double C { get; set; }

        public Line(MapPoint v1, MapPoint v2)
        {
            A = v2.Y - v1.Y;
            B = (v1.X - v2.X);
            C = v1.Y * (-1) * B - v1.X * A;
            if (Math.Abs(A) < double.Epsilon && Math.Abs(B) < double.Epsilon && Math.Abs(C) < double.Epsilon)
            {
                throw new LineCoefEqualsZeroException(" коэффициенты уравнения прямой= нулю");
            }
        }

        public Line()
        { }

        public override string ToString()
        {
            return string.Format("{0:f2}x + {1:f2}y + {2:f2} =0", A, B, C);
        }

        public double GetDistance(MapPoint v)
        {
            return Math.Abs(A * v.X + B * v.Y + C) / Math.Sqrt(A * A + B * B);
        }

        public double GetAngle(Line otherLine)
        {

            const double tolerance = 0.001;

            if ((Math.Abs(A) < tolerance && Math.Abs(otherLine.A) < tolerance) ||
                (Math.Abs(B) < tolerance && Math.Abs(otherLine.B) < tolerance))
            {
                return 0;
            }
            if (Math.Abs(A / B - otherLine.A / otherLine.B) < tolerance)
            {
                return 0;
            }

            double scalar = A * otherLine.A + B * otherLine.B;
            if (Math.Abs(scalar) < tolerance)
            {
                return 90;
            }
            double tangens = (A * otherLine.B - otherLine.A * B) / scalar;

            return (Math.Atan(tangens) * 180) / Math.PI;
        }

        public double GetRadiansAngle(Line otherLine)
        {

            const double tolerance = 0.001;
            var tangens = GetAngleTangens(otherLine);
            if (Math.Abs(tangens - 1000) < tolerance)
                return 90;
            if (Math.Abs(tangens) < tolerance)
                return 0;
            return (Math.Atan(tangens));
        }

        public double GetGradusAngle(Line otherLine)
        {
            var tangens = GetAngleTangens(otherLine);
            const double tolerance = 0.001;
            if (Math.Abs(tangens - 1000) < tolerance)
                return 90;
            if (Math.Abs(tangens) < tolerance)
                return 0;
            return (Math.Atan(tangens) * 180) / Math.PI;
        }

        public double GetAngleTangens(Line otherLine)
        {

            const double tolerance = 0.001;

            if ((Math.Abs(A) < tolerance && Math.Abs(otherLine.A) < tolerance) ||
                (Math.Abs(B) < tolerance && Math.Abs(otherLine.B) < tolerance))
            {
                return 0;
            }
            if (Math.Abs(A / B - otherLine.A / otherLine.B) < tolerance)
            { // прямые параллельны - угол = 180  тангенс равен нулю
                return 0;
            }

            double scalar = A * otherLine.A + B * otherLine.B;
            if (Math.Abs(scalar) < tolerance)
            {
                return 1000; // бесконечность
            }
            double tangens = (A * otherLine.B - otherLine.A * B) / scalar;

            return tangens;
        }

        public MapPoint GetIntersectionPoint(Line otherLine)
        {
            var delta = A * otherLine.B - B * otherLine.A;
            if (Math.Abs(delta) < double.Epsilon)
                return null;
            var delta1 = (-1 * C * otherLine.B + B * otherLine.C);
            var delta2 = A * otherLine.C * (-1) + otherLine.A * C;
            return new MapPoint { X = delta1 / delta, Y = delta2 / delta };
        }

        public MapPoint GetPerpendicularFoundationPoint(MapPoint initVertex)
        {
            var result = new MapPoint();
            var delta = B * B + A * A;
            var delta1 = (B * initVertex.X - A * initVertex.Y) * B - C * A;
            var delta2 = B * C * (-1) - (B * initVertex.X - A * initVertex.Y) * A;
            result.X = delta1 / delta;
            result.Y = delta2 / delta;
            return result;
        }
        public int GetSign(MapPoint v)
        {
            double result = A * v.X + B * v.Y + C;
            const double tolerance = 0.001;
            if (Math.Abs(result) < tolerance)
                return 0;
            if (result > 0)
                return 1;
            return -1;
        }

        public double GetY(double x)
        {
            if (Math.Abs(B) > double.Epsilon)
                return (A * x + C) / (-1 * B);
            return 0;
        }
        public double GetX(double y)
        {
            if (Math.Abs(A) > double.Epsilon)
                return (B * y + C) / (-1 * A);
            return 0;
        }
        /// <summary>
        /// возвращает прямую перепендикулярную данной и проходящую через заданную точку 
        /// </summary>
        /// <param name="point"> точка</param>
        /// <returns></returns>
        public Line GetPerpendicularLine(MapPoint point)
        {
            var line = new Line();
            line.A = B;
            line.B = -1 * A;
            line.C = point.Y * A - point.X * B;
            return line;
        }
    }

}
