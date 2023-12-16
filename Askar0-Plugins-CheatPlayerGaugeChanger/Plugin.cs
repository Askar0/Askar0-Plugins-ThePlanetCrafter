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
using SpaceCraft;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using HarmonyLib;
using System.Reflection;
using static UnityEngine.ParticleSystem.PlaybackState;
using BepInEx.Bootstrap;

namespace Askar0_Plugins_CheatPlayerGaugeChanger
{
    /// <summary>
    /// 
    /// </summary>
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    [BepInDependency(modCreativeModePluginGuid, BepInDependency.DependencyFlags.SoftDependency)]

    public class Plugin : BaseUnityPlugin
    {
        const string modCreativeModePluginGuid = "Askar0_Plugin_CheatCreativeModeToggle";
        static ConfigEntry<bool> isEnabled;
        //static ConfigEntry<Key> isKey;
        /// <summary>
        /// 
        /// </summary>
        public static ConfigEntry<float> custHealth;
        /// <summary>
        /// 
        /// </summary>
        public static ConfigEntry<float> custThirst;
        /// <summary>
        /// 
        /// </summary>
        public static ConfigEntry<float> custOxygen;
        /// <summary>
        /// 
        /// </summary>
        public static ConfigEntry<string> custModifier;
        static bool dLog = true;
        static bool pluginConflict = false;
        //static PlayerGaugesHandler playerGaugesHandler;


        /// <summary>
        /// 
        /// </summary>
        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
            isEnabled = Config.Bind("General", "Enable", true, "Is Plugin Enabled?");
            custModifier = Config.Bind("General", "CustomModifier", "Casual", "Choose one from the following: Custom, AFK, Baby, Lite, Casual, Standard, Intense, Hard, HC Plus, Nightmare, Hell");

            custHealth = Config.Bind("General", "CustomHealth", -0.060f, "Default: -0.060f  (Disabled at the moment)");
            custThirst = Config.Bind("General", "CustomThirst", -0.130f, "Default: -0.130f  (Disabled at the moment)");
            custOxygen = Config.Bind("General", "CustomOxygen", -1.900f, "Default: -1.900f  (Disabled at the moment)");
            
            if (Chainloader.PluginInfos.TryGetValue(modCreativeModePluginGuid, out var pi))
            {
                Logger.LogInfo("Found " + modCreativeModePluginGuid + ": (" + pi + "): disabling myself [Askar0_Plugins_CheatPlayerGaugeChanger] to prevent conflicts");
                pluginConflict = true;
            }
            else
            {
                Logger.LogInfo("Not Found " + modCreativeModePluginGuid);
                pluginConflict = false;
            }

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), $"{MyPluginInfo.PLUGIN_GUID}");

        }

        /// <summary>
        /// 
        /// </summary>
        private void Update()
        {
            if (!pluginConflict)
            {
                if (isEnabled.Value)
                {
                    float modValue = 0f;
                    modValue = GetModifierValue(custModifier.Value);
                    // playerGaugesHandler = Managers.GetManager<PlayersManager>()?.GetActivePlayerController()?.GetPlayerGaugesHandler();

                    if (modValue != 0f)
                    {
                        Managers.GetManager<GameSettingsHandler>().GetCurrentGameSettings().SetGaugeDrain(modValue);
                    }
                    if (dLog)
                    {
                        Logger.LogInfo("================================================================");
                        Logger.LogInfo("Gauge Oxygen Drain Mode: " + GaugesConsumptionHandler.GetOxygenConsumptionRate());
                        Logger.LogInfo("Gauge Thirst Drain Mode: " + GaugesConsumptionHandler.GetThirstConsumptionRate());
                        Logger.LogInfo("Gauge Health Drain Mode: " + GaugesConsumptionHandler.GetHealthConsumptionRate());
                        Logger.LogInfo("Gauge Drain Mode: " + Managers.GetManager<GameSettingsHandler>().GetCurrentGameSettings().GetModifierGaugeDrain());
                        Logger.LogInfo("================================================================");
                        dLog = false;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_Modifier"></param>
        /// <returns></returns>
        private float GetModifierValue(string _Modifier)
        {
            float modifierValue = 0f;
            if (_Modifier != null)
            {
                if (_Modifier == "AFK") { modifierValue = 0.025f; }
                if (_Modifier == "Baby") { modifierValue = 0.4f; }
                if (_Modifier == "Lite") { modifierValue = 0.65f; }
                if (_Modifier == "Casual") { modifierValue = 0.8f; } //  Basegame and Standard Gauge States (-0.060f, -0.130f, -1.900f)
                if (_Modifier == "Custom") { modifierValue = 1f; }   //  Basegame I am using as defaults    (1.0f Modifier)
                if (_Modifier == "Standard") { modifierValue = 1.25f; }
                if (_Modifier == "Intense") { modifierValue = 1.5f; }
                if (_Modifier == "Hardcore") { modifierValue = 2f; }
                if (_Modifier == "HC Plus") { modifierValue = 2.75f; }
                if (_Modifier == "Nightmare") { modifierValue = 3.75f; }
                if (_Modifier == "Hell") { modifierValue = 5f; }
                //if (modifierValue == 0f) { modifierValue = 1f; } // If incorrect spelling/case reset to default
            }
            return modifierValue;
        }

        /* NOT WORKING Looking for work around
        private class o2Thirst
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="___healthChangeValuePerSec"></param>
            /// <param name="___thirstChangeValuePerSec"></param>
            /// <param name="___oxygenChangeValuePerSec"></param>
            [HarmonyPrefix]
            [HarmonyPatch(typeof(PlayerGaugesHandler), "StartGaugesConsumption")]
            private static void o2Water(ref float ___healthChangeValuePerSec, ref float ___thirstChangeValuePerSec, ref float ___oxygenChangeValuePerSec)
            {
            ___thirstChangeValuePerSec = Plugin.custThirst.Value;
            ___oxygenChangeValuePerSec = Plugin.custOxygen.Value;
            ___healthChangeValuePerSec = Plugin.custHealth.Value;
            }
        }
        */
    }
}
