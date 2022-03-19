﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsLibrary
{
    public class Fourier_NotClosed : FourierDescFatherClass
    {
        #region After
        private List<MapPoint> pointCol = null;
        //private IPointCollection newPointCol = null;

        private long m_PointNum = 0;

        //Si
        private double[] m_accuDist = null;
        private double[] m_dis_betPoint = null;
        
        private double m_totalS = 0.0;
      
        private double[] Ax = null;
        private double[] Bx = null;
        private double[] Ay = null;
        private double[] By = null;
        
        private double[] ratio = null;

        
        public Fourier_NotClosed(List<MapPoint> pointcollection, long nTerm)
        {
            this.nTerm = nTerm;
            while (true)
            {
                bool bOK = true;
                pointCol = pointcollection;
                for (int i = 1; i < pointCol.Count; i++)
                {
                    MapPoint p1 = pointCol[i - 1];
                    MapPoint p2 = pointCol[i];
                    if (Math.Abs(p1.X - p2.X) < 0.1 && Math.Abs(p1.Y - p2.Y) < 0.1)
                    {
                        pointCol.RemoveAt(i);
                        i--;
                        bOK = false;
                    }
                }
                if (bOK)
                {
                    m_PointNum = pointCol.Count;
                    break;
                }
            }

        }
        public double GetAx(int index)
        {
            return Ax[index];
        }
        public double GetBx(int index)
        {
            return Bx[index];
        }
        public double GetAy(int index)
        {
            return Ay[index];
        }
        public double GetBy(int index)
        {
            return By[index];
        }
        public double GetShapeVector(int index)
        {
            return d[index];
        }
        public double GetRatio(int index)
        {
            return ratio[index];
        }
        public double[] ArrayAx()
        {
            return Ax;
        }
        public double[] ArrayBx()
        {
            return Bx;
        }
        public double[] ArrayAy()
        {
            return Ay;
        }
        public double[] ArrayBy()
        {
            return By;
        }
        public void CalculateAllValue()
        {
            GetAllDist();
            GetFourierXparameter();
            GetFourierYparameter();
            CalculateShapeVector(false);
        }

        //返回总长度
        public double GetAllDist()
        {
            //不闭合的曲线,点的个数m_PointNum比闭合曲线少一个,单独距离个数为m_PointNum*2-2,总距离个数为m_PointNum*2-1
            m_dis_betPoint = new double[m_PointNum * 2 - 2];
            m_accuDist = new double[m_PointNum * 2 - 1];

            for (int i = (int)m_PointNum; i < m_PointNum * 2 - 1; i++)
            {
                MapPoint p;
                p = new MapPoint();
                double X1 = pointCol[0].X;
                double Y1 = pointCol[0].Y;
                double X2 = pointCol[(int)m_PointNum - 1].X;
                double Y2 = pointCol[(int)m_PointNum - 1].Y;
                double A = Y2 - Y1;
                double B = X1 - X2;
                double C = X2 * Y1 - X1 * Y2;
                double C_after = (A * pointCol[(int)m_PointNum * 2 - 2 - i].X + B * pointCol[(int)m_PointNum * 2 - 2 - i].Y + C) / (A * A + B * B);
                double X_symmetry = pointCol[(int)m_PointNum * 2 - 2 - i].X - 2 * A * (C_after);
                double Y_symmetry = pointCol[(int)m_PointNum * 2 - 2 - i].Y - 2 * B * (C_after);
                p.X = X_symmetry;
                p.Y = Y_symmetry;
                //p.PutCoords(X_symmetry, Y_symmetry);
                pointCol.Add(p);
            }

            for (int i = 0; i < m_PointNum * 2 - 2; i++)
            {
                MapPoint p2 = pointCol[i + 1];
                MapPoint p1 = pointCol[i];
                double detx = p2.X - p1.X;
                double dety = p2.Y - p1.Y; ;
                double squareSum = detx * detx + dety * dety;
                m_dis_betPoint[i] = Math.Sqrt(squareSum);
            }
            double s = 0.0;
            m_accuDist[0] = 0.0;
            for (int i = 0; i < m_PointNum * 2 - 2; i++)
            {
                m_accuDist[i + 1] = s + m_dis_betPoint[i];
                s = m_accuDist[i + 1];
            }
            m_totalS = m_accuDist[m_PointNum * 2 - 2];
            return m_totalS;
        }
        public bool GetFourierXparameter()
        {
            long n = nTerm;
            if (n <= 0) return false;
            Ax = new double[n + 1];
            Bx = new double[n + 1];
            Bx[0] = 0.0;
            Ax[0] = 0.0;
            for (int i = 1; i < 2 * m_PointNum - 1; i++)
            {
                MapPoint p1 = pointCol[i - 1];
                MapPoint p2 = pointCol[i];
                double delx = (double)(p2.X - p1.X);
                double dels = m_accuDist[i] - m_accuDist[i - 1];
                double dels2 = m_accuDist[i] * m_accuDist[i] - m_accuDist[i - 1] * m_accuDist[i - 1];
                double cc = -1.0 * delx * m_accuDist[i - 1] / dels;
                double aa = p1.X + cc;
                double bb = delx / dels;
                double v1 = aa * dels;
                double v2 = bb / 2 * dels2;
                Ax[0] = (Ax[0] + v1 / m_totalS + v2 / m_totalS);
            }
            for (int k = 1; k < n + 1; k++)
            {
                Ax[k] = 0;
                double angle = 2 * Math.PI * k / m_totalS;
                for (int i = 1; i < 2 * m_PointNum - 1; i++)
                {
                    MapPoint p1 = pointCol[i - 1];
                    MapPoint p2 = pointCol[i];
                    double delx = p2.X - p1.X;
                    double dels = m_accuDist[i] - m_accuDist[i - 1];
                    double dels2 = m_accuDist[i] * m_accuDist[i] - m_accuDist[i - 1] * m_accuDist[i - 1];
                    double cc = -1.0 * delx * m_accuDist[i - 1] / dels;
                    double aa = p1.X + cc;
                    double bb = delx / dels;
                    double k1 = (double)k;
                    double aa1 = aa / Math.PI / k1;
                    double delsin = Math.Sin(2.0 * k1 * Math.PI * m_accuDist[i] / m_totalS) - Math.Sin(2.0 * k1 * Math.PI * m_accuDist[i - 1] / m_totalS);
                    double v1 = aa1 * delsin;
                    double bb1 = bb / Math.PI / k1;
                    double delsinS = Math.Sin(2.0 * k1 * Math.PI * m_accuDist[i] / m_totalS) * m_accuDist[i] - Math.Sin(2.0 * k1 * Math.PI * m_accuDist[i - 1] / m_totalS) * m_accuDist[i - 1];
                    double v2 = bb1 * delsinS;
                    double bb2 = bb1 * m_totalS / 2.0 / Math.PI / k1;
                    double delcos = Math.Cos(2.0 * k1 * Math.PI * m_accuDist[i] / m_totalS) - Math.Cos(2.0 * k1 * Math.PI * m_accuDist[i - 1] / m_totalS);
                    double v3 = bb2 * delcos;
                    Ax[k] = Ax[k] + v1 + v2 + v3;
                }
                Bx[k] = 0;
                for (int i = 1; i < 2 * m_PointNum - 1; i++)
                {
                    MapPoint p1 = pointCol[i - 1];
                    MapPoint p2 = pointCol[i];
                    double delx = p2.X - p1.X;
                    double dels = m_accuDist[i] - m_accuDist[i - 1];
                    double dels2 = m_accuDist[i] * m_accuDist[i] - m_accuDist[i - 1] * m_accuDist[i - 1];
                    double cc = -1.0 * delx * m_accuDist[i - 1] / dels;
                    double aa = p1.X + cc;
                    double bb = delx / dels;
                    double k1 = (double)k;
                    double aa1 = (-1.0) * aa / Math.PI / k1;
                    double delcos = Math.Cos(2.0 * k1 * Math.PI * m_accuDist[i] / m_totalS) - Math.Cos(2.0 * k1 * Math.PI * m_accuDist[i - 1] / m_totalS);
                    double v1 = aa1 * delcos;
                    double bb1 = (-1.0) * bb / Math.PI / k1;
                    double delcosS = Math.Cos(2.0 * k1 * Math.PI * m_accuDist[i] / m_totalS) * m_accuDist[i] - Math.Cos(2.0 * k1 * Math.PI * m_accuDist[i - 1] / m_totalS) * m_accuDist[i - 1];
                    double v2 = bb1 * delcosS;
                    double bb2 = (-1.0) * bb1 * m_totalS / 2.0 / Math.PI / k1;
                    double delsin = Math.Sin(2.0 * k1 * Math.PI * m_accuDist[i] / m_totalS) - Math.Sin(2.0 * k1 * Math.PI * m_accuDist[i - 1] / m_totalS);
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
            for (int i = 1; i < 2 * m_PointNum - 1; i++)
            {
                MapPoint p1 = pointCol[i - 1];
                MapPoint p2 = pointCol[i];
                double delx = p2.Y - p1.Y;
                double dels = m_accuDist[i] - m_accuDist[i - 1];
                double dels2 = m_accuDist[i] * m_accuDist[i] - m_accuDist[i - 1] * m_accuDist[i - 1];
                double cc = -1.0 * delx * m_accuDist[i - 1] / dels;
                double aa = p1.Y + cc;
                double bb = delx / dels;
                double v1 = aa * dels;
                double v2 = bb / 2 * dels2;
                Ay[0] = (Ay[0] + v1 / m_totalS + v2 / m_totalS);
            }
            for (int k = 1; k < n + 1; k++)
            {
                Ay[k] = 0;
                double angle = 2 * Math.PI * k / m_totalS;
                for (int i = 1; i < 2 * m_PointNum - 1; i++)
                {
                    MapPoint p1 = pointCol[i - 1];
                    MapPoint p2 = pointCol[i];
                    double delx = p2.Y - p1.Y;
                    double dels = m_accuDist[i] - m_accuDist[i - 1];
                    double dels2 = m_accuDist[i] * m_accuDist[i] - m_accuDist[i - 1] * m_accuDist[i - 1];
                    double cc = -1.0 * delx * m_accuDist[i - 1] / dels;
                    double aa = p1.Y + cc;
                    double bb = delx / dels;
                    double k1 = (double)k;
                    double aa1 = aa / Math.PI / k1;
                    double delsin = Math.Sin(2.0 * k1 * Math.PI * m_accuDist[i] / m_totalS) - Math.Sin(2.0 * k1 * Math.PI * m_accuDist[i - 1] / m_totalS);
                    double v1 = aa1 * delsin;
                    double bb1 = bb / Math.PI / k1;
                    double delsinS = Math.Sin(2.0 * k1 * Math.PI * m_accuDist[i] / m_totalS) * m_accuDist[i] - Math.Sin(2.0 * k1 * Math.PI * m_accuDist[i - 1] / m_totalS) * m_accuDist[i - 1];
                    double v2 = bb1 * delsinS;
                    double bb2 = bb1 * m_totalS / 2.0 / Math.PI / k1;
                    double delcos = Math.Cos(2.0 * k1 * Math.PI * m_accuDist[i] / m_totalS) - Math.Cos(2.0 * k1 * Math.PI * m_accuDist[i - 1] / m_totalS);
                    double v3 = bb2 * delcos;
                    Ay[k] = Ay[k] + v1 + v2 + v3;
                }
                By[k] = 0;
                for (int i = 1; i < 2 * m_PointNum - 1; i++)
                {
                    MapPoint p1 = pointCol[i - 1];
                    MapPoint p2 = pointCol[i];
                    double delx = p2.Y - p1.Y;
                    double dels = m_accuDist[i] - m_accuDist[i - 1];
                    double dels2 = m_accuDist[i] * m_accuDist[i] - m_accuDist[i - 1] * m_accuDist[i - 1];
                    double cc = -1.0 * delx * m_accuDist[i - 1] / dels;
                    double aa = p1.Y + cc;
                    double bb = delx / dels;
                    double k1 = (double)k;
                    double aa1 = (-1.0) * aa / Math.PI / k1;
                    double delcos = Math.Cos(2.0 * k1 * Math.PI * m_accuDist[i] / m_totalS) - Math.Cos(2.0 * k1 * Math.PI * m_accuDist[i - 1] / m_totalS);
                    double v1 = aa1 * delcos;
                    double bb1 = (-1.0) * bb / Math.PI / k1;
                    double delcosS = Math.Cos(2.0 * k1 * Math.PI * m_accuDist[i] / m_totalS) * m_accuDist[i] - Math.Cos(2.0 * k1 * Math.PI * m_accuDist[i - 1] / m_totalS) * m_accuDist[i - 1];
                    double v2 = bb1 * delcosS;
                    double bb2 = (-1.0) * bb1 * m_totalS / 2.0 / Math.PI / k1;
                    double delsin = Math.Sin(2.0 * k1 * Math.PI * m_accuDist[i] / m_totalS) - Math.Sin(2.0 * k1 * Math.PI * m_accuDist[i - 1] / m_totalS);
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
                double angle = 2.0 * Math.PI * (double)i * s / m_totalS;
                x = x + Ax[i] * Math.Cos(angle) + Bx[i] * Math.Sin(angle);
                y = y + Ay[i] * Math.Cos(angle) + By[i] * Math.Sin(angle);
            }
            pt.X = x;
            pt.Y = y;
            //pt.Z = 0;
        }

        //public void GetOriginPosCoordinate(long nCurrentTerm)
        //{
        //    if (nCurrentTerm > nTerm)
        //    {
        //        return;
        //    }
        //    newPointCol = new PolygonClass();
        //    object before = Type.Missing;
        //    object after = Type.Missing;
        //    for (int i = 0; i < 2 * m_PointNum - 1; i++)
        //    {
        //        IPoint pt;
        //        GetSinglePt(nCurrentTerm, m_accuDist[i], out pt);
        //        newPointCol.AddPoint(pt, ref before, ref after);
        //    }
        //}

        public double[,] GetRecoveryPoints(long FittingPointNumber, long Fouriers)
        {
            double x = Ax[0];
            double y = Ay[0];
            double s = GetAllDist();
            double[] ss = new double[FittingPointNumber + 1];
            ss[0] = 0.0;
            double s_average = m_totalS / (double)FittingPointNumber;
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
                for (int i = 1; i < Fouriers + 1; i++)
                {
                    double angle = 2.0 * Math.PI * (double)i * ss[j - 1] / m_totalS;
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

        public double[,] GetRecoveryPoints_NotClosed(long FittingPointNumber, long Fouriers)
        {
            double x = Ax[0];
            double y = Ay[0];
            double s = GetAllDist();
            double[] ss = new double[FittingPointNumber + 1];
            ss[0] = 0.0;
            double s_average = m_totalS / (double)FittingPointNumber;
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
                for (int i = 1; i < Fouriers + 1; i++)
                {
                    double angle = 2.0 * Math.PI * (double)i * ss[j - 1] / m_totalS;
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

        public double[,] GetRecoveryPoints2(long FittingPointNumber, double[] AX, double[] BX, double[] AY, double[] BY, long F_number, double m_totalS)
        {
            double x = AX[0];
            double y = AY[0];
            double[] ss = new double[FittingPointNumber + 1];
            ss[0] = 0.0;
            double s_average = (m_totalS / (double)FittingPointNumber) / (double)2.000;
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
                    double angle = 2.0 * Math.PI * (double)i * ss[j - 1] / m_totalS;
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
            for (int i = 0; i < Proportion.Length; i++)
            {
                Proportion[i] = d[i + 1];
            }

            double[] Entropy = new double[Proportion.Length];
            for (int i = 0; i < Entropy.Length; i++)
            {
                Entropy[i] = -Proportion[i] * Math.Log(Proportion[i], 2);
            }
            return Entropy;
        }

        
        #endregion
    }
}