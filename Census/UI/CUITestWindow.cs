using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Census.UI
{
    internal class CUITestWindow : CUIAbstractWindow
    {
        public CUITestWindow() : base("TestWindow") {
            description = "A test window for internal GUI examination.";
        }


        protected override void Build()
        {
            throw new NotImplementedException();
        }
    }
}
