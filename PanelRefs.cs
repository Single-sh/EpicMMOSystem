using UnityEngine;
using UnityEngine.UI;

namespace EpicMMOSystem;

public class ExpPanelRefs : MonoBehaviour
{
    [SerializeField] private RectTransform ExpPanelRoot;
    [SerializeField] private RectTransform ExpPanelBkg;
    [SerializeField] private Image ExpPanelBarFil;
    [SerializeField] private Text ExpPanelLvlTxt;
    [SerializeField] private Text ExpPanelExpTxt;
}

public class EpicHudPanelRefs : MonoBehaviour
{
    [SerializeField] private RectTransform ExpPanelRoot;
    [SerializeField] private RectTransform ExpPanelBkg;
    [SerializeField] private Image HealthBarFil;
    [SerializeField] private Image StaminaBarFil;
    [SerializeField] private Image EitrBarFil;
    [SerializeField] private Text EpicHudPanelLvlTxt;
    [SerializeField] private Text EpicHudPanelExpTxt;
}

public class FriendCellPanelRefs : MonoBehaviour
{
    [SerializeField] private RectTransform PanelRoot;
    [SerializeField] private RectTransform PanelBkg;
    [SerializeField] private Image IconClass;
    [SerializeField] private Text PlayerName;
    [SerializeField] private Text Level;
    [SerializeField] private Text Status;
    [SerializeField] private RectTransform ButtonGroup;
    [SerializeField] private Button AddGroup;
    [SerializeField] private Button AcceptButton;
    [SerializeField] private Button DeclineButton;
}

public class LevelHudPanelRefs : MonoBehaviour
{
    // MenuPanel Stuff
    [Header("MenuPanel")]
    [SerializeField] private RectTransform CanvasRoot;
    [SerializeField] private RectTransform MenuPanelRoot;
    [SerializeField] private RectTransform MenuPanelBkg;
    [SerializeField] private RectTransform ButtonGroup;
    [SerializeField] private RectTransform ButtonLevelSystem;
    [SerializeField] private RectTransform ButtonFriends;
    [SerializeField] private RectTransform ButtonQuests;
    [SerializeField] private RectTransform ButtonProfessions;
    [SerializeField] private RectTransform ButtonShop;

    // PointPanel Stuff
    [Header("PointPanel")]
    [SerializeField] private RectTransform PointPanelRoot;
    [SerializeField] private RectTransform PointPanelBkg;
    [SerializeField] private Text HeaderText;
    [SerializeField] private RectTransform PointSectionRoot;
    [SerializeField] private RectTransform StrengthRoot;
    [SerializeField] private Text StrengthText;
    [SerializeField] private Button StrengthP1Button;
    [SerializeField] private Button StrengthP5Button;
    [SerializeField] private Text AgilityText;
    [SerializeField] private Button AgilityP1Button;
    [SerializeField] private Button AgilityP5Button;
    [SerializeField] private Text IntellectText;
    [SerializeField] private Button IntellectP1Button;
    [SerializeField] private Button IntellectP5Button;
    [SerializeField] private Text BodyText;
    [SerializeField] private Button BodyP1Button;
    [SerializeField] private Button BodyP5Button;
    [SerializeField] private GameObject DummyListStat;
    [SerializeField] private Text FreePointText;
    [SerializeField] private Button FreePointApply;
    [SerializeField] private Button FreePointCancel;
    [SerializeField] private Button PointResetButton;
    [SerializeField] private Text CurrentLvlText;
    [SerializeField] private Text CurrentExpText;
    [SerializeField] private RectTransform DescStatsContentParent;
    [SerializeField] private RectTransform DescStatsDummyHeader;

    [SerializeField] private Text DescStatsDummyText;

    // ApplyResetPanel Stuff
    [Header("ApplyResetPanel")]
    [SerializeField] private RectTransform ApplyResetPanelRoot;
    [SerializeField] private RectTransform ApplyResetPanelBkg;
    [SerializeField] private RectTransform ApplyResetButtonGroup;
    [SerializeField] private Button ButtonYes;
    [SerializeField] private Button ButtonNo;
    [SerializeField] private Text Text;

    // FriendsListPanel Stuff
    [Header("FriendsListPanel")]
    [SerializeField] private RectTransform FriendsListPanelRoot;
    [SerializeField] private RectTransform FriendsListPanelBkg;
    [SerializeField] private RectTransform FriendsListPanelHeader;
    [SerializeField] private Image BorderImage;
    [SerializeField] private GameObject HeaderInvited;
    [SerializeField] private GameObject Invited;
    [SerializeField] private GameObject HeaderFriends;
    [SerializeField] private GameObject Friends;
    [SerializeField] private Text FriendsListPanelText;
    [SerializeField] private Button AddFriendButton;

    // SendInvitePanel Stuff
    [Header("SendInvitePanel")]
    [SerializeField] private RectTransform SendInvitePanelRoot;
    [SerializeField] private RectTransform SendInvitePanelBkg;
    [SerializeField] private InputField SendInviteInputField;
    [SerializeField] private Button SendButton;
    [SerializeField] private Text SendText;
    [SerializeField] private Button CancelButton;
    [SerializeField] private Text CancelText;
}