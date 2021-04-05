using System;
using ColossalFramework.UI;
using UnityEngine;

namespace DemographicsMod
{
    /**
     * Title Bar source code from Skylines-ExtendedPublicTransport
     * https://github.com/justacid/Skylines-ExtendedPublicTransport/tree/master/ExtendedPublicTransportUI
     **/
    public class UITitleContainer : UIPanel
    {
        private UISprite _icon;
        private UILabel _title;
        private UIButton _close;
        private UIButton _lock;
        private UIDragHandle _drag;

        public string IconSprite { get; set; }

        public string TitleText
        {
            get { return _title.text; }
            set { _title.text = value; }
        }

        public float Height 
        { 
            get { return height; } 
            set { height = value; } 
        }

        public float Width
        {
            get { return width; }
            set { width = value; }
        }

        public bool Locked { get; set; }

        public override void Awake()
        {
            base.Awake();

            _icon = AddUIComponent<UISprite>();
            _title = AddUIComponent<UILabel>();
            _close = AddUIComponent<UIButton>();
            _lock = AddUIComponent<UIButton>();
            _drag = AddUIComponent<UIDragHandle>();

            height = 40;
            width = 450;
            TitleText = "(None)";
            IconSprite = "";
            Locked = false;
        }

        public override void Start()
        {
            base.Start();

            if (parent == null)
            {
                Debug.Log(string.Format("Parent not set in {0}", this.GetType().Name));
                return;
            }

            width = parent.width;
            relativePosition = Vector3.zero;
            isVisible = true;
            canFocus = true;
            isInteractive = true;

            _drag.width = width - 80;
            _drag.height = height;
            _drag.relativePosition = Vector3.zero;
            _drag.target = parent;

            _icon.spriteName = IconSprite;
            _icon.relativePosition = new Vector3(5, 0);

            _title.relativePosition = new Vector3(50, 13);
            _title.text = TitleText;

            _lock.relativePosition = new Vector3(width - 70, 2);
            _lock.normalBgSprite = "LocationMarkerNormal";
            _lock.hoveredBgSprite = "LocationMarkerHovered";
            _lock.pressedBgSprite = "LocationMarkerPressed";
            _lock.eventClick += delegate(UIComponent component, UIMouseEventParameter param)
            {
                LockButtonOnEventClick(component, param);
            };

            _close.relativePosition = new Vector3(width - 35, 2);
            _close.normalBgSprite = "buttonclose";
            _close.hoveredBgSprite = "buttonclosehover";
            _close.pressedBgSprite = "buttonclosepressed";
            _close.eventClick += (component, param) => parent.Hide();
        }

        private void LockButtonOnEventClick(UIComponent component, UIMouseEventParameter eventParam)
        {
            Locked = !Locked;

            if (Locked)
            {
                _lock.normalBgSprite = "LocationMarkerActiveNormal";
                _lock.hoveredBgSprite = "LocationMarkerActiveHovered";
                _lock.pressedBgSprite = "LocationMarkerActivePressed";
            }
            else
            {
                _lock.normalBgSprite = "LocationMarkerNormal";
                _lock.hoveredBgSprite = "LocationMarkerHovered";
                _lock.pressedBgSprite = "LocationMarkerPressed";
            }
        }
    }
}
