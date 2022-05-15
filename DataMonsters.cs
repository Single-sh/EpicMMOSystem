using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
}