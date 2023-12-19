/*
** +========================================================================================+
**  (Bepinex Mod Plugin Library for the Unity/Steam Game - The Planet Crafter by Miju Games)
**    -  Copyright (C) 2023 By Askar0
**    -  Repository:  https://github.com/Askar0/Askar0-Plugins-ThePlanetCrafter
**  
**   This program is free software: you can redistribute it and/or modify
**   it under the terms of the GNU General Public License as published by
**   the Free Software Foundation, either version 3 of the License, or
**   (at your option) any later version.
**
**   This program is distributed in the hope that it will be useful,
**   but WITHOUT ANY WARRANTY; without even the implied warranty of
**   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
**   GNU General Public License for more details.
**
**   You should have received a copy of the GNU General Public License
**   along with this program.  If not, see <https://www.gnu.org/licenses/>.
** +========================================================================================+
**/

using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using SpaceCraft;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Askar0_Plugins_CheatRunningSpeedChanger
{
    /// <summary>
    /// 
    /// </summary>
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    class Plugin : BaseUnityPlugin
    {

        static ConfigEntry<bool> isEnabled;
        static ConfigEntry<float> speedChange;
        static ConfigEntry<float> speedChangePercent;
        static ManualLogSource logger;
        /// <summary>
        /// 
        /// </summary>
        private void Awake()
        {
            // Plugin startup logic

            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

            isEnabled = Config.Bind("General", "Enabled", true, "Is the mod enabled?");
            speedChange = Config.Bind("General", "Movement Speed", 12.5f, "Change Movement Speed [Range(0f, 12.5f)]");
            speedChangePercent = Config.Bind("General", "Movement Speed Percent", 125f, "Change Movement Speed Boost Percentage");

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), $"{MyPluginInfo.PLUGIN_GUID}");

            logger = base.Logger;
            logger.LogInfo("Setting movement speed to: " + speedChange.Value);
            logger.LogInfo("Setting movement speed boost percent to: " + speedChangePercent.Value);
        }
        /// <summary>
        /// MoveSpeed - [Range(0f, 12.5f)]
        /// </summary>
        /// <param name="___value"></param>
        [HarmonyPrefix]
        [HarmonyPatch(typeof(PlayerMovable), nameof(PlayerMovable.SetMoveSpeed))]
        static void PlayerMovable_SetMoveSpeed(ref float ___value)
        {
            if (isEnabled.Value)
            {
                ___value = speedChange.Value;
            }
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(PlayerMovable), nameof(PlayerMovable.SetMoveSpeedChangePercentage))]
        static void PlayerMovable_SetMoveSpeedChangePercentage(ref float ___value)
        {
            if (isEnabled.Value)
            {
                ___value = speedChangePercent.Value;
            }
        }
    }
}