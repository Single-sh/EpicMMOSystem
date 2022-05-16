using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using HarmonyLib;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EpicMMOSystem;


public static class DataMonsters
{
    private static Dictionary<string, Monster> dictionary = new();

    public static bool contains(string name)
    {
        return dictionary.ContainsKey(name);
    }

    public static int getExp(string name)
    {
        var monster = dictionary[name];
        int exp = Random.Range(monster.minExp, monster.maxExp);
        return exp;
    }

    public static int getMaxExp(string name)
    {
        return dictionary[name].maxExp;
    }

    public static int getLevel(string name)
    {
        return dictionary[name].level;
    }

    public static void createNewDataMonsters(string json)
    {
        dictionary.Clear();
        var monsters = fastJSON.JSON.ToObject<Monster[]>(json);
        foreach (var monster in monsters)
        {
            dictionary.Add($"{monster.name}(Clone)", monster);
        }
    }

    public static string getDefaultJsonMonster()
    {
        var assembly = Assembly.GetExecutingAssembly();
        string resourceName = assembly.GetManifestResourceNames()
            .Single(str => str.EndsWith("MonstersDB.json"));

        using (Stream stream = assembly.GetManifestResourceStream(resourceName))
        using (StreamReader reader = new StreamReader(stream))
        {
            return reader.ReadToEnd();
        }
    }
    
    [HarmonyPatch(typeof(Character), nameof(Character.GetHoverName))]
    [HarmonyPriority(Priority.Last)]
    public static class MonsterColorText
    {
        public static void Postfix(Character __instance, ref string __result)
        {
            if (!contains(__instance.gameObject.name)) return;
            int maxLevelExp = LevelSystem.Instance.getLevel() + EpicMMOSystem.maxLevelExp.Value;
            int minLevelExp = LevelSystem.Instance.getLevel() + EpicMMOSystem.minLevelExp.Value;
            int monsterLevel = getLevel(__instance.gameObject.name);
            if (monsterLevel > maxLevelExp)
            {
                __result = $"<color=red>{__result} [{monsterLevel}]</color>";
            } else if (monsterLevel < minLevelExp)
            {
                __result = $"<color=#A6A6A6>{__result} [{monsterLevel}]</color>";
            }
            else
            {
                __result = $"{__result} [{monsterLevel}]";
            }
        }
    }
    
    [HarmonyPatch(typeof(CharacterDrop), nameof(CharacterDrop.GenerateDropList))]
    public static class MonsterDropGenerate
    {
        public static void Postfix(CharacterDrop __instance, ref List<KeyValuePair<GameObject, int>> __result)
        {
            if (!contains(__instance.m_character.gameObject.name)) return;
            int maxLevelExp = LevelSystem.Instance.getLevel() + EpicMMOSystem.maxLevelExp.Value;
            int monsterLevel = getLevel(__instance.m_character.gameObject.name);
            if (monsterLevel > maxLevelExp)
            {
                __result = new();
            }
        }
    }
}