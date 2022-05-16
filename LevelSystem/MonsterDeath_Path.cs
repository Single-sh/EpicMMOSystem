using System;
using System.Collections.Generic;
using Groups;
using HarmonyLib;
using UnityEngine;

namespace EpicMMOSystem;

public static class MonsterDeath_Path
{
    private static readonly Dictionary<Character, long> CharacterLastDamageList = new();
    
    [HarmonyPatch(typeof(Game), nameof(Game.Start))]
    public static class RegisterRpc
    {
        
        public static void Postfix()
        {
            ZRoutedRpc.instance.Register($"{EpicMMOSystem.ModName} DeadMonsters", new Action<long, ZPackage>(RPC_DeadMonster));
            ZRoutedRpc.instance.Register($"{EpicMMOSystem.ModName} AddGroupExp", new Action<long, int, Vector3, int>(RPC_AddGroupExp));
            // ZRoutedRpc.instance.Register($"{EpicMMOSystem.ModName} ModifierDamage", new Action<long, string, HitData>(RPC_ModifierDamage));
        }
    }

    public static void RPC_AddGroupExp(long sender, int exp, Vector3 position, int monsterLevel)
    {
        if ((double)Vector3.Distance(position, Player.m_localPlayer.transform.position) >= 50f) return;
        int maxRangeLevel = LevelSystem.Instance.getLevel() + EpicMMOSystem.maxLevelExp.Value;
        if (monsterLevel > maxRangeLevel) return;
        int minRangeLevel = LevelSystem.Instance.getLevel() - EpicMMOSystem.minLevelExp.Value;
        if (monsterLevel < minRangeLevel)
        {
            exp = Convert.ToInt32( exp / ( minRangeLevel - monsterLevel));
        }
        LevelSystem.Instance.AddExp(exp);
    }
    

    public static void RPC_DeadMonster(long sender, ZPackage pkg)
    {
        if (!Player.m_localPlayer) return;
        if(Player.m_localPlayer.IsDead()) return;
        string monsterName = pkg.ReadString();
        int level = pkg.ReadInt();
        Vector3 position = pkg.ReadVector3();
        
        
        if (!DataMonsters.contains(monsterName))
        {
            EpicMMOSystem.print($"{EpicMMOSystem.ModName}: Can't find monster {monsterName}");
            return;
        }
        int monsterLevel = DataMonsters.getLevel(monsterName);
        
        int maxRangeLevel = LevelSystem.Instance.getLevel() + EpicMMOSystem.maxLevelExp.Value;
        if (monsterLevel > maxRangeLevel) return;
        
        if ((double)Vector3.Distance(position, Player.m_localPlayer.transform.position) >= 50f) return;

        int expMonster = DataMonsters.getExp(monsterName);
        int maxExp = DataMonsters.getMaxExp(monsterName);
        float lvlExp = EpicMMOSystem.expForLvlMonster.Value;
        float rate = EpicMMOSystem.rateExp.Value;
        var resultExp = expMonster + (maxExp * lvlExp * (level - 1));
        var exp = Convert.ToInt32(resultExp * rate);
        int minRangeLevel = LevelSystem.Instance.getLevel() - EpicMMOSystem.minLevelExp.Value;
        if (monsterLevel < minRangeLevel)
        {
            exp = Convert.ToInt32( exp / (minRangeLevel - monsterLevel));
        }
        LevelSystem.Instance.AddExp(exp);
        var groupFactor = EpicMMOSystem.groupExp.Value;
        foreach (var playerReference in Groups.API.GroupPlayers())
        {
            if (playerReference.name != Player.m_localPlayer.GetPlayerName())
            {
                var sendExp = exp * groupFactor;
                ZRoutedRpc.instance.InvokeRoutedRPC(
                    playerReference.peerId, 
                    $"{EpicMMOSystem.ModName} AddGroupExp", 
                    new object[] { (int)sendExp, position, monsterLevel}
                    );
            }
        }
    }

    // public static void RPC_ModifierDamage(long sender, string monsterName, HitData hitData)
    // {
    //     EpicMMOSystem.print("3");
    //     if (!DataMonsters.contains(monsterName)) return;
    //     int maxLevelExp = LevelSystem.Instance.getLevel() + EpicMMOSystem.maxLevelExp.Value;
    //     int monsterLevel = DataMonsters.getLevel(monsterName);
    //     if (monsterLevel > maxLevelExp)
    //     {
    //         var damageFactor = (float)maxLevelExp / monsterLevel;
    //         hitData.ApplyModifier(damageFactor);
    //         EpicMMOSystem.print($"4: {damageFactor}");
    //     }
    // }
        
    
    
    [HarmonyPatch(typeof(Character), nameof(Character.RPC_Damage))]
    static class QuestEnemyKill
    {
        static void Prefix(Character __instance, long sender, HitData hit)
        {
            var attacker = hit.GetAttacker();
            if (attacker)
            {
                if (attacker.IsPlayer())
                {
                    CharacterLastDamageList[__instance] = sender;
                }
                else
                {
                    if (!attacker.IsTamed())
                    {
                        CharacterLastDamageList[__instance] = 100;
                    }
                }
                
                // if (!__instance.IsPlayer() && attacker.IsPlayer())
                // {
                    // EpicMMOSystem.print($"1: {hit.GetTotalDamage()}");
                    // if (!DataMonsters.contains(__instance.gameObject.name));
                    // int maxLevelExp = LevelSystem.Instance.getLevel() + EpicMMOSystem.maxLevelExp.Value;
                    // int monsterLevel = DataMonsters.getLevel(__instance.gameObject.name);
                    // if (monsterLevel > maxLevelExp)
                    // {
                    //     var damageFactor = (float)maxLevelExp / monsterLevel;
                    //     hit.ApplyModifier(damageFactor);
                    //     EpicMMOSystem.print($"4: {damageFactor}");
                    // }
                    // ZRoutedRpc.instance.InvokeRoutedRPC(sender, $"{EpicMMOSystem.ModName} ModifierDamage", new object[] { __instance.gameObject.name, hit });
                // }
               
            }
        }
        
        static void Postfix(Character __instance, long sender, HitData hit)
        {
            // EpicMMOSystem.print($"2: {hit.GetTotalDamage()}");
            if (__instance.GetHealth() <= 0f && CharacterLastDamageList.ContainsKey(__instance))
            {
                var pkg = new ZPackage();
                pkg.Write(__instance.gameObject.name);
                pkg.Write(__instance.GetLevel());
                pkg.Write(__instance.transform.position);
                long attacker = CharacterLastDamageList[__instance];
                ZRoutedRpc.instance.InvokeRoutedRPC(attacker, $"{EpicMMOSystem.ModName} DeadMonsters", new object[] { pkg });
                CharacterLastDamageList.Remove(__instance);
            }
        }
    }
    
    [HarmonyPatch(typeof(Character), nameof(Character.ApplyDamage))]
    public static class ApplyDamage
    {
        public static void Postfix(Character __instance, HitData hit)
        {
            // EpicMMOSystem.print($"5: {hit.GetTotalDamage()}");
            if (__instance.GetHealth() <= 0f && CharacterLastDamageList.ContainsKey(__instance))
            {
                var pkg = new ZPackage();
                pkg.Write(__instance.gameObject.name);
                pkg.Write(__instance.GetLevel());
                pkg.Write(__instance.transform.position);
                long attacker = CharacterLastDamageList[__instance];
                ZRoutedRpc.instance.InvokeRoutedRPC(attacker, $"{EpicMMOSystem.ModName} DeadMonsters", new object[] { pkg });
                CharacterLastDamageList.Remove(__instance);
            }
        }
    }

    [HarmonyPatch(typeof(Character),nameof(Character.OnDestroy))]
    static class Character_OnDestroy_Patch
    {
        static void Postfix(Character __instance)
        {
            if (CharacterLastDamageList.ContainsKey(__instance)) CharacterLastDamageList.Remove(__instance);
        }
    }
}