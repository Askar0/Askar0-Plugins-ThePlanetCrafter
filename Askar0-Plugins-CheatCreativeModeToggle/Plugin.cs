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
            isDebug = Config.Bind("General", "Debug", true, "Debugging Enabled?");

            isKey = Config.Bind("General", "KeyBind", Key.K, "Creative Toggle Keybind");

            logger = base.Logger;

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), $"{MyPluginInfo.PLUGIN_GUID}");
        }
        /// <summary>
        /// 
        /// </summary>
        private void Update()
        {
            if (isEnabled.Value)
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

                        logger.LogInfo("FreeCraft Mode Was: " + Managers.GetManager<GameSettingsHandler>().GetCurrentGameSettings().GetFreeCraft());
                        logger.LogInfo("Save Game Mode: " + Managers.GetManager<GameSettingsHandler>().GetCurrentGameSettings().GetGameMode().ToString());
                        logger.LogInfo("Gauge Oxygen Drain Mode: " + GaugesConsumptionHandler.GetOxygenConsumptionRate());
                        logger.LogInfo("Gauge Thirst Drain Mode: " + GaugesConsumptionHandler.GetThirstConsumptionRate());
                        logger.LogInfo("Gauge Health Drain Mode: " + GaugesConsumptionHandler.GetHealthConsumptionRate());

                    }

                    logger.LogInfo("== Creative Mode Changing State ==");
                    Managers.GetManager<GameSettingsHandler>().TogglePlayMode();

                    // Managers.GetManager<GameSettingsHandler>().GetCurrentGameSettings().SetEverythingUnlocked(true);

                    bool gDrain = Managers.GetManager<GameSettingsHandler>().GetCurrentGameSettings().GetFreeCraft();

                    if (gDrain)
                    {
                        Managers.GetManager<GameSettingsHandler>().GetCurrentGameSettings().SetGaugeDrain(-10f);

                        //if (!Managers.GetManager<PlayerGaugesHandler>().IsFullHealth(5f)) { Managers.GetManager<PlayerGaugesHandler>().Eat(5); }
                        //if (!Managers.GetManager<PlayerGaugesHandler>().IsFullWater(5f)) { Managers.GetManager<PlayerGaugesHandler>().Drink(5); }
                        //if (!Managers.GetManager<PlayerGaugesHandler>().IsFullOxygen(5f)) { Managers.GetManager<PlayerGaugesHandler>().Breath(5); }

                        logger.LogInfo("Gauge Drain Mode: " + Managers.GetManager<GameSettingsHandler>().GetCurrentGameSettings().GetModifierGaugeDrain());
                    }
                    else
                    {
                        Managers.GetManager<GameSettingsHandler>().GetCurrentGameSettings().SetGaugeDrain(1f);
                        logger.LogInfo("Gauge Drain Mode: " + Managers.GetManager<GameSettingsHandler>().GetCurrentGameSettings().GetModifierGaugeDrain());
                    }

                    if (isDebug.Value)
                    {
                        // Debug Log
                        logger.LogInfo("FreeCraft Mode is Now: " + Managers.GetManager<GameSettingsHandler>().GetCurrentGameSettings().GetFreeCraft());
                        logger.LogInfo("Save Game Mode: " + Managers.GetManager<GameSettingsHandler>().GetCurrentGameSettings().GetGameMode().ToString());
                        logger.LogInfo("Gauge Oxygen Drain Mode: " + GaugesConsumptionHandler.GetOxygenConsumptionRate());
                        logger.LogInfo("Gauge Thirst Drain Mode: " + GaugesConsumptionHandler.GetThirstConsumptionRate());
                        logger.LogInfo("Gauge Health Drain Mode: " + GaugesConsumptionHandler.GetHealthConsumptionRate());

                        logger.LogInfo("== Changing State Finished ==");
                    }

                    //isAppCreative = !isAppCreative;
                    logger.LogInfo("Creative Mode State: " + Managers.GetManager<GameSettingsHandler>().GetCurrentGameSettings().GetFreeCraft());
                    logger.LogInfo("=====      Plugin Update Log: Done.      =====");

                }
            }
        }
    }
}
