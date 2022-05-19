using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;


namespace EpicMMOSystem;

public static partial class MyUI
{
    private static GameObject UI;
    private static Localization localization => EpicMMOSystem.localization;
    public static bool IsPanelVisible()
    {
        return UI && UI.activeSelf;
    }

    public static void Init()
    {
        UI = EpicMMOSystem.Instantiate(EpicMMOSystem._asset.LoadAsset<GameObject>("LevelHud"));
        EpicMMOSystem.DontDestroyOnLoad(UI);
        UI.SetActive(false);
        InitNavigationPanel();
        InitLevelSystem();
        InitFriendsList();
    }

    public static void Hide()
    {
        navigationPanel.SetActive(false);
        levelSystemPanel.SetActive(false);
        alertResetPointPanel.SetActive(false);
        UI.SetActive(false);
        LevelSystem.Instance.cancelDepositPoints();
    }

    public static void Show()
    {
        ShowNavigationPanel();
        navigationPanel.SetActive(true);
        UI.SetActive(true);
    }

    [HarmonyPatch(typeof(InventoryGui), nameof(InventoryGui.Show))]
    public static class ShowPanel_Path
    {
        public static void Postfix()
        {
            Show();
        }
    }
    
    [HarmonyPatch(typeof(InventoryGui), nameof(InventoryGui.Hide))]
    public static class HidePanel_Path
    {
        public static void Postfix()
        {
            Hide();
        }
    }

}