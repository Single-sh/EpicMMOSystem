using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using HarmonyLib;
using UnityEngine;

namespace EpicMMOSystem.StatusEffects;

public static class EffectPatches
{
    private static void AddMyStatusEffect(ObjectDB odb)
    {
        if (ObjectDB.instance == null || ObjectDB.instance.m_items.Count == 0 ||
            ObjectDB.instance.GetItemPrefab("Amber") == null) return;

        // if (!odb.m_StatusEffects.Find(se => se.name == SE_FallenGod.id))
        // {
        //     odb.m_StatusEffects.Add(ScriptableObject.CreateInstance<SE_FallenGod>());
        // }
        // if (ObjectDB.instance.GetItemPrefab(EpicMmoSystemPlugin.raidoPrefab.name.GetStableHashCode()) == null)
        // {
        //     ObjectDB.instance.m_items.Add(EpicMmoSystemPlugin.raidoPrefab);
        //     ObjectDB.instance.m_itemByHash[EpicMmoSystemPlugin.raidoPrefab.name.GetStableHashCode()] = EpicMmoSystemPlugin.raidoPrefab;
        // }

    }

    [HarmonyPatch(typeof(ObjectDB), "Awake")]
    public static class ObjectDBAwake
    {
        public static void Postfix(ObjectDB __instance)
        {
            AddMyStatusEffect(__instance);
        }
    }
    
    [HarmonyPatch(typeof(ObjectDB), "CopyOtherDB")]
    public static class ObjectDBCopyOtherDB
    {
        public static void Postfix(ObjectDB __instance)
        {
            AddMyStatusEffect(__instance);
        }
    }
}