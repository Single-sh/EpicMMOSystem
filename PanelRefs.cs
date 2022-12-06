using UnityEngine;
using UnityEngine.UI;

namespace EpicMMOSystem;

public class ExpPanelRefs : MonoBehaviour
{
    [SerializeField] public RectTransform ExpPanelRoot;
    [SerializeField] public RectTransform ExpPanelBkg;
    [SerializeField] public Image ExpPanelBarFil;
    [SerializeField] public Text ExpPanelLvlTxt;
    [SerializeField] public Text ExpPanelExpTxt;
}

public class EpicHudPanelRefs : MonoBehaviour
{
    [SerializeField] public RectTransform ExpPanelRoot;
    [SerializeField] public RectTransform ExpPanelBkg;
    [SerializeField] public Image HealthBarFil;
    [SerializeField] public Image StaminaBarFil;
    [SerializeField] public Image EitrBarFil;
    [SerializeField] public Text EpicHudPanelLvlTxt;
    [SerializeField] public Text EpicHudPanelExpTxt;
}

public class FriendCellPanelRefs : MonoBehaviour
{
    [SerializeField] public RectTransform PanelRoot;
    [SerializeField] public RectTransform PanelBkg;
    [SerializeField] public Image IconClass;
    [SerializeField] public Text PlayerName;
    [SerializeField] public Text Level;
    [SerializeField] public Text Status;
    [SerializeField] public RectTransform ButtonGroup;
    [SerializeField] public Button AddGroup;
    [SerializeField] public Button AcceptButton;
    [SerializeField] public Button DeclineButton;
}

public class LevelHudPanelRefs : MonoBehaviour
{
    // MenuPanel Stuff
    [Header("MenuPanel")]
    [SerializeField] public RectTransform CanvasRoot;
    [SerializeField] public RectTransform MenuPanelRoot;
    [SerializeField] public RectTransform MenuPanelBkg;
    [SerializeField] public RectTransform ButtonGroup;
    [SerializeField] public RectTransform ButtonLevelSystem;
    [SerializeField] public RectTransform ButtonFriends;
    [SerializeField] public RectTransform ButtonQuests;
    [SerializeField] public RectTransform ButtonProfessions;
    [SerializeField] public RectTransform ButtonShop;

    // PointPanel Stuff
    [Header("PointPanel")]
    [SerializeField] public RectTransform PointPanelRoot;
    [SerializeField] public RectTransform PointPanelBkg;
    [SerializeField] public Text HeaderText;
    [SerializeField] public RectTransform PointSectionRoot;
    [SerializeField] public RectTransform StrengthRoot;
    [SerializeField] public Text StrengthText;
    [SerializeField] public Button StrengthP1Button;
    [SerializeField] public Button StrengthP5Button;
    [SerializeField] public Text AgilityText;
    [SerializeField] public Button AgilityP1Button;
    [SerializeField] public Button AgilityP5Button;
    [SerializeField] public Text IntellectText;
    [SerializeField] public Button IntellectP1Button;
    [SerializeField] public Button IntellectP5Button;
    [SerializeField] public Text BodyText;
    [SerializeField] public Button BodyP1Button;
    [SerializeField] public Button BodyP5Button;
    [SerializeField] public GameObject DummyListStat;
    [SerializeField] public Text FreePointText;
    [SerializeField] public Button FreePointApply;
    [SerializeField] public Button FreePointCancel;
    [SerializeField] public Button PointResetButton;
    [SerializeField] public Text CurrentLvlText;
    [SerializeField] public Text CurrentExpText;
    [SerializeField] public RectTransform DescStatsContentParent;
    [SerializeField] public RectTransform DescStatsDummyHeader;

    [SerializeField] public Text DescStatsDummyText;

    // ApplyResetPanel Stuff
    [Header("ApplyResetPanel")]
    [SerializeField] public RectTransform ApplyResetPanelRoot;
    [SerializeField] public RectTransform ApplyResetPanelBkg;
    [SerializeField] public RectTransform ApplyResetButtonGroup;
    [SerializeField] public Button ButtonYes;
    [SerializeField] public Button ButtonNo;
    [SerializeField] public Text Text;

    // FriendsListPanel Stuff
    [Header("FriendsListPanel")]
    [SerializeField] public RectTransform FriendsListPanelRoot;
    [SerializeField] public RectTransform FriendsListPanelBkg;
    [SerializeField] public RectTransform FriendsListPanelHeader;
    [SerializeField] public Image BorderImage;
    [SerializeField] public GameObject HeaderInvited;
    [SerializeField] public GameObject Invited;
    [SerializeField] public GameObject HeaderFriends;
    [SerializeField] public GameObject Friends;
    [SerializeField] public Text FriendsListPanelText;
    [SerializeField] public Button AddFriendButton;

    // SendInvitePanel Stuff
    [Header("SendInvitePanel")]
    [SerializeField] public RectTransform SendInvitePanelRoot;
    [SerializeField] public RectTransform SendInvitePanelBkg;
    [SerializeField] public InputField SendInviteInputField;
    [SerializeField] public Button SendButton;
    [SerializeField] public Text SendText;
    [SerializeField] public Button CancelButton;
    [SerializeField] public Text CancelText;
}