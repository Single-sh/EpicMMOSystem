using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;

namespace EpicMMOSystem;

public partial class LevelSystem
{
    public float getAddHp()
    {
        var parameter = getParameter(Parameter.Body);
        var multiplayer = EpicMMOSystem.addHp.Value;
        return parameter * multiplayer;
    }

    public float getReducedStaminaBlock()
    {
        var parameter = getParameter(Parameter.Body);
        var multiplayer = EpicMMOSystem.staminaBlock.Value;
        return parameter * multiplayer;
    }
    
    public float getAddPhysicArmor()
    {
        var parameter = getParameter(Parameter.Body);
        var multiplayer = EpicMMOSystem.physicArmor.Value;
        return parameter * multiplayer;
    }
    
    public float getAddRegenHp()
    {
        var parameter = getParameter(Parameter.Body);
        var multiplayer = EpicMMOSystem.regenHp.Value;
        return parameter * multiplayer;
    }

    [HarmonyPatch(typeof(Player), nameof(Player.GetTotalFoodValue))]
    public static class AddHp_Path
    {
        public static void Postfix(ref float hp)
        {
            hp += Instance.getAddHp();
        }
    }
    
    [HarmonyPatch(typeof(Character), nameof(Character.RPC_Damage))]
    public static class PhysicArmor_Path
    {
        public static void Prefix(Character __instance, HitData hit)
        {
            if (!__instance.IsPlayer()) return;
            if (hit.GetAttacker() == __instance) return;

            float add = Instance.getAddPhysicArmor();
            var value = 1 - add / 100;

            hit.m_damage.m_blunt *= value;
            hit.m_damage.m_slash *= value;
            hit.m_damage.m_pierce *= value;
            hit.m_damage.m_chop *= value;
            hit.m_damage.m_pickaxe *= value;
        }
    }
    
    [HarmonyPatch(typeof(SEMan), nameof(SEMan.ModifyHealthRegen))]
    public static class RegenHp_Patch
    {
        public static void Postfix(SEMan __instance, ref float regenMultiplier)
        {
            if (__instance.m_character.IsPlayer())
            {
                float add = Instance.getAddRegenHp();
                regenMultiplier += add / 100;
            }
        }
    }
    
    [HarmonyPatch(typeof(Humanoid),nameof(Humanoid.BlockAttack))]
    static class Humanoid_BlockAttack_Patch
    {
        private static float ReturnMyValue()
        {
            return 1 - Instance.getReducedStaminaBlock() / 100;
        }

        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> StaminaBlock(IEnumerable<CodeInstruction> code)
        {
        
            var method = AccessTools.DeclaredMethod(typeof(Character), nameof(Character.UseStamina));
            var MyMethod = AccessTools.DeclaredMethod(typeof(Humanoid_BlockAttack_Patch), nameof(ReturnMyValue));
            foreach (var instruction in code)
            {
                if (instruction.opcode == OpCodes.Callvirt && instruction.operand == method)
                {
                    yield return new CodeInstruction(OpCodes.Call, MyMethod);
                    yield return new CodeInstruction(OpCodes.Mul);
                }
                yield return instruction;
                
            }
        }
    }
}