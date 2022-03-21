using System;
using System.Collections.Generic;

namespace AlgorithmsLibrary
{
    public class Fourier : FourierDescFatherClass
    {
        //Резервный набор точек.
        //private List<MapPoint> newPointCol = null;

        private double polyLineLength = 0.0;

        //private double[] Power = null;

        /// <summary>
        /// Конструктор дескриптора Фурье для замкнутой полилинии
        /// </summary>
        /// <param name="pointCollection">Коллекция точек, образующих полилинию</param>
        /// <param name="nTerm"></param>
        public Fourier(IEnumerable<MapPoint> pointCollection, long nTerm)
        {
            this.nTerm = nTerm;
            while (true)
            {
                bool distanceBetweenMapPointsNormal = true;
                arrayOfMapPoints = new List<MapPoint>(pointCollection);
                for (int i = 1; i < arrayOfMapPoints.Count; i++)
                {
                    MapPoint p1 = arrayOfMapPoints[i - 1];
                    MapPoint p2 = arrayOfMapPoints[i];

                    // Удаление точек слишком близких друг к другу
                    if (Math.Abs(p1.X - p2.X) < 0.1 && Math.Abs(p1.Y - p2.Y) < 0.1)
                    {
                        arrayOfMapPoints.RemoveAt(i--);
                        distanceBetweenMapPointsNormal = false;
                    }
                }

                if (distanceBetweenMapPointsNormal)
                {
                    countOfPointsObject = arrayOfMapPoints.Count;
                    break;
                }
            }
        }

        /// <summary>
        /// Производит рассчёт расстояний между точками полилинии.
        /// Задает общее расстояние полилинии
        /// Задает массив последовательного вычисления длины полилинии
        /// </summary>
        /// <returns>Длина полилинии</returns>
        public double GetAllDist()
        {
            arrayDistancesBetweenPoints = new double[countOfPointsObject - 1];
            sequentialCalculationPolylineLength = new double[countOfPointsObject];

            for (int i = 0; i < countOfPointsObject - 1; i++)
            {
                MapPoint p2 = arrayOfMapPoints[i + 1];
                MapPoint p1 = arrayOfMapPoints[i];

                arrayDistancesBetweenPoints[i] = p1.DistanceToVertex(p2);
            }

            double tmpDist = 0.0;
            sequentialCalculationPolylineLength[0] = 0.0;
            for (int i = 0; i < countOfPointsObject - 1; i++)
            {
                sequentialCalculationPolylineLength[i + 1] = tmpDist + arrayDistancesBetweenPoints[i];
                tmpDist = sequentialCalculationPolylineLength[i + 1];
            }

            polyLineLength = sequentialCalculationPolylineLength[countOfPointsObject - 1];
            return polyLineLength;
        }

        //public double[] ArrayPower()
        //{
        //    return Power;
        //}

        /// <summary>
        /// Выполняет рассчет всех параметров для дескриптора
        /// </summary>
        public void CalculateAllValue()
        {
            GetAllDist();
            GetFourierXparameter();
            GetFourierYparameter();
            CalculateShapeVector(true);
        }

        //public void GetAllDist()
        //{
        //    m_dis_betPoint = new double[m_PointNum - 1];
        //    m_accuDist = new double[m_PointNum];
        //    for (int i = 0; i < m_PointNum - 1; i++)
        //    {
        //        IPoint p2 = pointCol.get_Point(i + 1);
        //        IPoint p1 = pointCol.get_Point(i);
        //        double detx = p2.X - p1.X;
        //        double dety = p2.Y - p1.Y; ;
        //        double squareSum = detx * detx + dety * dety;
        //        m_dis_betPoint[i] = Math.Sqrt(squareSum);
        //    }
        //    double s = 0.0;
        //    m_accuDist[0] = 0.0;
        //    for (int i = 0; i < m_PointNum - 1; i++)
        //    {
        //        m_accuDist[i + 1] = s + m_dis_betPoint[i];
        //        s = m_accuDist[i + 1];
        //    }
        //    m_totalS = m_accuDist[m_PointNum - 1];
        //}

        

        public bool GetFourierXparameter()
        {
            long n = nTerm;
            if (n <= 0) return false;
            Ax = new double[n + 1];
            Bx = new double[n + 1];
            Bx[0] = 0.0;
            Ax[0] = 0.0;
            for (int i = 1; i < countOfPointsObject; i++)
            {
                MapPoint p1 = arrayOfMapPoints[i - 1];
                MapPoint p2 = arrayOfMapPoints[i];
                double delx = (double)(p2.X - p1.X);
                double dels = sequentialCalculationPolylineLength[i] - sequentialCalculationPolylineLength[i - 1];
                double dels2 = sequentialCalculationPolylineLength[i] * sequentialCalculationPolylineLength[i] - sequentialCalculationPolylineLength[i - 1] * sequentialCalculationPolylineLength[i - 1];
                double cc = -1.0 * delx * sequentialCalculationPolylineLength[i - 1] / dels;
                double aa = p1.X + cc;
                double bb = delx / dels;
                double v1 = aa * dels;
                double v2 = bb / 2 * dels2;
                Ax[0] = Ax[0] + v1 / polyLineLength + v2 / polyLineLength;
            }
            for (int k = 1; k < n + 1; k++)
            {
                Ax[k] = 0;
                double angle = 2 * Math.PI * k / polyLineLength;
                for (int i = 1; i < countOfPointsObject; i++)
                {
                    MapPoint p1 = arrayOfMapPoints[i - 1];
                    MapPoint p2 = arrayOfMapPoints[i];
                    double delx = p2.X - p1.X;
                    double dels = sequentialCalculationPolylineLength[i] - sequentialCalculationPolylineLength[i - 1];
                    double dels2 = sequentialCalculationPolylineLength[i] * sequentialCalculationPolylineLength[i] - sequentialCalculationPolylineLength[i - 1] * sequentialCalculationPolylineLength[i - 1];
                    double cc = -1.0 * delx * sequentialCalculationPolylineLength[i - 1] / dels;
                    double aa = p1.X + cc;
                    double bb = delx / dels;
                    double k1 = (double)k;
                    double aa1 = aa / Math.PI / k1;
                    double delsin = Math.Sin(2.0 * k1 * Math.PI * sequentialCalculationPolylineLength[i] / polyLineLength) - Math.Sin(2.0 * k1 * Math.PI * sequentialCalculationPolylineLength[i - 1] / polyLineLength);
                    double v1 = aa1 * delsin;
                    double bb1 = bb / Math.PI / k1;
                    double delsinS = Math.Sin(2.0 * k1 * Math.PI * sequentialCalculationPolylineLength[i] / polyLineLength) * sequentialCalculationPolylineLength[i] - Math.Sin(2.0 * k1 * Math.PI * sequentialCalculationPolylineLength[i - 1] / polyLineLength) * sequentialCalculationPolylineLength[i - 1];
                    double v2 = bb1 * delsinS;
                    double bb2 = bb1 * polyLineLength / 2.0 / Math.PI / k1;
                    double delcos = Math.Cos(2.0 * k1 * Math.PI * sequentialCalculationPolylineLength[i] / polyLineLength) - Math.Cos(2.0 * k1 * Math.PI * sequentialCalculationPolylineLength[i - 1] / polyLineLength);
                    double v3 = bb2 * delcos;
                    Ax[k] = Ax[k] + v1 + v2 + v3;
                }

                Bx[k] = 0;
                for (int i = 1; i < countOfPointsObject; i++)
                {
                    MapPoint p1 = arrayOfMapPoints[i - 1];
                    MapPoint p2 = arrayOfMapPoints[i];
                    double delx = p2.X - p1.X;
                    double dels = sequentialCalculationPolylineLength[i] - sequentialCalculationPolylineLength[i - 1];
                    double dels2 = sequentialCalculationPolylineLength[i] * sequentialCalculationPolylineLength[i] - sequentialCalculationPolylineLength[i - 1] * sequentialCalculationPolylineLength[i - 1];
                    double cc = -1.0 * delx * sequentialCalculationPolylineLength[i - 1] / dels;
                    double aa = p1.X + cc;
                    double bb = delx / dels;
                    double k1 = (double)k;
                    double aa1 = (-1.0) * aa / Math.PI / k1;
                    double delcos = Math.Cos(2.0 * k1 * Math.PI * sequentialCalculationPolylineLength[i] / polyLineLength) - Math.Cos(2.0 * k1 * Math.PI * sequentialCalculationPolylineLength[i - 1] / polyLineLength);
                    double v1 = aa1 * delcos;
                    double bb1 = (-1.0) * bb / Math.PI / k1;
                    double delcosS = Math.Cos(2.0 * k1 * Math.PI * sequentialCalculationPolylineLength[i] / polyLineLength) * sequentialCalculationPolylineLength[i] - Math.Cos(2.0 * k1 * Math.PI * sequentialCalculationPolylineLength[i - 1] / polyLineLength) * sequentialCalculationPolylineLength[i - 1];
                    double v2 = bb1 * delcosS;
                    double bb2 = (-1.0) * bb1 * polyLineLength / 2.0 / Math.PI / k1;
                    double delsin = Math.Sin(2.0 * k1 * Math.PI * sequentialCalculationPolylineLength[i] / polyLineLength) - Math.Sin(2.0 * k1 * Math.PI * sequentialCalculationPolylineLength[i - 1] / polyLineLength);
                    double v3 = bb2 * delsin;
                    Bx[k] = Bx[k] + v1 + v2 + v3;
                }
            }
            return true;

        }
        public bool GetFourierYparameter()
        {
            long n = nTerm;
            if (n <= 0) return false;
            Ay = new double[n + 1];
            By = new double[n + 1];
            By[0] = 0.0;
            Ay[0] = 0.0;
            for (int i = 1; i < countOfPointsObject; i++)
            {
                MapPoint p1 = arrayOfMapPoints[i - 1];
                MapPoint p2 = arrayOfMapPoints[i];
                double delx = p2.Y - p1.Y;
                double dels = sequentialCalculationPolylineLength[i] - sequentialCalculationPolylineLength[i - 1];
                double dels2 = sequentialCalculationPolylineLength[i] * sequentialCalculationPolylineLength[i] - sequentialCalculationPolylineLength[i - 1] * sequentialCalculationPolylineLength[i - 1];
                double cc = -1.0 * delx * sequentialCalculationPolylineLength[i - 1] / dels;
                double aa = p1.Y + cc;
                double bb = delx / dels;
                double v1 = aa * dels;
                double v2 = bb / 2 * dels2;
                Ay[0] = Ay[0] + v1 / polyLineLength + v2 / polyLineLength;
            }
            for (int k = 1; k < n + 1; k++)
            {
                Ay[k] = 0;
                double angle = 2 * Math.PI * k / polyLineLength;
                for (int i = 1; i < countOfPointsObject; i++)
                {
                    MapPoint p1 = arrayOfMapPoints[i - 1];
                    MapPoint p2 = arrayOfMapPoints[i];
                    double delx = p2.Y - p1.Y;
                    double dels = sequentialCalculationPolylineLength[i] - sequentialCalculationPolylineLength[i - 1];
                    double dels2 = sequentialCalculationPolylineLength[i] * sequentialCalculationPolylineLength[i] - sequentialCalculationPolylineLength[i - 1] * sequentialCalculationPolylineLength[i - 1];
                    double cc = -1.0 * delx * sequentialCalculationPolylineLength[i - 1] / dels;
                    double aa = p1.Y + cc;
                    double bb = delx / dels;
                    double k1 = (double)k;
                    double aa1 = aa / Math.PI / k1;
                    double delsin = Math.Sin(2.0 * k1 * Math.PI * sequentialCalculationPolylineLength[i] / polyLineLength) - Math.Sin(2.0 * k1 * Math.PI * sequentialCalculationPolylineLength[i - 1] / polyLineLength);
                    double v1 = aa1 * delsin;
                    double bb1 = bb / Math.PI / k1;
                    double delsinS = Math.Sin(2.0 * k1 * Math.PI * sequentialCalculationPolylineLength[i] / polyLineLength) * sequentialCalculationPolylineLength[i] - Math.Sin(2.0 * k1 * Math.PI * sequentialCalculationPolylineLength[i - 1] / polyLineLength) * sequentialCalculationPolylineLength[i - 1];
                    double v2 = bb1 * delsinS;
                    double bb2 = bb1 * polyLineLength / 2.0 / Math.PI / k1;
                    double delcos = Math.Cos(2.0 * k1 * Math.PI * sequentialCalculationPolylineLength[i] / polyLineLength) - Math.Cos(2.0 * k1 * Math.PI * sequentialCalculationPolylineLength[i - 1] / polyLineLength);
                    double v3 = bb2 * delcos;
                    Ay[k] = Ay[k] + v1 + v2 + v3;
                }

                By[k] = 0;
                for (int i = 1; i < countOfPointsObject; i++)
                {
                    MapPoint p1 = arrayOfMapPoints[i - 1];
                    MapPoint p2 = arrayOfMapPoints[i];
                    double delx = p2.Y - p1.Y;
                    double dels = sequentialCalculationPolylineLength[i] - sequentialCalculationPolylineLength[i - 1];
                    double dels2 = sequentialCalculationPolylineLength[i] * sequentialCalculationPolylineLength[i] - sequentialCalculationPolylineLength[i - 1] * sequentialCalculationPolylineLength[i - 1];
                    double cc = -1.0 * delx * sequentialCalculationPolylineLength[i - 1] / dels;
                    double aa = p1.Y + cc;
                    double bb = delx / dels;
                    double k1 = (double)k;
                    double aa1 = (-1.0) * aa / Math.PI / k1;
                    double delcos = Math.Cos(2.0 * k1 * Math.PI * sequentialCalculationPolylineLength[i] / polyLineLength) - Math.Cos(2.0 * k1 * Math.PI * sequentialCalculationPolylineLength[i - 1] / polyLineLength);
                    double v1 = aa1 * delcos;
                    double bb1 = (-1.0) * bb / Math.PI / k1;
                    double delcosS = Math.Cos(2.0 * k1 * Math.PI * sequentialCalculationPolylineLength[i] / polyLineLength) * sequentialCalculationPolylineLength[i] - Math.Cos(2.0 * k1 * Math.PI * sequentialCalculationPolylineLength[i - 1] / polyLineLength) * sequentialCalculationPolylineLength[i - 1];
                    double v2 = bb1 * delcosS;
                    double bb2 = (-1.0) * bb1 * polyLineLength / 2.0 / Math.PI / k1;
                    double delsin = Math.Sin(2.0 * k1 * Math.PI * sequentialCalculationPolylineLength[i] / polyLineLength) - Math.Sin(2.0 * k1 * Math.PI * sequentialCalculationPolylineLength[i - 1] / polyLineLength);
                    double v3 = bb2 * delsin;
                    By[k] = By[k] + v1 + v2 + v3;
                }
            }
            return true;
        }

        public void GetSinglePt(long nTerm, double s, out MapPoint pt)
        {
            pt = new MapPoint();
            double x = Ax[0];
            double y = Ay[0];
            for (int i = 1; i < nTerm + 1; i++)
            {
                double angle = 2.0 * Math.PI * (double)i * s / polyLineLength;
                x = x + Ax[i] * Math.Cos(angle) + Bx[i] * Math.Sin(angle);
                y = y + Ay[i] * Math.Cos(angle) + By[i] * Math.Sin(angle);
            }
            pt.X = x;
            pt.Y = y;
        }

        //public void GetOriginPosCoordinate(long nCurrentTerm)
        //{
        //    if (nCurrentTerm > nTerm)
        //    {
        //        return;
        //    }

        //    newPointCol = new List<MapPoint>();
        //    //var newPointColPolygon = new MapPolygon();
        //    for (int i = 0; i < m_PointNum; i++)
        //    {
        //        MapPoint pt;
        //        GetSinglePt(nCurrentTerm, m_accuDist[i], out pt);
        //        newPointCol.Add(pt);
        //    }
        //}

        //public double[] CalculatePower()
        //{
        //    long n = nTerm + 1;
        //    double[] D = new double[n];
        //    Power = new double[n];

        //    double sum = 0;
        //    for (int i = 1; i < n; i++)
        //    {
        //        double CX = Ax[i] + By[i];
        //        double CY = Bx[i] - Ay[i];
        //        D[i] = Math.Sqrt(CX * CX + CY * CY);
        //        sum = sum + D[i];
        //    }

        //    Power[0] = Math.Sqrt(Ax[0] * Ax[0] + Ay[0] * Ay[0]) / 4;
        //    for (int i = 1; i < n; i++)
        //    {
        //        Power[i] = D[i] * D[i] / 4;
        //    }
        //    return Power;
        //}

        public List<MapPoint> GetRecoveryPoints(long FittingPointNumber)
        {
            var ReturnPoints = new List<MapPoint>();

            MapPoint mapPoint = new MapPoint(Ax[0], Ay[0], 1, 1);
            
            double s = GetAllDist();
            double[] ss = new double[FittingPointNumber + 1];
            ss[0] = 0.0;
            double s_average = polyLineLength / (double)FittingPointNumber;
            for (int i = 1; i < FittingPointNumber; i++)
            {
                ss[i] = ss[i - 1] + s_average;
            }

            for (int j = 1; j < FittingPointNumber + 1; j++)
            {
                double Xin = 0.0;
                double Yin = 0.0;
                for (int i = 1; i < Ax.Length; i++)
                {
                    double angle = 2.0 * Math.PI * (double)i * ss[j - 1] / polyLineLength;
                    Xin += Ax[i] * Math.Cos(angle) + Bx[i] * Math.Sin(angle);
                    Yin += Ay[i] * Math.Cos(angle) + By[i] * Math.Sin(angle);
                }

                ReturnPoints.Add(new MapPoint(Xin + mapPoint.X, Yin + mapPoint.Y, j - 1, 1));
            }

            return ReturnPoints;
        }

        public double[,] GetRecoveryPoints_NotClosed(long FittingPointNumber, long Fouriers)
        {
            double x = Ax[0];
            double y = Ay[0];

            double s = GetAllDist();
            double[] ss = new double[FittingPointNumber + 1];
            ss[0] = 0.0;
            double s_average = polyLineLength / (double)FittingPointNumber;
            for (int i = 1; i < FittingPointNumber; i++)
            {
                ss[i] = ss[i - 1] + s_average;
            }

            double[] xx = new double[FittingPointNumber];
            double[] yy = new double[FittingPointNumber];

            for (int j = 1; j < FittingPointNumber + 1; j++)
            {
                double Xin = 0.0;
                double Yin = 0.0;
                for (int i = 1; i < Ax.Length; i++)
                {
                    double angle = 2.0 * Math.PI * (double)i * ss[j - 1] / polyLineLength;
                    Xin += Ax[i] * Math.Cos(angle) + Bx[i] * Math.Sin(angle);
                    Yin += Ay[i] * Math.Cos(angle) + By[i] * Math.Sin(angle);
                }
                xx[j - 1] = Xin + x;
                yy[j - 1] = Yin + y;
            }
            double[,] ReturnPoints = new double[FittingPointNumber, 2];
            for (int i = 0; i < FittingPointNumber; i++)
            {
                ReturnPoints[i, 0] = xx[i];
                ReturnPoints[i, 1] = yy[i];
            }
            return ReturnPoints;
        }

        public double[,] GetRecoveryPoints2(long FittingPointNumber, double[] AX, double[] BX, double[] AY, double[] BY, long F_number)
        {
            double x = AX[0];
            double y = AY[0];

            double s = GetAllDist();
            double[] ss = new double[FittingPointNumber + 1];
            ss[0] = 0.0;
            double s_average = polyLineLength / (double)FittingPointNumber;
            for (int i = 1; i < FittingPointNumber; i++)
            {
                ss[i] = ss[i - 1] + s_average;
            }

            double[] xx = new double[FittingPointNumber];
            double[] yy = new double[FittingPointNumber];

            for (int j = 1; j < FittingPointNumber + 1; j++)
            {
                double Xin = 0.0;
                double Yin = 0.0;
                for (int i = 1; i < F_number + 1; i++)
                {
                    double angle = 2.0 * Math.PI * (double)i * ss[j - 1] / polyLineLength;
                    Xin += AX[i] * Math.Cos(angle) + BX[i] * Math.Sin(angle);
                    Yin += AY[i] * Math.Cos(angle) + BY[i] * Math.Sin(angle);
                }
                xx[j - 1] = Xin + x;
                yy[j - 1] = Yin + y;
            }
            double[,] ReturnPoints = new double[FittingPointNumber, 2];
            for (int i = 0; i < FittingPointNumber; i++)
            {
                ReturnPoints[i, 0] = xx[i];
                ReturnPoints[i, 1] = yy[i];
            }
            return ReturnPoints;
        }

        public double[] CalculateEntropy()
        {
            double[] Proportion = new double[d.Length - 1];

            double d_sum = default(double);

            for (int i = 0; i < Proportion.Length; i++)
            {
                d_sum += d[i + 1];
            }

            for (int i = 0; i < Proportion.Length; i++)
            {
                Proportion[i] = d[i + 1] / d_sum;
            }

            double[] Entropy = new double[Proportion.Length];
            for (int i = 0; i < Entropy.Length; i++)
            {
                Entropy[i] = -Proportion[i] * Math.Log(Proportion[i], 2);
            }
            return Entropy;
        }

        
    }
}