using Census.Service;
using Census.Service.Debug;
using Census.Util.Demography;
using Census.Manager;
using ColossalFramework.UI;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Census.UI
{

    /// <summary>
    /// Displays the population pyramid of the mod.
    /// </summary>
    class PopPyramidWindow : UICensusWindow
    {

        private static PopPyramidWindow instance;
        /// <summary>
        /// Singleton instance.
        /// </summary>
        public static PopPyramidWindow Instance { 
            get
            {
                if (instance == null)
                {
                    DebugService.Log(DebugState.finest, "Set new instance reference.");
                    PopPyramidWindow newInstance = new PopPyramidWindow();
                    instance = newInstance;
                }
                DebugService.Log(DebugState.finest, "Return instance of PopPyramidWindow.");
                return instance;
            }
        }

        public PopPyramidWindow() : base()
        {
            DebugService.Log(DebugState.finest, "Create new PopPyramidWindow.");
        }

        public override void UpdateInstance(UICensusWindow newWindow)
        {
            DebugService.Log(DebugState.finest, "Update Instance...");
            if (newWindow == null)
            {
                ExceptionService.Instance.ThrowException(ExceptionService.Type.Severe, new ArgumentNullException("No windowinstance!"));
            } else if (newWindow.GetType() != typeof(PopPyramidWindow)) {
                ExceptionService.Instance.ThrowException(ExceptionService.Type.Severe, new ArgumentException("No PopPyramid instance!"));
            }
            instance = newWindow as PopPyramidWindow;
        }

        private List<UISprite> makeshiftBars;
        private int makeshiftMaxAge = 100;
        private int makeshiftGranularity = 25;
        private int makeshiftMaxCharLength = 170;
        private float makeshiftBarHeight = 1.5f;

        private UIScrollablePanel tableFrame;

        protected override void Build()
        {
            //this.autoLayout = true;
            //this.autoLayoutDirection = LayoutDirection.Vertical;
            this.Title = "Population Diagram";
            DebugService.Log(DebugState.info, "CUIPopPyramid: Add subBox.");
            UIButton dumpCSV = this.AddUIComponent<UIButton>();
            dumpCSV.text = "Dump .csv";
            dumpCSV.eventClicked += DumpCSV;
            dumpCSV.horizontalAlignment = UIHorizontalAlignment.Right;
            dumpCSV.normalFgSprite = "ButtonMenu";
            dumpCSV.hoveredFgSprite = "ButtonMenuHovered";
            dumpCSV.pressedFgSprite = "ButtonMenuPressed";

            UIPanel subBox = this.AddUIComponent<UIPanel>();
            subBox.name = GenerateComponentName("subBox");
            subBox.autoLayout = true;
            subBox.autoLayoutDirection = LayoutDirection.Horizontal;
            subBox.padding.top = PADDING_BACKUP;

            DebugService.Log(DebugState.info, "CUIPopPyramid: Add horizontalBar.");
            UIScrollbar scrollbar = subBox.AddUIComponent<UIScrollbar>();
            scrollbar.name = GenerateComponentName("scrollBar");
            scrollbar.orientation = UIOrientation.Horizontal;


            UISlicedSprite track = scrollbar.AddUIComponent<UISlicedSprite>();
            scrollbar.trackObject = track;
            track.width = 32;
            track.height = subBox.height;
            track.name = GenerateComponentName("scrollBar.track");
            track.spriteName = "ScrollbarTrack";
            track.fillDirection = UIFillDirection.Horizontal;
            
            UISlicedSprite thumb = track.AddUIComponent<UISlicedSprite>();
            thumb.name = GenerateComponentName("scrollBar.thumb");
            thumb.spriteName = "ScrollbarThumb";
            thumb.fillDirection = UIFillDirection.Horizontal;
            thumb.height = 300;
            thumb.width = track.width;
            scrollbar.thumbObject = thumb;

            DebugService.Log(DebugState.info, "CUIPopPyramid: Add tableFrame.");
            tableFrame = subBox.AddUIComponent<UIScrollablePanel>();
            tableFrame.name = GenerateComponentName("numberFrame");
            tableFrame.verticalScrollbar = scrollbar;
            tableFrame.height = this.height - PADDING_BACKUP * 2;
            tableFrame.width = this.width - PADDING_BACKUP * 2;
            tableFrame.relativePosition = new UnityEngine.Vector3(PADDING_BACKUP, PADDING_BACKUP, 0);
            tableFrame.scrollWheelAmount = 50;
            tableFrame.scrollWheelDirection = UIOrientation.Vertical;
            tableFrame.freeScroll = true;
            tableFrame.autoLayout = true;
            tableFrame.autoLayoutDirection = LayoutDirection.Vertical;
            tableFrame.autoLayoutPadding = new RectOffset(70,70,-5,0);

            PrintMakeshiftPopGraph();
            //UILabel text = AddUIComponent(typeof(UILabel)) as UILabel;


            //foreach(int i in DemographyUtil.GetAgeBreakdown(DemographyUtil.BreakdownMode.Inhabitant))
            //{
            //    sb.Append(i.ToString() + ",");
            //}
            //text.text = sb.ToString();
            //DebugService.Log(DebugState.warning, "Width: " + width);
            //text.width = this.width - MARGIN_BACKUP;
            //text.wordWrap = true;
        }


        public void PrintMakeshiftPopGraph()
        {
            DebugService.Log(DebugState.fine, "Get array of Citizen ages.");
            int[] ages = DemographyUtil.GetAgeBreakdown(DemographyUtil.AgeBreakdownMode.Inhabitant_All);
            

            int[] quantities = new int[this.makeshiftGranularity];
            double step = (double) this.makeshiftMaxAge / (double) this.makeshiftGranularity;

            // Initial initialization of bars after level loading.
            if (this.makeshiftBars == null)
            {
                this.makeshiftBars = new List<UISprite>();
                for (int i = 0; i < makeshiftGranularity; i++)
                {
                    UISprite bar = tableFrame.AddUIComponent<UISprite>();
                    bar.name = GenerateComponentName("makeShiftBar." + i);
                    bar.size = new Vector2(this.makeshiftBarHeight, 0f);
                    bar.spriteName = "InfoDisplayFocused";
                    
                    uint avgAge = (uint) Math.Round(this.makeshiftMaxAge - i * step);


                    UILabel text = bar.AddUIComponent<UILabel>();
                    text.name = bar.name + ".age";
                    text.text = avgAge.ToString();
                    bar.FitChildrenHorizontally();

                    bar.color = InternalCitizenManager.Instance.GetColorByAge(avgAge);
                    this.makeshiftBars.Add(bar);
                }
                DebugService.Log(Service.Debug.DebugState.fine, "Constructed bars.");
            }

            // Adapt width to maximum age.
            if(ages.Length < this.makeshiftMaxAge)
                {
                    this.makeshiftMaxAge = ages.Length;
                }

            DebugService.Log(Service.Debug.DebugState.fine, "Calculate cohort size.");
            for (int i = 0; i < this.makeshiftGranularity; i++)
                {
                for(int j = (int) Math.Ceiling(step * i) ; j < Math.Ceiling(step * (i+1)); j++)
                {
                    quantities[i] += ages[j];
                }
            }

            int maxQuantity = DemographyUtil.GetMaxQuantity(quantities);

            DebugService.Log(Service.Debug.DebugState.fine, "Update bar width.");
            int k = makeshiftBars.Count - 1;
            foreach (UISprite bar in makeshiftBars)
            {
                //DebugService.Log(DebugState.warning, quantities[k].ToString());

                if (quantities[k] > 0) {

                    bar.Show();
                    double lengthQuotient = (double) maxQuantity / (double) quantities[k];
                    //DebugService.Log(DebugState.warning, "LengthQuotient: " + lengthQuotient);
                    //DebugService.Log(DebugState.warning, "Div of MaxChar: " + (int)Math.Ceiling(makeshiftMaxCharLength / lengthQuotient));

                    bar.width = (float) makeshiftMaxCharLength / (float) lengthQuotient;


                } else
                {
                    bar.Hide();
                }
                k--;
            }

            DebugService.Log(Service.Debug.DebugState.info, "Updated cohort size of bars.");

        }

        protected void DumpCSV(UIComponent component, UIMouseEventParameter mouse)
        {
            DemographyUtil.PrintCSVAgeBreakdown();
            DebugService.Log(Service.Debug.DebugState.info, "Dumped!");
        }
    }
}
