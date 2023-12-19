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

namespace Askar0_Plugins_CheatFlightModeToggle
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
        static PlayerMovable playerMovable;

        /// <summary>
        /// Initialize the Plugin
        /// </summary>
        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

            isEnabled = Config.Bind("General", "Enabled", true, "Is the mod enabled?");
            isKey = Config.Bind("General", "KeyBind", Key.H, "Flight Toggle Keybind");
 
            logger = base.Logger;
            Harmony.CreateAndPatchAll(typeof(Plugin), $"{MyPluginInfo.PLUGIN_GUID}");
        }
        /// <summary>
        /// Toggles creative flying on keypress. Use shift to zoom (Fast) flight, Caution this mode can cause fall damage to player.
        /// </summary>
        private void Update()
        {
            playerMovable = FindAnyObjectByType<PlayerMovable>();
            if (isEnabled.Value || playerMovable != null)
            {
                bool wasPressedThisFrame = Keyboard.current[isKey.Value].wasPressedThisFrame;
                bool flag = wasPressedThisFrame;
                if (flag)
                {
                    logger.LogInfo("FlightMode Key: Was pressed.");
                    playerMovable.flyMode = !playerMovable.flyMode;
                    logger.LogInfo("FlightMode State Changed: Flying = " + playerMovable.flyMode);
                }
            }
        }
    }
}