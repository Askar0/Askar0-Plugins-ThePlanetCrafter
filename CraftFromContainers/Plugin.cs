﻿/*
** +========================================================================================+
**  (Bepinex Mod Plugin Library for the Unity/Steam Game - The Planet Crafter by Miju Games)
**    -  Copyright (C) 2023 By Aedenthorn (Revised by Askar0)
**    -  Repository:  https://github.com/Askar0/Askar0-Plugins-ThePlanetCrafter
**
**   Code Compatibility Patch for FreeBuild/CreativeMode
**   Source: https://github.com/aedenthorn/PlanetCrafterMods/tree/master/CraftFromContainers
**   Copyright © 2023 By Aedenthorn
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
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;
using AedenthornUtils;
using Debug = UnityEngine.Debug;

namespace CraftFromContainers
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private static Plugin context;

        private static ConfigEntry<bool> modEnabled;
        private static ConfigEntry<bool> isDebug;
        private static ConfigEntry<bool> pullFromChests;
        private static ConfigEntry<string> toggleKey;
        private static ConfigEntry<string> missingResources;
        private static ConfigEntry<float> range;

        private InputAction action;

        private static bool skip;

        public static void Dbgl(string str = "", LogLevel logLevel = LogLevel.Debug)
        {
            if (isDebug.Value)
                context.Logger.Log(logLevel, str);
        }
        private void Awake()
        {
            context = this;
            modEnabled = Config.Bind<bool>("General", "Enabled", true, "Enable this mod");
            isDebug = Config.Bind<bool>("General", "IsDebug", false, "Enable debug logs");
            pullFromChests = Config.Bind<bool>("Options", "PullFromChests", true, "Allow pulling from chests.");
            toggleKey = Config.Bind<string>("Options", "ToggleKey", "<Keyboard>/home", "Key to toggle pulling");
            missingResources = Config.Bind<string>("Options", "MissingResources", "Missing Resources!", "Message to display if you move out of resource range while building. Set to empty to disable.");
            range = Config.Bind<float>("Options", "Range", 20f, "Pull range (m)");

            if (!toggleKey.Value.Contains("<"))
                toggleKey.Value = "<Keyboard>/" + toggleKey.Value;

            action = new InputAction(binding: toggleKey.Value);
            action.Enable();

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), $"{MyPluginInfo.PLUGIN_GUID}");
            Dbgl($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

        }

        private void Update()
        {

            if (Managers.GetManager<WindowsHandler>()?.GetHasUiOpen() == false && action.WasPressedThisFrame())
            {
                modEnabled.Value = !modEnabled.Value;
                Dbgl($"Mod enabled: {modEnabled.Value}");
                if (Managers.GetManager<PopupsHandler>() != null)
                    AccessTools.FieldRefAccess<PopupsHandler, List<PopupData>>(Managers.GetManager<PopupsHandler>(), "popupsToPop").Add(new PopupData(null, $"Craft From Containers: {modEnabled.Value}", 2));
            }
        }

        [HarmonyPatch(typeof(PlayerBuilder), nameof(PlayerBuilder.InputOnAction))]
        private static class PlayerBuilder_InputOnAction_Patch
        {
            static bool Prefix(PlayerBuilder __instance, ref ConstructibleGhost ___ghost, float ___timeCreatedGhost, float ___timeCantBuildInterval, GroupConstructible ___ghostGroupConstructible)
            {
                if (!modEnabled.Value || ___ghost is null || (Time.time < ___timeCreatedGhost + ___timeCantBuildInterval) || Managers.GetManager<GameSettingsHandler>().GetCurrentGameSettings().GetFreeCraft())
                    return true;
                if (!__instance.GetComponent<PlayerBackpack>().GetInventory().ContainsItems(new List<Group> { ___ghostGroupConstructible }) && !__instance.GetComponent<PlayerBackpack>().GetInventory().ContainsItems(___ghostGroupConstructible.GetRecipe().GetIngredientsGroupInRecipe()))
                {
                    Dbgl("Resources missing! Cancelling build.");
                    if (!string.IsNullOrEmpty(missingResources.Value.Trim()) && Managers.GetManager<PopupsHandler>() != null)
                        AccessTools.FieldRefAccess<PopupsHandler, List<PopupData>>(Managers.GetManager<PopupsHandler>(), "popupsToPop").Add(new PopupData(___ghostGroupConstructible.GetImage(), missingResources.Value, 2));
                    Destroy(___ghost.gameObject);
                    ___ghost = null;
                    return false;
                }
                return true;
            }
        }
        [HarmonyPatch(typeof(Inventory), nameof(Inventory.RemoveItems))]
        private static class Inventory_RemoveItems_Patch
        {
            static void Prefix(Inventory __instance, List<Group> _groups, bool _destroyWorldObjects, bool _displayInformation)
            {
                if (!modEnabled.Value || skip || __instance != Managers.GetManager<PlayersManager>().GetActivePlayerController().GetPlayerBackpack().GetInventory() || Managers.GetManager<GameSettingsHandler>().GetCurrentGameSettings().GetFreeCraft())
                    return;
                List<Group> groupsCopy = new List<Group>();
                groupsCopy.AddRange(_groups);

                skip = true;
                List<bool> hasStatus = __instance.ItemsContainsStatus(_groups);
                skip = false;
                if (!hasStatus.Contains(false))
                    return;

                Dbgl($"Trying to remove missing items from player inventory:");
                for (int j = 0; j < hasStatus.Count; j++)
                {
                    if (!hasStatus[j])
                    {
                        Dbgl($"{_groups[j].GetId()}");
                    }
                }
                InventoryAssociated[] ial = FindObjectsByType<InventoryAssociated>(FindObjectsSortMode.None);
                Vector3 pos = Managers.GetManager<PlayersManager>().GetActivePlayerController().transform.position;

                Dbgl($"got {ial.Length} inventories");

                for (int i = 0; i < ial.Length; i++)
                {

                    var dist = Vector3.Distance(ial[i].transform.position, pos);
                    if (ial[i].name.Contains("Golden Container") || (!pullFromChests.Value && ial[i].name.Contains("Container1")) || dist > range.Value)
                        continue;
                    Inventory inventory = AccessTools.FieldRefAccess<InventoryAssociated, Inventory>(ial[i], "inventory");

                    if (inventory is null || inventory == Managers.GetManager<PlayersManager>().GetActivePlayerController().GetPlayerBackpack().GetInventory())
                        continue;

                    Dbgl($"checking close inventory {ial[i].name}: {ial[i].transform.position}, {pos}: {dist}m");
                    skip = true;
                    List<bool> hasItems = inventory.ItemsContainsStatus(groupsCopy);
                    skip = false;
                    List<Group> thisGroups = new List<Group>();
                    for (int j = 0; j < hasStatus.Count; j++)
                    {
                        if (!hasStatus[j] && groupsCopy.Contains(_groups[j]) && hasItems[groupsCopy.IndexOf(_groups[j])])
                        {
                            Dbgl($"\tFound item {_groups[j].GetId()} in {ial[i].name}");
                            hasStatus[j] = true;
                            thisGroups.Add(_groups[j]);
                            hasItems.RemoveAt(groupsCopy.IndexOf(_groups[j]));
                            groupsCopy.Remove(_groups[j]);
                        }
                    }
                    foreach (Group group in thisGroups)
                    {
                        for (int j = inventory.GetInsideWorldObjects().Count - 1; j > -1; j--)
                        {
                            Dbgl($"\thas {inventory.GetInsideWorldObjects()[j].GetGroup().GetId()}");

                            if (inventory.GetInsideWorldObjects()[j].GetGroup() == group)
                            {
                                inventory.RemoveItem(inventory.GetInsideWorldObjects()[j], false);
                                Dbgl($"\tremoved {group.GetId()}");
                                break;
                            }
                        }
                    }
                    if (!hasStatus.Contains(false))
                    {
                        Dbgl($"removed all missing items");
                        return;
                    }
                }
            }
        }

        [HarmonyPatch(typeof(Inventory), nameof(Inventory.ItemsContainsStatus))]
        private static class Inventory_ContainsItems_Patch
        {
            static void Postfix(Inventory __instance, List<bool> __result, List<Group> _groups)
            {
                if (!modEnabled.Value || skip || __instance != Managers.GetManager<PlayersManager>().GetActivePlayerController().GetPlayerBackpack().GetInventory() || !__result.Contains(false) || Managers.GetManager<GameSettingsHandler>().GetCurrentGameSettings().GetFreeCraft())
                    return;
                List<Group> groupsCopy = new List<Group>();
                groupsCopy.AddRange(_groups);
                //Dbgl($"checking status for missing items:");

                for (int j = 0; j < __result.Count; j++)
                {
                    if (!__result[j])
                    {
                        //Dbgl($"{groupsCopy[j].GetId()}");
                    }
                }


                InventoryAssociated[] ial = FindObjectsByType<InventoryAssociated>(FindObjectsSortMode.None);
                Vector3 pos = Managers.GetManager<PlayersManager>().GetActivePlayerController().transform.position;

                //Dbgl($"got {ial.Length} inventories");

                for (int i = 0; i < ial.Length; i++)
                {
                    var dist = Vector3.Distance(ial[i].transform.position, pos);
                    if (ial[i].name.Contains("Golden Container") || (!pullFromChests.Value && ial[i].name.Contains("Container1")) || dist > range.Value)
                    {
                        //Dbgl($"can't use {ial[i].name}; pfc {pullFromChests.Value}, dist {dist}/{range.Value} ");
                        continue;
                    }
                    Inventory inventory = AccessTools.FieldRefAccess<InventoryAssociated, Inventory>(ial[i], "inventory");

                    if (inventory is null || inventory == Managers.GetManager<PlayersManager>().GetActivePlayerController().GetPlayerBackpack().GetInventory())
                        continue;

                    //Dbgl($"checking close inventory {ial[i].name}: {ial[i].transform.position}, {pos}: {dist}m");
                    skip = true;
                    List<bool> hasItems = inventory.ItemsContainsStatus(groupsCopy);
                    skip = false;
                    for (int j = 0; j < __result.Count; j++)
                    {
                        if (!__result[j] && groupsCopy.Contains(_groups[j]) && hasItems[groupsCopy.IndexOf(_groups[j])])
                        {
                            //Dbgl($"Found item {_groups[j].GetId()} in {ial[i].name}");
                            __result[j] = true;
                            hasItems.RemoveAt(groupsCopy.IndexOf(_groups[j]));
                            groupsCopy.Remove(_groups[j]);
                        }
                    }
                    if (!__result.Contains(false))
                    {
                        //Dbgl($"found all items");
                        return;
                    }

                }
            }
        }
    }
}