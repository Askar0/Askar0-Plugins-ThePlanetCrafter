
# Askar0-Plugins-ThePlanetCrafter_Plugins
BepInEx+Harmony plugin/patcher/mods for the Unity/Steam game The Planet Crafter by Miju Games

Steam: https://store.steampowered.com/app/1284190/The_Planet_Crafter/

## Version <a href='https://github.com/Askar0/Askar0-Plugins-ThePlanetCrafter/releases'><img src='https://img.shields.io/github/v/release/Askar0/Askar0-Plugins-ThePlanetCrafter' alt='Latest GitHub Release Version'/></a>

:arrow_down_small: Download files from the releases: https://github.com/askar0/Askar0-Plugins-ThePlanetCrafter/releases

```
Note: 23/11/2023 - (Mods/Plugins Under Construction) - Some testing files have been uploaded. 
 - These will need testing and possibly additional updates before release versions will be made available.

Note: 15/12/2023
 - All github commits / releases / prereleases will be signed using my personal OpenPGP Key.
 - For verification all archive files will also have a signature file included for each plugin.
 - To download my OpenPGP - GnuPG Public Key use the following link.
 - https://keyserver.ubuntu.com/pks/lookup?op=get&search=0xdd6b58ca428c22a6
 - https://gpg4win.org/download.html to download the windows application(s) for handling PGP Keys.
 - **(These are not necessary to run the plugins/mods they only serve as an additional file integrity check)**

 Note: 02/01/2023
  - Made a code compatibility fix for Aedenthorn's CraftFromContainers Mod to properly handle Freebuild/Creative Mode
  - If you use my patched version remember to remove the existing CraftFromContainers Mod
  - Original Source: https://github.com/aedenthorn/PlanetCrafterMods/tree/master/CraftFromContainers
  - Adding small download with my Licence.txt, this Readme.md, My Avatar Icon and a Sample GuageChangeRate PNGImage
```

## Currently Intended Support for Game Versions:
ℹ️ Current Early Access / Experimental (Possibly)

:warning: I'll do my best to keep my mods up-to-date in case something drastic changes inside the 
  main game.

:warning: I may not have tested my mods with some versions of the developer/preview/demo releases.
  They might work just fine or suddenly break.

:warning: I cannot promise to fix my mods for these other versions as they can get quite out-of-sync 
  with the public release.

:warning: All code and compiled plugins are provided as-is with no warranty or guarantees
  implied or otherwise - use them at your own risk.

```
Note: I am still in the process of creating the form and code for my mods/plugins and currently not
 - working on the release versions yet. Please be patient.
```

## Preparation

In order to use my or anyone else's mods at this time, you need to install BepInEx first.
  The wiki has a guide for this:

Planet Crafter Modding Wiki pages: https://planet-crafter.fandom.com/wiki/Modding#Using_Mods

Guide on dnSpy-based manual patches: https://steamcommunity.com/sharedfiles/filedetails/?id=2784319459

## Installation

When installing my mods, unzip the mod into the `BepInEx\Plugins` directory, using the zipfile name
  (without the zip extension) as the folder to store the plugins in.

You should end up having a directory structure like this:

```
BepInEx\plugins\Askar0-Plugins\
		icon_202346961993.png  (My Pixie Avatar)
		LICENSE.txt (GNU General Public License) Freeware
		README.md (This File)
		Planetcrafter - Gauge RateOfChange.png (Sample gauge settings image for Askar0_Plugins_CheatPlayerGaugeChanger.dll Plugin/Mod)
BepInEx\plugins\Askar0-Plugins\SharedLibrary.Askar0.Plugin\Askar0_Plugins_SharedLibrary.dll (* Plugin Under Construction)
BepInEx\plugins\Askar0-Plugins\CheatUnderWaterBreathing.Askar0.Plugin\Askar0_Plugins_CheatUnderWaterBreathing.dll (* Plugin Under Construction)
BepInEx\plugins\Askar0-Plugins\CheatRunningSpeedChanger.Askar0.Plugin\Askar0_Plugins_CheatRunningSpeedChanger.dll (MoveSpeed Mod)
BepInEx\plugins\Askar0-Plugins\CheatPlayerGaugeStopper.Askar0.Plugin\Askar0_Plugins_CheatPlayerGaugeStopper.dll   (AFK Mod)
BepInEx\plugins\Askar0-Plugins\CheatPlayerGaugeChanger.Askar0.Plugin\Askar0_Plugins_CheatPlayerGaugeChanger.dll   (Change Health, Thirst, Oxygen Consumption Rates)
BepInEx\plugins\Askar0-Plugins\CheatUnlockAllBlueprintsPermanently.Askar0.Plugin\Askar0_Plugins_CheatUnlockAllBlueprintsPermanently.dll (Unlock Every Blueprint Permanently)
BepInEx\plugins\Askar0-Plugins\CheatCreativeModeToggle.Askar0.Plugin\Askar0_Plugins_CheatCreativeModeToggle.dll   (Creative, Unlock All Blueprints, Stop all consumption, Heal Player)
BepInEx\plugins\Askar0-Plugins\CheatDayNightToggle.Askar0.Plugin\Askar0_Plugins_CheatDayNightToggle.dll           (Toggle DayNight Cycle)  
BepInEx\plugins\Askar0-Plugins\CheatFlightModeToggle.Askar0.Plugin\Askar0_Plugins_CheatFlightModeToggle.dll       (Toggle Creative Flight Mode)
BepInEx\plugins\Askar0-Plugins\CheatMeteoritesToggle.Askar0.Plugin\Askar0_Plugins_CheatMeteoritesToggle.dll       (Toggle Random Meteorites ON/OFF)
```

Note: I intend to add folder names and subfolders to the beta or release versions of the plugins 
 - but at the moment I am just testing the mods so they may or may not already have folders setup
 - in the zipfiles. There will also be PDB debug libraries including in the testing set. 

Doing this helps avoid overwriting each others' files if they happen to be named the same as well as allows
 - removing plugin files together by deleting the directory itself.

Important!: Please remove and of my existing mods if you have them from my posts on the
planet crafter discord https://discord.gg/9YPjJ3C
Or direct downloads from my repository at https://github.com/Askar0/ThePlanetCrafter-Plugins 

I have rebuilt all the files contained there and moved the repository handling the files to:
+=+  https://github.com/Askar0/Askar0-Plugins-ThePlanetCrafter


## Mods

## Mods Descriptions

```
Planned Plugin Releases:
-----

CheatCreativeFlightmode.Askar0.Plugin.zip
+ Enables Creative Flightmode (Pressing shift also allows for zoom mode - much faster travel whilst flying).
+ Default Key H

CheatPlayerGaugeChanger.Askar0.Plugin.zip
+ Config option allowing different levels of player resource consumption based on predefined setting.
+ Example Image Add: Planetcrafter - Gauge RateOfChange.png
+ TODO: allow fully custom figures for Hunger, Thirst and Oxygen disabled currently

CheatPlayerGaugeStopper.Askar0.Plugin.zip
+ When The Planet Crafter game is not the active window will pause all loss of hunger/thirst and oxygen,
  until you have the game as the active window again.

Askar0-CheatToggleDay-NightCycle.zip
+ Toggles if daynight cycle is active, or permanent day
+ Toggle is in Config options set to true Day only, set to false(default) to cycle day/night.

CheatUnlockAllBlueprintsPermanently.Askar0.Plugin.zip
+ Permanent change: Unlocks all blueprints in a savegame. Requires manual savegame edit to remove state.
+ Config Disabled by Default, Self Disables after unlocking blueprints in a single savegame.

CheatCreativeModeToggle.Askar0.Plugin.zip
+ Toggles Creative mode and makes all blueprints available and stops all hunger/thirst and oxygen usage
  when active and slowly heals the player.
+ Config option for addition debug logging allowing it to be turned on/off.
+ Default Key K

CheatMeteoritesToggle.Askar0.Plugin.zip
+ Toggles if Random Meteorite Events Occur
+ Default Key J
---------------------


* Under Construction Plugins:
-----

CheatUnderWaterBreathing.Askar0.Plugin.zip
+ Allow for breathing underwater.
+ Toggle is in Config options set to true to Enable, (default) set to false.

SharedLibrary.Askar0.Plugins.zip
+ Contains all shared library code for all plugins
---------------------


** External Plugins (Compatibility Patch)
-----

Aedenthorn.CraftFromContainers.FreeBuildFix.Plugin.zip
+ Modified Code to properly handle Freebuild / Creative Mode
+ Minor Style Fixes
+ Default Key [Home]
Copyright © 2023 By Aedenthorn
Source: https://github.com/aedenthorn/PlanetCrafterMods/tree/master/CraftFromContainers
---------------------

Source code (zip) 
+ Source code download for all my plugins (Auto Generated by Github).
```

### Configuration

Any configuration files generated with the mod:
```
Askar0_Plugins.cfg  (* Under Construction *)
Askar0_Plugins_CheatCreativeModeToggle.cfg
Askar0_Plugins_CheatDayNightToggle.cfg
Askar0_Plugins_CheatFlightModeToggle.cfg
Askar0_Plugins_CheatMeteoritesToggle.cfg
Askar0_Plugins_CheatPlayerGaugeChanger.cfg
Askar0_Plugins_CheatPlayerGaugeStopper.cfg
Askar0_Plugins_CheatRunningSpeedChanger.cfg
Askar0_Plugins_CheatUnderWaterBreathing.cfg  (* Under Construction *)
Askar0_Plugins_CheatUnlockAllBlueprintsPermanently.cfg
```

Basic config settings for the plugins will be like the following:

```
[General]

## Is this mod enabled?
# Setting type: Boolean
# Default value: true
Enabled = true

```
