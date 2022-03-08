using Census.Service;
using Census.Service.Debug;
using Census.UI;
using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Census.Manager
{

    /// <summary>
    /// Manages the communication between Census and the in-game UI elements. 
    /// <br/>
    /// <br/>
    /// It is preferred to be used instead of in-game managers due to SoC. 
    /// </summary>
    class InternalUIManager : UICustomControl
    {

        /// <summary>
        /// Singleton instance.
        /// </summary>
        protected static InternalUIManager instance;
        public static InternalUIManager Instance { 
            get { 
                if(instance == null)
                {
                    DebugService.Log(DebugState.fine, "Create UIManager instance.");
                    new InternalUIManager();
                }

                DebugService.Log(DebugState.fine, "Return instance of UIManager.");
                return instance; 
            } 
        }

        private InternalUIManager() {
            instance = this;
        }

        /// <summary>
        /// The scene in which the gameplay takes place.
        /// </summary>
        private UIView gameView = UIView.GetAView();

        /// <summary>
        /// Adds the instance of the window type to the game view.
        /// </summary>
        /// <param name="windowClassName">String of the name of the window class.</param>
        /// <returns>Window instance.</returns>
        public UICensusWindow AddWindowToGameView(Type windowType)
        {

            DebugService.Log(DebugState.fine, "Start initializing window with type " + windowType + ".");
            if (windowType == null)
            {
                ExceptionService.Instance.ThrowException(ExceptionService.Type.Severe, new ArgumentNullException("No window class!"));
            }
            else if(windowType.IsInstanceOfType(typeof(UICensusWindow)))
            {
                ExceptionService.Instance.ThrowException(ExceptionService.Type.Severe, new ArgumentException("Invalid window class!"));
            }
            DebugService.Log(DebugState.info, "UICensusWindow " + windowType + " is valid, try to add it...");

            UICensusWindow window = gameView.AddUIComponent(windowType) as UICensusWindow;
            window.UpdateInstance(window);
            return window;
        }

        public void Erase(UIComponent comp, UIMouseEventParameter e)
        {
            Delete(comp);
        }

        public void EraseParent(UIComponent comp, UIMouseEventParameter e)
        {
            Delete(comp.parent);
        }

        public void Delete(UIComponent w)
        {
            foreach (UIComponent wu in w.components) {
                Delete(wu);
            }
            UnityEngine.Object.Destroy(w);
        }
       
    }
}
