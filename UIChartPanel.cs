using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CS_EmploymentDetailsExtender
{
    class UIChartPanel : UIPanel
    {
        private UIRadialChart[] m_RadialChart = new UIRadialChart[4];

        private static readonly Vector2 chartSize = new Vector2(112, 112);

        private static readonly int chartPadding = 5;
        private static readonly int chartSeparator = 10;

        public static readonly Vector2 panelSize = new Vector2(chartPadding * 2 + chartSeparator + chartSize.x * 2, chartPadding * 2 + chartSeparator + chartSize.y * 2);

        public override void Start()
        {
            base.Start();

            //backgroundSprite = "InfoviewPanel"; //White Bakground
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

        private UIRadialChart CreateRadialChart(int level)
        {
            UIRadialChart rc = AddUIComponent<UIRadialChart>();
            rc.isEnabled = true;
            rc.isVisible = true;
            rc.name = "EmploymentChartLevel" + level;
            rc.size = chartSize;
            rc.relativePosition = new Vector2(chartPadding + (level % 2 * (rc.size.x + chartSeparator)), chartPadding + (level / 2 * (rc.size.y + chartSeparator)));
            rc.zOrder = 2;
            rc.spriteName = "PieChartWhiteBg";

            rc.AddSlice();
            UIRadialChart.SliceSettings slice0 = rc.GetSlice(0);
            Color32 color = JobsUtils.educationLevelColors[level];
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
                    float percent = JobsUtils.GetPercentEmployedF(i);
                    m_RadialChart[i].SetValues(new float[] { percent, 1f - percent });
                }
                
            }
        }
    }
}
