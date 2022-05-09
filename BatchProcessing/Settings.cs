using System;

namespace BatchProcessing
{
    [Serializable]
    public class Settings
    {
        public int IterationQuantity { get; set; }
        public double[] ReducedValues;
        public double ErrorPercentValue { get; set; }
        public ReducedMode Mode { get; set; }
        public AlgmSettings DouglsSettings { get; set; }
        public AlgmSettings SleeveFitSettings { get; set; }
        public AlgmSettings VisWSettings { get; set; }
        public AlgmSettings LiSettings { get; set; }
         public AlgmSettings FourierSettings {get; set;}
        public Settings()
        { }
    }

    [Serializable]
    public class AlgmSettings
    {
        public int [] Params { get; set; }

        public AlgmSettings()
        {}
    }

    [Serializable]
    public enum ReducedMode
    {
        Points, Bends, Ghdistance
    }

}
