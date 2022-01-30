using Census.Manager;
using Census.Service;
using Census.Service.Debug;
using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Census.UI
{

    /// <summary>
    /// Base class for any Census window, according to a variation of the Decorator pattern on UIComponent.
    /// </summary>
    internal abstract class CUIAbstractWindow : UIPanel
    {

        public string Title { get; }

        protected string description;
        public string Description => description;

        protected const string BACKGROUND_SPRITE_NAME = "GenericPanel";
        protected const int MARGIN_BACKUP = 15;
        protected const int DEFAULT_WIDTH = 500;
        protected const int DEFAULT_HEIGHT = 500;

        public CUIAbstractWindow(string title) : base()
        {
            this.backgroundSprite = BACKGROUND_SPRITE_NAME;

            this.width = DEFAULT_WIDTH;
            this.height = DEFAULT_HEIGHT;

            this.autoLayout = true;

            UILabel titleLabel = AddUIComponent(typeof(UILabel)) as UILabel;
            titleLabel.width = this.width - MARGIN_BACKUP;
            titleLabel.wordWrap = true;
            Title = title;

            titleLabel.name = string.Format("census_window_title:{0}", title);
            titleLabel.text = title;

            eventClicked += InternalUIManager.Instance.Erase;
            isInteractive = true;

            Build();
            DebugService.Log(DebugState.info, string.Format("Window {0} has been loaded.", name));
        }

        protected abstract void Build();

        protected void Destroy()
        {
            InternalUIManager.Instance.Delete(this);
        }
    }
}
