using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EpicMMOSystem;

public partial class MyUI
{
    private static GameObject levelSystemPanel;
    //AlertsApplyReset
    private static GameObject alertResetPointPanel;
    private static Text alertResetPointText;
    //Stats
    private static List<ParameterButton> parameterButtons = new ();
    private static GameObject freePointsPanel;
    private static Text freePointsText;
    //CurentLevel
    private static Text currentLevelText;
    private static Text expText;
    
    //Description
    //Description strength
    private static Text physicDamageText;
    private static Text addWeightText;
    
    //Description agility
    private static Text speedAttackText;
    private static Text reducedStaminaText;
    
    //Description intellect
    private static Text magicDamageText;
    private static Text magicArmorText;
    
    //Description body
    private static Text addHpText;
    private static Text addStaminaText;
    private static Text physicArmorText;
    private static Text reducedStaminaBlockText;
    private static Text regenHpText;
    
    private static void InitLevelSystem()
    {
        levelSystemPanel = UI.transform.Find("Canvas/PointPanel").gameObject;
        levelSystemPanel.transform.Find("Background").gameObject.AddComponent<DragMenu>().menu = levelSystemPanel.transform;
        levelSystemPanel.transform.Find("Header").gameObject.AddComponent<DragMenu>().menu = levelSystemPanel.transform;
        levelSystemPanel.transform.Find("Header/Text").GetComponent<Text>().text = localization["$attributes"];
        
        //alertReset
        alertResetPointPanel = UI.transform.Find("Canvas/ApplyReset").gameObject;
        alertResetPointText = alertResetPointPanel.transform.Find("Text").GetComponent<Text>();
        alertResetPointPanel.transform.Find("Buttons/Yes").GetComponent<Button>().onClick.AddListener(ResetYes);
        alertResetPointPanel.transform.Find("Buttons/Yes/Text").GetComponent<Text>().text = localization["$yes"];
        alertResetPointPanel.transform.Find("Buttons/No").GetComponent<Button>().onClick.AddListener(ResetNo);
        alertResetPointPanel.transform.Find("Buttons/No/Text").GetComponent<Text>().text = localization["$no"];
        
        
        //Stats
        var strength = levelSystemPanel.transform.Find("Points/ListStat/Strength");
        parameterButtons.Add(new ParameterButton(strength, Parameter.Strength));
        var agility = levelSystemPanel.transform.Find("Points/ListStat/Agility");
        parameterButtons.Add(new ParameterButton(agility, Parameter.Agility));
        var intellect = levelSystemPanel.transform.Find("Points/ListStat/Intelect");
        parameterButtons.Add(new ParameterButton(intellect, Parameter.Intellect));
        var body = levelSystemPanel.transform.Find("Points/ListStat/Body");
        parameterButtons.Add(new ParameterButton(body, Parameter.Body));
        
        
        freePointsPanel = levelSystemPanel.transform.Find("Points/FreePoints").gameObject;
        freePointsText = levelSystemPanel.transform.Find("Points/FreePoints/Text").GetComponent<Text>();

        freePointsPanel.transform.Find("Cancel").GetComponent<Button>().onClick.AddListener(ClickCancel);
        freePointsPanel.transform.Find("Cancel/Text").GetComponent<Text>().text = localization["$cancel"];
        freePointsPanel.transform.Find("Apply").GetComponent<Button>().onClick.AddListener(ClickApply);
        freePointsPanel.transform.Find("Apply/Text").GetComponent<Text>().text = localization["$apply"];
        
        levelSystemPanel.transform.Find("Points/ResetButton/Button").GetComponent<Button>().onClick.AddListener(ResetParameters);
        levelSystemPanel.transform.Find("Points/ResetButton/Button/Text").GetComponent<Text>().text = localization["$reset_parameters"]; 
        
        //CurrentLevel
        currentLevelText = levelSystemPanel.transform.Find("CurrentLevel/Content/Lvl").GetComponent<Text>();
        expText = levelSystemPanel.transform.Find("CurrentLevel/Content/Exp").GetComponent<Text>();

        //Description
       // var paul = LevelHudPanelRefs.CanvasRoot;
        Transform contentStats = levelSystemPanel.transform.Find("DescriptionStats/Scroll View/Viewport/Content");
        contentStats.transform.Find("HeaderDamage/Text").GetComponent<Text>().text = localization["$damage"];
        contentStats.transform.Find("HeaderDefence/Text").GetComponent<Text>().text = localization["$armor"];
        contentStats.transform.Find("HeaderSurvarior/Text").GetComponent<Text>().text = localization["$survival"];
        //Description strength
        physicDamageText = contentStats.transform.Find("PhysicDamage").GetComponent<Text>();
        addWeightText = contentStats.transform.Find("Weight").GetComponent<Text>();
        //Description agility
        speedAttackText = contentStats.transform.Find("SpeedAttack").GetComponent<Text>();
        reducedStaminaText = contentStats.transform.Find("StaminaReduction").GetComponent<Text>();
        addStaminaText = contentStats.transform.Find("Stamina").GetComponent<Text>();
        //Description intellect
        magicDamageText = contentStats.transform.Find("MagicDamage").GetComponent<Text>();
        magicArmorText = contentStats.transform.Find("MagicDamageReduction").GetComponent<Text>();
        //Description body
        addHpText = contentStats.transform.Find("Hp").GetComponent<Text>();
        physicArmorText = contentStats.transform.Find("DamageReduction").GetComponent<Text>();
        reducedStaminaBlockText = contentStats.transform.Find("StaminaBlock").GetComponent<Text>();
        regenHpText = contentStats.transform.Find("HpRegeneration").GetComponent<Text>();
    }

    private static void ClickCancel()
    {
        LevelSystem.Instance.cancelDepositPoints();
    }
    
    private static void ClickApply()
    {
        LevelSystem.Instance.applyDepositPoints();
    }

    private static void ResetParameters()
    {
        var textCoin = EpicMMOSystem.viewTextCoins.Value;
        var price = LevelSystem.Instance.getPriceResetPoints();
        var text = localization["$reset_point_text"];
        alertResetPointText.text = String.Format(text, price, textCoin);
        levelSystemPanel.GetComponent<CanvasGroup>().interactable = false;
        alertResetPointPanel.SetActive(true);
    }

    private static void ResetYes()
    {
        ResetNo();
        LevelSystem.Instance.ResetAllParameterPayment();
    }
    
    private static void ResetNo()
    {
        alertResetPointPanel.SetActive(false);
        levelSystemPanel.GetComponent<CanvasGroup>().interactable = true;
    }

    public static void UpdateParameterPanel()
    {
        var points = LevelSystem.Instance.getFreePoints();
        var hasDeposit = LevelSystem.Instance.hasDepositPoints();
        parameterButtons.ForEach(p => p.UpdateParameters(points));
        
        freePointsPanel.SetActive(points > 0 || hasDeposit);
        freePointsText.text = $"{localization["$free_points"]}: {points}";
        
        currentLevelText.text = $"{localization["$level"]}: {LevelSystem.Instance.getLevel()}";
        var currentExp = LevelSystem.Instance.getCurrentExp();
        var needExp = LevelSystem.Instance.getNeedExp();
        var locText = localization["$exp"];
        expText.text = $"{locText}: {currentExp} / {needExp}";
        

        #region parameter
        //Description strength
        physicDamageText.text = $"{localization["$physic_damage"]}: +{LevelSystem.Instance.getAddPhysicDamage()}%";
        addWeightText.text = $"{localization["$add_weight"]}: +{LevelSystem.Instance.getAddWeight()}";

        //Description agility
        speedAttackText.text = $"{localization["$speed_attack"]}: -{LevelSystem.Instance.getAddStaminaAttack()}%";
        reducedStaminaText.text = $"{localization["$reduced_stamina"]}: -{LevelSystem.Instance.getStaminaReduction()}%";
        addStaminaText.text = $"{localization["$add_stamina"]}: +{LevelSystem.Instance.getAddStamina()}";
    
        //Description intellect
        magicDamageText.text = $"{localization["$magic_damage"]}: +{LevelSystem.Instance.getAddMagicDamage()}%";
        magicArmorText.text = $"{localization["$magic_armor"]}: +{LevelSystem.Instance.getAddMagicArmor()}%";
    
        //Description body
        addHpText.text = $"{localization["$add_hp"]}: +{LevelSystem.Instance.getAddHp()}";
        physicArmorText.text = $"{localization["$physic_armor"]}: +{LevelSystem.Instance.getAddPhysicArmor()}%";
        reducedStaminaBlockText.text = $"{localization["$reduced_stamina_block"]}: -{LevelSystem.Instance.getReducedStaminaBlock()}%";
        regenHpText.text = $"{localization["$regen_hp"]}: +{LevelSystem.Instance.getAddRegenHp()}%";
        #endregion
    }

    #region ParameterButton
    private class ParameterButton
    {
        private Parameter parameter;
        private Transform head;
        private GameObject buttons;
        private Text text;

        public ParameterButton(Transform head, Parameter parameter)
        {
            this.head = head;
            this.parameter = parameter;

            text = head.Find("Text").GetComponent<Text>();
            buttons = head.Find("Buttons").gameObject;
            buttons.transform.Find("Plus1").GetComponent<Button>().onClick.AddListener(ClickButton1);
            buttons.transform.Find("Plus5").GetComponent<Button>().onClick.AddListener(ClickButton5);
        }

        private void ClickButton1()
        {
            LevelSystem.Instance.addPointsParametr(parameter, 1);
            UpdateParameterPanel();
        }
        private void ClickButton5()
        {
            LevelSystem.Instance.addPointsParametr(parameter, 5);
            UpdateParameterPanel();
        }

        public void UpdateParameters(int freePoints)
        {
            var max = EpicMMOSystem.maxValueAttribute.Value;
            var current = LevelSystem.Instance.getParameter(parameter);
            text.text = $"{localization[$"$parameter_{parameter.ToString().ToLower()}"]}: {current}";
            buttons.SetActive(freePoints > 0 && current < max);
        }
    }
    #endregion
}