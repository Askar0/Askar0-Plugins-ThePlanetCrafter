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
using BepInEx.Logging;
using BepInEx.Configuration;
using HarmonyLib;
using System.Reflection;
using UnityEngine;
using SpaceCraft;
using UnityEngine.InputSystem;

namespace Askar0_Plugins_CheatPlayerGaugeStopper
{
    /// <summary>
    /// 
    /// </summary>
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private int originalTargetFramerate;

        static ConfigEntry<bool> isEnabled;
        static ConfigEntry<Key> isKey;

        static bool isAppAFK = false;

        static ManualLogSource logger;
        private static PlayerGaugesHandler playerGaugesHandler;
        /// <summary>
        /// 
        /// </summary>
        private void Awake()
        {
            // Plugin startup logic

            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

            isEnabled = Config.Bind("General", "Enabled", true, "Is the mod enabled?");
            isKey = Config.Bind("General", "KeyBind", Key.K, "AFK Toggle Keybind (Todo)");

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), $"{MyPluginInfo.PLUGIN_GUID}");

            logger = base.Logger;
            Application.focusChanged += Application_focusChanged;

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
                    isAppAFK = flag;
                    logger.LogInfo("Key Was Pressed: " + isAppAFK);  // flag
                    // Todo: Code StopGauges whilst window has focus
                    // logger.LogInfo("Toggle AFK Mode: " + isAppAFK);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hasFocus"></param>
        private void Application_focusChanged(bool hasFocus)
        {
            FPS();
            GaugeStatus(!hasFocus);
        }
        /// <summary>
        /// 
        /// </summary>
        private void FPS()
        {
            if (!Application.isFocused)
            {
                originalTargetFramerate = Application.targetFrameRate;
                Application.targetFrameRate = 5;
            }
            else
            {
                if (!originalTargetFramerate.Equals(default))
                    Application.targetFrameRate = originalTargetFramerate;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="isAFK"></param>
        private void GaugeStatus(bool isAFK)
        {
            playerGaugesHandler = Managers.GetManager<PlayersManager>()?.GetActivePlayerController()?.GetPlayerGaugesHandler();
            playerGaugesHandler?.GetPlayerGaugeHealth()?.SetInfinityStatus(isAFK);
            playerGaugesHandler?.GetPlayerGaugeOxygen()?.SetInfinityStatus(isAFK);
            playerGaugesHandler?.GetPlayerGaugeThirst()?.SetInfinityStatus(isAFK);
            logger.LogInfo("StopGauges AFK Status: " + isAFK);
        }
    }
}