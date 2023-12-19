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
using UnityEngine.InputSystem;

namespace Askar0_Plugins_CheatMeteoritesToggle
{
    /// <summary>
    /// Sets the base name of the plugin extending the BaseUnityPlugin Class
    /// </summary>
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        static ConfigEntry<bool> isEnabled;
        static ConfigEntry<Key> isKey;

        static ManualLogSource logger;

        static GameSettingsHandler gameSettings;
        static float meteoModifierValue = 0f;

        /// <summary>
        /// Initialize the Plugin
        /// </summary>
        private void Awake()
        {
            // Plugin startup logic

            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

            isEnabled = Config.Bind("General", "Enabled", true, "Is the mod enabled?");

            isKey = Config.Bind("General", "KeyBind", Key.J, "Meteorite Toggle Keybind");
            Plugin.logger = base.Logger;

            Harmony.CreateAndPatchAll(typeof(Plugin), $"{MyPluginInfo.PLUGIN_GUID}");
        }
        /// <summary>
        /// Toggles creative flying on keypress. Use shift to zoom (Fast) flight, Caution this mode can cause fall damage to player.
        /// </summary>
        private void Update()
        {
            gameSettings = Managers.GetManager<GameSettingsHandler>();
            if (isEnabled.Value && gameSettings != null)
            {
                
                meteoModifierValue = gameSettings.GetCurrentGameSettings().GetMeteoOccurenceModifier();
                float meteoModifier = meteoModifierValue;
                bool wasPressedThisFrame = Keyboard.current[isKey.Value].wasPressedThisFrame;
                bool flag = wasPressedThisFrame;
                if (flag)
                {
                    logger.LogInfo("Meteorite Toggle Key: Was pressed.");
                    if (meteoModifier != 0f)
                    {
                        gameSettings.GetCurrentGameSettings().modifierMeteoOccurence = 0f;
                        logger.LogInfo("Meteorite State Changed: " + gameSettings.GetCurrentGameSettings().GetMeteoOccurenceModifier());
                    }
                    else
                    {
                        gameSettings.GetCurrentGameSettings().modifierMeteoOccurence = meteoModifierValue;
                        logger.LogInfo("Meteorite State Changed: " + gameSettings.GetCurrentGameSettings().GetMeteoOccurenceModifier());

                    }
                }
            }
        }
    }
}