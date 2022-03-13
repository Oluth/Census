using Census.Service;
using Census.Service.Debug;
using ColossalFramework.UI;
using UnityEngine;

namespace Census.Manager
{

    /// <summary>
    /// Business logic partly adapted from <a href="https://github.com/algernon-A/Lifecycle-Rebalance-Revisited/blob/master/Code/Settings/OptionsPanel.cs">Lifecycle Rebalance Revisited</a> by <a href="https://github.com/algernon-A">algernon-A</a>.
    /// </summary>
    internal class InternalOptionManager
    {
        private GameObject censusOptionBase;
        private UIPanel gameOptionPanel;
        internal UIScrollablePanel gameOptionMenu;
        private bool isInitialized = false;
        /// <summary>
        /// Singleton instance.
        /// </summary>
        protected static InternalOptionManager instance;
        public static InternalOptionManager Instance
        {
            get
            {
                if (instance == null)
                {
                    DebugService.Log(DebugState.fine, "Create UIManager instance.");
                    new InternalOptionManager();
                }

                DebugService.Log(DebugState.fine, "Return instance of UIManager.");
                return instance;
            }
        }

        private InternalOptionManager()
        {
            instance = this;
        }

        /// <summary>
        /// Builds the option entry of Census if it's initially called, else updates it.
        /// </summary>
        public void BuildOptionPanel(UIHelper uiHelper)
        {
            DebugService.Log(DebugState.fine, "Build option panel.");
            gameOptionMenu = uiHelper.self as UIScrollablePanel;
            DebugService.Log(DebugState.fine, (gameOptionMenu == null).ToString());

            if (!isInitialized)
            {
                DebugService.Log(DebugState.fine, "Load option panel from Cities: Skylines from Panel library.");
                gameOptionPanel = UIView.library.Get<UIPanel>("OptionsPanel");
                gameOptionPanel.transform.parent = gameOptionMenu.transform;
                gameOptionPanel.eventVisibilityChanged += (component, isVisible) =>
                {
                    if (isVisible)
                    {
                        BuildOptionPanel(null);
                    }
                    else
                    {
                        DestroyOptionPanel();
                    }
                };

                isInitialized = true;
            }

            if (censusOptionBase == null)
            {
                DebugService.Log(DebugState.fine, "Create base GameObject for Census option panel components.");
                censusOptionBase = new GameObject("CensusOptionPanel");
            }

            UIButton aButton = gameOptionPanel.AddUIComponent<UIButton>();
        }

        public void DestroyOptionPanel()
        {
            if(censusOptionBase != null)
            {
                GameObject.Destroy(censusOptionBase);
                censusOptionBase = null;
            }
        }


    }
}
