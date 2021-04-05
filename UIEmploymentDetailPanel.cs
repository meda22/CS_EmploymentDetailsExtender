using System;
using ColossalFramework.UI;
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
        
        private UILabel[] m_WorkspaceDetailLabels = new UILabel[4];
        
        private UILabel[,] m_WorkspacesDetailValues = new UILabel[5, 4];

        private UILabel m_TableHeader;
        
        private UILabel m_BottomTableHeader;

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
            m_TableHeader.relativePosition = new Vector3(pad + 124, 50);
            m_TableHeader.textColor = new Color32(206, 248, 0, 255);
            m_TableHeader.textScale = 1f;
            m_TableHeader.text = "Employment           Workers vs Jobs";
            
            m_BottomTableHeader = AddUIComponent<UILabel>();
            m_BottomTableHeader.name = "BottomTableHeaderLabel";
            m_BottomTableHeader.relativePosition = new Vector3(pad, 330f);
            m_BottomTableHeader.textColor = new Color32(206, 248, 0, byte.MaxValue);
            m_BottomTableHeader.textScale = 1f;
            m_BottomTableHeader.text = "Jobs                      COM      OFF      IND       SRV       TOT";

            for (int i = 0; i < m_EmploymentLabels.Length; i++)
            {
                m_EmploymentLabels[i] = CreateEmploymentLabel(i);
                m_EmploymentValues[i] = CreateEmploymentValue(i);
                m_WorkplaceValues[i] = CreateWorkplaceValue(i);
                m_WorkspaceDetailLabels[i] = CreateWorkspaceDetailLabel(i);
                for (int j = 0; j < 5; j++)
                {
                    m_WorkspacesDetailValues[j, i] = CreateWorkspaceDetailValue(i, j);
                }
            }

            var rpos = m_EmploymentValues[m_EmploymentValues.Length - 1].relativePosition.y + sep + 30;

            m_RadialPanel = AddUIComponent<UIChartPanel>();
            m_RadialPanel.name = "ChartPanel";
            m_RadialPanel.relativePosition = new Vector3(pad + 40, rpos);

            m_RadialPanel1 = AddUIComponent<UIChartPanel>();
            m_RadialPanel1.RadialChartPrefix = "workplaces";
            m_RadialPanel1.name = "WorkplacesChartPanel";
            m_RadialPanel1.relativePosition = new Vector3(pad * 4 + UIChartPanel.panelSize.x + 30, rpos);

            var populationInfoViewPanel = UIView.library.Get<PopulationInfoViewPanel>(typeof(PopulationInfoViewPanel).Name);

            width = 450; 
            height = populationInfoViewPanel.component.height;

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
            label.textColor = JobsUtils.EducationLevelColors[level];
            label.textScale = 0.8f;

            return label;
        }

        private UILabel CreateWorkplaceValue(int level)
        {
            UILabel label = AddUIComponent<UILabel>();
            label.name = "Level" + level + "WorkplaceValue";
            label.relativePosition = new Vector3(valueAnchor1, 70 + level * (sep + sliderHeight));
            label.textColor = JobsUtils.EducationLevelColors[level];
            label.textScale = 0.8f;

            return label;
        }
        
        private UILabel CreateWorkspaceDetailLabel(int level)
        {
            UILabel uilabel = AddUIComponent<UILabel>();
            uilabel.name = "Level" + level + "WorkplaceDetailLabel";
            uilabel.relativePosition = new Vector3(pad, 350 + level * (sep + sliderHeight));
            uilabel.textColor = new Color32(206, 248, 0, byte.MaxValue);
            uilabel.textScale = 0.8f;
            return uilabel;
        }

        private UILabel CreateWorkspaceDetailValue(int level, int service)
        {
            UILabel uilabel = AddUIComponent<UILabel>();
            uilabel.name = string.Concat(new object[]
            {
                "Level",
                level,
                "service",
                service,
                "WorkplaceDetailValue"
            });
            uilabel.relativePosition = new Vector3(valueAnchor + service * 60, 350 + level * (sep + sliderHeight));
            uilabel.textColor = JobsUtils.EducationLevelColors[level];
            uilabel.textScale = 0.8f;
            return uilabel;
        }

        public override void Update()
        {
            if (isVisible && isEnabled)
            {
                for (int i = 0; i < m_EmploymentLabels.Length; i++)
                {
                    m_EmploymentLabels[i].text = JobsUtils.EducationLevelNames[i];
                    m_EmploymentValues[i].text = ": " + JobsUtils.GetEmploymentLabel(i);
                    m_WorkplaceValues[i].text = BuildingsInfoManager.GetWorkplacesLabel(i);
                    m_WorkspaceDetailLabels[i].text = JobsUtils.EducationLevelNames[i];
                    m_WorkspacesDetailValues[0, i].text = BuildingsInfoManager.comWorkplaces[i].ToString();
                    m_WorkspacesDetailValues[1, i].text = BuildingsInfoManager.offWorkplaces[i].ToString();
                    m_WorkspacesDetailValues[2, i].text = BuildingsInfoManager.indWorkplaces[i].ToString();
                    m_WorkspacesDetailValues[3, i].text = BuildingsInfoManager.GetServiceWorkspaceCount(i).ToString();
                    m_WorkspacesDetailValues[4, i].text = BuildingsInfoManager.GetWorkplacesByLevel(i).ToString();
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

