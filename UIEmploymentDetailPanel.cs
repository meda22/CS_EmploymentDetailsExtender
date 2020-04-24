using ColossalFramework;
using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CS_EmploymentDetailsExtender
{
    public class UIEmploymentDetailPanel : UIPanel
    {
        public static readonly string MOD_VERSION = "1.2";

        private UITitleContainer _title;

        private UILabel[] m_EmploymentLabels = new UILabel[4];
        private UILabel[] m_EmploymentValues = new UILabel[4];
        private UILabel[] m_WorkplaceValues  = new UILabel[4];

        private UILabel m_TableHeader;

        //private UISlider[] m_EmploymentSlider = new UISlider[4];

        private UIChartPanel m_RadialPanel;

        private static readonly int pad = 15;
        private static readonly int sep = 15;
        private static readonly int sliderHeight = 0;
        private static readonly int valueAnchor = 140;
        private static readonly int valueAnchor1 = 260;

        public override void Start()
        {
            base.Start();

            relativePosition = new Vector3(438, 58);
            backgroundSprite = "MenuPanel2";
            isVisible = false;
            canFocus = true;
            isInteractive = true;

            m_TableHeader = AddUIComponent<UILabel>();
            m_TableHeader.name = "TableHeaderLabel";
            m_TableHeader.relativePosition = new Vector3(pad + 130, 50);
            m_TableHeader.textColor = new Color32(206, 248, 0, 255);
            m_TableHeader.textScale = 1f;
            m_TableHeader.text = "Employees      Workplaces";

            for (int i = 0; i < m_EmploymentLabels.Length; i++)
            {
                m_EmploymentLabels[i] = CreateEmploymentLabel(i);
                m_EmploymentValues[i] = CreateEmploymentValue(i);
                //m_EmploymentSlider[i] = CreateEmploymentSlider(i);
                m_WorkplaceValues[i] = CreateWorkplaceValue(i);
            }

            m_RadialPanel = AddUIComponent<UIChartPanel>();
            m_RadialPanel.name = "ChartPanel";
            m_RadialPanel.relativePosition = new Vector3(pad, m_EmploymentValues[m_EmploymentValues.Length - 1].relativePosition.y + sep);

            width = UIChartPanel.panelSize.x + pad * 2 + 100;
            height = m_RadialPanel.relativePosition.y + UIChartPanel.panelSize.y + pad + 50;

            _title = AddUIComponent<UITitleContainer>();
            _title.name = "EmploymentTitlePanel";
            _title.IconSprite = "InfoIconPopulationPressed";
            _title.TitleText = "Employment & Workplaces";
        }

        private UILabel CreateEmploymentLabel(int level) {
            UILabel label = AddUIComponent<UILabel>();
            label.name = "Level" + level + "Label";
            label.relativePosition = new Vector3(pad, 70 + level * (sep + sliderHeight));
            label.textColor = new Color32(206, 248, 0, 255);
            label.textScale = 0.8f;

            return label;
        }

        private UILabel CreateEmploymentValue(int level)
        {
            UILabel label = AddUIComponent<UILabel>();
            label.name = "Level" + level + "Value";
            label.relativePosition = new Vector3(valueAnchor, 70 + level * (sep + sliderHeight));
            label.textColor = JobsUtils.educationLevelColors[level];
            label.textScale = 0.8f;

            return label;
        }

        private UILabel CreateWorkplaceValue(int level)
        {
            UILabel label = AddUIComponent<UILabel>();
            label.name = "Level" + level + "WorkplaceValue";
            label.relativePosition = new Vector3(valueAnchor1, 70 + level * (sep + sliderHeight));
            label.textColor = JobsUtils.educationLevelColors[level];
            label.textScale = 0.8f;

            return label;
        }

        /*
        private UISlider CreateEmploymentSlider(int level)
        {
            UISlider slider = AddUIComponent<UISlider>();
            slider.name = "Level" + level + "Slider";
            //slider.size = new Vector2(x, y);
            slider.relativePosition = new Vector3(valueAnchor, 50 + level * sep);
            slider.minValue = 0f;

            UITextureSprite uITextureSprite3 = base.Find<UITextureSprite>("LandfillGradient");
            uITextureSprite3.renderMaterial.SetColor("_ColorA", Singleton<InfoManager>.instance.m_properties.m_modeProperties[16].m_targetColor);
			uITextureSprite3.renderMaterial.SetColor("_ColorB", Singleton<InfoManager>.instance.m_properties.m_modeProperties[16].m_negativeColor);

            UISprite thumb = base.Find<UISprite>("Thumb");
            slider.thumbObject = thumb;

            slider.AttachUIComponent(thumb.gameObject);
            slider.AttachUIComponent(uITextureSprite3.gameObject);

            return slider;
        }
         * */

        public override void Update()
        {
            if (isVisible && isEnabled)
            {
                BuildingsInfoManager.calculateAllWorkplaces();
                for (int i = 0; i < m_EmploymentLabels.Length; i++)
                {
                    m_EmploymentLabels[i].text = JobsUtils.educationLevelNames[i];
                    m_EmploymentValues[i].text = ": " + JobsUtils.GetEmploymentLabel(i);
                    m_WorkplaceValues[i].text = BuildingsInfoManager.GetWorkplacesLabel(i);
                    /*
                    switch (i)
                    {
                        case 0: m_EmploymentValues[i].text += " / " + BuildingsInfoManager.WorkplacesUneducated; break;
                        case 1: m_EmploymentValues[i].text += " / " + BuildingsInfoManager.WorkplacesEducated; break;
                        case 2: m_EmploymentValues[i].text += " / " + BuildingsInfoManager.WorkplacesWellEducated; break;
                        default: m_EmploymentValues[i].text += " / " + BuildingsInfoManager.WorkplacesHighlyEducated; break;

                    }*/

                    /*
                    m_EmploymentSlider[i].maxValue = (float)JobsUtils.GetEmploymentMaxValue(i);
                    m_EmploymentSlider[i].value = (float)JobsUtils.GetEmploymentCurrentValue(i);
                    */
                }
            }
        }

        public void InfoPanelOnEventVisibilityChanged(UIComponent component, bool visibility)
        {
            if (!visibility && !_title.Locked)
            {
                isVisible = false;
            }
        }
    }
}

