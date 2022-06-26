using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Remoting.Messaging;
using BepInEx;
using fastJSON;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace EpicMMOSystem;


public static class DataMonsters
{
    private static Dictionary<string, Monster> dictionary = new();
    private static string MonsterDB = "[]";

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

    private static void createNewDataMonsters(string json)
    {
        dictionary.Clear();
        var monsters = fastJSON.JSON.ToObject<Monster[]>(json);
        foreach (var monster in monsters)
        {
            dictionary.Add($"{monster.name}(Clone)", monster);
        }
    }

    public static void Init()
    {
        var path = Path.Combine(Paths.PluginPath, EpicMMOSystem.ModName, $"MonsterDB_{2}.json");
        if (File.Exists(path))
        {
            MonsterDB = File.ReadAllText(path);
            
        }
        else
        {
            var pathOld = Path.Combine(Paths.PluginPath, EpicMMOSystem.ModName, $"MonsterDB.json");
            if (!File.Exists(pathOld))
            {
                var json = getDefaultJsonMonster();
                DirectoryInfo dir = new DirectoryInfo(Paths.PluginPath);
                dir.CreateSubdirectory(Path.Combine(Paths.PluginPath, EpicMMOSystem.ModName));
                File.WriteAllText(path,json);
                MonsterDB = json;
            }
            else
            {
                string newJson = getDefaultJsonMonster();
                createNewDataMonsters(newJson);
                string jsonOld = File.ReadAllText(pathOld);
                var monsters = fastJSON.JSON.ToObject<MonsterOld[]>(jsonOld);
                foreach (var monster in monsters)
                {
                    if (dictionary.ContainsKey($"{monster.name}(Clone)"))
                    {
                        var m = dictionary[$"{monster.name}(Clone)"];
                        m.minExp = monster.minExp;
                        m.maxExp = monster.maxExp;
                        dictionary[$"{monster.name}(Clone)"] = m;
                    }
                    else
                    {
                        dictionary.Add(
                            $"{monster.name}(Clone)", 
                            new Monster(
                                monster.name, 
                                monster.minExp, 
                                monster.maxExp, 
                                1)
                            );
                    }
                    
                }

                var obj = dictionary.Values.ToArray();
                string text = JSON.ToJSON(obj, new JSONParameters(){UseExtensions = false});
                File.WriteAllText(path,text);
                return;
            }
            
        }
        createNewDataMonsters(MonsterDB);
    }

    private static string getDefaultJsonMonster()
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
    
    [HarmonyPatch(typeof(ZNetScene), nameof(ZNetScene.Awake))]
    private static class ZrouteMethodsServerFeedback
    {
        private static void Postfix()
        {
            if (EpicMMOSystem._isServer) return;
            ZRoutedRpc.instance.Register($"{EpicMMOSystem.ModName} SetMonsterDB",
                new Action<long, string>(SetMonsterDB));
        }
    }

    private static void SetMonsterDB(long peer, string json)
    {
        createNewDataMonsters(json);
    }

    [HarmonyPatch(typeof(ZNet), "RPC_PeerInfo")]
    private static class ZnetSyncServerInfo
    {
        private static void Postfix(ZRpc rpc)
        {
            if (!EpicMMOSystem._isServer) return;
            if (!(ZNet.instance.IsServer() && ZNet.instance.IsDedicated())) return;
            ZNetPeer peer = ZNet.instance.GetPeer(rpc);
            if(peer == null) return;
            ZRoutedRpc.instance.InvokeRoutedRPC(peer.m_uid, $"{EpicMMOSystem.ModName} SetMonsterDB", MonsterDB);
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
            if (c.IsTamed()) return;
            if (!EpicMMOSystem.enabledLevelControl.Value) return;
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
                if (!EpicMMOSystem.enabledLevelControl.Value) return;
                foreach (KeyValuePair<Character, EnemyHud.HudData> keyValuePair in ___m_huds)
                {
                    Character key = keyValuePair.Key;
                    if (key.IsTamed()) return;
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
                            transform.GetComponent<Text>().fontSize = 13;
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
            if (__instance.m_character.IsTamed()) return;
            if (EpicMMOSystem.enabledLevelControl.Value && EpicMMOSystem.removeDrop.Value)
            {
                var playerLevel = __instance.m_character.m_nview.GetZDO().GetInt("epic playerLevel");
                if (playerLevel == 0) return;
                if (!contains(__instance.m_character.gameObject.name)) return;
                int maxLevelExp = playerLevel + EpicMMOSystem.maxLevelExp.Value;
                int monsterLevel = getLevel(__instance.m_character.gameObject.name) + __instance.m_character.m_level - 1;
                if (monsterLevel > maxLevelExp)
                {
                    __result = new();
                }
            }
        }
    }
}