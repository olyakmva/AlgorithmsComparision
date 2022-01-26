using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgorithmsLibrary
{
    public abstract class VisWhyattAlgm : ISimplificationAlgm
    {
        public SimplificationAlgmParameters Options { get; set; }

        public virtual void Run(MapData map)
        {
            Options.Tolerance = Options.Tolerance * Options.Tolerance;
            for (int i=0; i< map.VertexList.Count ; i++)
            {
                var chain = map.VertexList[i];
                int endIndex = chain.Count - 1;
                Run(ref chain, 0,  endIndex);
                map.VertexList[i] = chain;
            }
            Options.OutParam = Options.Tolerance;
        }

        private void Run(ref List<MapPoint> chain, int startIndex,  int endIndex)
        {
            if (endIndex - startIndex <= 2)
                return;
            
            LinkedList< MapPoint> list = new LinkedList<MapPoint>(chain);
            var heap = CreateHeap(chain, startIndex, endIndex);
            Process(heap, list);
            chain = list.ToList();
        }

        protected virtual void Process(UniqueHeap<double, MapPoint> heap, LinkedList<MapPoint> list)
        {
            var minWeightPoint = heap.GetMinElement(); 
            while (minWeightPoint.Key < Options.Tolerance)
            {
                var point = minWeightPoint.Value;
                heap.ExtractMinElement();
                var p = list.Find(point);
                if (p == null)
                    throw new ArgumentNullException("Point not found in LinkedList " + point);
                var nextPoint = p.Next;
                var prevPoint = p.Previous;
                list.Remove(p);
                CorrectPointWeight(heap, nextPoint);
                CorrectPointWeight(heap, prevPoint);
                minWeightPoint = heap.GetMinElement();
            }
        }

        protected UniqueHeap<double, MapPoint> CreateHeap(List<MapPoint> chain, int startIndex, int endIndex)
        {
            IComparer<double> comparer = Comparer<double>.Default; 
            UniqueHeap<double, MapPoint> heap = new UniqueHeap<double, MapPoint>(comparer, endIndex - startIndex);

            for (int i = startIndex + 1; i < endIndex; i++)
            {
                var t = new Triangle(chain[i - 1], chain[i], chain[i + 1]);
                var s = t.Square();
                if (s < double.Epsilon)
                {
                    if (chain[i].CompareTo(chain[i + 1]) == 0)
                    {
                        chain.RemoveAt(i);
                        endIndex--;
                    }
                    if (chain[i].CompareTo(chain[i - 1]) == 0)
                    {
                        chain.RemoveAt(i);
                        endIndex--;
                    }
                    continue;
                }
                chain[i].Weight = s;
                heap.Add(chain[i].Weight, chain[i]);
            }
            return heap;
        }

        protected void CorrectPointWeight(UniqueHeap<double, MapPoint> heap, LinkedListNode<MapPoint> pNode)
        {
            if(pNode == null)
                return;
            if(heap.Count==0)
                return;
            heap.Remove(pNode.Value);
            
            var prevNode = pNode.Previous;
            var nextNode = pNode.Next;
            if (prevNode == null || nextNode == null)
                return;
            var t = new Triangle(prevNode.Value, pNode.Value, nextNode.Value);
            pNode.Value.Weight = t.Square();
            heap.Add(pNode.Value.Weight, pNode.Value);
        }
    }

    public class VisWhyattAlgmWithTolerance : VisWhyattAlgm
    {
        protected override void Process(UniqueHeap<double, MapPoint> heap, LinkedList<MapPoint> list)
        {
            var minWeightPoint = heap.GetMinElement();
            while (minWeightPoint.Key < Options.Tolerance)
            {
                var point = minWeightPoint.Value;
                heap.ExtractMinElement();
                var p = list.Find(point);
                if (p == null)
                    throw new ArgumentNullException("Point not found in LinkedList " + point);
                var nextPoint = p.Next;
                var prevPoint = p.Previous;
                list.Remove(p);
                CorrectPointWeight(heap, nextPoint);
                CorrectPointWeight(heap, prevPoint);
                if (heap.Count > 0)
                {
                    minWeightPoint = heap.GetMinElement();
                }
                else break;
            }
        }

    }

    public class VisWhyattAlgmWithPercent : VisWhyattAlgm
    {
        private int _removingPointsQuantity;
        private int _error;
        public override void Run(MapData map)
        {
            _removingPointsQuantity = Convert.ToInt32(Math.Round(((100-Options.RemainingPercent) * map.Count) / 100));
            _error=Convert.ToInt32(Math.Round(map.Count * Options.PointNumberGap / 100));
            List<UniqueHeap< double,MapPoint>> lstHeaps = new List<UniqueHeap<double, MapPoint>>();
            List< LinkedList < MapPoint >> lstLists = new List<LinkedList<MapPoint>>();
            for (int i = 0; i < map.VertexList.Count; i++)
            {
                var chain = map.VertexList[i];
                int endIndex = chain.Count - 1;
                int startIndex = 0;
                if (endIndex - startIndex <= 2)
                    continue;
                LinkedList<MapPoint> list = new LinkedList<MapPoint>(chain);
                var heap = CreateHeap(chain, startIndex, endIndex);
                lstHeaps.Add(heap);
                lstLists.Add(list);
            }

            Process(lstHeaps, lstLists);
            int k = 0;
            for (int i = 0; i < map.VertexList.Count; i++)
            {
                var chain = map.VertexList[i];
                int endIndex = chain.Count - 1;
                int startIndex = 0;
                if (endIndex - startIndex <= 2)
                    continue;
                map.VertexList[i] = lstLists[k].ToList();
                k++;
            }
            

        }
        private void Process(List<UniqueHeap<double, MapPoint>> heaps, List<LinkedList<MapPoint>> lists)
        {
            int removingPointsCount = 0;
            var listMinPoints = new HeapElement<double, MapPoint>[heaps.Count];
            for (int i = 0; i < heaps.Count; i++)
            {
                listMinPoints[i] = heaps[i].GetMinElement();
            }

            while (Math.Abs(removingPointsCount  - _removingPointsQuantity) > _error )
            {
                int index;
                var minWeightPoint =GetMin( listMinPoints, out index);
                if(minWeightPoint == null)
                    return;
                Options.OutParam = minWeightPoint.Key;
                var point = minWeightPoint.Value;
                heaps[index].ExtractMinElement();
                var p = lists[index].Find(point);
                
                if (p == null)
                    throw new ArgumentNullException("Point not found in LinkedList " + point);
                var nextPoint = p.Next;
                var prevPoint = p.Previous;
                lists[index].Remove(p);
                removingPointsCount++;
                CorrectPointWeight(heaps[index], nextPoint);
                CorrectPointWeight(heaps[index], prevPoint);

                if (heaps[index].Count != 0)
                {
                    listMinPoints[index] = heaps[index].GetMinElement();
                }
                else listMinPoints[index] = null;
            }
        }

        private HeapElement<double, MapPoint> GetMin(HeapElement<double, MapPoint>[] list, out int index)
        {
            HeapElement<double, MapPoint> min ;
            index = 0;
            while (index < list.Length && list[index] == null)
                index++;
            if (index >= list.Length)
                return null;
            min = list[index];

            for (int i = index+1; i < list.Length; i++)
            {
                if (list[i]!=null && list[i].Key < min.Key)
                {
                    min = list[i];
                    index = i;
                }
            }
            return min;
        }
    }

    public class VisWhyattWithCriterion : VisWhyattAlgmWithTolerance
    {
        private readonly ICriterion _criterion;

        public VisWhyattWithCriterion(ICriterion cr)
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
                    Options.OutParam = Options.Tolerance;
                    Options.Tolerance = Math.Sqrt(Options.Tolerance);
                    base.Run(map);
                    break;
                }
                Options.Tolerance = Math.Sqrt(Options.Tolerance);
                _criterion.GetParamByCriterion(Options);
                tempMap = map.Clone();
            }
           
            
        }

    }

    
}
