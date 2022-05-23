using System;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Rendering;

namespace EpicMMOSystem;

public static class FriendsSystem
{
    private static bool isServer => SystemInfo.graphicsDeviceType == GraphicsDeviceType.Null;
    private static string modName => EpicMMOSystem.ModName;

    public static void Init()
    {
        
    }
    
    [HarmonyPatch(typeof(ZNetScene), nameof(ZNetScene.Awake))]
    private static class ZrouteMethodsServerFeedback
    {
        private static void Postfix()
        {
            if (isServer) return;
            //Пришло приглашение в друзья
            ZRoutedRpc.instance.Register($"{modName} InviteFriend",
                new Action<long, int, int>(RPC_InviteFriend));
            //Приняли приглашение в друзья
            ZRoutedRpc.instance.Register($"{modName} AcceptInviteFriend",
                new Action<long, int, int>(RPC_AcceptFriend));
            //Отклонили приглашение в друзья
            ZRoutedRpc.instance.Register($"{modName} RejectInviteFriend",
                new Action<long, string>(RPC_RejectFriend));
        }
    }
    
    //Отправить приглашение
    public static void inviteFriend(string name)
    {
        var players = ZNet.instance.GetPlayerList();
        
        foreach (var playerInfo in players)
        {
            if (playerInfo.m_name == name)
            {
                int level = LevelSystem.Instance.getLevel();
                ZRoutedRpc.instance.InvokeRoutedRPC(
                    playerInfo.m_characterID.m_userID, 
                    $"{modName} InviteFriend",
                    level, Player.m_localPlayer.GetZDO().GetInt("MagicOverhaulClass", 0)
                );
                Chat.instance.RPC_ChatMessage(200, Vector3.zero, 0, "", $"Запрос дружбы отправлен для {name} отправлен.");
            }
        }
        Chat.instance.RPC_ChatMessage(200, Vector3.zero, 0, "", $" Игрок {name} не найден");
        Chat.instance.SendText( Talker.Type.Whisper, $"Player {name} not found!");
    }

    public static void acceptInvite(FriendInfo info, ZNet.PlayerInfo player)
    {
        ZRoutedRpc.instance.InvokeRoutedRPC(
            player.m_characterID.m_userID,
            $"{modName} AcceptInviteFriend",
            info.level, Player.m_localPlayer.GetZDO().GetInt("MagicOverhaulClass", 0)
        );
    }
    
    public static void rejectInvite(FriendInfo info, ZNet.PlayerInfo player)
    {
        ZRoutedRpc.instance.InvokeRoutedRPC(
            player.m_characterID.m_userID,
            $"{modName} RejectInviteFriend",
            info.name
        );
    }

    //Пришло приглашение в друзья
    private static void RPC_InviteFriend(long sender, int level, int moClass)
    {
        var players = ZNet.instance.GetPlayerList();
        var senderInfo = players.Find(f => f.m_characterID.m_userID == sender);
        var info = new FriendInfo();
        info.name = senderInfo.m_name;
        info.host = senderInfo.m_host;
        info.level = level;
        info.moClass = moClass;
        Chat.instance.RPC_ChatMessage(200, Vector3.zero, 0, "", $"{info.name} отправил запрос дружбы.");
        MyUI.addInviteFriend(info, senderInfo);
    }
    
    //Приняли приглашение в друзья
    private static void RPC_AcceptFriend(long sender, int level, int moClass)
    {
        var players = ZNet.instance.GetPlayerList();
        var senderInfo = players.Find(f => f.m_characterID.m_userID == sender);
        var info = new FriendInfo();
        info.name = senderInfo.m_name;
        info.host = senderInfo.m_host;
        info.level = level;
        info.moClass = moClass;
        Chat.instance.RPC_ChatMessage(200, Vector3.zero, 0, "", $"{info.name} принял предложение дружбы");
        MyUI.acceptInvited(info);
        
    }
    
    //Отклонили приглашение в друзья
    private static void RPC_RejectFriend(long sender, string name)
    {
        Chat.instance.RPC_ChatMessage(200, Vector3.zero, 0, "", $"{name} отказал в дружбе");
    }
}