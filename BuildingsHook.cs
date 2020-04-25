using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ICities;
using ColossalFramework;
using UnityEngine;

namespace DemographicsMod
{
    public class BuildingsInfoManager : BuildingExtensionBase
    {
        public static int WorkplacesUneducated { get; private set; }
        public static int WorkplacesEducated { get; private set; }
        public static int WorkplacesWellEducated { get; private set; }
        public static int WorkplacesHighlyEducated { get; private set; }

        private static bool isDirty = true;

        public override void OnBuildingCreated(ushort id)
        {
            base.OnBuildingCreated(id);
            calculateAllWorkplaces();

        }

        public static void calculateAllWorkplaces()
        {
            if (!isDirty) return;

            WorkplacesEducated = 0;
            WorkplacesHighlyEducated = 0;
            WorkplacesUneducated = 0;
            WorkplacesWellEducated = 0;
            for (ushort i = 0; i < BuildingManager.instance.m_buildings.m_buffer.Length; i++)
            {
                var bb = BuildingManager.instance.m_buildings.m_buffer[i];
                var b = bb.Info;
                if ( (b != null) && ((bb.m_flags & Building.Flags.Created) != Building.Flags.None)) // && (b.GetService() != ItemClass.Service.Residential))
                {
                    var ai = b.GetAI() as PrivateBuildingAI;
                    if (ai != null)
                    {
                        // if (((bb.m_flags & Building.Flags.Created) != Building.Flags.None))
                        //(bb.m_flags & (Building.Flags.Abandoned | Building.Flags.Collapsed)) == Building.Flags.None)
                        //((bb.m_flags & Building.Flags.Created) != Building.Flags.None) )
                        {
                            int l0, l1, l2, l3 = 0;

                            ai.CalculateWorkplaceCount((ItemClass.Level)bb.m_level, new ColossalFramework.Math.Randomizer(i),
                                bb.Width, bb.Length, out l0, out l1, out l2, out l3);
                            ai.AdjustWorkplaceCount(i, ref bb, ref l0, ref l1, ref l2, ref l3);

                            WorkplacesUneducated += l0;
                            WorkplacesEducated += l1;
                            WorkplacesHighlyEducated += l3;
                            WorkplacesWellEducated += l2;
                        }
                    }
                    else
                    {
                        // Service Buildings don't have a common AI to calculate workplaces, so we need to do this ugly 
                        // boilerplate
                        // TODO: need to change 
                        switch (b.GetService())
                        {
                            case ItemClass.Service.FireDepartment:
                                var ai1 = b.GetAI() as FireStationAI;
                                if (ai1 == null) break;
                                WorkplacesUneducated += ai1.m_workPlaceCount0;
                                WorkplacesEducated += ai1.m_workPlaceCount1;
                                WorkplacesWellEducated += ai1.m_workPlaceCount2;
                                WorkplacesHighlyEducated += ai1.m_workPlaceCount3;
                                break;
                            case ItemClass.Service.PoliceDepartment:
                                var ai2 = b.GetAI() as PoliceStationAI;
                                if (ai2 == null) break;
                                WorkplacesUneducated += ai2.m_workPlaceCount0;
                                WorkplacesEducated += ai2.m_workPlaceCount1;
                                WorkplacesWellEducated += ai2.m_workPlaceCount2;
                                WorkplacesHighlyEducated += ai2.m_workPlaceCount3;
                                break;
                            case ItemClass.Service.HealthCare:
                                var ai31 = b.GetAI() as CemeteryAI;
                                if (ai31 != null)
                                {
                                    WorkplacesUneducated += ai31.m_workPlaceCount0;
                                    WorkplacesEducated += ai31.m_workPlaceCount1;
                                    WorkplacesWellEducated += ai31.m_workPlaceCount2;
                                    WorkplacesHighlyEducated += ai31.m_workPlaceCount3;
                                    break;
                                }
                                var ai3 = b.GetAI() as HospitalAI; 
                                if (ai3 == null) break;
                                WorkplacesUneducated += ai3.m_workPlaceCount0;
                                WorkplacesEducated += ai3.m_workPlaceCount1;
                                WorkplacesWellEducated += ai3.m_workPlaceCount2;
                                WorkplacesHighlyEducated += ai3.m_workPlaceCount3;
                                break;
                            case ItemClass.Service.Electricity:
                                var ai4 = b.GetAI() as PowerPlantAI;
                                if (ai4 == null) break;
                                WorkplacesUneducated += ai4.m_workPlaceCount0;
                                WorkplacesEducated += ai4.m_workPlaceCount1;
                                WorkplacesWellEducated += ai4.m_workPlaceCount2;
                                WorkplacesHighlyEducated += ai4.m_workPlaceCount3;
                                break;
                            case ItemClass.Service.Water:
                                var ai5 = b.GetAI() as WaterFacilityAI;
                                if (ai5 != null)
                                {
                                    WorkplacesUneducated += ai5.m_workPlaceCount0;
                                    WorkplacesEducated += ai5.m_workPlaceCount1;
                                    WorkplacesWellEducated += ai5.m_workPlaceCount2;
                                    WorkplacesHighlyEducated += ai5.m_workPlaceCount3;
                                    break;
                                }
                                var ai6 = b.GetAI() as WaterCleanerAI;
                                if (ai6 == null) break;
                                WorkplacesUneducated += ai6.m_workPlaceCount0;
                                WorkplacesEducated += ai6.m_workPlaceCount1;
                                WorkplacesWellEducated += ai6.m_workPlaceCount2;
                                WorkplacesHighlyEducated += ai6.m_workPlaceCount3;
                                break;


                            default: break;
                        }
                        
                        /*
                        var f0 = ai1.GetType().GetField("m_workPlaceCount0");
                        if (! f0.Equals(null))
                        {
                            WorkplacesUneducated += (int)f0.GetValue(ai1);
                        }*/
                    }
                }

            }

            isDirty = false;
        }

        public override void OnBuildingReleased(ushort id)
        {
            base.OnBuildingReleased(id);
            isDirty = true;
            //calculateAllWorkplaces();
        }

        public override void OnCreated(IBuilding building)
        {
            base.OnCreated(building);
            isDirty = true;
            //calculateAllWorkplaces();
            
        }

        private static int getWorkplacesByLevel (int i)
        {
            switch (i)
            {
                case 0: return WorkplacesUneducated;
                case 1: return WorkplacesEducated;
                case 2: return WorkplacesWellEducated;
                default: return WorkplacesHighlyEducated;
            }
        }

        public static float GetPercent(int educationLevel)
        {
            DistrictEducationData ded = JobsUtils.GetEducationData(educationLevel);
            int wp = getWorkplacesByLevel(educationLevel);
            float percent;
            if (wp == 0)
            {
                if (ded.m_finalEligibleWorkers == 0)
                    percent = 0;
                else percent = 1;
            }
                
            else
                percent = (float)ded.m_finalEligibleWorkers / (float)wp;
            return percent;
        }

        public static string GetWorkplacesLabel(int educationLevel)
        {
            DistrictEducationData ded = JobsUtils.GetEducationData(educationLevel);
            int wp = getWorkplacesByLevel(educationLevel);
            string percent;
            if (wp == 0)
                percent = " - ";
            else
                percent = ((int) ((float)ded.m_finalEligibleWorkers / ((float)wp) * 100)).ToString();

            return percent + "% (" + ded.m_finalEligibleWorkers + "/" + wp + ")";
        }
    }
}
