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
using Mono.Cecil.Cil;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.InputSystem;

namespace Askar0_Plugins;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    static ConfigEntry<bool> isEnabled;
    static ConfigEntry<bool> isDebug;
    static ConfigEntry<Key> isKey;

    static ManualLogSource logger;

    public const string HashKey = "{82937498-0EF5-3EBC-EE00-BA0BA789F201}";

    /// <summary>
    /// 
    /// </summary>
    private void Awake()
    {
        // Plugin startup logic
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        isEnabled = Config.Bind("General", "Enabled", true, "Is Plugin Enabled?");
        isDebug = Config.Bind("General", "Debug", true, "Debugging Enabled?");
        isKey = Config.Bind("General", "KeyBind", Key.Numpad5, "Dump Debug Info Key");

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), $"{MyPluginInfo.PLUGIN_GUID}");
        logger = base.Logger;
        logger.LogInfo("Test Plugin Loaded: " + HashKey);
        logger.LogInfo("Test Plugin Enabled: " + isEnabled);
        logger.LogInfo("Test Plugin Debug: " + isDebug);
        logger.LogInfo("Test Plugin Key: " + isKey);
    }
}
