using HarmonyLib;

namespace EpicMMOSystem;

public partial class LevelSystem
{
    public float getAddMagicDamage()
    {
        var parameter = getParameter(Parameter.Intellect);
        var multiplayer = EpicMMOSystem.magicDamage.Value;
        return parameter * multiplayer;
    }
    
    public float getAddMagicArmor()
    {
        var parameter = getParameter(Parameter.Intellect);
        var multiplayer = EpicMMOSystem.magicArmor.Value;
        return parameter * multiplayer;
    }

    public float getEitrRegen()
    {
        var parameter = getParameter(Parameter.Intellect);
        var multiplayer = EpicMMOSystem.MagicEitrRegen.Value;
        return parameter * multiplayer;
    }

    [HarmonyPatch(typeof(ItemDrop.ItemData), nameof(ItemDrop.ItemData.GetDamage), typeof(int))]
    public class AddDamageIntellect_Path
    {
        public static void Postfix(ref ItemDrop.ItemData __instance, ref HitData.DamageTypes __result)
        {
            if (Player.m_localPlayer == null) return;
            if (!Player.m_localPlayer.m_inventory.ContainsItem(__instance)) return;
            

            float add = Instance.getAddMagicDamage();
            var value = add / 100 + 1;

            __result.m_fire *= value;
            __result.m_frost *= value;
            __result.m_lightning *= value;
            __result.m_poison *= value;
            __result.m_spirit *= value;
        }
    }
    
    [HarmonyPatch(typeof(Character), nameof(Character.RPC_Damage))]
    public static class RPC_Damage
    {
        public static void Prefix(Character __instance, HitData hit)
        {
            if (!__instance.IsPlayer()) return;
            if (hit.GetAttacker() == __instance) return;

            float add = Instance.getAddMagicArmor();
            var value = 1 - add / 100;

            hit.m_damage.m_fire *= value;
            hit.m_damage.m_frost *= value;
            hit.m_damage.m_lightning *= value;
            hit.m_damage.m_poison *= value;
            hit.m_damage.m_spirit *= value;
            //hit.m_damage.m_elemntal?
        }
    }

    [HarmonyPatch(typeof(SEMan), nameof(SEMan.ModifyEitrRegen))]
    public static class RegenEitr_Patch
    {
        public static void Postfix(SEMan __instance, ref float eitrMultiplier)
        {
            if (__instance.m_character.IsPlayer())
            {
                float add = Instance.getEitrRegen();
                eitrMultiplier += add / 100;
            }
        }
    }
}