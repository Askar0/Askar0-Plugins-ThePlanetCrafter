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

using HarmonyLib;
using SpaceCraft;
using UnityEngine;

namespace Askar0_Plugins_CheatPlayerGaugeStopper
{
    internal class Patches
    {
        [HarmonyPatch(typeof(PlayerGaugesHandler), "SetOutsideOxygenChangeValue")]
        class SkipOxygenCalc
        {
            static bool Prefix()
            {
                return Application.isFocused;
            }
        }

        [HarmonyPatch(typeof(PlayerGaugeHealth), nameof(PlayerGaugeHealth.AddToCurrentValue))]
        class AFKHealth
        {
            static bool Prefix()
            {
                return Application.isFocused;
            }
        }

        [HarmonyPatch(typeof(PlayerGaugeThirst), nameof(PlayerGaugeThirst.AddToCurrentValue))]
        class AFKThirst
        {
            static bool Prefix()
            {
                return Application.isFocused;
            }
        }

        [HarmonyPatch(typeof(PlayerGaugeOxygen), nameof(PlayerGaugeOxygen.AddToCurrentValue))]
        class AFKOxygen
        {
            static bool Prefix()
            {
                return Application.isFocused;
            }
        }
    }
}