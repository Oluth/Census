using Census.Manager;
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
    internal abstract class CUIAbstractWindow : UIPanel
    {

        public string Title { get; }

        protected string description;
        public string Description => description;

        protected const string BACKGROUND_SPRITE_NAME = "GenericPanel";
        protected const int PADDING_BACKUP = 15;
        protected const int DEFAULT_WIDTH = 500;
        protected const int DEFAULT_HEIGHT = 500;

        private UIDragDropState dragDropState = UIDragDropState.None;
        private Vector3 dragPos = Vector3.zero;

        public override void Start()
        {
            Hide();
            CreateExitButton(this);
            this.backgroundSprite = BACKGROUND_SPRITE_NAME;

            this.width = DEFAULT_WIDTH;
            this.height = DEFAULT_HEIGHT;

            this.autoLayout = true;
            this.wrapLayout = true;
            this.clipChildren = true;


            UILabel titleLabel = AddUIComponent(typeof(UILabel)) as UILabel;
            titleLabel.textAlignment = UIHorizontalAlignment.Center;

            titleLabel.padding.top = PADDING_BACKUP;
            titleLabel.padding.bottom = PADDING_BACKUP;
            
            titleLabel.anchor = (int) UIAnchorStyle.CenterHorizontal + UIAnchorStyle.Top;

            titleLabel.name = string.Format("census_window_title:{0}", Title);
            titleLabel.text = Title;

            //eventClicked += InternalUIManager.Instance.Erase;
            isInteractive = true;

            Build();
            Show();
            DebugService.Log(DebugState.info, string.Format("Window {0} has been loaded.", name));

        }
        public CUIAbstractWindow(string title) : base()
        {
            Title = title;
        }

        protected override void OnDragEnd(UIDragEventParameter d)
        {
            DebugService.Log(DebugState.error, "DragEnd!");
            OnDragOver(d);
            dragPos = Vector3.zero;
            dragDropState = UIDragDropState.None;
        }

        
        protected override void OnDragOver(UIDragEventParameter d)
        {
            DebugService.Log(DebugState.error, "DragOver!");
            Vector3 newPos = absolutePosition;

            float xDiff = d.position.x - dragPos.x;
            float yDiff = d.position.y - dragPos.y;

            newPos.x = this.absolutePosition.x + xDiff;
            newPos.y = this.absolutePosition.y + yDiff;

            absolutePosition = newPos;
            dragPos = newPos;
        }

        protected override void OnDragStart(UIDragEventParameter d)
        {
            dragDropState = UIDragDropState.Dragging;
            dragPos = d.position;
            DebugService.Log(DebugState.error,"DragStart!");
        }

        protected UIButton CreateExitButton(UIComponent comp)
        {
            UIButton button = comp.AddUIComponent(typeof(UIButton)) as UIButton;
            DebugService.Log(DebugState.warning, button.parent.name);
            button.normalFgSprite = "buttonclose";
            button.hoveredFgSprite = "buttonclosehover";
            button.pressedFgSprite = "buttonclosepressed";
            button.eventClicked += InternalUIManager.Instance.EraseParent;
            button.Show();
            return button;
        }

        protected abstract void Build();

        protected void Destroy()
        {
            InternalUIManager.Instance.Delete(this);
        }
    }
}
