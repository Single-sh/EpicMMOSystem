using System;
using System.Reflection;

namespace EpicMMOSystem.OtherApi;

public static class Marketplace_API
{
    private static Marketplace_API_State state = Marketplace_API_State.NotReady;
    private enum Marketplace_API_State
    {
        NotReady,
        NotInstalled,
        Ready
    }

    private static MethodInfo MI_OpenJournalButton;
    private static MethodInfo MI_OpenMarketplace;
    private static MethodInfo MI_OpenGambler;
    private static MethodInfo MI_OpenTeleporter;
    private static MethodInfo MI_OpenInfo;
    private static MethodInfo MI_OpenFeedback;
    private static MethodInfo MI_OpenBanker;
    private static MethodInfo MI_OpenBuffer; 
    private static MethodInfo MI_OpenQuest;
    private static MethodInfo MI_OpenTrader;

    public static bool IsInstalled()
    {
        Init();
        return state is Marketplace_API_State.Ready;
    }

    public static void OpenJournalButton()
    {
        Init();
        if (MI_OpenJournalButton != null) MI_OpenJournalButton.Invoke(null, null);
    }
        
    public static void OpenMarketplace()
    {
        Init();
        if (MI_OpenMarketplace != null) MI_OpenMarketplace.Invoke(null, null);
    }
        
    public static void OpenFeedback()
    {
        Init();
        if (MI_OpenFeedback != null) MI_OpenFeedback.Invoke(null, null);
    }
        
    public static void OpenGambler(string profile)
    {
        Init();
        if (MI_OpenGambler != null) MI_OpenGambler.Invoke(null, new object[]{profile});
    }
        
    public static void OpenTeleporter(string profile)
    {
        Init();
        if (MI_OpenTeleporter != null) MI_OpenTeleporter.Invoke(null, new object[]{profile});
    }
        
    public static void OpenInfo(string profile, string npcName)
    {
        Init();
        if (MI_OpenInfo != null) MI_OpenInfo.Invoke(null, new object[]{profile, npcName});
    }
        
    public static void OpenBanker(string profile, string npcName)
    {
        Init();
        if (MI_OpenBanker != null) MI_OpenBanker.Invoke(null, new object[]{profile, npcName});
    }
        
    public static void OpenBuffer(string profile, string npcName)
    {
        Init();
        if (MI_OpenBuffer != null) MI_OpenBuffer.Invoke(null, new object[]{profile, npcName});
    }
        
    public static void OpenQuests(string profile, string npcName)
    {
        Init();
        if (MI_OpenQuest != null) MI_OpenQuest.Invoke(null, new object[]{profile, npcName});
    }
        
    public static void OpenTrader(string profile, string npcName)
    {
        Init();
        if (MI_OpenTrader != null) MI_OpenTrader.Invoke(null, new object[]{profile, npcName});
    }

    private static void Init()
    {
        if (state is Marketplace_API_State.Ready or Marketplace_API_State.NotInstalled) return;
        if (Type.GetType("MarketplaceRevamp.MarketplaceAndServerNPCs, MarketplaceRevamp") == null)
        {
            state = Marketplace_API_State.NotInstalled;
            return;
        }

        state = Marketplace_API_State.Ready;
        Type clientSide = Type.GetType("API.ClientSide, MarketplaceRevamp");
        if (clientSide == null) return;
        MI_OpenJournalButton = clientSide.GetMethod("QuestJournalButton",
            BindingFlags.Public | BindingFlags.Static);
        MI_OpenMarketplace = clientSide.GetMethod("OpenMarketplace",
            BindingFlags.Public | BindingFlags.Static);
        MI_OpenGambler = clientSide.GetMethod("OpenGambler",
            BindingFlags.Public | BindingFlags.Static);
        MI_OpenTeleporter = clientSide.GetMethod("OpenTeleporter",
            BindingFlags.Public | BindingFlags.Static);
        MI_OpenInfo = clientSide.GetMethod("OpenFeedback",
            BindingFlags.Public | BindingFlags.Static);
        MI_OpenFeedback = clientSide.GetMethod("OpenInfo",
        BindingFlags.Public | BindingFlags.Static);
        MI_OpenBanker = clientSide.GetMethod("OpenBanker",
            BindingFlags.Public | BindingFlags.Static);
        MI_OpenBuffer = clientSide.GetMethod("OpenBuffer",
            BindingFlags.Public | BindingFlags.Static);
        MI_OpenQuest = clientSide.GetMethod("OpenQuests",
            BindingFlags.Public | BindingFlags.Static);
        MI_OpenTrader = clientSide.GetMethod("OpenTrader",
            BindingFlags.Public | BindingFlags.Static);
    }
}