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
using System.Net;

namespace Askar0_Plugins_CheatPlayerGaugeChanger
{
    /// <summary>
    /// 
    /// </summary>
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    //[BepInDependency(modCreativeModePluginGuid, BepInDependency.DependencyFlags.SoftDependency)]

    public class Plugin : BaseUnityPlugin
    {
        //const string modCreativeModePluginGuid = "Askar0_Plugin_CheatCreativeModeToggle";
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
        // static bool pluginConflict = false;
        static float modValue = 0f;
        static GameSettingsHandler gameSettingsHandler;


        /// <summary>
        /// 
        /// </summary>
        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
            isEnabled = Config.Bind("General", "Enable", true, "Is Plugin Enabled?");
            custModifier = Config.Bind("General", "CustomModifier", "Casual", "Choose one from the following: HealME, Creative, Custom, AFK, Baby, Lite, Casual, Standard, Intense, Hard, HC Plus, Nightmare, Hell");

            custHealth = Config.Bind("General", "CustomHealth", -0.060f, "Default: -0.060f  (Disabled at the moment)");
            custThirst = Config.Bind("General", "CustomThirst", -0.130f, "Default: -0.130f  (Disabled at the moment)");
            custOxygen = Config.Bind("General", "CustomOxygen", -1.900f, "Default: -1.900f  (Disabled at the moment)");
            
            /*
            if (Chainloader.PluginInfos.TryGetValue(modCreativeModePluginGuid, out var pi))
            {
                Logger.LogInfo("Found " + modCreativeModePluginGuid + ": disabling myself [Askar0_Plugins_CheatPlayerGaugeChanger] to prevent conflicts");
                pluginConflict = true;
            }
            else
            {
                Logger.LogInfo("Not Found " + modCreativeModePluginGuid);
                pluginConflict = false;
            }
            */
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), $"{MyPluginInfo.PLUGIN_GUID}");
        }

        /// <summary>
        /// 
        /// </summary>
        private void Update()
        {
            //if (!pluginConflict)
            //{  
                gameSettingsHandler = Managers.GetManager<GameSettingsHandler>();
                if (isEnabled.Value && gameSettingsHandler != null)
                {
                    modValue = GetModifierValue(custModifier.Value);
                    gameSettingsHandler.GetCurrentGameSettings().SetGaugeDrain(modValue);
                    if (dLog)
                    {
                        Logger.LogInfo("================================================================");
                        Logger.LogInfo("Gauge Oxygen Drain Mode: " + GaugesConsumptionHandler.GetOxygenConsumptionRate());
                        Logger.LogInfo("Gauge Thirst Drain Mode: " + GaugesConsumptionHandler.GetThirstConsumptionRate());
                        Logger.LogInfo("Gauge Health Drain Mode: " + GaugesConsumptionHandler.GetHealthConsumptionRate());
                        Logger.LogInfo("Gauge Drain Mode: " + gameSettingsHandler.GetCurrentGameSettings().GetModifierGaugeDrain());
                        Logger.LogInfo("================================================================");
                        dLog = false;
                    }
                }
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_Modifier"></param>
        /// <returns></returns>
        public float GetModifierValue(string _Modifier)
        {
            float modifierValue = 0f;
            if (_Modifier != null)
            {
                bool gDrain = gameSettingsHandler.GetCurrentGameSettings().GetFreeCraft();
                if (_Modifier == "AFK")       { modifierValue = 0.025f; }
                if (_Modifier == "Baby")      { modifierValue = 0.325f; }
                if (_Modifier == "Lite")      { modifierValue = 0.650f; }
                if (_Modifier == "Casual")    { modifierValue = 1f; }       //  ==============================================
                if (_Modifier == "Custom")    { modifierValue = 1f; }       //  Basegame: Rates  ( Health,  Thirst,  Oxygen)
                if (_Modifier == "Standard")  { modifierValue = 1.25f; }    //  Casual           (-0.048f, -0.104f, -1.520f) *
                if (_Modifier == "Intense")   { modifierValue = 1.5f; }     //  Standard         (-0.060f, -0.130f, -1.900f)
                if (_Modifier == "Hardcore")  { modifierValue = 2f; }       //  Intense          (-0.072f, -0.156f, -2.280f)
                if (_Modifier == "HC Plus")   { modifierValue = 2.75f; }    //  Hard             (-0.096f, -0.208f, -3.040f)
                if (_Modifier == "Nightmare") { modifierValue = 3.75f; }    //  ==============================================
                if (_Modifier == "Hell")      { modifierValue = 5f; }
                if (_Modifier == "Creative" || _Modifier == "HealME" || gDrain)
                                              { modifierValue = -10f; }
            }
            return modifierValue;
        }
    }
}
