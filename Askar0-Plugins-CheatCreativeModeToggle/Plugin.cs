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

using System;
using System.Reflection;
using System.Xml;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using SpaceCraft;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Askar0_Plugins_CheatCreativeModeToggle
{
    /// <summary>
    /// 
    /// </summary>
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        static ConfigEntry<bool> isEnabled;
        static ConfigEntry<bool> isDebug;
        static ConfigEntry<Key> isKey;

        static ManualLogSource logger;
        static GameSettingsHandler gameSettingsHandler;
        // static PlayerGaugesHandler playerGaugesHandler;
        // private static bool isAppCreative = false;

        /// <summary>
        /// 
        /// </summary>
        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

            isEnabled = Config.Bind("General", "Enabled", true, "Is the mod enabled?");
            isDebug = Config.Bind("General", "Debug", false, "Debugging Enabled?");

            isKey = Config.Bind("General", "KeyBind", Key.K, "Creative Toggle Keybind");

            logger = base.Logger;

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), $"{MyPluginInfo.PLUGIN_GUID}");
        }
        /// <summary>
        /// 
        /// </summary>
        private void Update()
        {
            gameSettingsHandler = Managers.GetManager<GameSettingsHandler>();
            if (isEnabled.Value && gameSettingsHandler != null)
            {
                bool wasPressedThisFrame = Keyboard.current[isKey.Value].wasPressedThisFrame;
                bool flag = wasPressedThisFrame;
                if (flag)
                {
                    if (isDebug.Value)
                    {
                        //Debug Log
                        logger.LogInfo("=========================================================");
                        logger.LogInfo("=============  Creative Mode Toggle Plugin  =============");
                        logger.LogInfo("=========================================================");
                        logger.LogInfo("== Plugin Update Check Mode: " + isEnabled.Value);
                        logger.LogInfo("== Plugin Update Debug ==");

                        logger.LogInfo("FreeCraft Mode Was: " + gameSettingsHandler.GetCurrentGameSettings().GetFreeCraft());
                        logger.LogInfo("Save Game Mode: " + gameSettingsHandler.GetCurrentGameSettings().GetGameMode().ToString());
                        logger.LogInfo("Gauge Oxygen Drain Mode: " + GaugesConsumptionHandler.GetOxygenConsumptionRate());
                        logger.LogInfo("Gauge Thirst Drain Mode: " + GaugesConsumptionHandler.GetThirstConsumptionRate());
                        logger.LogInfo("Gauge Health Drain Mode: " + GaugesConsumptionHandler.GetHealthConsumptionRate());

                    }

                    logger.LogInfo("== Creative Mode Changing State ==");
                    gameSettingsHandler.TogglePlayMode();
                    bool gDrain = gameSettingsHandler.GetCurrentGameSettings().GetFreeCraft();
                    if (gDrain)
                    {
                        gameSettingsHandler.GetCurrentGameSettings().SetGaugeDrain(-10f); // Slowly restores health, thirst and oxygen.
                        logger.LogInfo("Gauge Drain Mode: " + gameSettingsHandler.GetCurrentGameSettings().GetModifierGaugeDrain());
                    }
                    else
                    {
                        gameSettingsHandler.GetCurrentGameSettings().SetGaugeDrain(1f); // Restores normal settings
                        logger.LogInfo("Gauge Drain Mode: " + gameSettingsHandler.GetCurrentGameSettings().GetModifierGaugeDrain());
                    }
                    if (isDebug.Value)
                    {
                        // Debug Log
                        logger.LogInfo("FreeCraft Mode is Now: " + gameSettingsHandler.GetCurrentGameSettings().GetFreeCraft());
                        logger.LogInfo("Save Game Mode: " + gameSettingsHandler.GetCurrentGameSettings().GetGameMode().ToString());
                        logger.LogInfo("Gauge Oxygen Drain Mode: " + GaugesConsumptionHandler.GetOxygenConsumptionRate());
                        logger.LogInfo("Gauge Thirst Drain Mode: " + GaugesConsumptionHandler.GetThirstConsumptionRate());
                        logger.LogInfo("Gauge Health Drain Mode: " + GaugesConsumptionHandler.GetHealthConsumptionRate());

                        logger.LogInfo("== Changing State Finished ==");
                    }
                    logger.LogInfo("Creative Mode State: " + gameSettingsHandler.GetCurrentGameSettings().GetFreeCraft());
                    logger.LogInfo("=====      Plugin Update Log: Done.      =====");
                }
            }
        }
    }
}
