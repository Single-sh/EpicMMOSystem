using System;
using EpicMMOSystem.MonoScripts;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace EpicMMOSystem;

public partial class MyUI
{
    internal static Text eLevelText;
    internal static Text eExpText;
    internal static Image eBarImage;
    internal static Transform Exp;

    internal static Text hpText;
    internal static Image hpImage;
    internal static Transform hp;

    internal static Text staminaText;
    internal static Image staminaImage;
    internal static Transform stamina;

    internal static Text Eitr;
    internal static Image EitrImage;
    internal static GameObject EitrGameObj;
    internal static Transform EitrTran;

    internal static Transform expPanel;
    internal static Transform expPanelRoot;
    internal static Color expPanelBackgroundColor;
    internal static GameObject expPanelBackground;

    internal static int flagforMove = 0;
    internal static bool firstload = false;



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
    internal static void InitHudPanel()
    {

        expPanel = expPanelRoot.Find("EpicHudPanel");

        expPanelBackground = expPanel.Find("Background").gameObject;
        expPanelBackgroundColor = expPanelBackground.GetComponent<Image>().color;


        expPanelRoot.GetComponent<CanvasScaler>().scaleFactor = EpicMMOSystem.HudBarScale.Value;// scale factor Need to move so dynamic
        if (EpicMMOSystem.HudExpBackgroundCol.Value == "none")
            expPanelBackground.SetActive(false);
        else
            expPanelBackgroundColor = ColorUtil.GetColorFromHex(EpicMMOSystem.HudExpBackgroundCol.Value);

        eLevelText = expPanel.Find("Container/Exp/Lvl").GetComponent<Text>();
        eExpText = expPanel.Find("Container/Exp/Exp").GetComponent<Text>();
        Exp = expPanel.Find("Container/Exp");

        //expPanel.Find("Conteiner/Exp/Lvl").localPosition += new Vector3(0, 30, 0);This is bottom right xp bar not monster
        eBarImage = expPanel.Find("Container/Exp/Bar/Fill").GetComponent<Image>();

        hpText = expPanel.Find("Container/Hp/Text").GetComponent<Text>();
        hpImage = expPanel.Find("Container/Hp/Bar/Fill").GetComponent<Image>();
        hp = expPanel.Find("Container/Hp");


        staminaText = expPanel.Find("Container/Stamina/Text").GetComponent<Text>();
        staminaImage = expPanel.Find("Container/Stamina/Bar/Fill").GetComponent<Image>();
        stamina = expPanel.Find("Container/Stamina");


        EitrTran = expPanel.Find("Container/Eitr");
        EitrGameObj = expPanel.Find("Container/Eitr").gameObject;
        Eitr = expPanel.Find("Container/Eitr/Text").GetComponent<Text>();
        EitrImage = expPanel.Find("Container/Eitr/Bar/Fill").GetComponent<Image>();

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
                //DragWindowCntrl.ApplyDragWindowCntrl(oldPanel);
                return;
            }

            GameObject panel = EpicMMOSystem._asset.LoadAsset<GameObject>("EpicHudPanelCanvas"); //DragWindowCntrl.ApplyDragWindowCntrl(panel); //ended up working for some reason, 
            // expPanelRoot = EpicMMOSystem.Instantiate(panel, __instance.m_rootObject.transform).transform;
            expPanelRoot = EpicMMOSystem.Instantiate(panel).transform;//GameObject.DontDestroyOnLoad(expPanelRoot.gameObject);
            expPanelRoot.gameObject.SetActive(false);

            InitHudPanel();

            __instance.m_healthPanel.Find("Health").gameObject.SetActive(false);
            if (EpicMMOSystem.HealthIcons.Value)
            {
                __instance.m_healthPanel.Find("healthicon").gameObject.SetActive(false);
            }
           

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
            expPanelRoot.GetComponent<Canvas>().gameObject.SetActive(true); // idk
            expPanelRoot.GetComponent<CanvasScaler>().scaleFactor = EpicMMOSystem.HudBarScale.Value;// scale factor Need to move so dynamic
            
            if (EpicMMOSystem.HudExpBackgroundCol.Value == "none")
                expPanelBackground.SetActive(false);
            else
            {
                expPanelBackground.SetActive(true);
                expPanelBackgroundColor = ColorUtil.GetColorFromHex(EpicMMOSystem.HudExpBackgroundCol.Value);
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
            if (firstload)
                flagforMove = 0;

            if (flagforMove > 0)
            {
                DragControl.RestoreWindow(hp.gameObject);
                DragControl.RestoreWindow(Exp.gameObject);
                DragControl.RestoreWindow(stamina.gameObject);
                if(flagforMove == 2)
                    DragControl.RestoreWindow(EitrGameObj);

                flagforMove = 0;
            }

            if (max < 1 && EitrGameObj.activeSelf)
            {
                EitrGameObj.SetActive(false);
                expPanel.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1050);
                // InitHudPanel();
                flagforMove = 1;

            }
            if (max > 0 && !EitrGameObj.activeSelf)
            {
                EitrGameObj.SetActive(true);
                expPanel.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1475);
                // InitHudPanel();
                flagforMove = 2; 


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
            firstload = true;
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
                if (!EpicMMOSystem.oldExpBar.Value)
                    expPanelRoot.gameObject.SetActive(true);

            }
            catch (Exception e)
            {
                EpicMMOSystem.print($"Error set expbar: {e.Message}");
                throw;
            }
            
        }
    }
}

