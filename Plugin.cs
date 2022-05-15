using System;
using System.IO;
using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using ServerSync;
using UnityEngine;
using UnityEngine.Rendering;

namespace EpicMMOSystem;

[BepInPlugin(ModGUID, ModName, ModVersion)]
public partial class EpicMMOSystem : BaseUnityPlugin
{
    internal const string ModName = "EpicMMOSystem";
    internal const string ModVersion = "1.0.0";
    internal const string Author = "LambaSun";
    private const string ModGUID = Author + "." + ModName;
    private static string ConfigFileName = ModGUID + ".cfg";
    private static string ConfigFileFullPath = Paths.ConfigPath + Path.DirectorySeparatorChar + ConfigFileName;
    private static bool _isServer => SystemInfo.graphicsDeviceType == GraphicsDeviceType.Null;
    internal static string ConnectionError = "";

    private readonly Harmony _harmony = new(ModGUID);

    public static readonly ManualLogSource MLLogger =
        BepInEx.Logging.Logger.CreateLogSource(ModName);

    private static readonly ConfigSync ConfigSync = new(ModGUID)
        { DisplayName = ModName, CurrentVersion = ModVersion, MinimumRequiredVersion = ModVersion };

    public static AssetBundle _asset;

    public static Localization localization;
    private static string MonsterDB = "[]";
    //Config
    public static ConfigEntry<string> language;
    //LevelSystem
    public static ConfigEntry<int> maxLevel;
    public static ConfigEntry<int> priceResetPoints;
    public static ConfigEntry<int> freePointForLevel;
    public static ConfigEntry<int> startFreePoint;
    public static ConfigEntry<int> levelExp;
    public static ConfigEntry<float> multiNextLevel;
    public static ConfigEntry<float> expForLvlMonster;
    public static ConfigEntry<int> rateExp;
    public static ConfigEntry<float> groupExp;
    
    #region Parameters
    //LevelSystem arg property <Strength>
    public static ConfigEntry<float> physicDamage;
    public static ConfigEntry<int> addWeight;
    
    //LevelSystem arg property <Agility>
    public static ConfigEntry<float> speedAttack;
    public static ConfigEntry<float> staminaReduction;
    public static ConfigEntry<float> addStamina;
    
    //LevelSystem arg property <Intellect>
    public static ConfigEntry<float> magicDamage;
    public static ConfigEntry<float> magicArmor;
    
    //LevelSystem arg property <Body>
    public static ConfigEntry<float> addHp;
    public static ConfigEntry<float> staminaBlock;
    public static ConfigEntry<float> physicArmor;
    public static ConfigEntry<float> regenHp;
    #endregion
    
    public void Awake()
    {
        string general = "0.General";
        _serverConfigLocked = config(general, "Force Server Config", true, "Force Server Config");
        language = config(general, "Language", "eng", "Language prefix");
        string levelSystem = "1.LevelSystem";
        maxLevel = config(levelSystem, "MaxLevel", 100, "Максимальный уровень");
        priceResetPoints = config(levelSystem, "PriceResetPoints", 3, "Цена сброса за один поинт");
        freePointForLevel = config(levelSystem, "FreePointForLevel", 5, "Свободных поинтов за один уровень");
        startFreePoint = config(levelSystem, "StartFreePoint", 5, "Доболнительных свободных поинтов");
        levelExp = config(levelSystem, "FirstLevelExperience", 500, "Количество опыта необходимого на 1 уровень");
        multiNextLevel = config(levelSystem, "MultiplyNextLevelExperience", 1.04f, "Умножитель опыта для следующего уровня");
        expForLvlMonster = config(levelSystem, "ExpForLvlMonster", 0.25f, "Доп опыт (из суммы основного опыта) за уровень монстра");
        rateExp = config(levelSystem, "RateExp", 1, "Множитель опыта");
        groupExp = config(levelSystem, "GroupExp", 0.70f, "Множитель опыта который получают остальные игроки в группе");
        
        #region ParameterCofig
        string levelSystemStrngth = "1.LevelSystem Strength";
        physicDamage = config(levelSystemStrngth, "PhysicDamage", 0.20f, "Умножитель урона за один поинт");
        addWeight = config(levelSystemStrngth, "AddWeight", 2, "Добавляет переносимый вес за один поинт");
        
        string levelSystemAgility = "1.LevelSystem Agility";
        speedAttack = config(levelSystemAgility, "StaminaAttack", 0.1f, "Уменьшает потребление стамины на атаку");
        staminaReduction = config(levelSystemAgility, "StaminaReduction", 0.15f, "Уменьшение расхода выносливости на бег, прыжок за один поинт");
        addStamina = config(levelSystemAgility, "AddStamina", 1f, "Увелечение выносливости за один поинт");
        
        string levelSystemIntellect = "1.LevelSystem Intellect";
        magicDamage = config(levelSystemIntellect, "MagicAttack", 0.20f, "Увелечение магической атаки за один поинт");
        magicArmor = config(levelSystemIntellect, "MagicArmor", 0.1f, "Увелечение магической защиты за один поинт");
        
        string levelSystemBody = "1.LevelSystem Body";
        addHp = config(levelSystemBody, "AddHp", 2f, "Увелечение здоровья за один поинт");
        staminaBlock = config(levelSystemBody, "StaminaBlock", 0.2f, "Уменьшение расхода выносливости на блок за один поинт");
        physicArmor = config(levelSystemBody, "PhysicArmor", 0.15f, "Увелечение физической защиты за один поинт");
        regenHp = config(levelSystemBody, "RegenHp", 0.1f, "Увелечение регенерации здоровья за один поинт");
        #endregion
        _ = ConfigSync.AddLockingConfigEntry(_serverConfigLocked);

        Assembly assembly = Assembly.GetExecutingAssembly();
        _harmony.PatchAll(assembly);
        

        _asset = GetAssetBundle("epicasset");

        localization = new Localization();
        MyUI.Init();
    }

    private void Start()
    {
        var path = Path.Combine(Paths.PluginPath, ModName, "MonsterDB.json");
        if (!File.Exists(path))
        {
            var json = DataMonsters.getDefaultJsonMonster();
            DirectoryInfo dir = new DirectoryInfo(Paths.PluginPath);
            dir.CreateSubdirectory(Path.Combine(Paths.PluginPath, ModName));
            File.WriteAllText(path,json);
            MonsterDB = json;
        }
        else
        {
            MonsterDB = File.ReadAllText(path);
        }
        DataMonsters.createNewDataMonsters(MonsterDB);
    }

    private void ReadConfigValues(object sender, FileSystemEventArgs e)
    {
        if (!File.Exists(ConfigFileFullPath)) return;
        try
        {
            MLLogger.LogDebug("ReadConfigValues called");
            Config.Reload();
        }
        catch
        {
            MLLogger.LogError($"There was an issue loading your {ConfigFileName}");
            MLLogger.LogError("Please check your config entries for spelling and format!");
        }
    }

    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Mouse4))
    //     {
    //         // LevelSystem.Instance.ResetAllParameter();
    //         PlayerFVX.levelUp();
    //     }
    // }


    #region ConfigOptions

    private static ConfigEntry<bool>? _serverConfigLocked;

    private ConfigEntry<T> config<T>(string group, string name, T value, ConfigDescription description,
        bool synchronizedSetting = true)
    {
        ConfigDescription extendedDescription =
            new(
                description.Description +
                (synchronizedSetting ? " [Synced with Server]" : " [Not Synced with Server]"),
                description.AcceptableValues, description.Tags);
        ConfigEntry<T> configEntry = Config.Bind(group, name, value, extendedDescription);
        //var configEntry = Config.Bind(group, name, value, description);

        SyncedConfigEntry<T> syncedConfigEntry = ConfigSync.AddConfigEntry(configEntry);
        syncedConfigEntry.SynchronizedConfig = synchronizedSetting;

        return configEntry;
    }

    private ConfigEntry<T> config<T>(string group, string name, T value, string description,
        bool synchronizedSetting = true)
    {
        return config(group, name, value, new ConfigDescription(description), synchronizedSetting);
    }

    private class ConfigurationManagerAttributes
    {
        public bool? Browsable = false;
    }

    #endregion
        
    public static AssetBundle GetAssetBundle(string filename)
    {
        var execAssembly = Assembly.GetExecutingAssembly();

        string resourceName = execAssembly.GetManifestResourceNames()
            .Single(str => str.EndsWith(filename));

        using (var stream = execAssembly.GetManifestResourceStream(resourceName))
        {
            return AssetBundle.LoadFromStream(stream);
        }
    }
    
    [HarmonyPatch(typeof(ZNetScene), nameof(ZNetScene.Awake))]
    private static class ZrouteMethodsServerFeedback
    {
        private static void Postfix()
        {
            if (_isServer) return;
            ZRoutedRpc.instance.Register($"{ModName} SetMonsterDB",
                new Action<long, string>(SetMonsterDB));
        }
    }

    private static void SetMonsterDB(long peer, string json)
    {
        DataMonsters.createNewDataMonsters(json);
    }
    
    
    [HarmonyPatch(typeof(ZNet), "RPC_PeerInfo")]
    private static class ZnetSyncServerInfo
    {
        private static void Postfix(ZRpc rpc)
        {
            if (!_isServer) return;
            if (!(ZNet.instance.IsServer() && ZNet.instance.IsDedicated())) return;
            ZNetPeer peer = ZNet.instance.GetPeer(rpc);
            if(peer == null) return;
            ZRoutedRpc.instance.InvokeRoutedRPC(peer.m_uid, $"{ModName} SetMonsterDB", MonsterDB);
        }
    }
    
    //VersionControl
    [HarmonyPatch(typeof(ZNet), nameof(ZNet.RPC_PeerInfo))]
    private static class PatchZNetRPC_PeerInfo
    {
        [HarmonyPriority(Priority.First)]
        private static bool Prefix(ZRpc rpc, ref ZPackage pkg)
        {
         
            long uid = pkg.ReadLong();
            string versionString = pkg.ReadString();
            if (ZNet.instance.IsServer())
                versionString += $"-{ModName}{ModVersion}";
            else
                versionString = versionString.Replace($"-{ModName}{ModVersion}", "");

            ZPackage newPkg = new ZPackage();
            newPkg.Write(uid);
            newPkg.Write(versionString);
            newPkg.m_writer.Write(pkg.m_reader.ReadBytes((int)(pkg.m_stream.Length - pkg.m_stream.Position)));
            pkg = newPkg;
            pkg.SetPos(0);
            return true;
        }
    }

    [HarmonyPatch(typeof(Version), nameof(Version.GetVersionString))]
    private static class PatchVersionGetVersionString
    {
        [HarmonyPriority(Priority.Last)]
        private static void Postfix(ref string __result)
        {
            if (ZNet.instance?.IsServer() == true) __result += $"-{ModName}{ModVersion}";
        }
    }
}