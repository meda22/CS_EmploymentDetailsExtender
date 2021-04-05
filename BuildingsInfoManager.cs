using System;
using System.Reflection;
using ICities;
using ColossalFramework;
using ColossalFramework.Math;
using UnityEngine;

namespace DemographicsMod
{
    public class BuildingsInfoManager : BuildingExtensionBase
    {
        public static int WorkplacesUneducated { get; private set; }
        public static int WorkplacesEducated { get; private set; }
        public static int WorkplacesWellEducated { get; private set; }
        public static int WorkplacesHighlyEducated { get; private set; }

        public static int[] comWorkplaces = new int[4];
        public static int[] offWorkplaces = new int[4];
        public static int[] indWorkplaces = new int[4];

        public static bool ShouldWeCount { get; set; }

        public override void OnBuildingCreated(ushort id)
        {
            base.OnBuildingCreated(id);
            ShouldWeCount = true;
            CalculateAllWorkplaces();
        }

        public static void CalculateAllWorkplaces()
        {
            if (!ShouldWeCount) return;

            WorkplacesEducated = 0;
            WorkplacesHighlyEducated = 0;
            WorkplacesUneducated = 0;
            WorkplacesWellEducated = 0;

            for (var i = 0; i < comWorkplaces.Length; i++)
            {
                comWorkplaces[i] = 0;
                offWorkplaces[i] = 0;
                indWorkplaces[i] = 0;
            }

            BuildingManager buildingManager = Singleton<BuildingManager>.instance;
            
            if (buildingManager == null || buildingManager.m_buildings == null) return;
            
            // go through all buildings
            for (ushort buildingID = 0; buildingID < buildingManager.m_buildings.m_buffer.Length; buildingID++)
            {
                try
                {
                    Building building = buildingManager.m_buildings.m_buffer[buildingID];

                    BuildingInfo buildingInfo = building.Info;

                    // we have build info and created building
                    if (buildingInfo != null && (building.m_flags & Building.Flags.Created) != Building.Flags.None)
                    {
                        var privateBuildingAI = buildingInfo.GetAI() as PrivateBuildingAI;
                        
                        // if it is zoned building (PlayerIndustry is for zoned special industry)
                        if (privateBuildingAI != null && 
                            (ItemClass.Service.Commercial == buildingInfo.GetService() 
                             || ItemClass.Service.Industrial == buildingInfo.GetService()
                             || ItemClass.Service.PlayerIndustry == buildingInfo.GetService()
                             || ItemClass.Service.Office == buildingInfo.GetService()))
                        {
                            int wp0, wp1, wp2, wp3;
                            privateBuildingAI.CalculateWorkplaceCount(
                                (ItemClass.Level) building.m_level,
                                new Randomizer(buildingID),
                                building.Width,
                                building.Length,
                                out wp0,
                                out wp1,
                                out wp2,
                                out wp3);

                            privateBuildingAI.AdjustWorkplaceCount(
                                buildingID,
                                ref building,
                                ref wp0,
                                ref wp1,
                                ref wp2,
                                ref wp3);

                            WorkplacesUneducated += wp0;
                            WorkplacesEducated += wp1;
                            WorkplacesWellEducated += wp2;
                            WorkplacesHighlyEducated += wp3;

                            ItemClass.Service service = buildingInfo.GetService();
                            switch (service)
                            {
                                case ItemClass.Service.Commercial:
                                    comWorkplaces[0] += wp0;
                                    comWorkplaces[1] += wp1;
                                    comWorkplaces[2] += wp2;
                                    comWorkplaces[3] += wp3;
                                    break;
                                case ItemClass.Service.Industrial:
                                case ItemClass.Service.PlayerIndustry:
                                    indWorkplaces[0] += wp0;
                                    indWorkplaces[1] += wp1;
                                    indWorkplaces[2] += wp2;
                                    indWorkplaces[3] += wp3;
                                    break;
                                case ItemClass.Service.Office:
                                    offWorkplaces[0] += wp0;
                                    offWorkplaces[1] += wp1;
                                    offWorkplaces[2] += wp2;
                                    offWorkplaces[3] += wp3;
                                    break;
                            }
                        } // if privateBuildingAI end
                        else
                        {
                            // this is for all other buildings - services, monuments, parks, INDUSTRY AREAS
                            if (buildingInfo.GetAI().GetType().IsSubclassOf(typeof(PlayerBuildingAI)))
                            {
                                var ai = buildingInfo.GetAI() as PlayerBuildingAI;
                                if (ai != null)
                                {
                                    int swp0, swp1, swp2, swp3;
                                    // Industry are buildings provide workplace info from this common method
                                    // But it does NOT work for raw material storages (they are like normal warehouses)
                                    ai.CountWorkPlaces(out swp0, out swp1, out swp2, out swp3); // works for industry areas

                                    // if it is sum of workplaces 0, it is another type of service building
                                    if (swp0 + swp1 + swp2 + swp3 == 0)
                                    {
                                        // we will try bit of reflection
                                        // we get Type of buildingAI
                                        var serviceAI = buildingInfo.GetAI();
                                        Type aiType = buildingInfo.GetAI().GetType();
                                        // we get all fields of that AI Type
                                        FieldInfo[] fieldInfos = aiType.GetFields();
                                        
                                        // we check if Types fields contains field m_workPlaceCount0
                                        if (fieldInfos.Length > 0 && serviceAI != null && aiType.GetField("m_workPlaceCount0") != null)
                                        {
                                            // and we count them
                                            swp0 = (int) (aiType.GetField("m_workPlaceCount0").GetValue(serviceAI) ?? 0);
                                            swp1 = (int) (aiType.GetField("m_workPlaceCount1").GetValue(serviceAI) ?? 0);
                                            swp2 = (int) (aiType.GetField("m_workPlaceCount2").GetValue(serviceAI) ?? 0);
                                            swp3 = (int) (aiType.GetField("m_workPlaceCount3").GetValue(serviceAI) ?? 0);
                                        }
                                    }

                                    WorkplacesUneducated += swp0;
                                    WorkplacesEducated += swp1;
                                    WorkplacesWellEducated += swp2;
                                    WorkplacesHighlyEducated += swp3;
                                }
                            }
                        }
                    }
                }
                catch (NullReferenceException e)
                {
                    // This should not happen now... 
                    Debug.Log("Demographics Mod: Trying to count building which does " +
                              "not have workplace information. We should not get here... " + e.Message);
                }
            }

            ShouldWeCount = false;
        }
        
        public override void OnBuildingReleased(ushort id)
        {
            base.OnBuildingReleased(id);
            ShouldWeCount = true;
            CalculateAllWorkplaces();
        }

        public override void OnCreated(IBuilding building)
        {
            base.OnCreated(building);
            ShouldWeCount = true;
            CalculateAllWorkplaces();
        }

        public static int GetWorkplacesByLevel (int i)
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
            DistrictEducationData districtEducationData = JobsUtils.GetEducationData(educationLevel);
            int wp = GetWorkplacesByLevel(educationLevel);
            float percent;
            if (wp == 0)
            {
                percent = districtEducationData.m_finalEligibleWorkers == 0 ? 0 : 1;
            }
            else
            {
                percent = districtEducationData.m_finalEligibleWorkers / (float) wp;
            }
            return percent;
        }

        public static string GetWorkplacesLabel(int educationLevel)
        {
            DistrictEducationData districtEducationData = JobsUtils.GetEducationData(educationLevel);
            int wp = GetWorkplacesByLevel(educationLevel);
            var percent = wp == 0 ? " - " : 
                ((int) (districtEducationData.m_finalEligibleWorkers / ((float)wp) * 100)).ToString();
            
            return percent + "% (" + districtEducationData.m_finalEligibleWorkers + "/" + wp + ")";
        }

        public static int GetServiceWorkspaceCount(int educationLevel)
        {
            return GetWorkplacesByLevel(educationLevel) - 
                   comWorkplaces[educationLevel] - 
                   offWorkplaces[educationLevel] - 
                   indWorkplaces[educationLevel];
        }
    }
}
