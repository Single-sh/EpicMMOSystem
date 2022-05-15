using UnityEngine;
using UnityEngine.UI;

namespace EpicMMOSystem;

public partial class MyUI
{
    private static GameObject navigationPanel;
    private static Transform buttonLevelSystem;
    private static void InitNavigationPanel()
    {
        navigationPanel = UI.transform.Find("Canvas/NavigatePanel").gameObject;
        buttonLevelSystem = navigationPanel.transform.Find("Buttons/ButtonLevelSystem");
        buttonLevelSystem.GetComponent<Button>().onClick.AddListener(ClickButtonLevelSystem);
    }

    private static void ClickButtonLevelSystem()
    {
        if (!levelSystemPanel.activeSelf) UpdateParameterPanel();
        levelSystemPanel.SetActive(!levelSystemPanel.activeSelf);
    }

    private static void ShowNavigationPanel()
    {
        var point = LevelSystem.Instance.getFreePoints();
        buttonLevelSystem.GetChild(0).gameObject.SetActive(point > 0);
    }
}