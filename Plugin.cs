﻿using System;
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
using LocalizationManager;
using System.Collections.Generic;
using fastJSON;
using Groups;

namespace EpicMMOSystem;

[BepInPlugin(ModGUID, ModName, ModVersion)]
[BepInDependency("org.bepinex.plugins.groups", BepInDependency.DependencyFlags.SoftDependency)]
public partial class EpicMMOSystem : BaseUnityPlugin
{
    internal const string ModName = "EpicMMOSystem";
    internal const string ModVersion = "1.5.0";
    internal const string Author = "WackyMole";
    private const string ModGUID = Author + "." + ModName;
    private static string ConfigFileName = ModGUID + ".cfg";
    private static string ConfigFileFullPath = Paths.ConfigPath + Path.DirectorySeparatorChar + ConfigFileName;
    public static bool _isServer => SystemInfo.graphicsDeviceType == GraphicsDeviceType.Null;
    internal static string ConnectionError = "";
    internal static bool NetisActive = false;

    private readonly Harmony _harmony = new(ModGUID);

    public static readonly ManualLogSource MLLogger =
        BepInEx.Logging.Logger.CreateLogSource(ModName);

    public static readonly ConfigSync ConfigSync = new(ModGUID)
    { DisplayName = ModName, CurrentVersion = ModVersion, MinimumRequiredVersion = ModVersion };

    public static AssetBundle _asset;
    private string folderpath = Path.Combine(Paths.ConfigPath, EpicMMOSystem.ModName);

    internal static EpicMMOSystem Instance;

    public static Localization localization;
    //Config
    public static ConfigEntry<string> language;
    public static ConfigEntry<bool> extraDebug;
    //LevelSystem
    public static ConfigEntry<int> maxLevel;
    public static ConfigEntry<int> priceResetPoints;
    public static ConfigEntry<int> freePointForLevel;
    public static ConfigEntry<int> startFreePoint;
    public static ConfigEntry<int> levelExp;
    public static ConfigEntry<float> multiNextLevel;
    public static ConfigEntry<float> expForLvlMonster;
    public static ConfigEntry<float> rateExp;
    public static ConfigEntry<float> groupExp;
    public static ConfigEntry<bool> lossExp;
    public static ConfigEntry<float> minLossExp;
    public static ConfigEntry<float> maxLossExp;
    public static ConfigEntry<int> maxValueAttribute;
    public static ConfigEntry<string> levelsForBinusFreePoint;

    #region Parameters
    //LevelSystem arg property <Strength>
    public static ConfigEntry<float> physicDamage;
    public static ConfigEntry<float> addWeight;

    //LevelSystem arg property <Agility>
    public static ConfigEntry<float> speedAttack;
    public static ConfigEntry<float> staminaReduction;
    public static ConfigEntry<float> addStamina;

    //LevelSystem arg property <Intellect>
    public static ConfigEntry<float> magicDamage;
    public static ConfigEntry<float> magicArmor;
    public static ConfigEntry<float> MagicEitrRegen;

    //LevelSystem arg property <Body>
    public static ConfigEntry<float> addHp;
    public static ConfigEntry<float> staminaBlock;
    public static ConfigEntry<float> physicArmor;
    public static ConfigEntry<float> regenHp;
    #endregion


    //Creature level control
    public static ConfigEntry<bool> enabledLevelControl;
    public static ConfigEntry<bool> removeDropMax;
    public static ConfigEntry<bool> removeDropMin;
    public static ConfigEntry<bool> removeBossDropMax;
    public static ConfigEntry<bool> removeBossDropMin;
    public static ConfigEntry<bool> mentor;
    public static ConfigEntry<bool> curveExp;
    public static ConfigEntry<bool> curveBossExp;
    public static ConfigEntry<bool> lowDamageLevel;
    public static ConfigEntry<int> maxLevelExp;
    public static ConfigEntry<int> minLevelExp;

    //Reset attributes items
    public static ConfigEntry<String> prefabNameCoins;
    public static ConfigEntry<String> viewTextCoins;

    //Hud
    public static ConfigEntry<bool> oldExpBar;
    public static ConfigEntry<bool> showMaxHp;
    public static ConfigEntry<float> HudBarScale;
    public static ConfigEntry<string> HudExpBackgroundCol;
    public static ConfigEntry<string> HudPostionCords;
    public static ConfigEntry<bool> HealthIcons;


    // Position Saves
    internal static ConfigEntry<Vector2> HudPanelPosition = null!;
    internal static ConfigEntry<Vector2> ExpPanelPosition = null!;
    internal static ConfigEntry<Vector2> HpPanelPosition = null!;
    internal static ConfigEntry<Vector2> StaminaPanelPosition = null!;
    internal static ConfigEntry<Vector2> EitrPanelPosition = null!;

    //Optional
    public static ConfigEntry<float> addDefaultHealth;
    public static ConfigEntry<float> addDefaultWeight;

    internal static Localization english = null!;
    internal static Localization russian = null!;
    internal static Localization spanish = null!;
    public void Awake()
    {
        // Localizer.Load(); - Doesn't seem to be working with yml
        // Localizer.AddText("$attributes", "Attributes WM");
        Instance = this;

        string general = "0.General---------------";
        _serverConfigLocked = config(general, "Force Server Config", true, "Force Server Config");
        language = config(general, "Language", "eng", "Language prefix", false);
        extraDebug = config(general, "EnableExtraDebug", false, "Enable Extra Debug mode for Debugging", false);
        string levelSystem = "1.LevelSystem-----------";
        maxLevel = config(levelSystem, "MaxLevel", 100, "Maximum level. Максимальный уровень");
        priceResetPoints = config(levelSystem, "PriceResetPoints", 3, "Reset price per point. Цена сброса за один поинт");
        freePointForLevel = config(levelSystem, "FreePointForLevel", 5, "Free points per level. Свободных поинтов за один уровень");
        startFreePoint = config(levelSystem, "StartFreePoint", 5, "Additional free points start. Дополнительных свободных поинтов");
        levelExp = config(levelSystem, "FirstLevelExperience", 500, "Amount of experience needed per level. Количество опыта необходимого на 1 уровень");
        multiNextLevel = config(levelSystem, "MultiplyNextLevelExperience", 1.04f, "Experience multiplier for the next level. Умножитель опыта для следующего уровня");
        expForLvlMonster = config(levelSystem, "ExpForLvlMonster", 0.25f, "Extra experience (from the sum of the basic experience) for the level of the monster. Доп опыт (из суммы основного опыта) за уровень монстра");
        rateExp = config(levelSystem, "RateExp", 1f, "Experience multiplier. Множитель опыта");
        groupExp = config(levelSystem, "GroupExp", 0.70f, "Experience multiplier that the other players in the group get. Множитель опыта который получают остальные игроки в группе");
        minLossExp = config(levelSystem, "MinLossExp", 0.05f, "Minimum Loss Exp if player death, default 5% loss");
        maxLossExp = config(levelSystem, "MaxLossExp", 0.25f, "Maximum Loss Exp if player death, default 25% loss");
        lossExp = config(levelSystem, "LossExp", true, "Enabled exp loss");
        maxValueAttribute = config(levelSystem, "MaxValueAttribute", 200, "Maximum number of points you can put into one attribute");
        levelsForBinusFreePoint = config(levelSystem, "BonusLevelPoints", "5:5,10:5", "Added bonus point for level. Example(level:points): 5:10,15:20 add all 30 points ");
        
        #region ParameterCofig
        string levelSystemStrngth = "1.LevelSystem Strength--";
        physicDamage = config(levelSystemStrngth, "PhysicDamage", 0.20f, "Damage multiplier per point. Умножитель урона за один поинт");
        addWeight = config(levelSystemStrngth, "AddWeight", 2f, "Adds carry weight per point. Добавляет переносимый вес за один поинт");
        
        string levelSystemAgility = "1.LevelSystem Agility---";
        speedAttack = config(levelSystemAgility, "StaminaAttack", 0.1f, "Reduces attack stamina consumption. Уменьшает потребление стамины на атаку");
        staminaReduction = config(levelSystemAgility, "StaminaReduction", 0.15f, "Decrease stamina consumption for running, jumping for one point. Уменьшение расхода выносливости на бег, прыжок за один поинт");
        addStamina = config(levelSystemAgility, "AddStamina", 1f, "One Point Stamina Increase. Увеличение  выносливости за один поинт");
        
        string levelSystemIntellect = "1.LevelSystem Intellect-";
        magicDamage = config(levelSystemIntellect, "MagicAttack", 0.20f, "Increase magic attack per point. Увеличение магической атаки за один поинт");
        magicArmor = config(levelSystemIntellect, "MagicArmor", 0.1f, "Increase magical protection per point. Увеличение магической защиты за один поинт");
        MagicEitrRegen = config(levelSystemIntellect, "MagicEitrReg", 0.3f, "Increase magical Eitr Regeneration per point. Увеличивает регенерацию магического Эйтра на единицу.");

        string levelSystemBody = "1.LevelSystem Body------";
        addHp = config(levelSystemBody, "AddHp", 2f, "One Point Health Increase. Увеличение здоровья за один поинт");
        staminaBlock = config(levelSystemBody, "StaminaBlock", 0.2f, "Decrease stamina consumption per unit per point. Уменьшение расхода выносливости на блок за один поинт");
        physicArmor = config(levelSystemBody, "PhysicArmor", 0.15f, "Increase in physical protection per point. Увеличение физической защиты за один поинт");
        regenHp = config(levelSystemBody, "RegenHp", 0.1f, "Increase health regeneration per point. Увеличение регенерации здоровья за один поинт");
        #endregion
        
        string creatureLevelControl = "2.Creature level control";
        mentor = config(creatureLevelControl, "Mentor", true, "Give full eXP for low level members in Groups");
        enabledLevelControl = config(creatureLevelControl, "Enabled_creature_level", true, "Enable creature Level control");
        removeDropMax = config(creatureLevelControl, "Remove_creature_drop_max", true, "Monsters after death do not give items if their level is higher than player level + MaxLevel");
        removeDropMin = config(creatureLevelControl, "Remove_creature_drop_min", false, "Monsters after death do not give items if their level is lower than player level - MinLevel");
        removeBossDropMax = config(creatureLevelControl, "Remove_boss_drop_max", true, "Bosses after death do not give items if their level is higher than player level + MaxLevel");
        removeBossDropMin = config(creatureLevelControl, "Remove_boss_drop_min", false, "Bosses after death do not give items if their level is lower than player level - Minlevel");
        curveExp = config(creatureLevelControl, "Curve_creature_exp", true, "Monsters after death will give less exp if player is outside Max or Min Level Range");
        curveBossExp = config(creatureLevelControl, "Curve_Boss_exp", true, "Bosses after death will give less exp if player is outside Max or Min Level Range");
        lowDamageLevel = config(creatureLevelControl, "Low_damage_level", false, "Decreased damage to the monster if the level is insufficient");
        minLevelExp = config(creatureLevelControl, "MinLevelRange", 10, "Character level - MinLevelRange is less than the level of the monster, then you will receive reduced experience. Уровень персонажа - MinLevelRange меньше уровня монстра, то вы будете получать урезанный опыт");
        maxLevelExp = config(creatureLevelControl, "MaxLevelRange", 10, "Character level + MaxLevelRange is less than the level of the monster, then you will not receive experience. Уровень персонажа + MaxLevelRange меньше уровня монстра, то вы не будете получать опыт");
        
        string resetAttributesItems = "3.Reset attributes items";
        prefabNameCoins = config(resetAttributesItems, "prefabName", "Coins", "Name prefab item");
        viewTextCoins = config(resetAttributesItems, "viewText", "coins", "Name item");
        
        string hud = "4.Hud--------------------";
        oldExpBar = config(hud, "eXP Bar Only", false, "Use eXP Bar only (need restart, not server sync) Does not move or scale", false);
        showMaxHp = config(hud, "ShowMaxHp", true, "Show max hp (100 / 100)", false);
        HudBarScale = config(hud, "ScaleforMoveableBar", .90f, "Scale for ExpBar whichis moveable", false);
        HudExpBackgroundCol = config(hud,"HudBackgroundCol", "#2F1600", "Background color in Hex, set to 'none' to make transparent", false);
        HudPanelPosition = config(hud, "1HudPanelPosition", new Vector2(0, 0), "Position of the HUD panel (x,y)", false);
        ExpPanelPosition = config(hud, "2ExpPanelPosition", new Vector2(0, 0), "Position of the Exp panel (x,y)", false);
        HpPanelPosition = config(hud, "4HpPanelPosition", new Vector2(0, 0), "Position of the Hp panel (x,y)", false);
        StaminaPanelPosition = config(hud, "3StaminaPanelPosition", new Vector2(0, 0), "Position of the Stamina panel (x,y)", false);
        EitrPanelPosition = config(hud, "5EitrPanelPosition", new Vector2(0, 0), "Position of the Eitr panel (x,y)", false);
        HealthIcons = config(hud, "DisabledHealthIcons", true, "Default is true, this may cause issues with some mods");


        string optionalEffect = "5.Optional perk---------";
        addDefaultHealth = config(optionalEffect, "AddDefaultHealth", 0f, "Add health by default");
        addDefaultWeight = config(optionalEffect, "AddDefaultWeight", 0f, "Add weight by default");
        
        _ = ConfigSync.AddLockingConfigEntry(_serverConfigLocked);

        Assembly assembly = Assembly.GetExecutingAssembly();
        _harmony.PatchAll(assembly);
        
        _asset = GetAssetBundle("epicasset");

        localization = new Localization();
        MyUI.Init();
    }

    private void Start()
    {
        DataMonsters.Init();
        FriendsSystem.Init();
        SetupWatcher();
        
    }

    private void OnDestroy()
    {
        Config.Save();
    }

    [HarmonyPatch(typeof(ZNetScene), nameof(ZNetScene.Awake))]
    private static class ZRouteAwake
    {
        private static void Postfix()
        {
            NetisActive = true;
        }
    }

    private void SetupWatcher()
    { 
            if (!Directory.Exists(folderpath))
            {
                Directory.CreateDirectory(folderpath);
            }

            FileSystemWatcher watcher = new(folderpath); // jsons in config
            watcher.Changed += ReadJsonValues;
            watcher.Created += ReadJsonValues;
            watcher.Renamed += ReadJsonValues;
            watcher.IncludeSubdirectories = true;
            watcher.SynchronizingObject = ThreadingHelper.SynchronizingObject;
            watcher.EnableRaisingEvents = true;

            FileSystemWatcher watcher2 = new(Paths.ConfigPath, ConfigFileName);
            watcher2.Changed += ReadConfigValues;
            watcher2.IncludeSubdirectories = false;
            watcher2.SynchronizingObject = ThreadingHelper.SynchronizingObject;
            watcher2.EnableRaisingEvents = true;

    }

    private void ReadJsonValues(object sender, FileSystemEventArgs e)
    {
        if (NetisActive)
        {
            if (ZNet.instance.IsServer() && ZNet.instance.IsDedicated()) // only dedicated
            {
                List<string> list = new List<string>();
                foreach (string file in Directory.GetFiles(folderpath, "*.json", SearchOption.AllDirectories))
                {
                    var nam = Path.GetFileName(file);
                    if (EpicMMOSystem.extraDebug.Value)
                        EpicMMOSystem.MLLogger.LogInfo(nam + " read");

                    var temp = File.ReadAllText(file);
                    list.Add(temp);

                }
                if (EpicMMOSystem.extraDebug.Value)
                    EpicMMOSystem.MLLogger.LogInfo($"Mobs Updated on Server");

                DataMonsters.MonsterDBL = list;
                List<ZNetPeer> peers = ZNet.instance.GetPeers();
                foreach (var peer in peers)
                {
                    if (peer == null) return;
                    ZRoutedRpc.instance.InvokeRoutedRPC(peer.m_uid, $"{EpicMMOSystem.ModName} SetMonsterDB", list); //sync list
                }
            }
        }
    }


        private void ReadConfigValues(object sender, FileSystemEventArgs e)
    {
        if (!File.Exists(ConfigFileFullPath)) return;
        try
        {
            if (EpicMMOSystem.extraDebug.Value)
                MLLogger.LogInfo("MMO Manual ReadConfigValues called");
            Config.Reload();
            DragControl.RestoreWindow(MyUI.expPanel.gameObject);
            DragControl.RestoreWindow(MyUI.hp.gameObject);
            DragControl.RestoreWindow(MyUI.Exp.gameObject);
            DragControl.RestoreWindow(MyUI.stamina.gameObject);
            DragControl.RestoreWindow(MyUI.EitrGameObj);
        }
        catch
        {
            MLLogger.LogError($"There was an issue loading your {ConfigFileName}");
            MLLogger.LogError("Please check your config entries for spelling and format!");
        }
    }

    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Mouse3))
    //     {
    //         // LevelSystem.Instance.ResetAllParameter();
    //         Player.m_localPlayer.m_timeSinceDeath = 3000f;
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


}