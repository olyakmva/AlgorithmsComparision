using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace AlgorithmsLibrary
{
    [Serializable]
    public class WangMullerAlgmParams
    {
        public double MinAngleValue { get; set; }
        public double ExaggregateFactor { get; set; }
        public double CombineFactor { get; set; }
    }

/// <summary>
/// Алгоритм Ванга-Мюллера
/// Параметром является диаметр(!!!) минимального изгиба
/// </summary>
    public class WangMullerAlgm : ISimplificationAlgm
    {
        public SimplificationAlgmParameters Options { get; set;}
        private double _minBendArea;
        private readonly WangMullerAlgmParams _params;
        private const string ParamsFileName = "WangMullerParams.xml";
        private List<MapPoint> _endPoints;

        public WangMullerAlgm()
        {
            if (File.Exists(ParamsFileName))
            {
                XmlSerializer slz = new XmlSerializer(typeof(WangMullerAlgmParams));
                var paramFile = File.OpenRead(ParamsFileName);
                _params = (WangMullerAlgmParams)slz.Deserialize(paramFile);
            }
            else
            {
                _params = new WangMullerAlgmParams
                {
                    MinAngleValue = 0.56, //радиан, 32 градусa 
                    ExaggregateFactor = 1.2,
                    CombineFactor = 1.2
                };
                XmlSerializer slz = new XmlSerializer(typeof(WangMullerAlgmParams));
               var f = new FileStream(ParamsFileName, FileMode.Create, FileAccess.Write, FileShare.None);
                slz.Serialize(f, _params);
                f.Close();
            }
        }
        public virtual void Run(MapData map)
        {
            _minBendArea = Math.Round(Math.PI * Options.Tolerance * Options.Tolerance / 8);
            Options.OutParam = Options.Tolerance;
            
            for (int i=0;i<map.VertexList.Count; i++)
            {
                var lst= map.VertexList[i];
                map.VertexList[i] = SimplificationStep(lst);
                
                //catch (Exception e)
                //{
                //    string msg = e.Message + DateTime.Now + e.StackTrace;
                //    ErrorLog.WriteToLogFile(msg);
                //}
                Filtration(map.VertexList[i]);
            }
        }

        private  List<MapPoint > SimplificationStep(List<MapPoint> list)
        {
            int count = 0;
            
            while (true)
            {
                if (list.Count < 4)
                    return list;
                _endPoints = new List<MapPoint>();
                var bendList = BendListCreation2(list);
                var minArea = bendList[0].Area();
                double compactIndx = bendList[0].CompactIndex();
                for (int i = 1; i < bendList.Count; i++)
                {
                    if (minArea > bendList[i].Area())
                    {
                        minArea = bendList[i].Area();
                        compactIndx = bendList[i].CompactIndex();
                    }
                }
                if (minArea*0.75 / compactIndx > _minBendArea)
                {
                    break;
                }
                bool wasChanged = false;
                for (int i = 1; i < bendList.Count - 2; i++)
                {
                    var currentBend = bendList[i];
                    if(currentBend.BendNodeList.Count <=2)
                        continue;
                    var previousBend = bendList[i - 1];
                    var nextBend = bendList[i + 1];
                    var nextNextBend = bendList[i + 2];
                    ////проверка изолированности
                    //if (currentBend.Area() > 2 * (previousBend.Area() + nextNextBend.Area() + nextBend.Area()) / 3)
                    //{
                    //    currentBend.ExaggerateRadial(ExaggregateFactor);
                    //    WasChanged = true;
                    //}
                    //else 
                    
                    if (IsSimilar(currentBend, nextNextBend))
                    {
                        //проверка схожести
                        currentBend.CombineWith2(nextNextBend);
                        bendList.RemoveAt(i + 1);
                        bendList.RemoveAt(i+1);
                        wasChanged = true;
                    }
                    else if (currentBend.Area() * 0.75 / currentBend.CompactIndex() < _minBendArea)
                    {
                        if(currentBend.BendNodeList.Count >=3)
                                 currentBend.BendNodeList.RemoveRange(1, currentBend.BendNodeList.Count - 2);
                        else if (currentBend.BendNodeList.Count == 2)
                            currentBend.BendNodeList.RemoveAt(1);
                        if(previousBend.BendNodeList.Count >=2 )
                            previousBend.BendNodeList.RemoveAt( previousBend.BendNodeList.Count-1);
                        nextBend.BendNodeList.RemoveAt( 0);
                        bendList.RemoveAt(i);
                        i--;
                        wasChanged = true;
                    }
                }
                
                // первый и последний
                if (bendList[0].Area() * 0.75 / bendList[0].CompactIndex() < _minBendArea)
                {
                    bendList[0].BendNodeList.RemoveRange(1, bendList[0].BendNodeList.Count - 2);
                    wasChanged = true;
                }
                if (bendList.Count >= 2)
                {
                    var prevBend = bendList[bendList.Count - 2];
                    var lastBend = bendList[bendList.Count - 1];
                    if (lastBend.Area() * 0.75 / lastBend.CompactIndex() < _minBendArea)
                    {
                        lastBend.BendNodeList.RemoveRange(1, lastBend.BendNodeList.Count - 2);
                        wasChanged = true;
                    }
                    if (prevBend.Area() * 0.75 / prevBend.CompactIndex() < _minBendArea)
                    {
                        prevBend.BendNodeList.RemoveRange(1, prevBend.BendNodeList.Count - 2);
                        wasChanged = true;
                    }
                }
                var resultChain = new List<MapPoint>();
                foreach (var bend in bendList)
                {
                    bend.BendNodeList[0].Weight = 5;
                    bend.BendNodeList[bend.BendNodeList.Count - 1].Weight = 25;
                    resultChain.AddRange(bend.BendNodeList);
                }
                if(_endPoints.Count>0)
                    resultChain .AddRange(_endPoints);
                for (int i = 0; i < resultChain.Count - 2; i++)
                {
                    if (resultChain[i].DistanceToVertex(resultChain[i + 1]) < 1)
                    {
                        resultChain.RemoveAt(i + 1);
                    }
                    if (resultChain.Count - i > 2 && resultChain[i].DistanceToVertex(resultChain[i + 2]) < 1)
                    {
                        resultChain.RemoveAt(i + 2);
                    }
                }
                list = resultChain;
                if (!wasChanged)
                {
                    break;
                }
                count++;
                if (count == 8)
                    break;
            }
            return list;
        }

        public List<Bend> BendListCreation(List<MapPoint> list)
        {
            int index = 0;
            var bendList = new List<Bend>();
            while (index < list.Count - 2)
            {
                var firstIndex = index;
                var b = new Bend();
                b.BendNodeList.Add(list[index]);
                b.BendNodeList.Add(list[index + 1]);
                b.BendNodeList.Add(list[index + 2]);
                index += 3;
                var bendOrient = Orientation(b.BendNodeList[0], b.BendNodeList[1], b.BendNodeList[2]);

                //looking for end of the bend
                while (index < list.Count)
                {
                    var orient = Orientation(b.BendNodeList[b.BendNodeList.Count - 2],
                        b.BendNodeList[b.BendNodeList.Count - 1], list[index]);
                   if (orient != bendOrient)
                        break;
                    b.BendNodeList.Add(list[index]);
                    index++;
                }
                index--;

                //добавление крайних точек к изгибу при достаточно маленьком отклонении от 180 градусов и уменьшении ширины основания
                //к концу
                while (index < list.Count - 1)
                {
                    if ((CosOfAngle(list[index - 1], list[index], list[index + 1]) > Math.Cos(_params.MinAngleValue))
                        && (b.BaseLineLength() < list[firstIndex].DistanceToVertex(list[index + 1])))
                    {
                        index++;
                        b.BendNodeList.Add(list[index]);
                    }
                    else
                    {
                        break;
                    }
                }
                if (bendList.Count >= 1)
                {
                    var prevBend = bendList[bendList.Count - 1];
                    if (prevBend.BendNodeList.Count == 3 || prevBend.Area() < b.Area())
                    {
                        prevBend.BendNodeList[prevBend.BendNodeList.Count - 1].Weight = 1;
                        prevBend.BendNodeList.AddRange(b.BendNodeList);
                        prevBend.BendNodeList[prevBend.BendNodeList.Count - 1].Weight = 25;
                    }
                    else
                    {
                        b.BendNodeList[0].Weight = 25;
                        b.BendNodeList[b.BendNodeList.Count - 1].Weight = 25;
                        bendList.Add(b);
                    }
                }
                else
                {
                    b.BendNodeList[0].Weight = 25;
                    b.BendNodeList[b.BendNodeList.Count - 1].Weight = 25;
                    bendList.Add(b);
                }
            }
            for(int i=index+1; i<list.Count; i++)
                _endPoints.Add(list[i]);
            return bendList;
        }

        public List<Bend> BendListCreation2(List<MapPoint> list)
        {
            _endPoints = new List<MapPoint>();
            int index = 0;
            var bendList = new List<Bend>();
            while (index < list.Count - 2)
            {
                var firstIndex = index;
                var b = new Bend();
                b.BendNodeList.Add(list[index]);
                b.BendNodeList.Add(list[index + 1]);
                b.BendNodeList.Add(list[index + 2]);
                index += 3;
                var bendOrient = Orientation(b.BendNodeList[0], b.BendNodeList[1], b.BendNodeList[2]);

                //looking for end of the bend
                while (index < list.Count)
                {
                    var orient = Orientation(b.BendNodeList[b.BendNodeList.Count - 2],
                        b.BendNodeList[b.BendNodeList.Count - 1], list[index]);
                    if (orient != bendOrient)
                        break;
                    b.BendNodeList.Add(list[index]);
                    index++;
                }
                index--;

                //добавление крайних точек к изгибу при достаточно маленьком отклонении от 180 градусов и уменьшении ширины основания
                //к концу
                while (index < list.Count - 1)
                {
                    if ((CosOfAngle(list[index - 1], list[index], list[index + 1]) > Math.Cos(_params.MinAngleValue))
                        && (b.BaseLineLength() < list[firstIndex].DistanceToVertex(list[index + 1])))
                    {
                        index++;
                        b.BendNodeList.Add(list[index]);
                    }
                    else
                    {
                        break;
                    }
                }
                if (bendList.Count >= 1)
                {
                    var prevBend = bendList[bendList.Count - 1];
                    if (prevBend.BendNodeList.Count == 3 || prevBend.Area() < b.Area())
                    {
                        prevBend.BendNodeList.RemoveAt(prevBend.BendNodeList.Count - 1);
                        prevBend .BendNodeList.RemoveAt(prevBend .BendNodeList.Count-1);
                        prevBend.BendNodeList.AddRange(b.BendNodeList);
                        prevBend.BendNodeList[prevBend.BendNodeList.Count - 1].Weight = 25;
                    }
                    else
                    {
                        b.BendNodeList[0].Weight = 5;
                        b.BendNodeList[b.BendNodeList.Count - 1].Weight = 25;
                        bendList.Add(b);
                    }
                }
                else
                {
                    b.BendNodeList[0].Weight = 5;
                    b.BendNodeList[b.BendNodeList.Count - 1].Weight = 25;
                    bendList.Add(b);
                }
                index--;
            }
            for (int i = index + 1; i < list.Count; i++)
                _endPoints.Add(list[i]);
            
            return bendList;
        }

        public void Filtration(List<MapPoint> list)
        {
            int index = 0;
            while (index < list.Count - 2)
            {
                var b = new Bend();
                b.BendNodeList.Add(list[index]);
                b.BendNodeList.Add(list[index + 1]);
                b.BendNodeList.Add(list[index + 2]);
                index += 3;
                var bendOrient = Orientation(b.BendNodeList[0], b.BendNodeList[1], b.BendNodeList[2]);

                //looking for end of the bend
                while (index < list.Count)
                {
                    var orient = Orientation(b.BendNodeList[b.BendNodeList.Count - 2],
                        b.BendNodeList[b.BendNodeList.Count - 1], list[index]);
                    if (orient != bendOrient)
                        break;
                    b.BendNodeList.Add(list[index]);
                    index++;
                }
                index--;
                if (b.Area() * 0.75 / b.CompactIndex() < _minBendArea )
                {
                    b.BendNodeList[0].Weight = 2;
                    b.BendNodeList[b.BendNodeList.Count - 1].Weight = 2;
                }
                else
                {
                    foreach (var v in b.BendNodeList)
                    {
                        v.Weight = 2;
                    }
                }
            }
            list.RemoveAll(v => v.Weight < 2);
        }
        private bool IsSimilar(Bend currentBend, Bend nextBend)
        {
            if (Math.Abs(currentBend.Area() - nextBend.Area()) < _minBendArea/2 &&
                Math.Abs(currentBend.BaseLineLength() - nextBend.BaseLineLength()) < Options.Tolerance/2
                && Math.Abs(currentBend.CompactIndex() - nextBend.CompactIndex())< 0.1
                )
            {
                if(Orientation(currentBend) == Orientation(nextBend))
                return true;
                return false;
            }
            return false;
        }

        private  bool Orientation(MapPoint u, MapPoint v, MapPoint w)
        {
            return (!((v.X - u.X) * (w.Y - u.Y) - (w.X - u.X) * (v.Y - u.Y) < 0));
        }

        private bool Orientation(Bend oneBend)
        {
            return Orientation(oneBend.BendNodeList[0], oneBend.BendNodeList[1], oneBend.BendNodeList[2]);
        }

        /// <summary>
        /// косинус угла между векторами [u,v] и [v,w]
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <param name="w"></param>
        /// <returns></returns>
        public  double CosOfAngle(MapPoint u, MapPoint v, MapPoint w)
        {
            return ((v.X - u.X)*(w.X - v.X) + (v.Y - u.Y)*(w.Y - v.Y))/(Math.Sqrt((Math.Pow(v.X - u.X, 2) + Math.Pow(v.Y - u.Y, 2)) * (Math.Pow(w.X - v.X, 2) + Math.Pow(w.Y - v.Y, 2))));
        }
    }

    public class WangMullerWithCriterion : WangMullerAlgm
    {
        readonly ICriterion _criterion;

        public WangMullerWithCriterion(ICriterion cr)
        {
            _criterion = cr;
        }

        public override void Run(MapData map)
        {
            _criterion.Init(map, Options);

            var tempMap = map.Clone();
            while (true)
            {
                base.Run(tempMap);
                if (_criterion.IsSatisfy(tempMap))
                {
                    break;
                }
                _criterion.GetParamByCriterion(Options);
                tempMap = map.Clone();
            }
            base.Run(map);
            Options.OutParam = Options.Tolerance;
        }

    }



    public class WangMullerAlgmWithPercent : WangMullerAlgm
    {    
        SortedDictionary<int, double> _iterationValues = new SortedDictionary<int, double>();
        private int _n = 0;

        public override void Run(MapData map)
        {
            int mapCount = map.Count;
            int neededMapCount = Convert.ToInt32(Math.Round((Options.RemainingPercent * mapCount) / 100));
            int delta = Convert.ToInt32(Math.Round(mapCount * Options.PointNumberGap / 100));
            var tempMap = map.Clone();
            Options.Tolerance = Math.Round(Options.OutScale * 1.0);

            while (true)
            {
                base.Run(tempMap);
                int currentPointNumber = tempMap.Count;
                _n++;
                if (!_iterationValues.ContainsKey(currentPointNumber))
                {
                    _iterationValues.Add(currentPointNumber, Options.Tolerance);
                }
                if (Math.Abs(currentPointNumber - neededMapCount) < delta)
                {
                    break;
                }
                if (_iterationValues.Count >= 2)
                {
                    var minPair = (from p in _iterationValues
                        where p.Key < neededMapCount
                        orderby p.Key descending
                        select p).Take(1).ToList();
                    var maxPair = (from p in _iterationValues
                        where p.Key > neededMapCount
                        orderby p.Key select p).Take(1).ToList();
                    if (minPair.Count == 0 || maxPair.Count == 0)
                    {
                        Options.Tolerance =
                            Math.Round(Options.Tolerance * ((double)currentPointNumber / (double)neededMapCount));
                    }
                    else Options.Tolerance = Math.Round((minPair[0].Value - maxPair[0].Value) / 2 + maxPair[0].Value);

                }
                else
                {
                    Options.Tolerance =
                        Math.Round(Options.Tolerance * ((double) currentPointNumber / (double) neededMapCount));
                }
                tempMap = map.Clone();
            }
            Options.OutParam = Options.Tolerance;
            base.Run(map);
        }
    }

}
