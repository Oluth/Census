using Census.Manager;
using Census.Service;
using Census.Service.Debug;
using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Census.UI
{

    /// <summary>
    /// Base class for any Census window, according to a variation of the Decorator pattern on UIComponent.
    /// <br/><br/>
    /// Windows are supposed to function like singletons. Since UICompoments cannot be attached after creation,
    /// a workaround is necessary: The method <tt>UpdateInstance</tt> deletes the old instance and assigns
    /// the new one to the static reference in the child class.
    /// </summary>
    abstract class UICensusWindow : UIPanel
    {
        private UIDragHandle dragHandle;
        private UILabel titleLabel;


        private int namedComponentCount = 0;

        /// <summary>
        /// Creates a standard component name by the pattern <tt>census.[ClassName].[indexNumber]</tt>.
        /// </summary>
        /// <returns>Standard component name.</returns>
        protected string GenerateComponentName()
        {
            return "Census." + this.name + "." + namedComponentCount.ToString();
        }

        /// <summary>
        /// Creates a standard component name by the pattern <tt>census.[ClassName].[name]</tt>.
        /// </summary>
        /// <param name="name">Custom component name.</param>
        /// <returns>Standard component name.</returns>
        protected string GenerateComponentName(string name)
        {
            return "Census." + this.name + "." + name;
        }

        public UICensusWindow() : base()
        {
            /// Drag handle is attached prior to all other elements above.
            dragHandle = AddUIComponent(typeof(UIDragHandle)) as UIDragHandle;
            dragHandle.size = parent.size;
            dragHandle.target = parent;
        }

        private string title;

        /// <summary>
        /// Displayed window title.
        /// </summary>
        public string Title { get
            {
                return title;
            }

            protected set
            {
                title = value;
                if(titleLabel != null) { 
                    titleLabel.text = Title;
                }
            }
        }


        protected string description;

        /// <summary>
        /// Internal description of the window.
        /// </summary>
        public string Description => description;

        protected const string BACKGROUND_SPRITE_NAME = "GenericPanel";

        /// <summary>
        /// Standard padding backup.
        /// </summary>
        protected const int PADDING_BACKUP = 15;

        protected const int DEFAULT_WIDTH = 500;
        protected const int DEFAULT_HEIGHT = 500;

        /// <summary>
        /// Builds general window features.
        /// </summary>
        public override void Start()
        {
            // Precautiously hide window.
            Hide();

            DebugService.Log(DebugState.info, "Begin creating window in UICensusWindow.");

            CreateExitButton(this);

            this.backgroundSprite = BACKGROUND_SPRITE_NAME;
            this.width = DEFAULT_WIDTH;
            this.height = DEFAULT_HEIGHT;
            this.autoLayout = true;
            this.wrapLayout = true;
            this.clipChildren = true;
            DebugService.Log(DebugState.info, "Window measurements set.");

            

            titleLabel = AddUIComponent(typeof(UILabel)) as UILabel;
            titleLabel.backgroundSprite = "ScrollbarTrack";
            Title = "Untitled Window";
            titleLabel.textAlignment = UIHorizontalAlignment.Center;
            titleLabel.padding.top = PADDING_BACKUP / 2;
            titleLabel.padding.bottom = PADDING_BACKUP / 2;
            titleLabel.padding.left = PADDING_BACKUP / 2;
            titleLabel.padding.right = PADDING_BACKUP / 2;
            titleLabel.anchor = (int) UIAnchorStyle.CenterHorizontal + UIAnchorStyle.Top;
            titleLabel.name = string.Format("census_window_title:{0}", Title);
            DebugService.Log(DebugState.info, "TitleLabel set.");


            isInteractive = true;

            // Build child
            Build();

            // Display window after complete build-up.
            Show();
            DebugService.Log(DebugState.info, string.Format("Window {0} has been loaded.", name));

        }

        protected UIButton CreateExitButton(UIComponent comp)
        {
            UIButton button = comp.AddUIComponent(typeof(UIButton)) as UIButton;
            DebugService.Log(DebugState.warning, "Button parent: " + button.parent.name);
            button.normalFgSprite = "buttonclose";
            button.hoveredFgSprite = "buttonclosehover";
            button.pressedFgSprite = "buttonclosepressed";
            button.eventClicked += InternalUIManager.Instance.EraseParent;
            button.Show();
            DebugService.Log(DebugState.info, "Exit button created.");
            return button;
        }

        /// <summary>
        /// Starting point for all window-specific code.
        /// </summary>
        protected abstract void Build();

        /// <summary>
        /// Updates the singleton of the child class.
        /// </summary>
        public abstract void UpdateInstance(UICensusWindow newWindow);

        protected void Destroy()
        {
            InternalUIManager.Instance.Delete(this);
        }
    }
}
