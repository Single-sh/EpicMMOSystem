using System;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace EpicMMOSystem;

public partial class MyUI
{
    private static Text eLevelText;
    private static Text eExpText;
    private static Image eBarImage;


    public static void updateExpBar()
    {
        var level = LevelSystem.Instance.getLevel();
        var exp = LevelSystem.Instance.getCurrentExp();
        var need = LevelSystem.Instance.getNeedExp();
        string expPersent = ((float)exp / need * 100).ToString("0.00");
        eLevelText.text = $"{localization["$lvl"]} {level}";
        eExpText.text = $"{expPersent.Replace(',','.')} %";
        eBarImage.fillAmount = (float)exp / need;
    }
    
    [HarmonyPatch(typeof(Hud), nameof(Hud.Awake))]
    public static class InstantiateExpPanel
    {
        static void Postfix(Hud __instance)
        {
            GameObject panel = EpicMMOSystem._asset.LoadAsset<GameObject>("ExpPanel");
            Transform expPanel = EpicMMOSystem.Instantiate(panel, __instance.m_rootObject.transform).transform;
            eLevelText = expPanel.Find("Lvl").GetComponent<Text>();
            eExpText = expPanel.Find("Exp").GetComponent<Text>();
            eBarImage = expPanel.Find("Bar/Fil").GetComponent<Image>();
            EpicMMOSystem.print("spawn expbar");
        }
    }
    
    [HarmonyPatch(typeof(Game), nameof(Game.SpawnPlayer))]
    public static class UpdateExpPanelForStart
    {
        static void Postfix()
        {
            try
            {
                updateExpBar();
            }
            catch (Exception e)
            {
                EpicMMOSystem.print($"Error set expbar: {e.Message}");
                throw;
            }
        }
    }
}

