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

namespace Askar0_Plugins_CheatUnlockAllBlueprintsPermanently
{
    /// <summary>
    /// 
    /// </summary>
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {

        static ConfigEntry<bool> isEnabled;
        static GameSettingsHandler gameSettingsHandler;

        /// <summary>
        /// 
        /// </summary>
        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
            isEnabled = Config.Bind("General", "Enabled", false, "Is the mod enabled? If you enable this mod it will permanently change any of the saves you open to have all blueprints unlocked whilst it is active.");
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), $"{MyPluginInfo.PLUGIN_GUID}");

        }
        /// <summary>
        /// 
        /// </summary>
        private void Update()
        {
            if (isEnabled.Value)
            {
                LoadAllBlueprints(true);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="isActive"></param>
        private void LoadAllBlueprints(bool isActive)
        {
            gameSettingsHandler = Managers.GetManager<GameSettingsHandler>();
            if (isActive == true && gameSettingsHandler != null)
            {
                gameSettingsHandler.GetCurrentGameSettings().SetEverythingUnlocked(true);
                Logger.LogInfo("Permanent Change: All Blueprints Unlocked");
                Logger.LogInfo("This change will persist even if plugin is disabled again or even removed. To remove this state you will need to edit the unlockedEverything parameter in the save file option near the end of the file to \"unlockedEverything\":false.");
                isEnabled.Value = false; // Failsafe automatically turns off after activating all blueprints in a savefile.
            }
        }
    }
}
