using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Remoting.Messaging;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
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
    
    // [HarmonyPatch(typeof(Character), nameof(Character.GetHoverName))]
    // [HarmonyPriority(Priority.First)]
    // public static class MonsterColorText
    // {
    //     public static void Postfix(Character __instance, ref string __result)
    //     {
    //         if (!contains(__instance.gameObject.name)) return;
    //         int maxLevelExp = LevelSystem.Instance.getLevel() + EpicMMOSystem.maxLevelExp.Value;
    //         int minLevelExp = LevelSystem.Instance.getLevel() + EpicMMOSystem.minLevelExp.Value;
    //         int monsterLevel = getLevel(__instance.gameObject.name);
    //         if (monsterLevel > maxLevelExp)
    //         {
    //             __result = $"<color=red>{__result} [{monsterLevel}]</color>";
    //         } else if (monsterLevel < minLevelExp)
    //         {
    //             __result = $"<color=#2FFFDC>{__result} [{monsterLevel}]</color>";
    //         }
    //         else
    //         {
    //             __result = $"{__result} [{monsterLevel}]";
    //         }
    //     }
    // }
    
    [HarmonyPatch(typeof(EnemyHud), nameof(EnemyHud.ShowHud))]
    [HarmonyPriority(Priority.Last)]
    public static class MonsterColorTexts
    {
        public static void Postfix(EnemyHud __instance, Character c, Dictionary<Character, EnemyHud.HudData> ___m_huds, bool __state)
        {
            if (!contains(c.gameObject.name)) return;
            Transform go = ___m_huds[c].m_gui.transform.Find("Name/Name(Clone)");
            if (go) return;
            int maxLevelExp = LevelSystem.Instance.getLevel() + EpicMMOSystem.maxLevelExp.Value;
            int minLevelExp = LevelSystem.Instance.getLevel() - EpicMMOSystem.minLevelExp.Value;
            int monsterLevel = getLevel(c.gameObject.name) + c.m_level - 1;
            GameObject component = ___m_huds[c].m_gui.transform.Find("Name").gameObject;
            GameObject levelName = Object.Instantiate(component, component.transform);
            levelName.GetComponent<RectTransform>().anchoredPosition = new Vector2(37, -30);
            levelName.GetComponent<Text>().text = $"[{monsterLevel}]";
            Color color = monsterLevel > maxLevelExp ? Color.red : Color.white;
            if (monsterLevel < minLevelExp) color = Color.cyan;
            component.GetComponent<Text>().color = color;
            levelName.GetComponent<Text>().color = color;
            if (___m_huds[c].m_gui.transform.Find("extraeffecttext"))
            {
                ___m_huds[c].m_gui.transform.Find("extraeffecttext").GetComponent<Text>().color = color;
            }

        }
        
        [HarmonyPatch(typeof(EnemyHud), nameof(EnemyHud.UpdateHuds))]
        public static class StarVisibility
        {
            private static void Postfix(Dictionary<Character, EnemyHud.HudData> ___m_huds)
            {
                foreach (KeyValuePair<Character, EnemyHud.HudData> keyValuePair in ___m_huds)
                {
                    Character key = keyValuePair.Key;
                    if (key != null && keyValuePair.Value.m_gui)
                    {
                        if (!contains(key.gameObject.name)) return;
                        int maxLevelExp = LevelSystem.Instance.getLevel() + EpicMMOSystem.maxLevelExp.Value;
                        int minLevelExp = LevelSystem.Instance.getLevel() - EpicMMOSystem.minLevelExp.Value;
                        int monsterLevel = getLevel(key.gameObject.name) + key.m_level - 1;
                        Transform transform = keyValuePair.Value.m_gui.transform.Find("Name/Name(Clone)");
                        if (transform != null)
                        {
                            transform.gameObject.SetActive(true);
                        }
                        else
                        {
                            GameObject component = keyValuePair.Value.m_gui.transform.Find("Name").gameObject;
                            transform = Object.Instantiate(component, component.transform).transform;
                            transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(37, -30);
                            transform.GetComponent<Text>().text = $"[{monsterLevel}]";
                        }
                        Color color = monsterLevel > maxLevelExp ? Color.red : Color.white;
                        if (monsterLevel < minLevelExp) color = Color.cyan;
                        transform.GetComponent<Text>().color = color;
                        keyValuePair.Value.m_gui.transform.Find("Name").GetComponent<Text>().color = color;
                        if (keyValuePair.Value.m_gui.transform.Find("extraeffecttext"))
                        {
                            keyValuePair.Value.m_gui.transform.Find("extraeffecttext").GetComponent<Text>().color = color;
                        }
                    }
                }
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
            int monsterLevel = getLevel(__instance.m_character.gameObject.name) + __instance.m_character.m_level - 1;
            if (monsterLevel > maxLevelExp)
            {
                __result = new();
            }
        }
    }
}