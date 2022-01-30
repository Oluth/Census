using Census.Service;
using Census.Service.Debug;
using Census.Util.Demography;
using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Census.UI
{
    internal class CUIPopPyramid : CUIAbstractWindow
    {
        public CUIPopPyramid() : base("Population pyramid") { }

        protected override void Build()
        {
            UILabel text = AddUIComponent(typeof(UILabel)) as UILabel;
            StringBuilder sb = new StringBuilder();
            foreach(int i in DemographyUtil.GetAgeBreakdown(DemographyUtil.BreakdownMode.Inhabitant))
            {
                sb.Append(i.ToString() + ",");
            }
            text.text = sb.ToString();
            DebugService.Log(DebugState.warning, "Width: " + width);
            text.width = this.width - MARGIN_BACKUP;
            text.wordWrap = true;
        }
    }
}
