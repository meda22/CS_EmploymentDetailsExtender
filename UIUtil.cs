﻿using ColossalFramework.UI;
using System.Collections.Generic;

namespace DemographicsMod
{
    /**
     * UIUtil source code from Skylines-ExtendedPublicTransport
     * https://github.com/justacid/Skylines-ExtendedPublicTransport/tree/master/ExtendedPublicTransportUI
     **/
    public static class UIUtil
    {
        public static List<UIComponent> FindUIComponents(string searchString)
        {
            var uics = new List<UIComponent>();
            var components = UnityEngine.Object.FindObjectsOfType<UIComponent>();

            foreach (var uic in components)
            {
                if (!uic.name.Contains(searchString))
                    continue;
                uics.Add(uic);
            }

            return uics;
        }

        public static UIComponent FindUIComponent(string searchString)
        {
            var components = UnityEngine.Object.FindObjectsOfType<UIComponent>();

            foreach (var uic in components)
            {
                if (uic.name == searchString)
                    return uic;
            }

            return null;
        }
    }
}

