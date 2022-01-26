
namespace AlgorithmsLibrary
{
    public static class AlgmFabrics
    {
        public static ISimplificationAlgm GetAlgmByNameAndParam(string algmName, bool isPercent, bool isBend)
        {
            ISimplificationAlgm algm = null;
            switch (algmName)
            {
                case "DouglasPeuckerAlgm":
                    if (isPercent)
                    {
                        algm = new DouglasPeuckerAlgmWithCriterion( new PointPercentCriterion());
                    }
                    else if(isBend) algm = new DouglasPeuckerAlgmWithCriterion( new BendNumberCriterion());
                    else algm = new DouglasPeuckerAlgm();
                    break;
                case "LiOpenshawAlgm":
                    if (isPercent)
                    {
                        algm = new LiOpenshawWithCriterion(new PointPercentCriterion());
                    }
                    else if (isBend) algm = new LiOpenshawWithCriterion(new BendNumberCriterion());
                    else algm = new LiOpenshawAlgm();
                    break; 
                case "VisvWhyattAlgm": if (isPercent ) algm= new VisWhyattAlgmWithPercent();
                                       else if (isBend) algm = new VisWhyattWithCriterion(new VisWBendNumberCriterion());
                                       else algm= new VisWhyattAlgmWithTolerance();
                    break;
                case "SleeveFitAlgm":
                    if (isPercent) algm = new SleeveFitWithCriterion(new PointPercentCriterion());
                    else if (isBend) algm = new SleeveFitWithCriterion(new BendNumberCriterion());
                    else algm = new SleeveFitAlgm();
                    break;
                case "WangMullerAlgm":
                    if (isPercent) algm = new WangMullerAlgmWithPercent();
                    else if (isBend) algm = new WangMullerWithCriterion(new BendNumberCriterion());
                    else algm = new WangMullerAlgm();
                    break;
            }
            return algm;

        }
    }
}
