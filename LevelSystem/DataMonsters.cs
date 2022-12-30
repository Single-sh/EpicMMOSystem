using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Remoting.Messaging;
using System.Threading;
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
    private static string MonsterDB = "";
    public static List<string> MonsterDBL;

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

    private static void createNewDataMonsters(string json) // multiplayer sync
    {
        /*
        dictionary.Clear();
        var monsters = fastJSON.JSON.ToObject<Monster[]>(json);

        foreach (var monster in monsters)
        {
            if (EpicMMOSystem.extraDebug.Value) 
                EpicMMOSystem.MLLogger.LogInfo($"{monster.name}(Clone)");

            dictionary.Add($"{monster.name}(Clone)", monster);
        }
        */ // No need already loaded from list json
    }

    private static void createNewDataMonsters(List<string> json)
    {
        dictionary.Clear();

            foreach (var monster2 in json)
            {
            if (EpicMMOSystem.extraDebug.Value)
                EpicMMOSystem.MLLogger.LogInfo($"/n Json loading /n");

                //var temp = JsonUtility.FromJson<Monster[]>(monster2);
                var temp = (fastJSON.JSON.ToObject<Monster[]>(monster2));
                foreach (var monster in temp)
                {
                    if (EpicMMOSystem.extraDebug.Value)
                        EpicMMOSystem.MLLogger.LogInfo($"{monster.name}");

                    dictionary.Add($"{monster.name}(Clone)", monster);
                }
            
        }
     
    }

    public static void Init()
    {
        var versionpath = Path.Combine(Paths.ConfigPath, EpicMMOSystem.ModName, $"Version.txt");
        var folderpath = Path.Combine(Paths.ConfigPath, EpicMMOSystem.ModName);
        var json = "MonsterDB_Default.json";
        var json1 = "MonsterDB_AirAnimals.json";
        var json2 = "MonsterDB_LandAnimals.json";
        var json3 = "MonsterDB-Fantasy-Creatures.json";
        var json4 = "MonsterDB_SeaAnimals.json";
        var json5 = "MonsterDB_MonsterLabZ.json";
        var json6 = "MonsterDB_Outsiders.json";
        var json7 = "MonsterDB_DoorDieMonsters.json";
        var json8 = "MonsterDB_MajesticChickens.json";

        if (!Directory.Exists(folderpath)){
            Directory.CreateDirectory(folderpath);
        }
        var cleartowrite = true;
        if (File.Exists(versionpath))
        {
            //MonsterDB = File.ReadAllText(path);
            var filev = File.ReadAllText(versionpath);
            cleartowrite = false; // default is false because it exists in the first place

            if (filev == "1.4.0")
                cleartowrite = true;
            if (filev == "1.4.1")
                cleartowrite = true;
            if (filev == "1.5.0")
                cleartowrite = true;

            if (filev == "1.5.3")
                cleartowrite = false;

            if (filev == "NO" || filev == "no" || filev == "No" || filev == "STOP" || filev == "stop" || filev == "Stop")
            {// don't update
                cleartowrite = false;
            }

        }
        if (cleartowrite)
        {
            //list.Clear();
            File.WriteAllText(versionpath, "1.5.3"); // Write Version file

            File.WriteAllText(Path.Combine(folderpath, json), getDefaultJsonMonster(json));

            File.WriteAllText(Path.Combine(folderpath, json1), getDefaultJsonMonster(json1));

            File.WriteAllText(Path.Combine(folderpath, json2), getDefaultJsonMonster(json2));

            File.WriteAllText(Path.Combine(folderpath, json3), getDefaultJsonMonster(json3));

            File.WriteAllText(Path.Combine(folderpath, json4), getDefaultJsonMonster(json4));

            File.WriteAllText(Path.Combine(folderpath, json5), getDefaultJsonMonster(json5));

            File.WriteAllText(Path.Combine(folderpath, json6), getDefaultJsonMonster(json6));

            File.WriteAllText(Path.Combine(folderpath, json7), getDefaultJsonMonster(json7));

            File.WriteAllText(Path.Combine(folderpath, json8), getDefaultJsonMonster(json8));


            if (EpicMMOSystem.extraDebug.Value)
                EpicMMOSystem.MLLogger.LogInfo($"Mobs Written");
        }
        List<string> list = new List<string>();
        foreach (string file in Directory.GetFiles(folderpath, "*.json", SearchOption.AllDirectories))
        { 
            var nam = Path.GetFileName(file);
            if (EpicMMOSystem.extraDebug.Value)
                EpicMMOSystem.MLLogger.LogInfo(nam + " read");

            var temp = File.ReadAllText(file);
            list.Add(temp);
            MonsterDB += temp;
           
        }
        if (EpicMMOSystem.extraDebug.Value)
            EpicMMOSystem.MLLogger.LogInfo($"Mobs Read");


        MonsterDBL = list;
        createNewDataMonsters(list);
        
    }

    private static string getDefaultJsonMonster(string jsonname)
    {
        var assembly = Assembly.GetExecutingAssembly();
        string resourceName = assembly.GetManifestResourceNames()
            .Single(str => str.EndsWith(jsonname));

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
                new Action<long, List<string>>(SetMonsterDB));
        }
    }

    public static void SetMonsterDB(long peer, List<string> json)
    {
        createNewDataMonsters(json);
    }

    [HarmonyPatch(typeof(ZNet), "RPC_PeerInfo")]
    private static class ZnetSyncServerInfo
    {
        private static void Postfix(ZRpc rpc)
        {
            if (!EpicMMOSystem._isServer) return; // doesn't work on Coop
            //if (!(ZNet.instance.IsServer() && ZNet.instance.IsDedicated())) return;
            ZNetPeer peer = ZNet.instance.GetPeer(rpc);
            if(peer == null) return;
            ZRoutedRpc.instance.InvokeRoutedRPC(peer.m_uid, $"{EpicMMOSystem.ModName} SetMonsterDB", MonsterDBL); //sync list
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
    [HarmonyPriority(1)] //almost last
    public static class MonsterColorTexts
    {
        public static void Postfix(EnemyHud __instance, Character c, Dictionary<Character, EnemyHud.HudData> ___m_huds, bool __state)
        {
            if (c.IsTamed()) return; // might remove this in future
            if (!EpicMMOSystem.enabledLevelControl.Value) return;
            if (!contains(c.gameObject.name)) return;
            Transform go = ___m_huds[c].m_gui.transform.Find("Name/Name(Clone)");
            if (go) return;
            int maxLevelExp = LevelSystem.Instance.getLevel() + EpicMMOSystem.maxLevelExp.Value;
            int minLevelExp = LevelSystem.Instance.getLevel() - EpicMMOSystem.minLevelExp.Value;
            int monsterLevel = getLevel(c.gameObject.name) + c.m_level - 1;
            GameObject component = ___m_huds[c].m_gui.transform.Find("Name").gameObject;
            GameObject levelName = Object.Instantiate(component, component.transform);
            levelName.GetComponent<RectTransform>().anchoredPosition = new Vector2(40,-30);
            if (c.m_boss)
            {
                levelName.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 30);
            }
            
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
        [HarmonyPriority(1)] // maybe stop epic loot? Last is 0, so 1 will be almost last for any other mod

        public static void Postfix(CharacterDrop __instance, ref List<KeyValuePair<GameObject, int>> __result)
        {
            if (__instance.m_character.IsTamed()) return;
            if (EpicMMOSystem.enabledLevelControl.Value && (EpicMMOSystem.removeDropMax.Value || EpicMMOSystem.removeDropMin.Value || EpicMMOSystem.removeBossDropMax.Value || EpicMMOSystem.removeBossDropMin.Value))
            {
                var playerLevel = __instance.m_character.m_nview.GetZDO().GetInt("epic playerLevel");
                if (playerLevel == 0) return;
                if (!contains(__instance.m_character.gameObject.name)) return;
                var Regmob = true;
                if (EpicMMOSystem.extraDebug.Value) 
                    EpicMMOSystem.MLLogger.LogInfo("Player level " +playerLevel);
                if (playerLevel > 0) // postive so boss
                {
                    Regmob = false;
                }else // reg mobs
                {
                    Regmob = true;
                    playerLevel = -playerLevel;
                }
                int maxLevelExp = playerLevel + EpicMMOSystem.maxLevelExp.Value;
                int minLevelExp = playerLevel - EpicMMOSystem.minLevelExp.Value;
                int monsterLevel = getLevel(__instance.m_character.gameObject.name) + __instance.m_character.m_level - 1; // interesting that it's using m_char as well
                if ((monsterLevel > maxLevelExp) && (EpicMMOSystem.removeBossDropMax.Value && !Regmob || EpicMMOSystem.removeDropMax.Value && Regmob))
                {
                    __result = new();
                }
                if ((monsterLevel < minLevelExp) && (EpicMMOSystem.removeBossDropMin.Value && !Regmob || EpicMMOSystem.removeDropMin.Value && Regmob))
                {
                    __result = new();
                }
            }
        }
    }
}