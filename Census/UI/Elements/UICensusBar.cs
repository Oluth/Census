using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Census.Service;
using Census.Service.Debug;
using Census.Util.Demography;
using ColossalFramework.UI;
namespace Census.UI.Elements
{

    class UICensusBar : UISprite
    {
        public enum TextDisplayMode
        {
            ReferenceAge,
            Percentage,
            Quantity
        }

        public DemographyUtil.BreakdownMode populationMode;


        private int maximumValue;
        private float maximumWidth;

        /// <summary>
        /// The first age index at which this bar begins.
        /// </summary>
        public int Index
        {
            get;
            set;
        }

        /// <summary>
        /// How many ages it is including.
        /// </summary>
        public int Span
        {
            get;
            set;
        }

        /// <summary>
        /// This number serves as a relative measure for the bar's width.
        /// </summary>
        public int MaximumValue
        {
            get { return maximumValue; }
            set
            {
                maximumValue = value;
            }
        }

        public float MaximumWidth
        {
            get { return maximumWidth; }
            set
            {
                maximumWidth = value;
                UpdateValue();
            }
        }

        /// <summary>
        /// For AddUIComponent only. Setting the index of the bar is necessary afterwards!
        /// </summary>
        public UICensusBar() : this(0)
        {

        }

        public UICensusBar(int index) : base()
        {
            this.Index = index;
            spriteName = "InfoDisplayFocused";
        }

        public float Value
        {
            get
            {
                return this.width;
            }
            set
            {
                if (Value > MaximumValue)
                {
                    throw new ArgumentOutOfRangeException("New maximum value is smaller than actual value!");
                }
                this.width =  value / MaximumValue * MaximumWidth;
            }
        }

        /// <summary>
        /// Auxiliary function in case of measurement changes.
        /// </summary>
        private void UpdateValue()
        {
            Value = Value;
        }

        public void OnPopulationChanged(object sender, PopChangeEventArgs args)
        {

            // Irrelevant event for this bar.
            if(args.mode != populationMode)
            {
                return;
            }

            DebugService.Log(DebugState.fine, "Population change event in " + this.ToString() + ".");
            int quantity = 0;
            for(int i = Index; i < Index + Span; i++)
            {
                quantity += args.NewValues[i];
            }
            DebugService.Log(DebugState.finest, "Quantity: " + quantity);
            if (quantity > 0)
            {
                int maxVal = DemographyUtil.GetMaxQuantity(args.NewValues, Span);
                DebugService.Log(DebugState.finest, "MaximalValue fetched: " + maxVal);

                MaximumValue = maxVal;
                DebugService.Log(DebugState.finest, "MaximalValue set.");

                Value = quantity;
                DebugService.Log(DebugState.finest, "Value set.");
                Show();
            }
            else
            {
                Hide();
            }
        }

        public override string ToString()
        {
            return "[UICensus] Index: " + Index + ", Mode: " + populationMode;
        }
    }
}
