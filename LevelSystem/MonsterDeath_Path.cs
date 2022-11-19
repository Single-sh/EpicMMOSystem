using System;
using System.Collections.Generic;
using Groups;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UIElements;

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
        }
    }

    public static void RPC_AddGroupExp(long sender, int exp, Vector3 position, int monsterLevel)
    {
        if ((double)Vector3.Distance(position, Player.m_localPlayer.transform.position) >= 50f) return;
        int maxRangeLevel = LevelSystem.Instance.getLevel() + EpicMMOSystem.maxLevelExp.Value;
        if (monsterLevel > maxRangeLevel && !EpicMMOSystem.mentor.Value) return;
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
        int monsterLevel = DataMonsters.getLevel(monsterName) + level - 1;

        var DontSkip = true;
        if (!EpicMMOSystem.curveBossExp.Value)
        {
            switch (monsterName) // if a boss then skip lvl check if disableBossExp if false
            {
                case "Eikthyr": DontSkip = false; break;
                case "gd_king": DontSkip = false; break;
                case "Bonemass":DontSkip = false; break;
                case "Dragon":  DontSkip= false; break;
                case "GoblinKing":DontSkip= false; break;
                default: DontSkip = true; break;
            }
        }
        
        
        if ((double)Vector3.Distance(position, Player.m_localPlayer.transform.position) >= 50f) return;

        int expMonster = DataMonsters.getExp(monsterName);
        int maxExp = DataMonsters.getMaxExp(monsterName);
        float lvlExp = EpicMMOSystem.expForLvlMonster.Value;
        var resultExp = expMonster + (maxExp * lvlExp * (level - 1));
        var exp = Convert.ToInt32(resultExp);
        var playerExp = exp;


        if (EpicMMOSystem.enabledLevelControl.Value && (EpicMMOSystem.curveExp.Value) && DontSkip)
        {
            EpicMMOSystem.MLLogger.LogDebug("Checking player lvl");
            int maxRangeLevel = LevelSystem.Instance.getLevel() + EpicMMOSystem.maxLevelExp.Value;
            if (monsterLevel > maxRangeLevel)
            {
                playerExp = Convert.ToInt32(exp / (monsterLevel - maxRangeLevel ));
            }
            int minRangeLevel = LevelSystem.Instance.getLevel() - EpicMMOSystem.minLevelExp.Value;
            if (monsterLevel < minRangeLevel)
            {
                playerExp = Convert.ToInt32( exp / (minRangeLevel - monsterLevel));
            }
        }
        
        LevelSystem.Instance.AddExp(playerExp);
        if (!Groups.API.IsLoaded()) return;
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

    [HarmonyPatch(typeof(Character), nameof(Character.Damage))]
    public static class ModifierDamage
    {
        public static void Prefix(Character __instance, HitData hit)
        {
            if (!EpicMMOSystem.enabledLevelControl.Value) return;
            //if (EpicMMOSystem.removeDrop.Value) hit.m_toolTier = LevelSystem.Instance.getLevel(); // using toolTier to pass the Lvl of player
            hit.m_toolTier = LevelSystem.Instance.getLevel();
            if (EpicMMOSystem.lowDamageLevel.Value)
            {
                if (__instance.IsPlayer()) return;
                if (!DataMonsters.contains(__instance.gameObject.name)) return;
                int playerLevel = LevelSystem.Instance.getLevel();
                int maxLevelExp = playerLevel + EpicMMOSystem.maxLevelExp.Value;
                int monsterLevel = DataMonsters.getLevel(__instance.gameObject.name) + __instance.m_level - 1;
                if (monsterLevel > maxLevelExp)
                {
                    var damageFactor = (float)playerLevel / monsterLevel;
                    hit.ApplyModifier(damageFactor);
                }
            }
        }
    }
    
        
    
    
    [HarmonyPatch(typeof(Character), nameof(Character.RPC_Damage))]
    static class QuestEnemyKill
    {
        static void Prefix(Character __instance, long sender, HitData hit)
        {
            if (__instance.GetHealth() <= 0) return;
            var BossDropFlag = false;
            if (__instance.GetFaction() == Character.Faction.Boss )
            {
                BossDropFlag = true; 
            }
            var attacker = hit.GetAttacker();
            if (attacker)
            {
                if (attacker.IsPlayer())
                {
                    CharacterLastDamageList[__instance] = sender;
                    if (EpicMMOSystem.enabledLevelControl.Value && (EpicMMOSystem.removeBossDropMax.Value || EpicMMOSystem.removeBossDropMin.Value) && BossDropFlag)// removeboss drop and is a boss
                    {
                        __instance.m_nview.GetZDO().Set("epic playerLevel", hit.m_toolTier); // Check level because is boss

                    }
                    else if (EpicMMOSystem.enabledLevelControl.Value && (EpicMMOSystem.removeDropMax.Value || EpicMMOSystem.removeDropMin.Value) && !BossDropFlag) //remove mobdrop and is not a boss
                    {
                        __instance.m_nview.GetZDO().Set("epic playerLevel", -hit.m_toolTier); // reg mob check for lvl 
                    }
                    else /// No lvl check
                    {
                        EpicMMOSystem.MLLogger.LogInfo("else ZDO epic playerLevel to 0");
                        if (0 != __instance.m_nview.GetZDO().GetInt("epic playerLevel"))
                        {
                            __instance.m_nview.GetZDO().Set("epic playerLevel", 0); // if not set to 0 then set to 0 - minimize zdo traffic
                            EpicMMOSystem.MLLogger.LogInfo("Set ZDO epic playerLevel to 0");
                        }  
                    }
                   
                }
                else
                {
                    if (!attacker.IsTamed())
                    {
                        CharacterLastDamageList[__instance] = 100;
                        if (EpicMMOSystem.enabledLevelControl.Value && (EpicMMOSystem.removeBossDropMax.Value || EpicMMOSystem.removeBossDropMin.Value || EpicMMOSystem.removeDropMax.Value || EpicMMOSystem.removeDropMin.Value))
                        {
                            EpicMMOSystem.MLLogger.LogInfo("Not A player that dmg mob");
                            __instance.m_nview.GetZDO().Set("epic playerLevel", 0);
                        }
                    }
                }
            }
        }
        
        static void Postfix(Character __instance, long sender, HitData hit)
        {
            if (__instance.IsTamed()) return;
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
            if (__instance.IsTamed()) return;
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