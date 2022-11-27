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
    
    private static Text hpText;
    private static Image hpImage;
    
    private static Text staminaText;
    private static Image staminaImage;


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
                eBarImage = oldExpPanel.Find("Bar/Fil").GetComponent<Image>();
                return;
            }
            
            GameObject panel = EpicMMOSystem._asset.LoadAsset<GameObject>("EpicHudPanel");
            Transform expPanel = EpicMMOSystem.Instantiate(panel, __instance.m_rootObject.transform).transform;
            eLevelText = expPanel.Find("Conteiner/Exp/Lvl").GetComponent<Text>();
            eExpText = expPanel.Find("Conteiner/Exp/Exp").GetComponent<Text>();
            //expPanel.Find("Conteiner/Exp/Lvl").localPosition += new Vector3(0, 30, 0);This is bottom right xp bar not monster
            eBarImage = expPanel.Find("Conteiner/Exp/Bar/Fil").GetComponent<Image>();
            
            hpText = expPanel.Find("Conteiner/Hp/Text").GetComponent<Text>();
            hpImage = expPanel.Find("Conteiner/Hp/Bar/Fill").GetComponent<Image>();
            
            staminaText = expPanel.Find("Conteiner/Stamina/Text").GetComponent<Text>();
            staminaImage = expPanel.Find("Conteiner/Stamina/Bar/Fill").GetComponent<Image>();
            
            __instance.m_healthPanel.Find("Health").gameObject.SetActive(false);
            __instance.m_healthPanel.Find("healthicon").gameObject.SetActive(false);

            var buildInfo = __instance.m_buildHud.transform.Find("SelectedInfo");
            if (buildInfo)
            {
                buildInfo.localPosition += new Vector3(0, 30, 0);
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

