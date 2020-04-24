using System;
using ICities;
using ColossalFramework;
using UnityEngine;

namespace CS_EmploymentDetailsExtender
{
    public static class JobsUtils
    {
        private static readonly DistrictManager dm = Singleton<DistrictManager>.instance;

        /* TODO : replace by dynamic ingame values */
        public static readonly Color32[] educationLevelColors = new Color32[] {
            new Color32(241, 136, 136, 255),
            new Color32(251, 212, 0, 255),
            new Color32(141, 149, 55, 255),
            new Color32(131, 213, 141, 255)
        };

        /* TODO : replace by dynamic ingame values (and if possible localized) */
        public static readonly String[] educationLevelNames = new String[] {
            "Uneducated",
            "Educated",
            "Well Educated",
            "Highly Educated"
        };

        public static string GetEmploymentLabel(int educationLevel)
        {
            DistrictEducationData ded = GetEducationData(educationLevel);
            int percent = GetPercentEmployed(educationLevel);

            return percent + "% (" + (ded.m_finalEligibleWorkers - ded.m_finalUnemployed) + "/" + ded.m_finalEligibleWorkers + ")";
        }

        public static int GetEmploymentMaxValue(int educationLevel)
        {
            DistrictEducationData ded = GetEducationData(educationLevel);
            return (int)ded.m_finalEligibleWorkers;
        }

        public static int GetEmploymentCurrentValue(int educationLevel)
        {
            DistrictEducationData ded = GetEducationData(educationLevel);
            return (int)(ded.m_finalEligibleWorkers - ded.m_finalUnemployed);
        }

        public static DistrictEducationData GetEducationData(int educationLevel)
        {
            District d = dm.m_districts.m_buffer[0];

            DistrictEducationData ded = d.m_educated0Data;
            switch (educationLevel)
            {
                case 0:
                    ded = d.m_educated0Data;
                    break;
                case 1:
                    ded = d.m_educated1Data;
                    break;
                case 2:
                    ded = d.m_educated2Data;
                    break;
                case 3:
                    ded = d.m_educated3Data;
                    break;

            }

            return ded;
        }

        public static int GetPercentEmployed(int educationLevel)
        {
            return 100 - GetPercentUnemployed(educationLevel);
        }

        public static float GetPercentEmployedF(int educationLevel)
        {
            return 1f - GetPercentUnemployedF(educationLevel);
        }

        public static int GetPercentUnemployed(int educationLevel)
        {
            float num = GetPercentUnemployedF(educationLevel);
            return Mathf.Clamp(Mathf.RoundToInt(num * 100f), 0, 100);
        }

        public static float GetPercentUnemployedF(int educationLevel)
        {
            DistrictEducationData ded = GetEducationData(educationLevel);
            return (float)ded.m_finalUnemployed / (float)ded.m_finalEligibleWorkers;
        }
    }
}
