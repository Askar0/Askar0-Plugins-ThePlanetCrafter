/*
** +========================================================================================+
**  (Bepinex Mod Plugin Library for the Unity/Steam Game - The Planet Crafter by Miju Games)
**    -  Copyright (C) 2023 By Aedenthorn (Revised by Askar0)
**    -  Repository:  https://github.com/Askar0/Askar0-Plugins-ThePlanetCrafter
**
**   Code Compatibility Patch for FreeBuild/CreativeMode
**   Source: https://github.com/aedenthorn/PlanetCrafterMods/tree/master/AedenthornUtils
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

using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace AedenthornUtils
{
    public class AedenthornUtils
    {
        public static bool CheckKeyDown(string value)
        {
            try
            {
                return Input.GetKeyDown(value.ToLower());
            }
            catch
            {
                return false;
            }
        }
        public static bool CheckKeyUp(string value)
        {
            try
            {
                return Input.GetKeyUp(value.ToLower());
            }
            catch
            {
                return false;
            }
        }
        public static bool CheckKeyHeld(string value, bool req = true)
        {
            try
            {
                return Input.GetKey(value.ToLower());
            }
            catch
            {
                return !req;
            }
        }

        public static void ShuffleList<T>(List<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = Random.Range(0, n);
                // var value = list[k];
                // list[k] = list[n];
                // list[n] = value;
                (list[n], list[k]) = (list[k], list[n]); // 0: Warning IDE0180 fix
            }
        }
        public static string GetAssetPath(object obj, bool create = false)
        {
            return GetAssetPath(obj.GetType().Namespace, create);
        }
        public static string GetAssetPath(string name, bool create = false)
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), name);
            if (create && !Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }
        public static string GetTransformPath(Transform t)
        {
            if (!t.parent)
            {
                return t.name;

            }
            return GetTransformPath(t.parent) + "/" + t.name;
        }

    }
}