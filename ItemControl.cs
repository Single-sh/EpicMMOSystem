using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace EpicMMOSystem;

public partial class EpicMMOSystem
{
    [HarmonyPatch(typeof(ZNetScene), nameof(ZNetScene.Awake))]
    private static class InitCustomItemsClass
    {
        private static void Postfix(ZNetScene __instance)
        {
            var vfx = _asset.LoadAsset<GameObject>("LevelUpVFX");
            __instance.m_prefabs.Add(vfx);
            __instance.m_namedPrefabs.Add(vfx.name.GetStableHashCode(), vfx);
        }
    }
    
    // Не выводить сохраненные значения в компендиум
     [HarmonyPatch(typeof(Player), nameof(Player.GetKnownTexts))]
     private static class FixCompendium
     {
         private static void Postfix(ref List<KeyValuePair<string, string>> __result)
         {
             __result = __result.Where(p => !p.Key.StartsWith(ModName)).ToList();
         }
     }
}