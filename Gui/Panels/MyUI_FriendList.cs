using HarmonyLib;
using UnityEngine;

namespace EpicMMOSystem;

public partial class MyUI
{
    private static GameObject friendsListPanel;
    private static GameObject friendsCell;
    
    private static GameObject headerInvited;
    private static GameObject contentInvited;
    private static GameObject headerFriends;
    private static GameObject contentFriends;
    
    private static void InitFriendsList()
    {
        friendsListPanel = UI.transform.Find("Canvas/FriendList").gameObject;
        friendsCell = EpicMMOSystem._asset.LoadAsset<GameObject>("FriendCell");

        headerInvited = friendsListPanel.transform.Find("Scroll View/Viewport/Content/HeaderInvited").gameObject;
        contentInvited = friendsListPanel.transform.Find("Scroll View/Viewport/Content/Invited").gameObject;
        headerFriends = friendsListPanel.transform.Find("Scroll View/Viewport/Content/HeaderFriends").gameObject;
        contentFriends = friendsListPanel.transform.Find("Scroll View/Viewport/Content/Friends").gameObject;
    }

    private static bool hasInvite()
    {
        return false;
    }
}