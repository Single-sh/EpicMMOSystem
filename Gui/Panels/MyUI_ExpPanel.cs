using System;
using EpicMMOSystem.MonoScripts;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace EpicMMOSystem;

public partial class MyUI
{
    private static Text eLevelText;
    private static Text eExpText;
    private static Image eBarImage;
    
    private static Text hpText;
    private static Image hpImage;
    
    private static Text staminaText;
    private static Image staminaImage;

    private static Text Eitr;
    private static Image EitrImage;
    private static GameObject EitrBar;

    private static Transform expPanel;


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
            if (EpicMMOSystem.oldExpBar.Value)
            {
                GameObject oldPanel = EpicMMOSystem._asset.LoadAsset<GameObject>("ExpPanel");
                Transform oldExpPanel = EpicMMOSystem.Instantiate(oldPanel, __instance.m_rootObject.transform).transform;
                eLevelText = oldExpPanel.Find("Lvl").GetComponent<Text>();
                eExpText = oldExpPanel.Find("Exp").GetComponent<Text>();
                eBarImage = oldExpPanel.Find("Bar/Fill").GetComponent<Image>();
                return;
            }
            GameObject panel = EpicMMOSystem._asset.LoadAsset<GameObject>("EpicHudPanel");
            panel.AddComponent<DragWindowCntrl>();

            expPanel = EpicMMOSystem.Instantiate(panel, __instance.m_rootObject.transform).transform;

            eLevelText = expPanel.Find("Container/Exp/Lvl").GetComponent<Text>();
            eExpText = expPanel.Find("Container/Exp/Exp").GetComponent<Text>();

            //expPanel.Find("Conteiner/Exp/Lvl").localPosition += new Vector3(0, 30, 0);This is bottom right xp bar not monster
            eBarImage = expPanel.Find("Container/Exp/Bar/Fill").GetComponent<Image>();

            hpText = expPanel.Find("Container/Hp/Text").GetComponent<Text>();
            hpImage = expPanel.Find("Container/Hp/Bar/Fill").GetComponent<Image>();
            
            staminaText = expPanel.Find("Container/Stamina/Text").GetComponent<Text>();
            staminaImage = expPanel.Find("Container/Stamina/Bar/Fill").GetComponent<Image>();

            EitrBar = expPanel.Find("Container/Eitr").gameObject;
            Eitr = expPanel.Find("Container/Eitr/Text").GetComponent<Text>();
            EitrImage = expPanel.Find("Container/Eitr/Bar/Fill").GetComponent<Image>();


            __instance.m_healthPanel.Find("Health").gameObject.SetActive(false);
            __instance.m_healthPanel.Find("healthicon").gameObject.SetActive(false);
           

            var buildInfo = __instance.m_buildHud.transform.Find("SelectedInfo"); // move build menu up
            if (buildInfo)
            {
                buildInfo.localPosition += new Vector3(0, 45, 0); // from 30
            }
        }
    }
    
    
    [HarmonyPatch(typeof(Hud), nameof(Hud.UpdateHealth))]
    public static class UpdateHealth
    {
        static bool Prefix(Player player)
        {
            if (EpicMMOSystem.oldExpBar.Value)
            {
                return true;
            }
            
            var current = player.GetHealth();
            var max = player.GetMaxHealth();
            
            hpImage.fillAmount = current / max;
            string text = "";
            if (EpicMMOSystem.showMaxHp.Value)
            {
                text = $"{Mathf.CeilToInt(current).ToString()} / {Mathf.CeilToInt(max).ToString()}";
            }
            else
            {
                text = Mathf.CeilToInt(current).ToString();
            }
            hpText.text = text;
            return false;
        }
    }
    
    [HarmonyPatch(typeof(Hud), nameof(Hud.UpdateStamina))]
    public static class UpdateStamina
    {
        static bool Prefix(Player player)
        {
            if (EpicMMOSystem.oldExpBar.Value)
            {
                return true;
            }
            
            var current = player.GetStamina();
            var max = player.GetMaxStamina();
            
            staminaImage.fillAmount = current / max;
            string text = "";
            if (EpicMMOSystem.showMaxHp.Value)
            {
                text = $"{Mathf.CeilToInt(current).ToString()} / {Mathf.CeilToInt(max).ToString()}";
            }
            else
            {
                text = Mathf.CeilToInt(current).ToString();
            }
            staminaText.text = text;
            return false;
        }
    }

    [HarmonyPatch(typeof(Hud), nameof(Hud.UpdateEitr))]
    public static class UpdateEitr
    {
        static bool Prefix(Player player)
        {
            if (EpicMMOSystem.oldExpBar.Value)
            {
                return true;
            }

            var current = player.GetEitr();
            var max = player.GetMaxEitr();

            if (max < 1 && EitrBar.activeSelf)
            {
                EitrBar.SetActive(false);
                expPanel.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1050);
            }
            if (max > 0 && !EitrBar.activeSelf)
            {
                EitrBar.SetActive(true);
                expPanel.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1475);
            }
                

            EitrImage.fillAmount = current / max;
            string text = "";
            if (EpicMMOSystem.showMaxHp.Value)
            {
                text = $"{Mathf.CeilToInt(current).ToString()} / {Mathf.CeilToInt(max).ToString()}";
            }
            else
            {
                text = Mathf.CeilToInt(current).ToString();
            }
            Eitr.text = text;
            return false; // doesn't update the UI information then, this can't really live update from switch oldExpBars
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

