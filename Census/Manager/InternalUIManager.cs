using Census.Service;
using Census.Service.Debug;
using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Census.Manager
{
    internal class InternalUIManager : UICustomControl
    {
        private static InternalUIManager instance;

        private InternalUIManager() { }

        public static InternalUIManager Instance { 
            get { 
                if(instance == null)
                {
                    instance = new InternalUIManager();
                }
                return instance; 
            } 
        }

        private UIView view = UIView.GetAView();

        public void OpenTestWindow()
        {
            UIPanel panel = view.AddUIComponent(typeof(UIPanel)) as UIPanel;
            panel.backgroundSprite = "GenericPanel";
            panel.Show();
            //panel.OnEnable();
            DebugService.Log(DebugState.info, "Window has been loaded.");
        }
    }
}
