using Census.Util.Demography;
using System;
namespace Census.UI.Elements
{

    /// <summary>
    /// Sent as an event in case of a population change.
    /// </summary>
    class PopChangeEventArgs : EventArgs
    {
        public DemographyUtil.BreakdownMode mode;

        public int[] OldValues
        {
            get;
            set;
        }
        public int[] NewValues
        {
            get; 
            set;
        }

        public int NewMaximumValue
        {
            get;
            set;
        }

    }
}