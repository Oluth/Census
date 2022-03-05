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
using System.Threading.Tasks;

namespace Census.UI
{
    class CUIPopPyramid : CUIAbstractWindow
    {

        public static CUIPopPyramid Instance { get; private set; }

        private List<UIComponent> makeshiftTextLabels;
        private int makeshiftMaxAge = 80;
        private int makeshiftGranularity = 45;
        private int makeshiftMaxCharLength = 80;
        private char makeshiftChar = '█';

        private UIScrollablePanel tableFrame;
        public CUIPopPyramid() { 
        
        }

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
            Instance = this;
        }

        private void OnPropertyChanged(UIComponent uc, UIMouseEventParameter e)
        {
            DebugService.Log(DebugState.warning, e.ToString());
        }

        public void PrintMakeshiftPopGraph()
        {
            int[] ages = DemographyUtil.GetAgeBreakdown(DemographyUtil.AgeBreakdownMode.Inhabitant_All);
            int[] quantities = new int[this.makeshiftGranularity];
            double step = (double) this.makeshiftMaxAge / (double) this.makeshiftGranularity;

            DebugService.Log(DebugState.warning, "Got ages.");
            if (this.makeshiftTextLabels == null)
            {
                this.makeshiftTextLabels = new List<UIComponent>();
                for (int i = 0; i < makeshiftGranularity; i++)
                {
                    UILabel text = tableFrame.AddUIComponent<UILabel>();
                    text.name = GenerateComponentName("makeShiftLabel." + i);
                    text.textScale = 0.3f;
                    text.text = i.ToString();

                    uint avgAge = (uint) (this.makeshiftMaxAge - i * step);
                    
                    text.textColor = InternalCitizenManager.GetColorByAge(avgAge);
                    makeshiftTextLabels.Add(text);

                }
            }

            if(ages.Length < this.makeshiftMaxAge)
                {
                    this.makeshiftMaxAge = ages.Length;
                }

            for (int i = 0; i < this.makeshiftGranularity; i++)
                {
                for(int j = (int) Math.Ceiling(step * i) ; j < Math.Ceiling(step * (i+1)); j++)
                {
                    quantities[i] += ages[j];
                }
            }

            int maxQuantity = DemographyUtil.GetMaxQuantity(quantities);
            DebugService.Log(DebugState.warning, "MaxQuantity: " + maxQuantity);


            int k = makeshiftTextLabels.Count - 1;
            foreach (UILabel text in makeshiftTextLabels)
            {
                StringBuilder sb = new StringBuilder();
                DebugService.Log(DebugState.warning, quantities[k].ToString());

                if (quantities[k] > 0) { 
                double lengthQuotient = (double) maxQuantity / (double) quantities[k];
                    DebugService.Log(DebugState.warning, "LengthQuotient: " + lengthQuotient);
                    DebugService.Log(DebugState.warning, "Div of MaxChar: " + (int)Math.Ceiling(makeshiftMaxCharLength / lengthQuotient));

                    for (int j = 0; j < (int)Math.Ceiling(makeshiftMaxCharLength / (double)lengthQuotient); j++)
                    {
                        sb.Append(makeshiftChar);
                    }

                } else
                {
                    sb.Append(' ');
                }

                sb.Append(" - " + Math.Ceiling((double) k * step));
                text.text = sb.ToString();
                k--;
            }

            
        }

        protected void DumpCSV(UIComponent component, UIMouseEventParameter mouse)
        {
            DemographyUtil.PrintCSVAgeBreakdown();
        }
    }
}
