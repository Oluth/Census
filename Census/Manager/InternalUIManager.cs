using Census.Service;
using Census.Service.Debug;
using Census.UI;
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
        protected static InternalUIManager instance;

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

        public CUIAbstractWindow AddWindow(Type windowType)
        {
            return view.AddUIComponent(windowType) as CUIAbstractWindow;
        }

        public void OpenTestWindow()
        {
            this.AddWindow(typeof(CUIPopPyramid));
        }

        public void Erase(UIComponent comp, UIMouseEventParameter e)
        {
            DebugService.Log(DebugState.error, e.position.x + ", " + e.position.y);
            Delete(comp);
        }

        public void Delete(UIComponent w)
        {
            foreach (UIComponent wu in w.components) {
                DebugService.Log(DebugState.error, "Child: " + wu.ToString());
                Delete(wu);
            }
            DebugService.Log(DebugState.error, "Parent: " + w.ToString());
            UnityEngine.GameObject.Destroy(w);
        }
        
    }
}
