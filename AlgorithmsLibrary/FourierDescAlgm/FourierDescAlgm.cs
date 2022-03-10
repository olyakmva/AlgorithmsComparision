using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsLibrary.FourierDescAlgm
{
    public class FourierDescAlgm : ISimplificationAlgm
    {
        public SimplificationAlgmParameters Options { get; set; }

        public virtual void Run(MapData map)
        {
            throw new NotImplementedException();
        }

        private void Run(ref List<MapPoint> chain, int startIndex, int endIndex)
        {
            
        }
    }
}
