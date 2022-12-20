using System;
using HarmonyLib;

namespace EpicMMOSystem;

public partial class LevelSystem
{
    public float getAddPhysicDamage()
    {
        var parameter = getParameter(Parameter.Strength);
        var multiplayer = EpicMMOSystem.physicDamage.Value;
        return parameter * multiplayer;
    }
    
    public float getAddWeight()
    {
        if (!Player.m_localPlayer) return 0;
        var parameter = getParameter(Parameter.Strength);
        var multiplayer = EpicMMOSystem.addWeight.Value;
        return parameter * multiplayer;
    }

    public float getStaminaRegen()
    {
        var parameter = getParameter(Parameter.Strength);
        var multiplayer = EpicMMOSystem.staminaRegen.Value;
        return parameter * multiplayer;
    }

    [HarmonyPatch(typeof(ItemDrop.ItemData), nameof(ItemDrop.ItemData.GetDamage), typeof(int))]
    public class AddDamageStrength_Path
    {
        public static void Postfix(ref ItemDrop.ItemData __instance, ref HitData.DamageTypes __result)
        {
            if (Player.m_localPlayer == null) return;
            if (!Player.m_localPlayer.m_inventory.ContainsItem(__instance)) return;
            float add = Instance.getAddPhysicDamage();
            var value = add / 100 + 1;

            __result.m_blunt *= value;
            __result.m_slash *= value;
            __result.m_pierce *= value;
            __result.m_chop *= value;
            __result.m_pickaxe *= value;
        }
    }
    
    [HarmonyPatch(typeof(Player), nameof(Player.GetMaxCarryWeight))]
    public class AddWeight_Path
    {
        static void Postfix(ref float __result)
        {
            var addWeight = Instance.getAddWeight() + EpicMMOSystem.addDefaultWeight.Value;
            __result += (float)Math.Round(addWeight);
        }
    }

    [HarmonyPatch(typeof(SEMan), nameof(SEMan.ModifyStaminaRegen))]
    public static class RegenStamina_Patch
    {
        public static void Postfix(SEMan __instance, ref float staminaMultiplier)
        {
            if (__instance.m_character.IsPlayer())
            {
                float add = Instance.getStaminaRegen();
                staminaMultiplier += add / 100;
            }
        }
    }
}