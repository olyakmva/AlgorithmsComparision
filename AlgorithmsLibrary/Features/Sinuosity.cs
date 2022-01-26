using System;
using System.Collections.Generic;

namespace AlgorithmsLibrary.Features
{
    public static class Sinuosity
    {
        /// <summary>
        /// вычисление извилистости данных на карте
        /// DEF: извилистость ломаной – отношение её длины к расстоянию между её концами
        /// извилистость данных на карте считается как отношение суммы длин всех цепей к сумме расстояний между концами цепей
        /// </summary>
        /// <param name="md">данные в формате набора цепей</param>
        /// <returns>извилистость</returns>
        public static double Get(MapData md)
        {
            double fullLength = 0;
            double fullBaselineLength = 0;
            foreach (var chain in md.VertexList)
            {
                fullLength += GetLength( chain);
                fullBaselineLength += GetBaselineLength(chain);
            }

            return fullLength / fullBaselineLength;
        }
        

        private static  double GetBaselineLength(List< MapPoint>list )
        {
            return list[0].DistanceToVertex(list[list.Count-1]);
        }


        private static double GetLength(List<MapPoint> list)
        {
            double length = 0;
            for (int i = 0; i < list.Count - 1; i++)
            {
                length += list[i].DistanceToVertex(list[i + 1]);
            }
            return length;
        }


        /// <summary>
        /// вычисление извилистости на карте
        /// с заданной частотой дискретизации
        /// </summary>
        /// <param name="chmd">данные карты</param>
        /// <param name="stepLength">частота дискретизации: разбиваем данные на участки длиной @stepLength</param>
        /// <returns>извилистость</returns>
        public static double ComputeSinuosity(MapData chmd, double stepLength)
        {
            double fullLength = 0;
            double fullBaselineLength = 0;

            foreach (var chain in chmd.VertexList)
            {
                double currLength = 0;
                int initIndex = 0;
                int currIndex = 0;
                while (currIndex < chain.Count - 1)
                {
                    double edgeLength = chain[currIndex].DistanceToVertex(chain[currIndex + 1]);
                    //если это первое ребро, в любом случае его включаем
                    if (currIndex - initIndex == 0)
                    {
                        currLength += edgeLength;
                        currIndex++;
                    }
                    else
                    {
                        //если длина шага превышена смотрим, стоит ли добавлять следующее ребро
                        if (currLength + edgeLength > stepLength)
                        {
                            //не нужно добавлять следующее ребро
                            if (stepLength < currLength + edgeLength / 2)
                            {
                                fullLength += currLength;
                                fullBaselineLength += chain[initIndex].DistanceToVertex(chain[currIndex]);
                                initIndex = currIndex;
                                currLength = edgeLength;
                                currIndex++;
                            }
                            else  //нужно добавлять следующее ребро
                            {
                                currLength += edgeLength;
                                fullLength += currLength;
                                currIndex++;
                                fullBaselineLength += chain[initIndex].DistanceToVertex(chain[currIndex]);
                                initIndex = currIndex;
                                currLength = 0;
                            }
                        }
                        else
                        {
                            currLength += edgeLength;
                            currIndex++;
                        }
                    }
                }
                //добавляем останки (может, не надо?)
                if (currLength > 0)
                {
                    fullLength += currLength;
                    fullBaselineLength += chain[initIndex].DistanceToVertex(chain[currIndex]);
                }
            }

            return fullLength / fullBaselineLength;
        }

        /// <summary>
        /// Подсчет интегрального коэффициента соответствия:
        /// Для каждой цепи: берем исходную и результирующую цепочку, считаем квадрат разности извилистостей;
        /// суммируем по всем цепочкам.
        /// Примечание: если существуют две цепочки с одинаковыми началом и концом, то они удаляются из вычислений, поэтому возможна небольшая погрешность.
        /// </summary>
        /// <param name="inChmd"></param>
        /// <param name="outChmd"></param>
        /// <param name="stepLength"></param>
        /// <returns></returns>
        public static double ComputeIntegralCorrespondence(MapData inChmd, MapData outChmd, double stepLength)
        {
            double result = 0;

            foreach (var inChain in inChmd.VertexList)
            {
                //if (inChain.BaselineLength == 0) break;

                //флаг, показывающий, найдена ли цепочка с таким же началом и концом
                bool chainFound = false;

                //разность между извилистостью входной и результирующей линий
                double sinDifference = 0;

                //ищем соответствующую цепочку в результирующих данных
                foreach (var outChain in outChmd.VertexList)
                {
                    if ((inChain[0].Equals(outChain[0]) && inChain[inChain.Count - 1].Equals(outChain[outChain.Count - 1])) ||
                        (inChain[0].Equals(outChain[outChain.Count - 1]) && inChain[inChain.Count - 1].Equals(outChain[0])))
                    {
                        //если нашли вторую цепочку с тем же началом и концом – не рассматриваем их
                        if (chainFound)
                        {
                            sinDifference = 0;
                            break;
                        }

                        //нашли соответствующую цепочку
                        //проверяем, не является ли она нулевой
                        if (!(outChain.Count == 2 && outChain[0].Equals(outChain[outChain.Count - 1])))
                        {
                            double inSinuosity = ComputeSinuosity(inChain, stepLength);
                            double outSinuosity = ComputeSinuosity(outChain, stepLength);
                            sinDifference = inSinuosity - outSinuosity;
                            chainFound = true;
                        }
                        else
                        {
                            //если найденная цепочка нулевая, выходим из цикла
                            sinDifference = 0;
                            break;
                        }
                    }
                }

                if (Math.Abs(sinDifference) > double.Epsilon)
                {
                    result += Math.Pow(sinDifference, 2);
                }
            }

            return result;
        }

        /// <summary>
        /// вычисление извилистости цепи
        /// DEF: извилистость цепи – отношение её длины к расстоянию между её концами
        /// </summary>
        /// <param name="ch">цепь</param>
        /// <param name="stepLength"> шаг</param>
        /// <returns>извилистость цепи</returns>
        public static double ComputeSinuosity(List<MapPoint> ch, double stepLength)
        {
            double fullLength = 0;
            double fullBaselineLength = 0;

            double currLength = 0;
            int initIndex = 0;
            int currIndex = 0;
            while (currIndex < ch.Count - 1)
            {
                double edgeLength = ch[currIndex].DistanceToVertex(ch[currIndex + 1]);
                //если это первое ребро, в любом случае его включаем
                if (currIndex - initIndex == 0)
                {
                    currLength += edgeLength;
                    currIndex++;
                }
                else
                {
                    //если длина шага превышена смотрим, стоит ли добавлять следующее ребро
                    if (currLength + edgeLength > stepLength)
                    {
                        //не нужно добавлять следующее ребро
                        if (stepLength < currLength + edgeLength / 2)
                        {
                            fullLength += currLength;
                            fullBaselineLength += ch[initIndex].DistanceToVertex(ch[currIndex]);
                            initIndex = currIndex;
                            currLength = edgeLength;
                            currIndex++;
                        }
                        else  //нужно добавлять следующее ребро
                        {
                            currLength += edgeLength;
                            fullLength += currLength;
                            currIndex++;
                            fullBaselineLength += ch[initIndex].DistanceToVertex(ch[currIndex]);
                            initIndex = currIndex;
                            currLength = 0;
                        }
                    }
                    else
                    {
                        currLength += edgeLength;
                        currIndex++;
                    }
                }
            }
            //добавляем останки (может, не надо?)
            if (currLength > 0)
            {
                fullLength += currLength;
                fullBaselineLength += ch[initIndex].DistanceToVertex(ch[currIndex]);
            }

            if (Math.Abs(fullBaselineLength) > double.Epsilon )
            {
                return fullLength / fullBaselineLength;
            }
            return 0;
        }
    }

}
