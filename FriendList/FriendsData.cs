using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;

namespace EpicMMOSystem;

public class FriendsData
{
    private Dictionary<string, FriendInfo> friends = new ();
    public Dictionary<string, FriendInfo> getFriends()
    {
        if (friends.Count == 0)
        {
            LoadFriends();
        }
        return friends;
    }

    public void addFriends(FriendInfo friend)
    {
        if (friends.ContainsKey(friend.name))
        {
            return;
        }
        friends.Add(friend.name, friend);
        SaveFriends();
    }

    public void updateFriends(FriendInfo friend)
    {
        friends[friend.name] = friend;
        SaveFriends();
    }

    public void deleteFriend(FriendInfo friend)
    {
        friends.Remove(friend.name);
        SaveFriends();
    }

    private void SaveFriends()
    {
        if (!Player.m_localPlayer) return;
        var array = friends.Values.ToArray();
        var json = fastJSON.JSON.ToJSON(array);
        Player.m_localPlayer.m_knownTexts[$"{EpicMMOSystem.ModName}_friend_list"] = json ?? "";
    }
    
    private void LoadFriends()
    {
        if (!Player.m_localPlayer) return ;
        if (!Player.m_localPlayer.m_knownTexts.ContainsKey($"{EpicMMOSystem.ModName}_friend_list"))
        {
            return;
        }
        friends.Clear();
        var json = Player.m_localPlayer.m_knownTexts[$"{EpicMMOSystem.ModName}_friend_list"] ?? "";
        var array = fastJSON.JSON.ToObject<FriendInfo[]>(json);
        if (array == null) return;
        foreach (var friendInfo in array)
        {
            friends.Add(friendInfo.name, friendInfo);
        }
    }

    public void ClearFriend()
    {
        friends.Clear();
    }
}

[Serializable]
public class FriendInfo
{
    public string name;
    public string host;
    public int level;
    public int moClass;
}

