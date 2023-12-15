using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using Mono.Cecil.Cil;
using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace Askar0_Plugins;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    static ConfigEntry<bool> isDebug;
    static ConfigEntry<Key> isKey;


    static ManualLogSource logger;

    public const string HashKey = "{}";

    /// <summary>
    /// 
    /// </summary>
    private void Awake()
    {
        // Plugin startup logic
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        isDebug = Config.Bind("General", "Debug", true, "Debugging Enabled?");
        isKey = Config.Bind("General", "KeyBind", Key.K, "Dump Debug Info Key");
        logger = base.Logger;

    }
}
