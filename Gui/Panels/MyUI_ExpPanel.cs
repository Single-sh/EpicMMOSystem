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
    private static Transform Exp;
    
    private static Text hpText;
    private static Image hpImage;
    private static Transform hp;

    private static Text staminaText;
    private static Image staminaImage;
    private static Transform stamina;

    private static Text Eitr;
    private static Image EitrImage;
    private static GameObject EitrGameObj;
    private static Transform EitrTran;

    private static Transform expPanel;
    private static Transform expPanelRoot;
    private static Color expPanelBackgroundColor;
    private static GameObject expPanelBackground;



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
            //GameObject panel = EpicMMOSystem._asset.LoadAsset<GameObject>("EpicHudPanel");
            GameObject panel = EpicMMOSystem._asset.LoadAsset<GameObject>("EpicHudPanelCanvas"); // I had problems with this one, Canvas wouldn't show up until I disabled and renabled it - Azu question 
            //DragWindowCntrl.ApplyDragWindowCntrl(panel); //ended up working for some reason, 

           // expPanelRoot = EpicMMOSystem.Instantiate(panel, __instance.m_rootObject.transform).transform;
            
            expPanelRoot = EpicMMOSystem.Instantiate(panel).transform;
            //GameObject.DontDestroyOnLoad(expPanelRoot.gameObject);
           expPanelRoot.gameObject.SetActive(false);

            expPanel = expPanelRoot.Find("EpicHudPanel");
            


            expPanelBackground = expPanel.Find("Background").gameObject;
            expPanelBackgroundColor = expPanelBackground.GetComponent<Image>().color;

            //expPanelRoot.GetComponent<Canvas>().gameObject.SetActive(false); // idk
            expPanelRoot.GetComponent<CanvasScaler>().scaleFactor = EpicMMOSystem.HudBarScale.Value;// scale factor Need to move so dynamic
            if (EpicMMOSystem.HudExpBackgroundCol.Value == "none")
                expPanelBackground.SetActive(false);
            else
                expPanelBackgroundColor = ColorUtil.GetColorFromHex(EpicMMOSystem.HudExpBackgroundCol.Value);

            eLevelText = expPanel.Find("Container/Exp/Lvl").GetComponent<Text>();
            eExpText = expPanel.Find("Container/Exp/Exp").GetComponent<Text>();
            Exp = expPanel.Find("Container/Exp");
            EpicMMOSystem.SaveWindowPositions(Exp.gameObject, true);

            //expPanel.Find("Conteiner/Exp/Lvl").localPosition += new Vector3(0, 30, 0);This is bottom right xp bar not monster
            eBarImage = expPanel.Find("Container/Exp/Bar/Fill").GetComponent<Image>();

            hpText = expPanel.Find("Container/Hp/Text").GetComponent<Text>();
            hpImage = expPanel.Find("Container/Hp/Bar/Fill").GetComponent<Image>();
            hp = expPanel.Find("Container/Hp");
            EpicMMOSystem.SaveWindowPositions(hp.gameObject, true);


            staminaText = expPanel.Find("Container/Stamina/Text").GetComponent<Text>();
            staminaImage = expPanel.Find("Container/Stamina/Bar/Fill").GetComponent<Image>();
            stamina = expPanel.Find("Container/Stamina");
            EpicMMOSystem.SaveWindowPositions(stamina.gameObject, true);

            EitrTran = expPanel.Find("Container/Eitr");
            EitrGameObj = expPanel.Find("Container/Eitr").gameObject;
            Eitr = expPanel.Find("Container/Eitr/Text").GetComponent<Text>();
            EitrImage = expPanel.Find("Container/Eitr/Bar/Fill").GetComponent<Image>();
            EpicMMOSystem.SaveWindowPositions(EitrGameObj, true);


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

            if (max < 1 && EitrGameObj.activeSelf)
            {
                EitrGameObj.SetActive(false);
                expPanel.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1050);
            }
            if (max > 0 && !EitrGameObj.activeSelf)
            {
                EitrGameObj.SetActive(true);
                expPanel.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1475);

                for (int i = 1; i < 5; i++)
                {
                    EpicMMOSystem.MLLogger.LogInfo("cords spawn " + i + " " + EpicMMOSystem.HudCords[i]);

                    if (EpicMMOSystem.HudCords[i] == null || EpicMMOSystem.HudCords[i] == "")
                        continue;


                    switch (i)
                    {
                        case 1:
                            Exp.gameObject.GetComponent<RectTransform>().position = StringToVector2Special(EpicMMOSystem.HudCords[i]);
                            break;
                        case 2:
                            hp.position = StringToVector2Special(EpicMMOSystem.HudCords[i]);
                            break;
                        case 3:
                            stamina.gameObject.GetComponent<RectTransform>().position = StringToVector2Special(EpicMMOSystem.HudCords[i]);
                            break;
                        case 4:
                            EitrGameObj.GetComponent<RectTransform>().position = StringToVector3Special(EpicMMOSystem.HudCords[i]);
                            break;

                    }
                }
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
                expPanelRoot.gameObject.SetActive(true);
                expPanel.transform.position = StringToVector3Special(EpicMMOSystem.HudCords[0]);
               
            }
            catch (Exception e)
            {
                EpicMMOSystem.print($"Error set expbar: {e.Message}");
                throw;
            }
        }
    }

    public static Vector3 StringToVector3Special(string sVector)
    {

        if (sVector == null || sVector == "") // if empty
        {
            Vector3 paul= new (0, 0, 0); // somehow get default
            return paul;
        }
        if (sVector.StartsWith("(") && sVector.EndsWith(")"))
        {
            sVector = sVector.Substring(1, sVector.Length - 2);
        }

        // split the items
        string[] sArray = sVector.Split(',');

        // store as a Vector3
        Vector3 result = new Vector3(
            float.Parse(sArray[0]),
            float.Parse(sArray[1]),
            float.Parse(sArray[2]));

        return result;

    }

    public static Vector2 StringToVector2Special(string sVector)
    {

        if (sVector == null || sVector == "") // if empty
        {
            Vector2 paul = new(0, 0); // somehow get default
            return paul;
        }
        if (sVector.StartsWith("(") && sVector.EndsWith(")"))
        {
            sVector = sVector.Substring(1, sVector.Length - 2);
        }

        // split the items
        string[] sArray = sVector.Split(',');

        // store as a Vector3
        Vector2 result = new Vector2(
            float.Parse(sArray[0]),
            float.Parse(sArray[1]));

        return result;

    }
}

