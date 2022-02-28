﻿using Census.Manager;
using Census.Service;
using Census.Service.Debug;
using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Census.UI
{

    /// <summary>
    /// Base class for any Census window, according to a variation of the Decorator pattern on UIComponent.
    /// </summary>
    abstract class CUIAbstractWindow : UIPanel
    {
        private UIDragHandle dragHandle;

        public CUIAbstractWindow() : base()
        {
            Title = "Untitled Window";

            dragHandle = AddUIComponent(typeof(UIDragHandle)) as UIDragHandle;
            dragHandle.size = parent.size;
            dragHandle.target = parent;
        }

        private string title;

        public string Title { get
            {
                return title;
            }

            private set
            {
                title = value;
            }
        }

        protected string description;
        public string Description => description;

        protected const string BACKGROUND_SPRITE_NAME = "GenericPanel";
        protected const int PADDING_BACKUP = 15;
        protected const int DEFAULT_WIDTH = 500;
        protected const int DEFAULT_HEIGHT = 500;

        public override void Start()
        {
            Hide();
            DebugService.Log(DebugState.info, "Begin creating window in CUIAbstractWindow.");
            CreateExitButton(this);
            this.backgroundSprite = BACKGROUND_SPRITE_NAME;

            this.width = DEFAULT_WIDTH;
            this.height = DEFAULT_HEIGHT;

            this.autoLayout = true;
            this.wrapLayout = true;
            this.clipChildren = true;

            DebugService.Log(DebugState.info, "Window measurements set.");

            UILabel titleLabel = AddUIComponent(typeof(UILabel)) as UILabel;
            DebugService.Log(DebugState.info, "UILabel set.");
            titleLabel.textAlignment = UIHorizontalAlignment.Center;

            titleLabel.padding.top = PADDING_BACKUP;
            titleLabel.padding.bottom = PADDING_BACKUP;
            
            titleLabel.anchor = (int) UIAnchorStyle.CenterHorizontal + UIAnchorStyle.Top;


            titleLabel.name = string.Format("census_window_title:{0}", Title);
            titleLabel.text = Title;
            DebugService.Log(DebugState.info, "TitleLabel set.");

            IOService instance = IOService.Instance;

            DebugService.Log(DebugState.info, "IOService set.");
            isInteractive = true;

            Build();
            Show();
            DebugService.Log(DebugState.info, string.Format("Window {0} has been loaded.", name));

        }
        public CUIAbstractWindow(string title) : this()
        {
            if (title != null)
            {
                this.Title = title;
            }
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

        protected abstract void Build();

        protected void Destroy()
        {
            InternalUIManager.Instance.Delete(this);
        }
    }
}