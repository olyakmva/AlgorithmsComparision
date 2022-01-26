using System;
namespace AlgorithmsLibrary
{
    public class Triangle
    {
        public MapPoint A, B, C;

        public Triangle(MapPoint d, MapPoint e, MapPoint f)
        {
            A = d;
            B = e;
            C = f;
        }

        public double Square()
        {
            double a = A.DistanceToVertex(B);
            double b = B.DistanceToVertex(C);
            double c = C.DistanceToVertex(A);
            double p = (a + b + c) / 2;
            const double epsilon = 0.001;
            if (Math.Abs(p - a) < epsilon || Math.Abs(p - b) < epsilon || Math.Abs(p - c) < epsilon)
            {
                return 0;
            }
            double s = Math.Sqrt(p * (p - a) * (p - b) * (p - c));
            return s;
        }

        public double GetBaseLine()
        {
            return A.DistanceToVertex(C);
        }
        public double GetLength()
        {
            return A.DistanceToVertex(B) + B.DistanceToVertex(C);
        }
        public double GetHeight()
        {
            Line ac = new Line(A, C);
            return ac.GetDistance(B);
        }

        public double GetWidth()
        {
            var baseLine = new Line(A,C);
            var leftPoint = baseLine.GetPerpendicularFoundationPoint(B);
            var leftLine = new Line(B, leftPoint);
            if (leftLine.GetSign(A) != leftLine.GetSign(C))
            {
                return A.DistanceToVertex(C);
            }
            return Math.Max(A.DistanceToVertex(leftPoint), C.DistanceToVertex(leftPoint));
        }

    }
}
