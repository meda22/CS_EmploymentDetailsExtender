using ColossalFramework.UI;
using UnityEngine;

namespace DemographicsMod
{
    class UIChartPanel : UIPanel
    {
        private UIRadialChart[] m_RadialChart = new UIRadialChart[4];

        private UIGraph m_graph;

        private static readonly Vector2 chartSize = new Vector2(64, 64);

        private static readonly int chartPadding = 5;
        private static readonly int chartSeparator = 10;

        private static readonly float[] _chartLine = {
            1,4,3,8,6,12,14,21,18,13,29
        };

        public string RadialChartPrefix { get; set; }

        public static readonly Vector2 panelSize = 
            new Vector2(chartPadding * 2 + chartSeparator + chartSize.x * 2f, 
                chartPadding * 2 + chartSeparator + chartSize.y * 2f);

        public override void Start()
        {
            base.Start();
            isVisible = true;
            canFocus = true;
            isInteractive = true;
            width = panelSize.x;
            height = panelSize.y;

            for (int i = 0; i < m_RadialChart.Length; i++)
            {
                m_RadialChart[i] = CreateRadialChart(i);
            }
        }

        private void CreateGraph()
        {
            m_graph = AddUIComponent<UIGraph>();
            m_graph.enabled = true;
            m_graph.isVisible = true;
            m_graph.name = "EmploymentGraph";
            m_graph.size = new Vector2(250, 120);
            m_graph.relativePosition = new Vector2(chartPadding, 200);
            m_graph.color = new Color32(200, 200, 200,255);
            m_graph.AddCurve("TestData", "EN-US", _chartLine, 2, new Color32(132, 55, 55, 255));

        }

        private UIRadialChart CreateRadialChart(int level)
        {
            UIRadialChart rc = AddUIComponent<UIRadialChart>();
            rc.isEnabled = true;
            rc.isVisible = true;
            rc.name = RadialChartPrefix + "EmploymentChartLevel" + level;
            rc.size = chartSize;
            rc.relativePosition = new Vector2(chartPadding + (level % 2 * (rc.size.x + chartSeparator)),
                chartPadding + (float)(level / 2) * (rc.size.y + (float)chartSeparator));
            rc.zOrder = 2;
            rc.spriteName = "PieChartWhiteBg";

            rc.AddSlice();
            UIRadialChart.SliceSettings slice0 = rc.GetSlice(0);
            Color32 color = JobsUtils.EducationLevelColors[level];
            slice0.outterColor = color;
            slice0.innerColor = color;

            rc.AddSlice();
            UIRadialChart.SliceSettings slice1 = rc.GetSlice(1);
            Color32 color2 = new Color32(132, 110, 110, 255);
            slice1.outterColor = color2;
            slice1.innerColor = color2;

            return rc;
        }

        public override void Update()
        {
            if (isVisible && isEnabled)
            {
                for (int i = 0; i < m_RadialChart.Length; i++)
                {
                    float percent;
                    switch (RadialChartPrefix)
                    {
                        case "workplaces":
                            percent = BuildingsInfoManager.GetPercent(i);
                            break;
                        default:
                            percent = JobsUtils.GetPercentEmployedF(i);
                            break;

                    }
                    
                    m_RadialChart[i].SetValues(new float[] { percent, 1f - percent });
                }
                
            }
        }
    }
}
