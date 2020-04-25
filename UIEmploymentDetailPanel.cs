using ColossalFramework;
using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DemographicsMod
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
        private UIChartPanel m_RadialPanel1;

        private static readonly int pad = 15;
        private static readonly int sep = 15;
        private static readonly int sliderHeight = 0;
        private static readonly int valueAnchor = 140;
        private static readonly int valueAnchor1 = 290;

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
            m_TableHeader.text = "Employees            Utilization";

            for (int i = 0; i < m_EmploymentLabels.Length; i++)
            {
                m_EmploymentLabels[i] = CreateEmploymentLabel(i);
                m_EmploymentValues[i] = CreateEmploymentValue(i);
                //m_EmploymentSlider[i] = CreateEmploymentSlider(i);
                m_WorkplaceValues[i] = CreateWorkplaceValue(i);
            }

            var rpos = m_EmploymentValues[m_EmploymentValues.Length - 1].relativePosition.y + sep + 30;

            m_RadialPanel = AddUIComponent<UIChartPanel>();
            m_RadialPanel.name = "ChartPanel";
            m_RadialPanel.relativePosition = new Vector3(pad, rpos);

            m_RadialPanel1 = AddUIComponent<UIChartPanel>();
            m_RadialPanel1.RadialChartPrefix = "workplaces";
            m_RadialPanel1.name = "WorkplacesChartPanel";

            m_RadialPanel1.relativePosition = new Vector3(pad * 4 + UIChartPanel.panelSize.x, rpos);

            var emp = UIView.library.Get<PopulationInfoViewPanel>(typeof(PopulationInfoViewPanel).Name);

            width = 450; //UIChartPanel.panelSize.x + pad * 2 + 100;
            height = emp.component.height;//m_RadialPanel.relativePosition.y + UIChartPanel.panelSize.y + pad + 50;

            _title = AddUIComponent<UITitleContainer>();
            _title.name = "EmploymentTitlePanel";
            _title.IconSprite = "InfoIconPopulationPressed";
            _title.TitleText = "Employment & Workplaces";

            tooltip = "Employed workers by education level and total workers compared to workplaces";
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

