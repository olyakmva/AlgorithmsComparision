using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsLibrary.FourierDescAlgm
{
    public class ShapeAnalysis
    {
        private MapData m_polygon = null;
        public List<double> lengthArray = null;
        public List<double> lenAcculateArray = null;
        public List<double> angleArray = null;
        public List<LineSegment> LineSegmentArray = null;
        public double ZERO_FLOAT = +1.401298E-45;
        public double m_jifeng = 0.0;

        public ShapeAnalysis(MapData polygon)
        {
            m_polygon = polygon;
        }
        public void GetPerSegmentDist(int pos)
        {
            object missing = Type.Missing;
            lengthArray = new List<double>();
            lenAcculateArray = new List<double>();
            angleArray = new List<double>();

            //проверить корректность периметра
            double perimeter = m_polygon.GetLength();
            List<MapPoint> pointCollection1 = m_polygon.GetAllVertices();
            List<MapPoint> pointCollection2 = new List<MapPoint>();
            List<MapPoint> pointCollection = new List<MapPoint>();

            // как посчитать площадь полигона
            double dArea = m_polygon.Area;
            int N = pointCollection1.Count;
            if (dArea > 0)
            {

                for (int i = N - 1; i > 0; i--)
                {
                    pointCollection2.Add(pointCollection1[i]);
                }
            }
            else
            {
                for (int i = 0; i < N - 1; i++)
                {
                    pointCollection2.Add(pointCollection1[i]);
                }
            }

            for (int i = pos; i < N - 1; i++)
            {
                pointCollection.Add(pointCollection2[i]);
            }

            for (int i = 0; i <= pos; i++)
            {
                pointCollection.Add(pointCollection2[i]);
            }

            int num = pointCollection.Count;
            double currentAccu = 0.0;
            double ang = 0.0000000;
            double lastEdgeAng = 0.000000;

            m_jifeng = 0.000;

            for (int i = 0; i < num - 1; i++)
            {
                MapPoint pt1 = pointCollection[i];
                MapPoint pt2 = pointCollection[i + 1];
                Line Line = new Line(pt1, pt2);
                double dSegmentLen = pt1.DistanceToVertex(pt2);
                double edge_ratio = dSegmentLen / perimeter;
                edge_ratio = Math.Round(edge_ratio, 10, MidpointRounding.AwayFromZero);
                currentAccu = currentAccu + edge_ratio;
                lengthArray.Add(edge_ratio);
                lenAcculateArray.Add(currentAccu);
                double a1 = Line.GetAngle();
                if (a1 < 0) a1 = a1 + 2.0 * Math.PI;
                double angleTest1 = a1 * 180.0 / Math.PI;

                double jiajiao = a1 - lastEdgeAng;
                double currentAng = 0.0;
                if (i == 0)
                {
                    currentAng = jiajiao;
                }
                else
                {
                    if (jiajiao < 0) jiajiao = 2.0 * Math.PI + jiajiao;
                    double angleTest2 = a1 * 180.0 / Math.PI;
                    if (jiajiao > Math.PI) jiajiao = -1.0 * (2 * Math.PI - jiajiao);
                    currentAng = ang + jiajiao;
                    double angleTest3 = currentAng * 180.0 / Math.PI;
                }

                lastEdgeAng = a1;
                angleArray.Add(currentAng);
                m_jifeng = m_jifeng + currentAng * edge_ratio;
                ang = currentAng;

            }

            //проверка
            MapPoint pt3 = pointCollection[0];
            MapPoint pt4 = pointCollection[1];
            Line Line1 = new Line(pt3, pt4);
            double a2 = Line1.GetAngle();
            if (a2 < 0) a2 = a2 + 2.0 * Math.PI;
            double jiajiao1 = a2 - lastEdgeAng;
            if (jiajiao1 < 0) jiajiao1 = 2.0 * Math.PI + jiajiao1;
            if (jiajiao1 > Math.PI) jiajiao1 = -1.0 * (2 * Math.PI - jiajiao1);
            double endAng = ang + jiajiao1;
            double testAng = angleArray[0] + 2.0 * Math.PI;
            // double angleTest3 = currentAng * 180.0 / Math.PI;
            int num22 = lenAcculateArray.Count;
            lenAcculateArray[num22 - 1] = 1.0;
        }
        //вычисление азимута (в радианах)                  
        double Bearing(double deltaX, double deltaY)
        {
            double angle;

            double dist = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
            if (dist <= ZERO_FLOAT)
            {
                angle = -10.0 * Math.PI;
                return angle;
            }

            if (Math.Abs(deltaX) > Math.Abs(deltaY))
            {
                angle = Math.Asin(deltaY / dist);    //(-PI/2 ~ +PI/2)
                if (deltaX < 0) angle = Math.PI - angle;
            }
            else
            {
                if (Math.Abs(deltaX) < ZERO_FLOAT) angle = Math.PI / 2.0;
                else angle = Math.Acos(deltaX / dist);    //(0 ~ 2*PI)
                if (deltaY < 0) angle = 2.0 * Math.PI - angle;
            }

            if (angle < 0) angle += 2.0 * Math.PI;
            return angle;
        }

        // вычисление азимута между двух вершин
        double Bearing(MapPoint first, MapPoint second)
        {
            return Bearing(second.X - first.X, second.Y - first.Y);
        }

        // приведение угла радиан с точностью до pi/2
        double AdjustAngle(double angle)
        {
            double dRadian = angle;
            while (dRadian < 0)
            {
                dRadian += Math.PI * 2.0;
            }

            if (Math.Abs(dRadian) < 0.00001)
            {
                dRadian = 0.0;
                return dRadian;
            }

            while (dRadian > Math.PI * 2.0)
            {
                dRadian -= Math.PI * 2.0;
            }

            while (Math.Abs(dRadian - Math.PI * 2.0) < 0.00001)
            {
                dRadian = 0.0;
            }

            return dRadian;
        }

        public void GetSegmentArray()
        {
            LineSegmentArray = new List<LineSegment>();
            int num = lenAcculateArray.Count;
            for (int i = 0; i < num; i++)
            {
                LineSegment lineSeg = new LineSegment()
                {
                    Id = i,
                    StartPos = i == 0 ? 0.0 : lenAcculateArray[i - 1],
                    EndPos = lenAcculateArray[i],
                    Orient = angleArray[i]
                };

                LineSegmentArray.Add(lineSeg);
            }

        }
        public double AreaOfAngle_and_len
        {
            get
            {
                return m_jifeng;
            }
        }
    }
}
